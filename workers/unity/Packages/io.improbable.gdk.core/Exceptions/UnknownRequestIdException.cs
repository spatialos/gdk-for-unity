using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents an error that occurs when the SpatialOS GDK for Unity encounters a request id in a command
    ///     that it does not know.
    /// </summary>
    internal class UnknownRequestIdException : Exception
    {
        public UnknownRequestIdException(string message) : base(message)
        {
        }
    }
}
