using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityCommandSendingSystemGenerator
    {
        private const string Namespace = "";
        private List<CommandGroupInfo> commandGroupInfos;

        public string Generate(List<CommandGroupInfo> commandGroupInfos)
        {
            this.commandGroupInfos = commandGroupInfos;

            return TransformText();
        }
    }

    public struct CommandGroupInfo
    {
        public string QualifiedNamespace;
        public List<UnityComponentDefinition.UnityCommandDefinition> CommandDefinitions;

        public List<UnityCommandDetails> GetDetails()
        {
            return CommandDefinitions.Select(commandDefinition => new UnityCommandDetails(commandDefinition)).ToList();
        }
    }
}