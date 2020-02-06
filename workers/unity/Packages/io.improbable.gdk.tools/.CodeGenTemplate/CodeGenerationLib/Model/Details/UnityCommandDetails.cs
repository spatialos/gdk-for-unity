using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityCommandDetails
    {
        public readonly string RawCommandName;
        public readonly string CommandName;
        public readonly string CamelCaseCommandName;

        public readonly string FqnRequestType;
        public readonly string FqnResponseType;

        public readonly uint CommandIndex;

        public UnityCommandDetails(ComponentDefinition.CommandDefinition rawCommandDefinition)
        {
            RawCommandName = rawCommandDefinition.Name;
            CommandName = Formatting.SnakeCaseToPascalCase(RawCommandName);
            CamelCaseCommandName = Formatting.PascalCaseToCamelCase(CommandName);

            FqnRequestType = CommonDetailsUtils.GetCapitalisedFqnTypename(rawCommandDefinition.RequestType);
            FqnResponseType = CommonDetailsUtils.GetCapitalisedFqnTypename(rawCommandDefinition.ResponseType);

            CommandIndex = rawCommandDefinition.CommandIndex;
        }
    }
}
