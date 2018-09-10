using System;

namespace Improbable.Gdk.GameObjectRepresentation
{
    public class AuthorityComponentNotFoundException : Exception
    {
        public AuthorityComponentNotFoundException(string message) : base(message)
        {
        }
    }
}
