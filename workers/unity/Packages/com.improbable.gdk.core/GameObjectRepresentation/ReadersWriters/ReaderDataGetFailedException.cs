using System;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public class ReaderDataGetFailedException : Exception
    {
        // entity index to be added at the end
        private const string FailureMessage = "Failed to retrieve data for reader on entity with ECS entity index ";

        public ReaderDataGetFailedException(Exception innerException, int entityIndex)
            : base(FailureMessage + entityIndex, innerException)
        {
        }
    }
}
