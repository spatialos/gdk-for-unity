using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityCommandDetails : Details
    {
        public readonly string FqnRequestType;
        public readonly string FqnResponseType;

        public readonly uint CommandIndex;

        public UnityCommandDetails(ComponentDefinition.CommandDefinition rawCommandDefinition)
            : base(rawCommandDefinition.Name)
        {
            FqnRequestType = Formatting.CapitaliseQualifiedNameParts(rawCommandDefinition.RequestType);
            FqnResponseType = Formatting.CapitaliseQualifiedNameParts(rawCommandDefinition.ResponseType);

            CommandIndex = rawCommandDefinition.CommandIndex;
        }
    }
}
