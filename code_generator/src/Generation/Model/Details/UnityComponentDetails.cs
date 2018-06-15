using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityComponentDetails
    {
        public string ComponentName;
        public string TypeName;
        public string CamelCaseTypeName;
        public string FullyQualifiedSpatialTypeName;

        public UnityComponentDetails(UnityComponentDefinition componentDefinition)
        {
            ComponentName = Formatting.QualifiedNameToCapitalisedCamelCase(componentDefinition.Name);
            TypeName = "SpatialOS" + ComponentName;
            CamelCaseTypeName = "spatialOS" + ComponentName;
            FullyQualifiedSpatialTypeName =
                "global::" + Formatting.CapitaliseQualifiedNameParts(componentDefinition.QualifiedName);
        }
    }
}
