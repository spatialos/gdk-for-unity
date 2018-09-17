using System;

namespace Improbable.Gdk.Core
{
    internal class UnknownComponentIdException : Exception
    {
        public UnknownComponentIdException(string message) : base(message)
        {
        }
    }
}

