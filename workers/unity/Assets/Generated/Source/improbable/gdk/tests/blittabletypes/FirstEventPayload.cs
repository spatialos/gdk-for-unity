// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{ 
    
    public struct FirstEventPayload
    {
        public BlittableBool Field1;
        public int Field2;
    
        public static class Serialization
        {
            public static void Serialize(FirstEventPayload instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                obj.AddBool(1, instance.Field1);
                obj.AddInt32(2, instance.Field2);
            }
    
            public static FirstEventPayload Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new FirstEventPayload();
                instance.Field1 = obj.GetBool(1);
                instance.Field2 = obj.GetInt32(2);
                return instance;
            }
        }
    }
    
}
