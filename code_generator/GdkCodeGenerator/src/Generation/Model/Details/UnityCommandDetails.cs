using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityCommandDetails
    {
        public string CommandName;
        public string CamelCaseName;
        public string RequestType;
        public string ResponseType;
        public uint CommandIndex;

        public UnityCommandDetails(UnityComponentDefinition.UnityCommandDefinition commandDefinition)
        {
            CommandName = Formatting.SnakeCaseToCapitalisedCamelCase(commandDefinition.Name);
            CamelCaseName = Formatting.QualifiedNameToCapitalisedCamelCase(commandDefinition.Name);
            RequestType = "global::" + UnityTypeMappings.PackagePrefix +
                Formatting.CapitaliseQualifiedNameParts(commandDefinition.RequestType.typeDefinition.QualifiedName);
            ResponseType = "global::" + UnityTypeMappings.PackagePrefix +
                Formatting.CapitaliseQualifiedNameParts(commandDefinition.ResponseType.typeDefinition.QualifiedName);
            CommandIndex = commandDefinition.CommandIndex;
        }
    }
}
