using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Improbable.Gdk.CodeGeneration.Model;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;

namespace Improbable.Gdk.CodeGenerator
{
    public class DetailsStore
    {
        public IReadOnlyDictionary<Identifier, UnityTypeDetails> Types { get; }
        public IReadOnlyDictionary<Identifier, UnityEnumDetails> Enums { get; }
        public IReadOnlyDictionary<Identifier, UnityComponentDetails> Components { get; }
        public readonly ImmutableHashSet<Identifier> BlittableMap;
        public IReadOnlyList<string> SchemaFiles { get; }

        public static readonly HashSet<string> NonBlittableSchemaTypes = new HashSet<string> { BuiltInSchemaTypes.BuiltInString, BuiltInSchemaTypes.BuiltInBytes };

        private Dictionary<Identifier, bool> blittableMap = new Dictionary<Identifier, bool>();

        private readonly SchemaBundle bundle;

        public DetailsStore(SchemaBundle bundle)
        {
            this.bundle = bundle;

            PopulateBlittableMaps();
            BlittableMap = ImmutableHashSet.CreateRange(blittableMap.Where(kv => kv.Value).Select(kv => kv.Key));

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
            return Types.Select(t => t.Key).Where(t => IsIdentifierChild(identifier, t))
                .Concat(Enums.Select(t => t.Key).Where(t => IsIdentifierChild(identifier, t)))
                .ToHashSet();
        }

        private bool IsIdentifierChild(Identifier parent, Identifier potentialChild)
        {
            return potentialChild.QualifiedName.StartsWith($"{parent.QualifiedName}.")
                && potentialChild.Path.Count == parent.Path.Count + 1;
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
                List<Field> fields;

                if (component.Data != null)
                {
                    var dataType = bundle.BundleContents.TypeDefinitions.FirstOrDefault(type =>
                        type.Identifier.QualifiedName == component.Data.QualifiedName);

                    if (dataType == null)
                    {
                        throw new Exception($"Invalid bundle JSON. Could not find type reference: {component.Data.QualifiedName}");
                    }

                    fields = dataType.Fields;
                }
                else
                {
                    fields = component.Fields;
                }

                var blittableCheckResult = CheckBlittable(fields);

                if (!blittableCheckResult.HasValue)
                {
                    throw new InvalidOperationException("Could not check blittable-ness of component");
                }

                blittableMap.Add(component.Identifier, blittableCheckResult.Value);
            }
        }

        private bool? CheckBlittable(IEnumerable<Field> fields)
        {
            var results = fields.Select(CheckBlittable);

            if (results.Any(res => res == null))
            {
                return null;
            }

            return !results.Any(res => res.HasValue && !res.Value);
        }

        private bool? CheckBlittable(Field field)
        {
            if (field.Map != null || field.List != null || field.Option != null)
            {
                blittableMap[field.Identifier] = false;
                return false;
            }

            var innerSingularType = field.Singular.Type;

            if (innerSingularType.UserType != null)
            {
                if (!blittableMap.TryGetValue(CommonDetailsUtils.CreateIdentifier(innerSingularType.UserType.QualifiedName), out var isFieldBlittable))
                {
                    return null;
                }

                blittableMap[field.Identifier] = isFieldBlittable;
                return isFieldBlittable;
            }

            if (innerSingularType.Primitive != null && NonBlittableSchemaTypes.Contains(innerSingularType.Primitive))
            {
                blittableMap[field.Identifier] = false;
                return false;
            }

            // No need to check enums - they are always blittable.
            blittableMap[field.Identifier] = true;
            return true;
        }
    }
}
