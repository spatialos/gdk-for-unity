using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents an error that occurs when the player fails to authenticate via the
    ///     anonymous authentication flow.
    /// </summary>
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(string message) : base(message)
        {
        }
    }
}
