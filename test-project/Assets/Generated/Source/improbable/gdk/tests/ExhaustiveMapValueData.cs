// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests
{
    
    [global::System.Serializable]
    public struct ExhaustiveMapValueData
    {
        public global::System.Collections.Generic.Dictionary<string,BlittableBool> Field1;
        public global::System.Collections.Generic.Dictionary<string,float> Field2;
        public global::System.Collections.Generic.Dictionary<string,byte[]> Field3;
        public global::System.Collections.Generic.Dictionary<string,int> Field4;
        public global::System.Collections.Generic.Dictionary<string,long> Field5;
        public global::System.Collections.Generic.Dictionary<string,double> Field6;
        public global::System.Collections.Generic.Dictionary<string,string> Field7;
        public global::System.Collections.Generic.Dictionary<string,uint> Field8;
        public global::System.Collections.Generic.Dictionary<string,ulong> Field9;
        public global::System.Collections.Generic.Dictionary<string,int> Field10;
        public global::System.Collections.Generic.Dictionary<string,long> Field11;
        public global::System.Collections.Generic.Dictionary<string,uint> Field12;
        public global::System.Collections.Generic.Dictionary<string,ulong> Field13;
        public global::System.Collections.Generic.Dictionary<string,int> Field14;
        public global::System.Collections.Generic.Dictionary<string,long> Field15;
        public global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntityId> Field16;
        public global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType> Field17;
        public global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeEnum> Field18;
    
        public ExhaustiveMapValueData(global::System.Collections.Generic.Dictionary<string,BlittableBool> field1, global::System.Collections.Generic.Dictionary<string,float> field2, global::System.Collections.Generic.Dictionary<string,byte[]> field3, global::System.Collections.Generic.Dictionary<string,int> field4, global::System.Collections.Generic.Dictionary<string,long> field5, global::System.Collections.Generic.Dictionary<string,double> field6, global::System.Collections.Generic.Dictionary<string,string> field7, global::System.Collections.Generic.Dictionary<string,uint> field8, global::System.Collections.Generic.Dictionary<string,ulong> field9, global::System.Collections.Generic.Dictionary<string,int> field10, global::System.Collections.Generic.Dictionary<string,long> field11, global::System.Collections.Generic.Dictionary<string,uint> field12, global::System.Collections.Generic.Dictionary<string,ulong> field13, global::System.Collections.Generic.Dictionary<string,int> field14, global::System.Collections.Generic.Dictionary<string,long> field15, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntityId> field16, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType> field17, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeEnum> field18)
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
            public static void Serialize(ExhaustiveMapValueData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var keyValuePair in instance.Field1)
                    {
                        var mapObj = obj.AddObject(1);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddBool(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field2)
                    {
                        var mapObj = obj.AddObject(2);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddFloat(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field3)
                    {
                        var mapObj = obj.AddObject(3);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddBytes(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field4)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddInt32(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field5)
                    {
                        var mapObj = obj.AddObject(5);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddInt64(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field6)
                    {
                        var mapObj = obj.AddObject(6);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddDouble(2, keyValuePair.Value);
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
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddUint32(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field9)
                    {
                        var mapObj = obj.AddObject(9);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddUint64(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field10)
                    {
                        var mapObj = obj.AddObject(10);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddSint32(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field11)
                    {
                        var mapObj = obj.AddObject(11);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddSint64(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field12)
                    {
                        var mapObj = obj.AddObject(12);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddFixed32(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field13)
                    {
                        var mapObj = obj.AddObject(13);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddFixed64(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field14)
                    {
                        var mapObj = obj.AddObject(14);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddSfixed32(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field15)
                    {
                        var mapObj = obj.AddObject(15);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddSfixed64(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field16)
                    {
                        var mapObj = obj.AddObject(16);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddEntityId(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field17)
                    {
                        var mapObj = obj.AddObject(17);
                        mapObj.AddString(1, keyValuePair.Key);
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                    }
                    
                }
                {
                    foreach (var keyValuePair in instance.Field18)
                    {
                        var mapObj = obj.AddObject(18);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddEnum(2, (uint) keyValuePair.Value);
                    }
                    
                }
            }
    
            public static ExhaustiveMapValueData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new ExhaustiveMapValueData();
                {
                    instance.Field1 = new global::System.Collections.Generic.Dictionary<string,BlittableBool>();
                    var map = instance.Field1;
                    var mapSize = obj.GetObjectCount(1);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetBool(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field2 = new global::System.Collections.Generic.Dictionary<string,float>();
                    var map = instance.Field2;
                    var mapSize = obj.GetObjectCount(2);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFloat(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field3 = new global::System.Collections.Generic.Dictionary<string,byte[]>();
                    var map = instance.Field3;
                    var mapSize = obj.GetObjectCount(3);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetBytes(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field4 = new global::System.Collections.Generic.Dictionary<string,int>();
                    var map = instance.Field4;
                    var mapSize = obj.GetObjectCount(4);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetInt32(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field5 = new global::System.Collections.Generic.Dictionary<string,long>();
                    var map = instance.Field5;
                    var mapSize = obj.GetObjectCount(5);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetInt64(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field6 = new global::System.Collections.Generic.Dictionary<string,double>();
                    var map = instance.Field6;
                    var mapSize = obj.GetObjectCount(6);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetDouble(2);
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
                    instance.Field8 = new global::System.Collections.Generic.Dictionary<string,uint>();
                    var map = instance.Field8;
                    var mapSize = obj.GetObjectCount(8);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetUint32(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field9 = new global::System.Collections.Generic.Dictionary<string,ulong>();
                    var map = instance.Field9;
                    var mapSize = obj.GetObjectCount(9);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetUint64(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field10 = new global::System.Collections.Generic.Dictionary<string,int>();
                    var map = instance.Field10;
                    var mapSize = obj.GetObjectCount(10);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSint32(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field11 = new global::System.Collections.Generic.Dictionary<string,long>();
                    var map = instance.Field11;
                    var mapSize = obj.GetObjectCount(11);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSint64(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field12 = new global::System.Collections.Generic.Dictionary<string,uint>();
                    var map = instance.Field12;
                    var mapSize = obj.GetObjectCount(12);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFixed32(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field13 = new global::System.Collections.Generic.Dictionary<string,ulong>();
                    var map = instance.Field13;
                    var mapSize = obj.GetObjectCount(13);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFixed64(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field14 = new global::System.Collections.Generic.Dictionary<string,int>();
                    var map = instance.Field14;
                    var mapSize = obj.GetObjectCount(14);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSfixed32(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field15 = new global::System.Collections.Generic.Dictionary<string,long>();
                    var map = instance.Field15;
                    var mapSize = obj.GetObjectCount(15);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSfixed64(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field16 = new global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntityId>();
                    var map = instance.Field16;
                    var mapSize = obj.GetObjectCount(16);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntityIdStruct(2);
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field17 = new global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType>();
                    var map = instance.Field17;
                    var mapSize = obj.GetObjectCount(17);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(mapObj.GetObject(2));
                        map.Add(key, value);
                    }
                    
                }
                {
                    instance.Field18 = new global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeEnum>();
                    var map = instance.Field18;
                    var mapSize = obj.GetObjectCount(18);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = (global::Improbable.Gdk.Tests.SomeEnum) mapObj.GetEnum(2);
                        map.Add(key, value);
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
