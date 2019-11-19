using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityTypeDetails
    {
        public string Package { get; }
        public string CapitalisedName { get; }
        public string CamelCaseName { get; }
        public string FullyQualifiedTypeName { get; }

        public IReadOnlyList<UnityFieldDetails> FieldDetails { get; internal set; }

        public IReadOnlyList<UnityTypeDetails> ChildTypes;
        public IReadOnlyList<UnityEnumDetails> ChildEnums;

        public SerializationOverride SerializationOverride;

        public bool HasSerializationOverride => SerializationOverride != null;

        private TypeDefinition raw;

        public UnityTypeDetails(string package, TypeDefinition typeDefinitionRaw)
        {
            Package = package;
            CapitalisedName = typeDefinitionRaw.Name;
            CamelCaseName = Formatting.PascalCaseToCamelCase(CapitalisedName);
            FullyQualifiedTypeName = $"global::{Formatting.CapitaliseQualifiedNameParts(typeDefinitionRaw.QualifiedName)}";

            raw = typeDefinitionRaw;
        }

        public string GetPartialResourceTypeName()
        {
            return FullyQualifiedTypeName.Split("::")[1];
        }

        public void PopulateChildren(DetailsStore store)
        {
            var children = store.GetNestedTypes(raw.QualifiedName);

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

        public bool IsValid()
        {
            var isValid = true;
            var typeName = CapitalisedName;

            foreach (var fieldDetail in FieldDetails)
            {
                var clashingChildEnums = ChildEnums
                    .Where(childEnum => fieldDetail.PascalCaseName.Equals(childEnum.TypeName));
                foreach (var childEnum in clashingChildEnums)
                {
                    isValid = false;
                    Console.Error.WriteLine(
                        $"Error in type \"{typeName}\". " +
                        $"Field \"{fieldDetail.Raw.Name}\" clashes with child enum \"{childEnum.TypeName}\".");
                }

                var clashingChildTypes = ChildTypes
                    .Where(childType => fieldDetail.PascalCaseName.Equals(childType.CapitalisedName));
                foreach (var childType in clashingChildTypes)
                {
                    isValid = false;
                    Console.Error.WriteLine(
                        $"Error in type \"{typeName}\". " +
                        $"Field \"{fieldDetail.Raw.Name}\" clashes with child type \"{childType.CamelCaseName}\".");
                }
            }

            return isValid;
        }
    }

    public class SerializationOverride
    {
        private string staticClassFqn;

        public SerializationOverride(string staticClassFqn)
        {
            this.staticClassFqn = staticClassFqn;
        }

        public string GetSerializationString(string instance, string schemaObject)
        {
            return $"{staticClassFqn}.Serialize({instance}, {schemaObject});";
        }

        public string GetDeserializeString(string schemaObject)
        {
            return $"{staticClassFqn}.Deserialize({schemaObject})";
        }
    }
}
