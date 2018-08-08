using System;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public class WriterDataUpdateFailedException : Exception
    {
        private const string FailureMessage = "Failed to update data for writer";

        public WriterDataUpdateFailedException(Exception innerException)
            : base(FailureMessage, innerException)
        {
        }
    }
}
