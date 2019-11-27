using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Improbable.Gdk.CodeGeneration.FileHandling;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class DetailsStore
    {
        public IReadOnlyDictionary<string, UnityTypeDetails> Types { get; }
        public IReadOnlyDictionary<string, UnityEnumDetails> Enums { get; }
        public IReadOnlyDictionary<string, UnityComponentDetails> Components { get; }
        public readonly ImmutableHashSet<string> BlittableSet;
        public IReadOnlyList<string> SchemaFiles { get; }

        public static readonly HashSet<PrimitiveType> NonBlittableSchemaTypes = new HashSet<PrimitiveType>
            { PrimitiveType.Bytes, PrimitiveType.String, PrimitiveType.Entity };

        public IFileTree FileTree { get; }

        private Dictionary<string, bool> blittableMap = new Dictionary<string, bool>();

        private readonly SchemaBundle bundle;

        private Logger logger;

        public DetailsStore(SchemaBundle bundle, List<string> serializationOverrides, IFileTree fileTree)
        {
            logger = LogManager.GetCurrentClassLogger();

            FileTree = fileTree;
            this.bundle = bundle;

            logger.Info("Loading serialization overrides");
            var overrideMap = serializationOverrides.Select(@override =>
            {
                var parts = @override.Split(";");

                if (parts.Length != 2)
                {
                    throw new ArgumentException($"Serialization override malformed: {@override}");
                }

                logger.Info($"Found serialization override {parts[1]} for {parts[0]}");

                return (parts[0], parts[1]);
            }).ToDictionary(pair => pair.Item1, pair => pair.Item2);
            logger.Info($"Found {overrideMap.Count} serialization {(overrideMap.Count == 1 ? "override" : "overrides")}");

            logger.Info("Loading enums, types and components");

            PopulateBlittableMaps();
            BlittableSet = ImmutableHashSet.CreateRange(blittableMap.Where(kv => kv.Value).Select(kv => kv.Key));

            var enums = new Dictionary<string, UnityEnumDetails>();
            var types = new Dictionary<string, UnityTypeDetails>();
            var components = new Dictionary<string, UnityComponentDetails>();

            foreach (var file in bundle.SchemaFiles)
            {
                logger.Info($"Initialising details from {file.CanonicalPath}");

                foreach (var enumm in file.Enums)
                {
                    enums.Add(enumm.QualifiedName, new UnityEnumDetails(file.Package.Name, enumm));
                }

                foreach (var type in file.Types)
                {
                    var typeDetails = new UnityTypeDetails(file.Package.Name, type);

                    if (overrideMap.TryGetValue(typeDetails.FullyQualifiedTypeName, out var staticClassFqn))
                    {
                        typeDetails.SerializationOverride = new SerializationOverride(staticClassFqn);
                        logger.Trace($"Added serialization override {staticClassFqn} for {typeDetails.FullyQualifiedTypeName}");
                    }

                    types.Add(type.QualifiedName, typeDetails);
                }

                foreach (var component in file.Components)
                {
                    components.Add(component.QualifiedName, new UnityComponentDetails(file.Package.Name, component, this));
                }

                logger.Trace($"Enums added: {file.Enums.Count}");
                logger.Trace($"Types added: {file.Types.Count}");
                logger.Trace($"Components added: {file.Components.Count}");
            }

            Enums = new ReadOnlyDictionary<string, UnityEnumDetails>(enums);
            Types = new ReadOnlyDictionary<string, UnityTypeDetails>(types);
            Components = new ReadOnlyDictionary<string, UnityComponentDetails>(components);

            logger.Info("Retrieving canonical paths of schema files");
            SchemaFiles = bundle.SchemaFiles
                .Select(file => file.CanonicalPath)
                .ToList().AsReadOnly();
            logger.Info($"Retrieved canonical paths of {SchemaFiles.Count} schema files");

            logger.Info("Populating all type details");
            foreach (var kv in Types)
            {
                kv.Value.Populate(this);
            }
            logger.Info($"Populated details of {Types.Count} types");

            logger.Info($"Populating all component field details");
            foreach (var kv in Components)
            {
                kv.Value.PopulateFields(this);
            }
            logger.Info($"Populated field details of {Components.Count} components");

            logger.Info("Removing all recursive options");
            var fieldsRemoved = RemoveRecursiveOptions();
            logger.Info($"Removed {fieldsRemoved} recursive options");
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
                    return !NonBlittableSchemaTypes.Contains(innerSingularType.Type.Primitive);
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
                    .Where(field => field.Raw.TypeSelector == FieldType.Singular)
                    .Where(field => field.Raw.SingularType.Type.ValueTypeSelector == ValueType.Type)
                    .Select(field => field.Raw.SingularType.Type.Type);

                var optionChildTypes = typeDetails.FieldDetails
                    .Where(field => field.Raw.TypeSelector == FieldType.Option)
                    .Where(field => field.Raw.OptionType.InnerType.ValueTypeSelector == ValueType.Type)
                    .Select(field => field.Raw.OptionType.InnerType.Type);

                return directChildTypes.Any(parentTypes.Contains) ||
                    optionChildTypes.Any(parentTypes.Contains) ||
                    directChildTypes.Any(childType => IsRecursive(childType, parentTypes.Append(childType))) ||
                    optionChildTypes.Any(childType => IsRecursive(childType, parentTypes.Append(childType)));
            }

            var toRemove = new Dictionary<string, List<UnityFieldDetails>>();

            foreach (var type in Types)
            {
                var recursiveOptions = type.Value.FieldDetails
                    .Where(field => field.Raw.TypeSelector == FieldType.Option && field.Raw.OptionType.InnerType.ValueTypeSelector == ValueType.Type)
                    .Where(field => IsRecursive(field.Raw.OptionType.InnerType.Type, new[] { type.Key }))
                    .ToList();

                if (recursiveOptions.Any())
                {
                    toRemove[type.Key] = recursiveOptions;
                }
            }

            var fieldsRemoved = 0;
            foreach (var pair in toRemove)
            {
                var type = Types[pair.Key];

                type.FieldDetails = type.FieldDetails
                    .Where(field =>
                    {
                        if (!pair.Value.Contains(field))
                        {
                            return true;
                        }

                        fieldsRemoved++;
                        logger.Info($"Excluding field {field.CamelCaseName} from type {type.QualifiedName}");
                        return false;
                    })
                    .ToList()
                    .AsReadOnly();
            }

            return fieldsRemoved;
        }
    }
}
