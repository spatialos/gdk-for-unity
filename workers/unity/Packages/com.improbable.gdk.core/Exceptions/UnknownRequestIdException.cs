using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    internal class UnknownRequestIdException : Exception
    {
        public UnknownRequestIdException(string message) : base(message)
        {
        }
    }
}
