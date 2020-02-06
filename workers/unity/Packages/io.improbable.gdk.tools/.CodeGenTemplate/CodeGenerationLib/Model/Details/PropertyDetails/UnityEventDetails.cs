using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEventDetails : Details
    {
        public new string Name => PascalCaseName;

        public readonly string FqnPayloadType;
        public readonly uint EventIndex;

        public UnityEventDetails(ComponentDefinition.EventDefinition rawEventDefinition)
            : base(rawEventDefinition)
        {
            FqnPayloadType = DetailsUtils.GetCapitalisedFqnTypename(rawEventDefinition.Type);
            EventIndex = rawEventDefinition.EventIndex;
        }
    }
}
