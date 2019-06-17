using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityCommandDetails
    {
        public string CommandName { get; }
        public string CamelCaseCommandName { get; }

        public string FqnRequestType { get; }
        public string FqnResponseType { get; }

        public uint CommandIndex { get; }

        public UnityCommandDetails(ComponentDefinitionRaw.CommandDefinitionRaw commandDefinitionRaw)
        {
            CommandName = Formatting.SnakeCaseToPascalCase(commandDefinitionRaw.Identifier.Name);
            CamelCaseCommandName = Formatting.PascalCaseToCamelCase(CommandName);
            FqnRequestType =
                CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinitionRaw.RequestType.Type.QualifiedName);
            FqnResponseType =
                CommonDetailsUtils.GetCapitalisedFqnTypename(commandDefinitionRaw.ResponseType.Type.QualifiedName);

            CommandIndex = commandDefinitionRaw.CommandIndex;
        }
    }
}
