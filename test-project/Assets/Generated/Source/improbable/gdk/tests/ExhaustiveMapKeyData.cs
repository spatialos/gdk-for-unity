// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests
{
    
    [global::System.Serializable]
    public struct ExhaustiveMapKeyData
    {
        public global::System.Collections.Generic.Dictionary<BlittableBool,string> Field1;
        public global::System.Collections.Generic.Dictionary<float,string> Field2;
        public global::System.Collections.Generic.Dictionary<byte[],string> Field3;
        public global::System.Collections.Generic.Dictionary<int,string> Field4;
        public global::System.Collections.Generic.Dictionary<long,string> Field5;
        public global::System.Collections.Generic.Dictionary<double,string> Field6;
        public global::System.Collections.Generic.Dictionary<string,string> Field7;
        public global::System.Collections.Generic.Dictionary<uint,string> Field8;
        public global::System.Collections.Generic.Dictionary<ulong,string> Field9;
        public global::System.Collections.Generic.Dictionary<int,string> Field10;
        public global::System.Collections.Generic.Dictionary<long,string> Field11;
        public global::System.Collections.Generic.Dictionary<uint,string> Field12;
        public global::System.Collections.Generic.Dictionary<ulong,string> Field13;
        public global::System.Collections.Generic.Dictionary<int,string> Field14;
        public global::System.Collections.Generic.Dictionary<long,string> Field15;
        public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string> Field16;
        public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string> Field17;
        public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string> Field18;
    
        public ExhaustiveMapKeyData(global::System.Collections.Generic.Dictionary<BlittableBool,string> field1, global::System.Collections.Generic.Dictionary<float,string> field2, global::System.Collections.Generic.Dictionary<byte[],string> field3, global::System.Collections.Generic.Dictionary<int,string> field4, global::System.Collections.Generic.Dictionary<long,string> field5, global::System.Collections.Generic.Dictionary<double,string> field6, global::System.Collections.Generic.Dictionary<string,string> field7, global::System.Collections.Generic.Dictionary<uint,string> field8, global::System.Collections.Generic.Dictionary<ulong,string> field9, global::System.Collections.Generic.Dictionary<int,string> field10, global::System.Collections.Generic.Dictionary<long,string> field11, global::System.Collections.Generic.Dictionary<uint,string> field12, global::System.Collections.Generic.Dictionary<ulong,string> field13, global::System.Collections.Generic.Dictionary<int,string> field14, global::System.Collections.Generic.Dictionary<long,string> field15, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string> field16, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string> field17, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string> field18)
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
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(keyValuePair.Key, mapObj.AddObject(1));
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
                    instance.Field1 = new global::System.Collections.Generic.Dictionary<BlittableBool,string>();
                    var map = instance.Field1;
                    var mapSize = obj.GetObjectCount(1);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field2 = new global::System.Collections.Generic.Dictionary<float,string>();
                    var map = instance.Field2;
                    var mapSize = obj.GetObjectCount(2);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field3 = new global::System.Collections.Generic.Dictionary<byte[],string>();
                    var map = instance.Field3;
                    var mapSize = obj.GetObjectCount(3);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field4 = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = instance.Field4;
                    var mapSize = obj.GetObjectCount(4);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field5 = new global::System.Collections.Generic.Dictionary<long,string>();
                    var map = instance.Field5;
                    var mapSize = obj.GetObjectCount(5);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field6 = new global::System.Collections.Generic.Dictionary<double,string>();
                    var map = instance.Field6;
                    var mapSize = obj.GetObjectCount(6);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field7 = new global::System.Collections.Generic.Dictionary<string,string>();
                    var map = instance.Field7;
                    var mapSize = obj.GetObjectCount(7);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field8 = new global::System.Collections.Generic.Dictionary<uint,string>();
                    var map = instance.Field8;
                    var mapSize = obj.GetObjectCount(8);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field9 = new global::System.Collections.Generic.Dictionary<ulong,string>();
                    var map = instance.Field9;
                    var mapSize = obj.GetObjectCount(9);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field10 = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = instance.Field10;
                    var mapSize = obj.GetObjectCount(10);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field11 = new global::System.Collections.Generic.Dictionary<long,string>();
                    var map = instance.Field11;
                    var mapSize = obj.GetObjectCount(11);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field12 = new global::System.Collections.Generic.Dictionary<uint,string>();
                    var map = instance.Field12;
                    var mapSize = obj.GetObjectCount(12);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field13 = new global::System.Collections.Generic.Dictionary<ulong,string>();
                    var map = instance.Field13;
                    var mapSize = obj.GetObjectCount(13);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field14 = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = instance.Field14;
                    var mapSize = obj.GetObjectCount(14);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field15 = new global::System.Collections.Generic.Dictionary<long,string>();
                    var map = instance.Field15;
                    var mapSize = obj.GetObjectCount(15);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field16 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>();
                    var map = instance.Field16;
                    var mapSize = obj.GetObjectCount(16);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field17 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>();
                    var map = instance.Field17;
                    var mapSize = obj.GetObjectCount(17);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field18 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>();
                    var map = instance.Field18;
                    var mapSize = obj.GetObjectCount(18);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.Gdk.Tests.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
