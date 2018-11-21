using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityCommandDetails
    {
        public readonly string PascalCaseName;
        public readonly string CamelCaseName;

        public readonly string FullyQualifiedRequestTypeName;
        public readonly string FullyQualifiedResponseTypeName;

        public readonly uint CommandIndex;

        public UnityCommandDetails(ComponentDefinitionRaw.CommandDefinitionRaw commandRaw, SchemaBundle.Bundle bundle)
        {
            (PascalCaseName, CamelCaseName, _) = commandRaw.Identifier.GetNameSet();
            FullyQualifiedRequestTypeName = Formatting.FullyQualify(commandRaw.RequestType.Type.QualifiedName);
            FullyQualifiedResponseTypeName = Formatting.FullyQualify(commandRaw.ResponseType.Type.QualifiedName);
            CommandIndex = commandRaw.CommandIndex;
        }
    }
}
