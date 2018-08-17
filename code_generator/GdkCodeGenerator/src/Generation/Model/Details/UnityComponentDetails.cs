using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityComponentDetails
    {
        public string ComponentName;
        public string TypeName;
        public string CamelCaseName;
        public int ComponentId;
        public bool IsBlittable;

        public UnityComponentDetails(UnityComponentDefinition componentDefinition)
        {
            ComponentName = Formatting.QualifiedNameToCapitalisedCamelCase(componentDefinition.Name);
            TypeName = "SpatialOS" + ComponentName;
            CamelCaseName = "spatialOS" + ComponentName;
            ComponentId = componentDefinition.Id;
            IsBlittable = componentDefinition.IsBlittable;
        }
    }
}
