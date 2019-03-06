// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    
    [System.Serializable]
    public struct FirstEventPayload
    {
        public BlittableBool Field1;
        public int Field2;
    
        public FirstEventPayload(BlittableBool field1, int field2)
        {
            Field1 = field1;
            Field2 = field2;
        }
        public static class Serialization
        {
            public static void Serialize(FirstEventPayload instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, instance.Field1);
                }
                {
                    obj.AddInt32(2, instance.Field2);
                }
            }
    
            public static FirstEventPayload Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new FirstEventPayload();
                {
                    instance.Field1 = obj.GetBool(1);
                }
                {
                    instance.Field2 = obj.GetInt32(2);
                }
                return instance;
            }
        }
    }
    
}
