using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityCommandDetails
    {
        public string RawCommandName { get; private set; }
        public string CommandName { get; private set; }
        public string CamelCaseCommandName { get; private set; }

        public string FqnRequestType { get; }
        public string FqnResponseType { get; }

        public uint CommandIndex { get; }

        public UnityCommandDetails(ComponentDefinition.CommandDefinition commandDefinitionRaw)
        {
            SetNames(commandDefinitionRaw.Name);

            FqnRequestType =
                CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinitionRaw.RequestType);
            FqnResponseType =
                CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinitionRaw.ResponseType);

            CommandIndex = commandDefinitionRaw.CommandIndex;
        }

        public void ResolveClash()
        {
            SetNames(RawCommandName + "_command");
        }

        private void SetNames(string rawCommandName)
        {
            RawCommandName = rawCommandName;
            CommandName = Formatting.SnakeCaseToPascalCase(RawCommandName);
            CamelCaseCommandName = Formatting.PascalCaseToCamelCase(CommandName);
        }
    }
}
