using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityEventDetails
    {
        public string EventName;
        public string CamelCaseTypeName;
        public string FullyQualifiedPayloadTypeName;

        public UnityEventDetails(UnityComponentDefinition.UnityEventDefinition eventDefinition)
        {
            EventName = Formatting.SnakeCaseToCapitalisedCamelCase(eventDefinition.Name);
            CamelCaseTypeName = Formatting.SnakeCaseToCamelCase(eventDefinition.Name);
            var payloadTypeName = eventDefinition.RawType.TypeName;
            FullyQualifiedPayloadTypeName = "global::" + UnityTypeMappings.PackagePrefix +
                Formatting.CapitaliseQualifiedNameParts(payloadTypeName);
        }
    }
}
