using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Describes why the connection failed.
    /// </summary>
    public enum ConnectionErrorReason
    {
        CannotEstablishConnection,
        DeploymentNotFound,
        InvalidConfig,
        ExceededMaximumRetries,
        EditorApplicationStopped
    }

    /// <summary>
    ///     Represents an error that occurs when a connection attempt failed.
    /// </summary>
    public class ConnectionFailedException : Exception
    {
        public ConnectionErrorReason Reason;

        public ConnectionFailedException(string message, ConnectionErrorReason reason) : base(message)
        {
            Reason = reason;
        }
    }
}
