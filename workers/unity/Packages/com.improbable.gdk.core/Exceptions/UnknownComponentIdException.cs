using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents an error that occurs when the SpatialOS GDK for Unity encounters a component ID it does not
    ///     know.
    /// </summary>
    internal class UnknownComponentIdException : Exception
    {
        public UnknownComponentIdException(string message) : base(message)
        {
        }
    }
}

