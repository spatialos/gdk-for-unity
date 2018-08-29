using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityEventDetails
    {
        public string EventName;
        public string CamelCaseName;
        public string PascalCaseName;
        public string FullyQualifiedPayloadTypeName;
        public uint EventIndex;

        public UnityEventDetails(UnityComponentDefinition.UnityEventDefinition eventDefinition)
        {
            EventName = Formatting.SnakeCaseToCapitalisedCamelCase(eventDefinition.Name);
            CamelCaseName = Formatting.SnakeCaseToCamelCase(eventDefinition.Name);
            PascalCaseName = EventName;
            var payloadTypeName = eventDefinition.RawType.TypeName;
            FullyQualifiedPayloadTypeName = "global::" + UnityTypeMappings.PackagePrefix +
                Formatting.CapitaliseQualifiedNameParts(payloadTypeName);
            EventIndex = eventDefinition.EventIndex;
        }
    }
}
