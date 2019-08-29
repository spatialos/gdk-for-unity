// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.TestSchema
{
    
    [global::System.Serializable]
    public struct TypeC
    {
        public global::System.Collections.Generic.List<global::Improbable.TestSchema.TypeB> BList;
        public global::System.Collections.Generic.Dictionary<string,global::Improbable.TestSchema.TypeB> BMap;
    
        public TypeC(global::System.Collections.Generic.List<global::Improbable.TestSchema.TypeB> bList, global::System.Collections.Generic.Dictionary<string,global::Improbable.TestSchema.TypeB> bMap)
        {
            BList = bList;
            BMap = bMap;
        }
        public static class Serialization
        {
            public static void Serialize(TypeC instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var value in instance.BList)
                    {
                        global::Improbable.TestSchema.TypeB.Serialization.Serialize(value, obj.AddObject(2));
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.BMap)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddString(1, keyValuePair.Key);
                        global::Improbable.TestSchema.TypeB.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                    }
                    
                }
            }
    
            public static TypeC Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new TypeC();
                {
                    instance.BList = new global::System.Collections.Generic.List<global::Improbable.TestSchema.TypeB>();
                    var list = instance.BList;
                    var listLength = obj.GetObjectCount(2);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(global::Improbable.TestSchema.TypeB.Serialization.Deserialize(obj.IndexObject(2, (uint) i)));
                    }
                    
                }
                {
                    instance.BMap = new global::System.Collections.Generic.Dictionary<string,global::Improbable.TestSchema.TypeB>();
                    var map = instance.BMap;
                    var mapSize = obj.GetObjectCount(4);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = global::Improbable.TestSchema.TypeB.Serialization.Deserialize(mapObj.GetObject(2));
                        map.Add(key, value);
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
