using System;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Represents an error that occurs when a writer could not send a component update because the underlying data
    ///     did not exist.
    /// </summary>
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
