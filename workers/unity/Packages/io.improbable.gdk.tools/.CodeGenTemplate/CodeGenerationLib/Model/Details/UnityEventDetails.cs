using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEventDetails
    {
        public string RawEventName { get; private set; }
        public string EventName { get; private set; }
        public string CamelCaseEventName { get; private set; }
        public string FqnPayloadType { get; }
        public uint EventIndex { get; }

        public UnityEventDetails(ComponentDefinition.EventDefinition eventDefinitionRaw)
        {
            SetNames(eventDefinitionRaw.Name);

            FqnPayloadType = CommonDetailsUtils.GetCapitalisedFqnTypename(eventDefinitionRaw.Type);

            EventIndex = eventDefinitionRaw.EventIndex;
        }

        public void ResolveClash()
        {
            SetNames(RawEventName + "_event");
        }

        private void SetNames(string rawEventName)
        {
            RawEventName = rawEventName;
            EventName = Formatting.SnakeCaseToPascalCase(RawEventName);
            CamelCaseEventName = Formatting.PascalCaseToCamelCase(EventName);
        }
    }
}
