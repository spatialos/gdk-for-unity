using Improbable.CodeGeneration.Model;
using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityEnumDetails
    {
        public string PascalCaseName;
        public string CamelCaseName;
        public string FullyQualifiedSpatialTypeName;
        public string FullyQualifiedTypeName;

        public UnityEnumDetails(EnumDefinitionRaw enumDefinition)
        {
            PascalCaseName = Formatting.SnakeCaseToCapitalisedCamelCase(enumDefinition.name);
            CamelCaseName = Formatting.SnakeCaseToCamelCase(enumDefinition.name);
            FullyQualifiedSpatialTypeName =
                "global::" + Formatting.CapitaliseQualifiedNameParts(enumDefinition.qualifiedName);
            FullyQualifiedTypeName = "global::" + UnityTypeMappings.PackagePrefix +
                Formatting.CapitaliseQualifiedNameParts(enumDefinition.qualifiedName);
        }
    }
}
