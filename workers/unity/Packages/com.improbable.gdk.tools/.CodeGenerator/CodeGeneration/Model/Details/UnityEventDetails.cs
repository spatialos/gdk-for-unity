using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEventDetails
    {
        public readonly string PascalCaseName;
        public readonly string CamelCaseName;
        public readonly string FullyQualifiedPayloadTypeName;

        public readonly uint EventIndex;

        public UnityEventDetails(ComponentDefinitionRaw.EventDefinitionRaw eventDefinitionRaw, SchemaBundle.Bundle bundle)
        {
            (PascalCaseName, CamelCaseName, _) = eventDefinitionRaw.Identifier.GetNameSet();
            FullyQualifiedPayloadTypeName = Formatting.FullyQualify(eventDefinitionRaw.Type.Type.QualifiedName);
            EventIndex = eventDefinitionRaw.EventIndex;
        }
    }
}
