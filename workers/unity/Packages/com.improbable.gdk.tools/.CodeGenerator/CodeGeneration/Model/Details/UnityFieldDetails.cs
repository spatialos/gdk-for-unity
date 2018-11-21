using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityFieldDetails
    {
        public readonly string PascalCaseName;
        public readonly string CamelCaseName;
        public readonly uint FieldId;

        public UnityFieldDetails(TypeDefinitionRaw.Field fieldRaw, SchemaBundle.Bundle bundle)
        {
            (PascalCaseName, CamelCaseName, _) = fieldRaw.Identifier.GetNameSet();
            FieldId = fieldRaw.FieldId;
        }
    }
}
