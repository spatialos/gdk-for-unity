using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Improbable.Gdk.CodeGeneration;
using ValueType = Improbable.Gdk.CodeGeneration.ValueType;

namespace Improbable.Gdk.CodeGenerator
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

        private Dictionary<string, bool> blittableMap = new Dictionary<string, bool>();

        private readonly SchemaBundle bundle;

        public DetailsStore(SchemaBundle bundle)
        {
            this.bundle = bundle;

            PopulateBlittableMaps();
            BlittableSet = ImmutableHashSet.CreateRange(blittableMap.Where(kv => kv.Value).Select(kv => kv.Key));

            var enums = new Dictionary<string, UnityEnumDetails>();
            var types = new Dictionary<string, UnityTypeDetails>();
            var components = new Dictionary<string, UnityComponentDetails>();

            foreach (var file in bundle.SchemaFiles)
            {
                foreach (var enumm in file.Enums)
                {
                    enums.Add(enumm.QualifiedName, new UnityEnumDetails(file.Package.Name, enumm));
                }

                foreach (var type in file.Types)
                {
                    types.Add(type.QualifiedName, new UnityTypeDetails(file.Package.Name, type));
                }

                foreach (var component in file.Components)
                {
                    components.Add(component.QualifiedName, new UnityComponentDetails(file.Package.Name, component, this));
                }
            }

            Enums = new ReadOnlyDictionary<string, UnityEnumDetails>(enums);
            Types = new ReadOnlyDictionary<string, UnityTypeDetails>(types);
            Components = new ReadOnlyDictionary<string, UnityComponentDetails>(components);

            SchemaFiles = bundle.SchemaFiles
                .Select(file => file.CanonicalPath)
                .ToList().AsReadOnly();

            foreach (var kv in Types)
            {
                kv.Value.PopulateFields(this);
                kv.Value.PopulateChildren(this);
            }

            foreach (var kv in Components)
            {
                kv.Value.PopulateFields(this);
            }
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
    }
}
