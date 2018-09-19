using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityTypeDetails
    {
        public string CapitalisedName;
        public string CamelCaseName;
        public string FullyQualifiedSpatialTypeName;
        public string FullyQualifiedTypeName;

        public UnityTypeDetails(UnityTypeDefinition typeDefinition)
        {
            CapitalisedName = Formatting.SnakeCaseToCapitalisedCamelCase(typeDefinition.Name);
            CamelCaseName = Formatting.SnakeCaseToCamelCase(typeDefinition.Name);
            FullyQualifiedSpatialTypeName =
                "global::" + Formatting.CapitaliseQualifiedNameParts(typeDefinition.QualifiedName);
            FullyQualifiedTypeName = "global::" + Formatting.CapitaliseQualifiedNameParts(typeDefinition.QualifiedName);
        }
    }
}
