using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;

namespace Improbable.Gdk.CodeGenerator
{
    public class DetailsStore
    {
        public IReadOnlyDictionary<Identifier, UnityTypeDetails> Types { get; }
        public IReadOnlyDictionary<Identifier, UnityEnumDetails> Enums { get; }
        public IReadOnlyDictionary<Identifier, UnityComponentDetails> Components { get; }
        public readonly ReadOnlyDictionary<Identifier, bool> BlittableMap;
        public IReadOnlyList<string> SchemaFiles { get; }

        public static readonly HashSet<string> NonBlittableSchemaTypes = new HashSet<string> { "String", "Bytes" };

        private Dictionary<Identifier, bool> blittableMap = new Dictionary<Identifier, bool>();

        private readonly SchemaBundle bundle;

        public DetailsStore(SchemaBundle bundle)
        {
            this.bundle = bundle;

            PopulateBlittableMaps();
            BlittableMap = new ReadOnlyDictionary<Identifier, bool>(blittableMap);

            var enums = bundle.BundleContents.EnumDefinitions
                .Select(enumm => (enumm.EnumIdentifier, new UnityEnumDetails(enumm)))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            Enums = new ReadOnlyDictionary<Identifier, UnityEnumDetails>(enums);

            var types = bundle.BundleContents.TypeDefinitions
                .Select(type => (type.Identifier, new UnityTypeDetails(type)))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            Types = new ReadOnlyDictionary<Identifier, UnityTypeDetails>(types);

            var components = bundle.BundleContents.ComponentDefinitions
                .Select(component => (component.Identifier, new UnityComponentDetails(component, this)))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            Components = new ReadOnlyDictionary<Identifier, UnityComponentDetails>(components);

            SchemaFiles = bundle.SourceMap.SourceReferences.Values
                .Select(sourceRef => sourceRef.FilePath)
                .Distinct()
                .ToList()
                .AsReadOnly();

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

        public HashSet<Identifier> GetNestedTypes(Identifier identifier)
        {
            return Types.Select(t => t.Key).Where(identifier.IsChild)
                .Concat(Enums.Select(t => t.Key).Where(identifier.IsChild))
                .ToHashSet();
        }

        private void PopulateBlittableMaps()
        {
            // Populate all enums.
            foreach (var enumm in bundle.BundleContents.EnumDefinitions)
            {
                blittableMap.Add(enumm.EnumIdentifier, true);
            }

            var typesToTraverse = new Queue<TypeDefinitionRaw>(bundle.BundleContents.TypeDefinitions);

            while (typesToTraverse.Count > 0)
            {
                var type = typesToTraverse.Dequeue();
                var blittableCheckResult = CheckBlittable(type.Fields);

                if (!blittableCheckResult.HasValue)
                {
                    typesToTraverse.Enqueue(type);
                    continue;
                }

                blittableMap.Add(type.Identifier, blittableCheckResult.Value);
            }

            foreach (var component in bundle.BundleContents.ComponentDefinitions)
            {
                var blittableCheckResult = CheckBlittable(component.Fields);

                if (!blittableCheckResult.HasValue)
                {
                    throw new InvalidOperationException("Could not check blittable-ness of component");
                }

                blittableMap.Add(component.Identifier, blittableCheckResult.Value);
            }
        }

        private bool? CheckBlittable(IEnumerable<Field> fields)
        {
            foreach (var field in fields)
            {
                if (field.Map != null || field.List != null || field.Option != null)
                {
                    return false;
                }

                var innerSingularType = field.Singular.Type;

                if (innerSingularType.UserType != null)
                {
                    if (!blittableMap.TryGetValue(Identifier.FromQualifiedName(innerSingularType.UserType.QualifiedName), out var isFieldBlittable))
                    {
                        return null;
                    }

                    if (!isFieldBlittable)
                    {
                        return false;
                    }
                }
                else if (innerSingularType.Primitive != null && NonBlittableSchemaTypes.Contains(innerSingularType.Primitive))
                {
                    return false;
                }

                // No need to check enums - they are always blittable.
            }

            return true;
        }
    }
}
