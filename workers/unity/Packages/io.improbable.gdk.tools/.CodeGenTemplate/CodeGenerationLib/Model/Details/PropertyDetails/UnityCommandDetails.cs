using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityCommandDetails : Details
    {
        public new string Name => PascalCaseName;

        public readonly string FqnRequestType;
        public readonly string FqnResponseType;

        public readonly uint CommandIndex;

        public UnityCommandDetails(ComponentDefinition.CommandDefinition rawCommandDefinition)
            : base(rawCommandDefinition)
        {
            FqnRequestType = DetailsUtils.GetCapitalisedFqnTypename(rawCommandDefinition.RequestType);
            FqnResponseType = DetailsUtils.GetCapitalisedFqnTypename(rawCommandDefinition.ResponseType);

            CommandIndex = rawCommandDefinition.CommandIndex;
        }
    }
}
