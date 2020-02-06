using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEventDetails
    {
        public readonly string RawEventName;
        public readonly string EventName;
        public readonly string CamelCaseEventName;

        public readonly string FqnPayloadType;

        public readonly uint EventIndex;

        public UnityEventDetails(ComponentDefinition.EventDefinition rawEventDefinition)
        {
            RawEventName = rawEventDefinition.Name;
            EventName = Formatting.SnakeCaseToPascalCase(RawEventName);
            CamelCaseEventName = Formatting.PascalCaseToCamelCase(EventName);

            FqnPayloadType = CommonDetailsUtils.GetCapitalisedFqnTypename(rawEventDefinition.Type);

            EventIndex = rawEventDefinition.EventIndex;
        }
    }
}
