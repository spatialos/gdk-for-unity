using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityCommandPayloadGenerator
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

        private List<UnityCommandDetails> GetCommandDetailsList()
        {
            return unityComponentDefinition.CommandDefinitions
                .Select(commandDefinition => new UnityCommandDetails(commandDefinition)).ToList();
        }
    }
}
