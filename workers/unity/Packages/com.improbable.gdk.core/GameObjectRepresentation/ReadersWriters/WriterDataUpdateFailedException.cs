using System;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public class WriterDataUpdateFailedException : Exception
    {
        // entity index to be added at the end
        private const string FailureMessage = "Failed to update data for writer on entity with ECS entity index ";

        public WriterDataUpdateFailedException(Exception innerException, int entityIndex)
            : base(FailureMessage + entityIndex, innerException)
        {
        }
    }
}
