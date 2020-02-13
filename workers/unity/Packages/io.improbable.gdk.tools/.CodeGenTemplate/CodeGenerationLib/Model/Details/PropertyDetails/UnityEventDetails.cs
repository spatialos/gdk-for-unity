using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEventDetails : Details
    {
        public readonly string FqnPayloadType;
        public readonly uint EventIndex;

        public UnityEventDetails(ComponentDefinition.EventDefinition rawEventDefinition)
            : base(rawEventDefinition, defaultCase: Case.PascalCase)
        {
            FqnPayloadType = DetailsUtils.GetCapitalisedFqnTypename(rawEventDefinition.Type);
            EventIndex = rawEventDefinition.EventIndex;
        }
    }
}
