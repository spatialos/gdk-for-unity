using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Utils;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityTypeDetails : GeneratorInputDetails
    {
        public string PartialResourceTypeName => FullyQualifiedName.Split("::")[1];

        public IReadOnlyList<UnityFieldDetails> FieldDetails { get; internal set; }

        public IReadOnlyList<UnityTypeDetails> ChildTypes { get; private set; }
        public IReadOnlyList<UnityEnumDetails> ChildEnums { get; private set; }

        public SerializationOverride SerializationOverride { get; internal set; }

        public bool HasSerializationOverride => SerializationOverride != null;

        private readonly TypeDefinition rawTypeDefinition;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UnityTypeDetails(string package, TypeDefinition rawTypeDefinition)
            : base(package, rawTypeDefinition)
        {
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
                            if (!fieldDetail.Name.Equals(childEnum.Name))
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
                            if (!fieldDetail.Name.Equals(childType.Name))
                            {
                                return false;
                            }

                            Logger.Error($"Error in type \"{Name}\". Field \"{fieldDetail.RawFieldDefinition.Name}\" clashes with child type \"{childType.CamelCaseName}\".");
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
