using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityCommandDetails
    {
        public string CommandName { get; }
        public string CamelCaseCommandName { get; }

        public string FqnRequestType { get; }
        public string FqnResponseType { get; }
        public string RawCommandName { get; }

        public uint CommandIndex { get; }

        public UnityCommandDetails(ComponentDefinition.CommandDefinition commandDefinitionRaw)
        {
            CommandName = Formatting.SnakeCaseToPascalCase(commandDefinitionRaw.Name);
            CamelCaseCommandName = Formatting.PascalCaseToCamelCase(CommandName);
            FqnRequestType =
                CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinitionRaw.RequestType);
            FqnResponseType =
                CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinitionRaw.ResponseType);

            RawCommandName = commandDefinitionRaw.Name;
            CommandIndex = commandDefinitionRaw.CommandIndex;
        }
    }
}
