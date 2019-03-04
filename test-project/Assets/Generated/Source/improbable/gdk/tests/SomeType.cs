// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests
{
    
    [System.Serializable]
    public struct SomeType
    {
        public static class Serialization
        {
            public static void Serialize(SomeType instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
            }
    
            public static SomeType Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new SomeType();
                return instance;
            }
        }
    }
    
}
