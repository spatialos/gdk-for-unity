// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.TestSchema
{
    [global::System.Serializable]
    public struct TypeA
    {
        public global::System.Collections.Generic.List<global::Improbable.TestSchema.TypeA> AList;
        public global::System.Collections.Generic.Dictionary<int, global::Improbable.TestSchema.TypeA> AMapValue;
    
        public TypeA(global::System.Collections.Generic.List<global::Improbable.TestSchema.TypeA> aList, global::System.Collections.Generic.Dictionary<int, global::Improbable.TestSchema.TypeA> aMapValue)
        {
            AList = aList;
            AMapValue = aMapValue;
        }
    
        public static class Serialization
        {
            public static void Serialize(TypeA instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var value in instance.AList)
                    {
                        global::Improbable.TestSchema.TypeA.Serialization.Serialize(value, obj.AddObject(2));
                    }
                }
                {
                    foreach (var keyValuePair in instance.AMapValue)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddInt32(1, keyValuePair.Key);
                        global::Improbable.TestSchema.TypeA.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                    }
                }
            }
    
            public static TypeA Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new TypeA();
                {
                    {
                        instance.AList = new global::System.Collections.Generic.List<global::Improbable.TestSchema.TypeA>();
                        var list = instance.AList;
                        var listLength = obj.GetObjectCount(2);
                    
                        for (var i = 0; i < listLength; i++)
                        {
                            list.Add(global::Improbable.TestSchema.TypeA.Serialization.Deserialize(obj.IndexObject(2, (uint) i)));
                        }
                    }
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<int, global::Improbable.TestSchema.TypeA>();
                        var mapSize = obj.GetObjectCount(4);
                        instance.AMapValue = map;
                    
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(4, (uint) i);
                            var key = mapObj.GetInt32(1);
                            var value = global::Improbable.TestSchema.TypeA.Serialization.Deserialize(mapObj.GetObject(2));
                            map.Add(key, value);
                        }
                    }
                }
                return instance;
            }
        }
    }
    
}
