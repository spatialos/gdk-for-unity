// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    
    [global::System.Serializable]
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
            public static void Serialize(SecondEventPayload instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddFloat(1, instance.Field1);
                }
                {
                    foreach (var value in instance.Field2)
                    {
                        obj.AddDouble(2, value);
                    }
                    
                }
            }
    
            public static SecondEventPayload Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new SecondEventPayload();
                {
                    instance.Field1 = obj.GetFloat(1);
                }
                {
                    instance.Field2 = new global::System.Collections.Generic.List<double>();
                    var list = instance.Field2;
                    var listLength = obj.GetDoubleCount(2);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexDouble(2, (uint) i));
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
