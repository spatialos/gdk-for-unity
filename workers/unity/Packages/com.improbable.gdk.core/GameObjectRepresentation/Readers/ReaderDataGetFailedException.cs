using System;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public class ReaderDataGetFailedException : Exception
    {
        private const string FailureMessage = "Failed to retrieve data for reader";

        public ReaderDataGetFailedException(Exception innerException)
            : base(FailureMessage, innerException)
        {
        }
    }
}