// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.TestSchema
{
    
    [global::System.Serializable]
    public struct ExhaustiveMapKeyData
    {
        public global::System.Collections.Generic.Dictionary<bool, string> Field1;
        public global::System.Collections.Generic.Dictionary<float, string> Field2;
        public global::System.Collections.Generic.Dictionary<byte[], string> Field3;
        public global::System.Collections.Generic.Dictionary<int, string> Field4;
        public global::System.Collections.Generic.Dictionary<long, string> Field5;
        public global::System.Collections.Generic.Dictionary<double, string> Field6;
        public global::System.Collections.Generic.Dictionary<string, string> Field7;
        public global::System.Collections.Generic.Dictionary<uint, string> Field8;
        public global::System.Collections.Generic.Dictionary<ulong, string> Field9;
        public global::System.Collections.Generic.Dictionary<int, string> Field10;
        public global::System.Collections.Generic.Dictionary<long, string> Field11;
        public global::System.Collections.Generic.Dictionary<uint, string> Field12;
        public global::System.Collections.Generic.Dictionary<ulong, string> Field13;
        public global::System.Collections.Generic.Dictionary<int, string> Field14;
        public global::System.Collections.Generic.Dictionary<long, string> Field15;
        public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string> Field16;
        public global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string> Field17;
        public global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string> Field18;
    
        public ExhaustiveMapKeyData(global::System.Collections.Generic.Dictionary<bool, string> field1, global::System.Collections.Generic.Dictionary<float, string> field2, global::System.Collections.Generic.Dictionary<byte[], string> field3, global::System.Collections.Generic.Dictionary<int, string> field4, global::System.Collections.Generic.Dictionary<long, string> field5, global::System.Collections.Generic.Dictionary<double, string> field6, global::System.Collections.Generic.Dictionary<string, string> field7, global::System.Collections.Generic.Dictionary<uint, string> field8, global::System.Collections.Generic.Dictionary<ulong, string> field9, global::System.Collections.Generic.Dictionary<int, string> field10, global::System.Collections.Generic.Dictionary<long, string> field11, global::System.Collections.Generic.Dictionary<uint, string> field12, global::System.Collections.Generic.Dictionary<ulong, string> field13, global::System.Collections.Generic.Dictionary<int, string> field14, global::System.Collections.Generic.Dictionary<long, string> field15, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string> field16, global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string> field17, global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string> field18)
        {
            Field1 = field1;
            Field2 = field2;
            Field3 = field3;
            Field4 = field4;
            Field5 = field5;
            Field6 = field6;
            Field7 = field7;
            Field8 = field8;
            Field9 = field9;
            Field10 = field10;
            Field11 = field11;
            Field12 = field12;
            Field13 = field13;
            Field14 = field14;
            Field15 = field15;
            Field16 = field16;
            Field17 = field17;
            Field18 = field18;
        }
        public static class Serialization
        {
            public static void Serialize(ExhaustiveMapKeyData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var keyValuePair in instance.Field1)
                    {
                        var mapObj = obj.AddObject(1);
                        mapObj.AddBool(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field2)
                    {
                        var mapObj = obj.AddObject(2);
                        mapObj.AddFloat(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field3)
                    {
                        var mapObj = obj.AddObject(3);
                        mapObj.AddBytes(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field4)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddInt32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field5)
                    {
                        var mapObj = obj.AddObject(5);
                        mapObj.AddInt64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field6)
                    {
                        var mapObj = obj.AddObject(6);
                        mapObj.AddDouble(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field7)
                    {
                        var mapObj = obj.AddObject(7);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field8)
                    {
                        var mapObj = obj.AddObject(8);
                        mapObj.AddUint32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field9)
                    {
                        var mapObj = obj.AddObject(9);
                        mapObj.AddUint64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field10)
                    {
                        var mapObj = obj.AddObject(10);
                        mapObj.AddSint32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field11)
                    {
                        var mapObj = obj.AddObject(11);
                        mapObj.AddSint64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field12)
                    {
                        var mapObj = obj.AddObject(12);
                        mapObj.AddFixed32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field13)
                    {
                        var mapObj = obj.AddObject(13);
                        mapObj.AddFixed64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field14)
                    {
                        var mapObj = obj.AddObject(14);
                        mapObj.AddSfixed32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field15)
                    {
                        var mapObj = obj.AddObject(15);
                        mapObj.AddSfixed64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field16)
                    {
                        var mapObj = obj.AddObject(16);
                        mapObj.AddEntityId(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field17)
                    {
                        var mapObj = obj.AddObject(17);
                        global::Improbable.TestSchema.SomeType.Serialization.Serialize(keyValuePair.Key, mapObj.AddObject(1));
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field18)
                    {
                        var mapObj = obj.AddObject(18);
                        mapObj.AddEnum(1, (uint) keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
            }
    
            public static ExhaustiveMapKeyData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new ExhaustiveMapKeyData();
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<bool, string>();
                        var mapSize = obj.GetObjectCount(1);
                        instance.Field1 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(1, (uint) i);
                            var key = mapObj.GetBool(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<float, string>();
                        var mapSize = obj.GetObjectCount(2);
                        instance.Field2 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(2, (uint) i);
                            var key = mapObj.GetFloat(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<byte[], string>();
                        var mapSize = obj.GetObjectCount(3);
                        instance.Field3 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(3, (uint) i);
                            var key = mapObj.GetBytes(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<int, string>();
                        var mapSize = obj.GetObjectCount(4);
                        instance.Field4 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(4, (uint) i);
                            var key = mapObj.GetInt32(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<long, string>();
                        var mapSize = obj.GetObjectCount(5);
                        instance.Field5 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(5, (uint) i);
                            var key = mapObj.GetInt64(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<double, string>();
                        var mapSize = obj.GetObjectCount(6);
                        instance.Field6 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(6, (uint) i);
                            var key = mapObj.GetDouble(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<string, string>();
                        var mapSize = obj.GetObjectCount(7);
                        instance.Field7 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(7, (uint) i);
                            var key = mapObj.GetString(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<uint, string>();
                        var mapSize = obj.GetObjectCount(8);
                        instance.Field8 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(8, (uint) i);
                            var key = mapObj.GetUint32(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<ulong, string>();
                        var mapSize = obj.GetObjectCount(9);
                        instance.Field9 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(9, (uint) i);
                            var key = mapObj.GetUint64(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<int, string>();
                        var mapSize = obj.GetObjectCount(10);
                        instance.Field10 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(10, (uint) i);
                            var key = mapObj.GetSint32(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<long, string>();
                        var mapSize = obj.GetObjectCount(11);
                        instance.Field11 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(11, (uint) i);
                            var key = mapObj.GetSint64(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<uint, string>();
                        var mapSize = obj.GetObjectCount(12);
                        instance.Field12 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(12, (uint) i);
                            var key = mapObj.GetFixed32(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<ulong, string>();
                        var mapSize = obj.GetObjectCount(13);
                        instance.Field13 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(13, (uint) i);
                            var key = mapObj.GetFixed64(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<int, string>();
                        var mapSize = obj.GetObjectCount(14);
                        instance.Field14 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(14, (uint) i);
                            var key = mapObj.GetSfixed32(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<long, string>();
                        var mapSize = obj.GetObjectCount(15);
                        instance.Field15 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(15, (uint) i);
                            var key = mapObj.GetSfixed64(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>();
                        var mapSize = obj.GetObjectCount(16);
                        instance.Field16 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(16, (uint) i);
                            var key = mapObj.GetEntityIdStruct(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>();
                        var mapSize = obj.GetObjectCount(17);
                        instance.Field17 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(17, (uint) i);
                            var key = global::Improbable.TestSchema.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                {
                    {
                        var map = new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>();
                        var mapSize = obj.GetObjectCount(18);
                        instance.Field18 = map;
                        for (var i = 0; i < mapSize; i++)
                        {
                            var mapObj = obj.IndexObject(18, (uint) i);
                            var key = (global::Improbable.TestSchema.SomeEnum) mapObj.GetEnum(1);
                            var value = mapObj.GetString(2);
                            map.Add(key, value);
                        }
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
