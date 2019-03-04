// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    
    [System.Serializable]
    public struct Empty
    {
        public static class Serialization
        {
            public static void Serialize(Empty instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
            }
    
            public static Empty Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new Empty();
                return instance;
            }
        }
    }
    
}
