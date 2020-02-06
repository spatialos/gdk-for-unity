using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Utils;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityTypeDetails
    {
        public readonly string Name;
        public readonly string Namespace;

        private readonly string camelCaseName;

        public readonly string QualifiedName;

        public string PartialResourceTypeName => fullyQualifiedTypeName.Split("::")[1];
        private readonly string fullyQualifiedTypeName;

        public IReadOnlyList<UnityFieldDetails> FieldDetails { get; internal set; }

        public IReadOnlyList<UnityTypeDetails> ChildTypes { get; private set; }
        public IReadOnlyList<UnityEnumDetails> ChildEnums { get; private set; }

        public readonly SerializationOverride SerializationOverride;

        public bool HasSerializationOverride => SerializationOverride != null;

        private readonly TypeDefinition rawTypeDefinition;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UnityTypeDetails(string package, TypeDefinition rawTypeDefinition, SerializationOverride serializationOverride)
        {
            Name = rawTypeDefinition.Name;
            Namespace = Formatting.CapitaliseQualifiedNameParts(package);

            camelCaseName = Formatting.PascalCaseToCamelCase(Name);

            QualifiedName = rawTypeDefinition.QualifiedName;

            fullyQualifiedTypeName = CommonDetailsUtils.GetCapitalisedFqnTypename(rawTypeDefinition.QualifiedName);

            SerializationOverride = serializationOverride;

            this.rawTypeDefinition = rawTypeDefinition;
        }

        public void Populate(DetailsStore store)
        {
            PopulateChildren(store);
            PopulateFields(store);
        }

        private void PopulateChildren(DetailsStore store)
        {
            var children = store.GetNestedTypes(rawTypeDefinition.QualifiedName);

            Logger.Trace($"Populating child type details for type {rawTypeDefinition.QualifiedName}.");
            ChildTypes = store.Types
                .Where(kv => children.Contains(kv.Key))
                .Select(kv => kv.Value)
                .ToList()
                .AsReadOnly();

            Logger.Trace($"Populating child enum details for type {rawTypeDefinition.QualifiedName}.");
            ChildEnums = store.Enums
                .Where(kv => children.Contains(kv.Key))
                .Select(kv => kv.Value)
                .ToList()
                .AsReadOnly();
        }

        private void PopulateFields(DetailsStore store)
        {
            Logger.Trace($"Populating field details for type {rawTypeDefinition.QualifiedName}.");
            FieldDetails = rawTypeDefinition.Fields
                .Select(field => new UnityFieldDetails(field, store))
                .Where(fieldDetail =>
                {
                    var clashingChildEnums = ChildEnums
                        .Where(childEnum =>
                        {
                            // When field does not clash with child enum, return false
                            if (!fieldDetail.PascalCaseName.Equals(childEnum.Name))
                            {
                                return false;
                            }

                            Logger.Error($"Error in type \"{Name}\". Field \"{fieldDetail.RawFieldDefinition.Name}\" clashes with child enum \"{childEnum.Name}\".");
                            return true;
                        });

                    var clashingChildTypes = ChildTypes
                        .Where(childType =>
                        {
                            // When field does not clash with child type, return false
                            if (!fieldDetail.PascalCaseName.Equals(childType.Name))
                            {
                                return false;
                            }

                            Logger.Error($"Error in type \"{Name}\". Field \"{fieldDetail.RawFieldDefinition.Name}\" clashes with child type \"{childType.camelCaseName}\".");
                            return true;
                        });

                    // Only return true if the field has no name clashes with child enums and types
                    return !clashingChildEnums.Any() && !clashingChildTypes.Any();
                })
                .ToList()
                .AsReadOnly();
        }
    }

    public class SerializationOverride
    {
        private readonly string staticClassFqn;

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
