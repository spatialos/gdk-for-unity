// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.DependentSchema
{
    
    [global::System.Serializable]
    public struct DependentType
    {
        public global::Improbable.TestSchema.ExhaustiveRepeatedData A;
        public global::Improbable.TestSchema.SomeEnum B;
        public global::Improbable.TestSchema.SomeEnum? C;
        public global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType> D;
        public global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum,global::Improbable.TestSchema.SomeType> E;
    
        public DependentType(global::Improbable.TestSchema.ExhaustiveRepeatedData a, global::Improbable.TestSchema.SomeEnum b, global::Improbable.TestSchema.SomeEnum? c, global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType> d, global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum,global::Improbable.TestSchema.SomeType> e)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
        }
        public static class Serialization
        {
            public static void Serialize(DependentType instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    global::Improbable.TestSchema.ExhaustiveRepeatedData.Serialization.Serialize(instance.A, obj.AddObject(1));
                }
                {
                    obj.AddEnum(2, (uint) instance.B);
                }
                {
                    if (instance.C.HasValue)
                    {
                        obj.AddEnum(3, (uint) instance.C.Value);
                    }
                    
                }
                {
                    foreach (var value in instance.D)
                    {
                        global::Improbable.TestSchema.SomeType.Serialization.Serialize(value, obj.AddObject(4));
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.E)
                    {
                        var mapObj = obj.AddObject(5);
                        mapObj.AddEnum(1, (uint) keyValuePair.Key);
                        global::Improbable.TestSchema.SomeType.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                    }
                    
                }
            }
    
            public static DependentType Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new DependentType();
                {
                    instance.A = global::Improbable.TestSchema.ExhaustiveRepeatedData.Serialization.Deserialize(obj.GetObject(1));
                }
                {
                    instance.B = (global::Improbable.TestSchema.SomeEnum) obj.GetEnum(2);
                }
                {
                    if (obj.GetEnumCount(3) == 1)
                    {
                        instance.C = new global::Improbable.TestSchema.SomeEnum?((global::Improbable.TestSchema.SomeEnum) obj.GetEnum(3));
                    }
                    
                }
                {
                    instance.D = new global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>();
                    var list = instance.D;
                    var listLength = obj.GetObjectCount(4);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(global::Improbable.TestSchema.SomeType.Serialization.Deserialize(obj.IndexObject(4, (uint) i)));
                    }
                    
                }
                {
                    instance.E = new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum,global::Improbable.TestSchema.SomeType>();
                    var map = instance.E;
                    var mapSize = obj.GetObjectCount(5);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = (global::Improbable.TestSchema.SomeEnum) mapObj.GetEnum(1);
                        var value = global::Improbable.TestSchema.SomeType.Serialization.Deserialize(mapObj.GetObject(2));
                        map.Add(key, value);
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
