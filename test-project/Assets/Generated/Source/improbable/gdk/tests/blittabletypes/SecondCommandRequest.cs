// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    
    [System.Serializable]
    public struct SecondCommandRequest
    {
        public long Field;
    
        public SecondCommandRequest(long field)
        {
            Field = field;
        }
        public static class Serialization
        {
            public static void Serialize(SecondCommandRequest instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddInt64(1, instance.Field);
                }
            }
    
            public static SecondCommandRequest Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new SecondCommandRequest();
                {
                    instance.Field = obj.GetInt64(1);
                }
                return instance;
            }
        }
    }
    
}
