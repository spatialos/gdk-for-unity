// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{ 
    
    public struct SecondEventPayload
    {
        public float Field1;
        public global::System.Collections.Generic.List<double> Field2;
    
        public SecondEventPayload(float field1, global::System.Collections.Generic.List<double> field2)
        {
            Field1 = field1;
            Field2 = field2;
        }
    
        public static class Serialization
        {
            public static void Serialize(SecondEventPayload instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                obj.AddFloat(1, instance.Field1);
                foreach (var value in instance.Field2)
                {
                    obj.AddDouble(2, value);
                }
            }
    
            public static SecondEventPayload Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new SecondEventPayload();
                instance.Field1 = obj.GetFloat(1);
                var field2 = instance.Field2 = new global::System.Collections.Generic.List<double>();
                for (var i = 0; i < obj.GetDoubleCount(2); i++)
                {
                    field2.Add(obj.IndexDouble(2, (uint) i));
                }
    
                return instance;
            }
        }
    }
    
}
