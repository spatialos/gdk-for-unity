using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents an error that occurs when the local state of a SpatialOS entity is not valid.
    /// </summary>
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(string message) : base(message)
        {
        }
    }
}
