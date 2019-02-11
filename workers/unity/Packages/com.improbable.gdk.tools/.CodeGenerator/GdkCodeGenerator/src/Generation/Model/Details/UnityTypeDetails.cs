using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Utils;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityTypeDetails
    {
        public string CapitalisedName { get; }
        public string CamelCaseName { get; }
        public string FullyQualifiedTypeName { get; }

        public IReadOnlyList<UnityFieldDetails> FieldDetails { get; private set; }

        public Identifier Identifier { get; }

        public IReadOnlyList<UnityTypeDetails> ChildTypes;
        public IReadOnlyList<UnityEnumDetails> ChildEnums;

        private TypeDefinitionRaw raw;

        public UnityTypeDetails(TypeDefinitionRaw typeDefinitionRaw)
        {
            CapitalisedName = typeDefinitionRaw.Identifier.Name;
            CamelCaseName = Formatting.PascalCaseToCamelCase(CapitalisedName);
            FullyQualifiedTypeName =
                $"global::{Formatting.CapitaliseQualifiedNameParts(typeDefinitionRaw.Identifier.QualifiedName)}";

            Identifier = typeDefinitionRaw.Identifier;
            raw = typeDefinitionRaw;
        }

        public string GetPartialResourceTypeName()
        {
            return FullyQualifiedTypeName.Split("::")[1];
        }

        public void PopulateChildren(DetailsStore store)
        {
            var children = store.GetNestedTypes(Identifier);

            ChildTypes = store.Types
                .Where(kv => children.Contains(kv.Key))
                .Select(kv => kv.Value)
                .ToList()
                .AsReadOnly();

            ChildEnums = store.Enums
                .Where(kv => children.Contains(kv.Key))
                .Select(kv => kv.Value)
                .ToList()
                .AsReadOnly();
        }

        public void PopulateFields(DetailsStore store)
        {
            FieldDetails = raw.Fields
                .Select(field => new UnityFieldDetails(field, store))
                .ToList()
                .AsReadOnly();
        }
    }
}
