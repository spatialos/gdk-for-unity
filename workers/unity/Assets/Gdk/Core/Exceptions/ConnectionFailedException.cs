using System;

namespace Improbable.Gdk.Core
{
    public enum ConnectionErrorReason
    {
        CannotEstablishConnection,
        DeploymentNotFound,
        InvalidConfig
    }

    public class ConnectionFailedException : Exception
    {
        public ConnectionErrorReason Reason;

        public ConnectionFailedException(string message, ConnectionErrorReason reason) : base(message)
        {
            Reason = reason;
        }
    }
}
