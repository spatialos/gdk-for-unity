using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityCommandDetails
    {
        public string CommandName => Formatting.SnakeCaseToCapitalisedCamelCase(commandDefinition.Name);
        public string CamelCaseCommandName => Formatting.SnakeCaseToCamelCase(commandDefinition.Name);

        public string FqnRequestType =>
            CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinition.RequestType.typeDefinition.QualifiedName);

        public string FqnResponseType =>
            CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinition.ResponseType.typeDefinition.QualifiedName);

        public uint CommandIndex => commandDefinition.CommandIndex;

        private readonly UnityComponentDefinition.UnityCommandDefinition commandDefinition;

        public UnityCommandDetails(UnityComponentDefinition.UnityCommandDefinition commandDefinition)
        {
            this.commandDefinition = commandDefinition;
        }
    }
}
