using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityCommandDetails
    {
        public string RawCommandName { get; }
        public string CommandName { get; }
        public string CamelCaseCommandName { get; }

        public string FqnRequestType { get; }
        public string FqnResponseType { get; }

        public uint CommandIndex { get; }

        public UnityCommandDetails(ComponentDefinition.CommandDefinition commandDefinitionRaw)
        {
            RawCommandName = commandDefinitionRaw.Name;
            CommandName = Formatting.SnakeCaseToPascalCase(RawCommandName);
            CamelCaseCommandName = Formatting.PascalCaseToCamelCase(CommandName);

            FqnRequestType =
                CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinitionRaw.RequestType);
            FqnResponseType =
                CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinitionRaw.ResponseType);

            CommandIndex = commandDefinitionRaw.CommandIndex;
        }
    }
}
