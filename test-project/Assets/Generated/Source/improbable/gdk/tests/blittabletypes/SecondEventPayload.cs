// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.BlittableTypes
{ 
    
    public struct SecondEventPayload
    {
        public float Field1;
        public double Field2;
    
        public SecondEventPayload(float field1, double field2)
        {
            Field1 = field1;
            Field2 = field2;
        }
    
        public static class Serialization
        {
            public static void Serialize(SecondEventPayload instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    obj.AddFloat(1, instance.Field1);
                }
                {
                    obj.AddDouble(2, instance.Field2);
                }
            }
    
            public static SecondEventPayload Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new SecondEventPayload();
                {
                    instance.Field1 = obj.GetFloat(1);
                }
                {
                    instance.Field2 = obj.GetDouble(2);
                }
                return instance;
            }
        }
    }
    
}
