using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEventGenerator
    {
        private string qualifiedNamespace;
        private UnityComponentDefinition unityComponentDefinition;

        public string Generate(UnityComponentDefinition unityComponentDefinition, string package)
        {
            qualifiedNamespace = package;
            this.unityComponentDefinition = unityComponentDefinition;

            return TransformText();
        }

        private UnityComponentDetails GetComponentDetails()
        {
            return new UnityComponentDetails(unityComponentDefinition);
        }

        private List<UnityEventDetails> GetEventDetailsList()
        {
            return unityComponentDefinition.EventDefinitions
                .Select(eventDefinition => new UnityEventDetails(eventDefinition)).ToList();
        }
    }
}
