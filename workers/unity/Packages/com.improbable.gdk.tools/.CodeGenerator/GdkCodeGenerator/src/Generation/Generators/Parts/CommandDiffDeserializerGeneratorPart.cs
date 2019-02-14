using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class CommandDiffDeserializerGenerator
    {
        private string qualifiedNamespace;
        private string spatialNamespace;
        private UnityComponentDefinition unityComponentDefinition;
        private HashSet<string> enumSet;

        public string Generate(UnityComponentDefinition unityComponentDefinition, string package,
            HashSet<string> enumSet)
        {
            qualifiedNamespace = package;
            spatialNamespace = package;
            this.unityComponentDefinition = unityComponentDefinition;
            this.enumSet = enumSet;

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
