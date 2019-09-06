using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEventDetails
    {
        public string EventName { get; }
        public string CamelCaseEventName { get; }
        public string FqnPayloadType { get; }
        public uint EventIndex { get; }

        public UnityEventDetails(ComponentDefinition.EventDefinition eventDefinitionRaw)
        {
            EventName = Formatting.SnakeCaseToPascalCase(eventDefinitionRaw.Name);
            CamelCaseEventName = Formatting.PascalCaseToCamelCase(EventName);
            FqnPayloadType = CommonDetailsUtils.GetCapitalisedFqnTypename(eventDefinitionRaw.Type);
            EventIndex = eventDefinitionRaw.EventIndex;
        }
    }
}
