using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityComponentDetails
    {
        public string ComponentName => Formatting.QualifiedNameToCapitalisedCamelCase(componentDefinition.Name);
        public int ComponentId => componentDefinition.Id;
        public bool IsBlittable => componentDefinition.IsBlittable;

        private UnityComponentDefinition componentDefinition;

        public UnityComponentDetails(UnityComponentDefinition componentDefinition)
        {
            this.componentDefinition = componentDefinition;
        }
    }
}
