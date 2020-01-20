// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.TestSchema
{
    [global::System.Serializable]
    public struct TypeB
    {
        public global::System.Collections.Generic.List<global::Improbable.TestSchema.TypeC> CList;
        public global::System.Collections.Generic.Dictionary<int, global::Improbable.TestSchema.TypeC> CMapValue;

        public TypeB(global::System.Collections.Generic.List<global::Improbable.TestSchema.TypeC> cList, global::System.Collections.Generic.Dictionary<int, global::Improbable.TestSchema.TypeC> cMapValue)
        {
            CList = cList;
            CMapValue = cMapValue;
        }

        public static class Serialization
        {
            public static void Serialize(TypeB instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var value in instance.CList)
                    {
                        global::Improbable.TestSchema.TypeC.Serialization.Serialize(value, obj.AddObject(2));
                    }
                }

                {
                    foreach (var keyValuePair in instance.CMapValue)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddInt32(1, keyValuePair.Key);
                        global::Improbable.TestSchema.TypeC.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                    }
                }
            }

            public static TypeB Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new TypeB();

                {
                    {
                        instance.CList = new global::System.Collections.Generic.List<global::Improbable.TestSchema.TypeC>();
                        var list = instance.CList;
                        var listLength = obj.GetObjectCount(2);

                        for (var i = 0; i < listLength; i++)
                        {
                            list.Add(global::Improbable.TestSchema.TypeC.Serialization.Deserialize(obj.IndexObject(2, (uint) i)));
                        }
                    }
                }

                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<int, global::Improbable.TestSchema.TypeC>();
                        var mapSize = obj.GetObjectCount(4);
                        instance.CMapValue = map;

                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(4, (uint) i);
                            var key = mapObj.GetInt32(1);
                            var value = global::Improbable.TestSchema.TypeC.Serialization.Deserialize(mapObj.GetObject(2));
                            map.Add(key, value);
                        }
                    }
                }

                return instance;
            }
        }
    }
}
