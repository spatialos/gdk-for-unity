using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class DetailsStore
    {
        public readonly IReadOnlyDictionary<string, UnityTypeDetails> Types;
        public readonly IReadOnlyDictionary<string, UnityEnumDetails> Enums;
        public readonly IReadOnlyDictionary<string, UnityComponentDetails> Components;
        public readonly IReadOnlyList<ComponentSetDetails> ComponentSets;
        public readonly IReadOnlyList<string> WorkerTypes;

        public readonly ImmutableHashSet<string> BlittableSet;
        public readonly IReadOnlyList<string> SchemaFiles;

        private readonly Dictionary<string, bool> blittableMap = new Dictionary<string, bool>();

        public readonly IFileTree FileTree;
        private readonly SchemaBundle bundle;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public DetailsStore(SchemaBundle bundle, IEnumerable<string> serializationOverrides, IFileTree fileTree,
            string workerJsonDirectory)
        {
            this.bundle = bundle;
            FileTree = fileTree;

            Logger.Trace("Loading serialization overrides.");
            var overrideMap = serializationOverrides.Select(@override =>
            {
                var parts = @override.Split(";");

                if (parts.Length != 2)
                {
                    throw new ArgumentException($"Serialization override malformed: {@override}");
                }

                Logger.Info($"Found serialization override {parts[1]} for {parts[0]}.");

                return (parts[0], parts[1]);
            }).ToDictionary(pair => pair.Item1, pair => pair.Item2);
            Logger.Trace($"Found {overrideMap.Count} serialization {(overrideMap.Count == 1 ? "override" : "overrides")}.");

            PopulateBlittableMaps();
            BlittableSet = ImmutableHashSet.CreateRange(blittableMap.Where(kv => kv.Value).Select(kv => kv.Key));

            var enums = new Dictionary<string, UnityEnumDetails>();
            var types = new Dictionary<string, UnityTypeDetails>();
            var components = new Dictionary<string, UnityComponentDetails>();

            Logger.Trace("Processing schema files.");
            foreach (var file in bundle.SchemaFiles)
            {
                Logger.Trace($"Initialising details from {file.CanonicalPath}.");

                foreach (var rawEnum in file.Enums)
                {
                    enums.Add(rawEnum.QualifiedName, new UnityEnumDetails(file.Package.Name, file.CanonicalPath, rawEnum));
                }

                foreach (var type in file.Types)
                {
                    var typeDetails = new UnityTypeDetails(file.Package.Name, file.CanonicalPath, type);

                    if (overrideMap.TryGetValue(typeDetails.FullyQualifiedName, out var staticClassFqn))
                    {
                        typeDetails.SerializationOverride = new SerializationOverride(staticClassFqn);
                        Logger.Trace($"Adding serialization override {staticClassFqn} for {typeDetails.QualifiedName}.");
                    }

                    types.Add(type.QualifiedName, typeDetails);
                }

                foreach (var component in file.Components)
                {
                    components.Add(component.QualifiedName, new UnityComponentDetails(file.Package.Name, file.CanonicalPath, component, this));
                }

                Logger.Trace($"Enums added: {file.Enums.Count}.");
                Logger.Trace($"Types added: {file.Types.Count}.");
                Logger.Trace($"Components added: {file.Components.Count}.");
            }

            Logger.Info($"Processed {bundle.SchemaFiles.Count} schema files.");

            Enums = new ReadOnlyDictionary<string, UnityEnumDetails>(enums);
            Types = new ReadOnlyDictionary<string, UnityTypeDetails>(types);
            Components = new ReadOnlyDictionary<string, UnityComponentDetails>(components);
            ComponentSets = bundle.SchemaFiles
                .SelectMany(file => file.ComponentSets)
                .Select(componentSet => new ComponentSetDetails(componentSet, this))
                .ToList();

            SchemaFiles = bundle.SchemaFiles
                .Select(file => file.CanonicalPath)
                .ToList().AsReadOnly();
            Logger.Trace($"Retrieved canonical paths of {SchemaFiles.Count} schema files.");

            Logger.Trace("Populating all type details.");
            foreach (var kv in Types)
            {
                kv.Value.Populate(this);
            }

            Logger.Trace($"Populated details of {Types.Count} types.");

            Logger.Trace($"Populating all component field details.");
            foreach (var kv in Components)
            {
                kv.Value.PopulateFields(this);
            }

            Logger.Trace($"Populated field details of {Components.Count} components.");

            Logger.Trace("Removing all recursive options.");
            var numFieldsRemoved = RemoveRecursiveOptions();
            if (numFieldsRemoved > 0)
            {
                Logger.Trace($"Removed {numFieldsRemoved} recursive options.");
            }

            WorkerTypes = ExtractWorkerTypes(workerJsonDirectory);
        }

        public HashSet<string> GetNestedTypes(string qualifiedName)
        {
            var typeChildren = Types.Select(pair => pair.Key).Where(maybeChild => IsChild(qualifiedName, maybeChild));
            var enumChildren = Enums.Select(pair => pair.Key).Where(maybeChild => IsChild(qualifiedName, maybeChild));

            return typeChildren.Concat(enumChildren).ToHashSet();
        }

        private bool IsChild(string parent, string potentialChild)
        {
            return potentialChild.StartsWith(parent) &&
                potentialChild.Split(".").Length == parent.Split(".").Length + 1;
        }

        private void PopulateBlittableMaps()
        {
            foreach (var enumm in bundle.SchemaFiles.SelectMany(file => file.Enums))
            {
                blittableMap.Add(enumm.QualifiedName, true);
            }

            var typesToTraverse = new Queue<TypeDefinition>(bundle.SchemaFiles.SelectMany(file => file.Types));

            while (typesToTraverse.Count > 0)
            {
                var type = typesToTraverse.Dequeue();

                if (!CanCheckBlittable(type))
                {
                    typesToTraverse.Enqueue(type);
                    continue;
                }

                blittableMap.Add(type.QualifiedName, CheckBlittable(type.Fields));
            }

            foreach (var component in bundle.SchemaFiles.SelectMany(file => file.Components))
            {
                IReadOnlyList<FieldDefinition> fields;

                if (!string.IsNullOrEmpty(component.DataDefinition))
                {
                    var dataType = bundle.SchemaFiles
                        .SelectMany(file => file.Types)
                        .FirstOrDefault(type => type.QualifiedName == component.DataDefinition);

                    if (dataType == null)
                    {
                        throw new Exception(
                            $"Invalid bundle JSON. Could not find type reference: {component.QualifiedName}");
                    }

                    fields = dataType.Fields;
                }
                else
                {
                    fields = component.Fields;
                }

                blittableMap.Add(component.QualifiedName, CheckBlittable(fields));
            }
        }

        private bool CheckBlittable(IEnumerable<FieldDefinition> fields)
        {
            // Any isn't specialized.
            return fields.Select(CheckBlittable).All(@bool => @bool);
        }

        public bool CheckBlittable(FieldDefinition field)
        {
            if (field.MapType != null || field.ListType != null || field.OptionType != null)
            {
                return false;
            }

            var innerSingularType = field.SingularType;

            switch (innerSingularType.Type.ValueTypeSelector)
            {
                case ValueType.Enum:
                    return true;
                case ValueType.Primitive:
                    return innerSingularType.Type.Primitive.IsBlittable();
                case ValueType.Type:
                    return blittableMap[innerSingularType.Type.Type];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CanCheckBlittable(TypeDefinition typeDefinition)
        {
            return typeDefinition.Fields
                .Where(type => type.SingularType != null && type.SingularType.Type.ValueTypeSelector == ValueType.Type)
                .Select(type => type.SingularType.Type.Type)
                .All(qualifiedName => blittableMap.ContainsKey(qualifiedName));
        }

        private int RemoveRecursiveOptions()
        {
            bool IsRecursive(string fieldType, IEnumerable<string> parentTypes)
            {
                var typeDetails = Types[fieldType];

                var directChildTypes = typeDetails.FieldDetails
                    .Where(field => field.RawFieldDefinition.TypeSelector == FieldType.Singular)
                    .Where(field => field.RawFieldDefinition.SingularType.Type.ValueTypeSelector == ValueType.Type)
                    .Select(field => field.RawFieldDefinition.SingularType.Type.Type);

                var optionChildTypes = typeDetails.FieldDetails
                    .Where(field => field.RawFieldDefinition.TypeSelector == FieldType.Option)
                    .Where(field => field.RawFieldDefinition.OptionType.InnerType.ValueTypeSelector == ValueType.Type)
                    .Select(field => field.RawFieldDefinition.OptionType.InnerType.Type);

                return directChildTypes.Any(parentTypes.Contains) ||
                    optionChildTypes.Any(parentTypes.Contains) ||
                    directChildTypes.Any(childType => IsRecursive(childType, parentTypes.Append(childType))) ||
                    optionChildTypes.Any(childType => IsRecursive(childType, parentTypes.Append(childType)));
            }

            var toRemove = new Dictionary<string, List<UnityFieldDetails>>();

            foreach (var (qualifiedTypeName, typeDetails) in Types)
            {
                var recursiveOptions = typeDetails.FieldDetails
                    .Where(field => field.RawFieldDefinition.TypeSelector == FieldType.Option && field.RawFieldDefinition.OptionType.InnerType.ValueTypeSelector == ValueType.Type)
                    .Where(field => IsRecursive(field.RawFieldDefinition.OptionType.InnerType.Type, new[] { qualifiedTypeName }))
                    .ToList();

                if (recursiveOptions.Any())
                {
                    toRemove[qualifiedTypeName] = recursiveOptions;
                }
            }

            var numFieldsRemoved = 0;
            foreach (var (qualifiedTypeName, fieldDetails) in toRemove)
            {
                var type = Types[qualifiedTypeName];

                type.FieldDetails = type.FieldDetails
                    .Where(field =>
                    {
                        if (!fieldDetails.Contains(field))
                        {
                            return true;
                        }

                        numFieldsRemoved++;
                        Logger.Warn($"Excluding field {field.CamelCaseName} from type {type.QualifiedName}. Recursive option types are unsupported.");
                        return false;
                    })
                    .ToList()
                    .AsReadOnly();
            }

            return numFieldsRemoved;
        }

        private static List<string> ExtractWorkerTypes(string path)
        {
            Logger.Trace($"Extracting worker types from {path}.");

            const string workerTypeFlag = "+workerType";
            var workerTypes = new List<string>();

            var fileNames = Directory.EnumerateFiles(path, "*.json").ToList();
            Logger.Trace($"Found {fileNames.Count} worker json files:\n - {string.Join("\n - ", fileNames)}");

            foreach (var fileName in fileNames)
            {
                Logger.Trace($"Extracting worker type from {fileName}.");
                var text = File.ReadAllText(fileName);
                if (!text.Contains(workerTypeFlag))
                {
                    Logger.Warn($"{fileName} does not contain the '{workerTypeFlag}' flag.");
                    continue;
                }

                var jsonRep = JObject.Parse(text);
                var arguments = jsonRep.SelectToken("external.default.windows.arguments");
                if (arguments == null)
                {
                    Logger.Warn($"Could not navigate to external > default > windows > arguments in {fileName}.");
                    continue;
                }

                for (var i = 0; i < arguments.Count() - 1; i++)
                {
                    if (!workerTypeFlag.Equals(arguments[i].ToString()))
                    {
                        continue;
                    }

                    var workerType = arguments[i + 1].ToString();
                    Logger.Trace($"Adding {workerType} to list of worker types.");
                    workerTypes.Add(workerType);
                }
            }

            Logger.Trace($"Found {workerTypes.Count} worker types:\n - {string.Join("\n - ", workerTypes)}");
            return workerTypes;
        }
    }
}
