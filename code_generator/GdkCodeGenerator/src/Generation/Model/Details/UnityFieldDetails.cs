using Improbable.CodeGeneration.Model;
using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityFieldDetails
    {
        public string Type;
        public string PascalCaseName;
        public string CamelCaseName;
        public FieldDefinitionRaw RawFieldDefinition;

        public UnityFieldDetails(FieldDefinitionRaw rawFieldDefinition)
        {
            PascalCaseName = Formatting.SnakeCaseToCapitalisedCamelCase(rawFieldDefinition.name);
            CamelCaseName = Formatting.SnakeCaseToCamelCase(rawFieldDefinition.name);
            var packagePrefix = UnityTypeMappings.PackagePrefix;
            RawFieldDefinition = rawFieldDefinition;

            if (rawFieldDefinition.IsOption())
            {
                var containedType = GetTypeFromTypeReference(rawFieldDefinition.optionType.valueType, packagePrefix);
                Type = string.Format("global::System.Nullable<{0}>", containedType);
            }
            else if (rawFieldDefinition.IsList())
            {
                var containedType = GetTypeFromTypeReference(rawFieldDefinition.listType.valueType, packagePrefix);
                Type = string.Format("global::System.Collections.Generic.List<{0}>", containedType);
            }
            else if (rawFieldDefinition.IsMap())
            {
                var containedKeyType = GetTypeFromTypeReference(rawFieldDefinition.mapType.keyType, packagePrefix);
                var containedValueType = GetTypeFromTypeReference(rawFieldDefinition.mapType.valueType, packagePrefix);
                Type = string.Format("global::System.Collections.Generic.Dictionary<{0}, {1}>", containedKeyType,
                    containedValueType);
            }
            else
            {
                Type = GetTypeFromTypeReference(rawFieldDefinition.singularType, packagePrefix);
            }
        }

        private static string GetTypeFromTypeReference(TypeReferenceRaw typeReference, string packagePrefix)
        {
            return typeReference.IsBuiltInType
                ? UnityTypeMappings.BuiltInSchemaTypeToUnityNativeType[typeReference.TypeName]
                : string.Format("global::{0}{1}", packagePrefix,
                    Formatting.CapitaliseQualifiedNameParts(typeReference.TypeName));
        }
    }
}
