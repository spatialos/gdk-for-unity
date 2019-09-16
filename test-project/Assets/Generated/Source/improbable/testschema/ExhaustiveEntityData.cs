// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.TestSchema
{
    
    [global::System.Serializable]
    public struct ExhaustiveEntityData
    {
        public global::Improbable.Gdk.Core.EntitySnapshot Field1;
        public global::Improbable.Gdk.Core.EntitySnapshot? Field2;
        public global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot> Field3;
        public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string> Field4;
        public global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot> Field5;
    
        public ExhaustiveEntityData(global::Improbable.Gdk.Core.EntitySnapshot field1, global::Improbable.Gdk.Core.EntitySnapshot? field2, global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot> field3, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string> field4, global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot> field5)
        {
            Field1 = field1;
            Field2 = field2;
            Field3 = field3;
            Field4 = field4;
            Field5 = field5;
        }
        public static class Serialization
        {
            public static void Serialize(ExhaustiveEntityData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddEntity(1, instance.Field1);
                }
                {
                    if (instance.Field2.HasValue)
                    {
                        obj.AddEntity(2, instance.Field2.Value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field3)
                    {
                        obj.AddEntity(3, value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field4)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddEntity(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field5)
                    {
                        var mapObj = obj.AddObject(5);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddEntity(2, keyValuePair.Value);
                    }
                    
                }
            }
    
            public static ExhaustiveEntityData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new ExhaustiveEntityData();
                {
                    instance.Field1 = obj.GetEntity(1);
                }
                {
                    if (obj.GetEntityCount(2) == 1)
                    {
                        instance.Field2 = new global::Improbable.Gdk.Core.EntitySnapshot?(obj.GetEntity(2));
                    }
                    
                }
                {
                    {
                        instance.Field3 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>();
                        var list = instance.Field3;
                        var listLength = obj.GetEntityCount(3);
                        for (var i = 0; i < listLength; i++)
                        {
                            list.Add(obj.IndexEntity(3, (uint) i));
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>();
                        var mapSize = obj.GetObjectCount(4);
                        instance.Field4 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(4, (uint) i);
                            var key = mapObj.GetEntity(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>();
                        var mapSize = obj.GetObjectCount(5);
                        instance.Field5 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(5, (uint) i);
                            var key = mapObj.GetString(1);
                            var value = mapObj.GetEntity(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
