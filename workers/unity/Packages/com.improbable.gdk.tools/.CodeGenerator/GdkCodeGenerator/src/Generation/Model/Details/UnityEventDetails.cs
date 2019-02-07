using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityEventDetails
    {
        public string EventName { get; }
        public string CamelCaseEventName { get; }
        public string FqnPayloadType { get; }
        public uint EventIndex { get; }

        public UnityEventDetails(ComponentDefinitionRaw.EventDefinitionRaw eventDefinitionRaw)
        {
            EventName = Formatting.SnakeCaseToPascalCase(eventDefinitionRaw.Identifier.Name);
            CamelCaseEventName = Formatting.PascalCaseToCamelCase(EventName);
            FqnPayloadType = CommonDetailsUtils.GetCapitalisedFqnTypename(eventDefinitionRaw.Type.Type.QualifiedName);
            EventIndex = eventDefinitionRaw.EventIndex;
        }
    }
}
