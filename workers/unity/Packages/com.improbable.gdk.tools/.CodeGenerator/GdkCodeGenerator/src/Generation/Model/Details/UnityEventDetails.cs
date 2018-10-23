using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityEventDetails
    {
        public string EventName => Formatting.SnakeCaseToCapitalisedCamelCase(eventDefinition.Name);
        public string CamelCaseEventName => Formatting.SnakeCaseToCamelCase(eventDefinition.Name);
        public string FqnPayloadType => CommonDetailsUtils.GetCapitalisedFqnTypename(eventDefinition.RawType.TypeName);
        public uint EventIndex => eventDefinition.EventIndex;

        private readonly UnityComponentDefinition.UnityEventDefinition eventDefinition;
        
        public UnityEventDetails(UnityComponentDefinition.UnityEventDefinition eventDefinition)
        {
            this.eventDefinition = eventDefinition;
        }
    }
}
