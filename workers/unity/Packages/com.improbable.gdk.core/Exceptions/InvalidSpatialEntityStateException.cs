using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents an error that occurs when the local state of a SpatialOS entity is not valid.
    /// </summary>
    internal class InvalidSpatialEntityStateException : Exception
    {
        public InvalidSpatialEntityStateException(string message) : base(message)
        {
        }
    }
}


