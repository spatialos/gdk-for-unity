using System;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Represents an error which occurs when no underlying ECS authority component is found on an entity when
    ///     the authority state is requested.
    /// </summary>
    public class AuthorityComponentNotFoundException : Exception
    {
        public AuthorityComponentNotFoundException(string message) : base(message)
        {
        }
    }
}
