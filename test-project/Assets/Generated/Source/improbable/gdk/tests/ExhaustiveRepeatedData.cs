// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests
{
    
    [global::System.Serializable]
    public struct ExhaustiveRepeatedData
    {
        public global::System.Collections.Generic.List<BlittableBool> Field1;
        public global::System.Collections.Generic.List<float> Field2;
        public global::System.Collections.Generic.List<byte[]> Field3;
        public global::System.Collections.Generic.List<int> Field4;
        public global::System.Collections.Generic.List<long> Field5;
        public global::System.Collections.Generic.List<double> Field6;
        public global::System.Collections.Generic.List<string> Field7;
        public global::System.Collections.Generic.List<uint> Field8;
        public global::System.Collections.Generic.List<ulong> Field9;
        public global::System.Collections.Generic.List<int> Field10;
        public global::System.Collections.Generic.List<long> Field11;
        public global::System.Collections.Generic.List<uint> Field12;
        public global::System.Collections.Generic.List<ulong> Field13;
        public global::System.Collections.Generic.List<int> Field14;
        public global::System.Collections.Generic.List<long> Field15;
        public global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId> Field16;
        public global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType> Field17;
        public global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum> Field18;
    
        public ExhaustiveRepeatedData(global::System.Collections.Generic.List<BlittableBool> field1, global::System.Collections.Generic.List<float> field2, global::System.Collections.Generic.List<byte[]> field3, global::System.Collections.Generic.List<int> field4, global::System.Collections.Generic.List<long> field5, global::System.Collections.Generic.List<double> field6, global::System.Collections.Generic.List<string> field7, global::System.Collections.Generic.List<uint> field8, global::System.Collections.Generic.List<ulong> field9, global::System.Collections.Generic.List<int> field10, global::System.Collections.Generic.List<long> field11, global::System.Collections.Generic.List<uint> field12, global::System.Collections.Generic.List<ulong> field13, global::System.Collections.Generic.List<int> field14, global::System.Collections.Generic.List<long> field15, global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId> field16, global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType> field17, global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum> field18)
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
            public static void Serialize(ExhaustiveRepeatedData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var value in instance.Field1)
                    {
                        obj.AddBool(1, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field2)
                    {
                        obj.AddFloat(2, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field3)
                    {
                        obj.AddBytes(3, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field4)
                    {
                        obj.AddInt32(4, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field5)
                    {
                        obj.AddInt64(5, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field6)
                    {
                        obj.AddDouble(6, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field7)
                    {
                        obj.AddString(7, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field8)
                    {
                        obj.AddUint32(8, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field9)
                    {
                        obj.AddUint64(9, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field10)
                    {
                        obj.AddSint32(10, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field11)
                    {
                        obj.AddSint64(11, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field12)
                    {
                        obj.AddFixed32(12, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field13)
                    {
                        obj.AddFixed64(13, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field14)
                    {
                        obj.AddSfixed32(14, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field15)
                    {
                        obj.AddSfixed64(15, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field16)
                    {
                        obj.AddEntityId(16, value);
                    }
                    
                }
                {
                    foreach (var value in instance.Field17)
                    {
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(value, obj.AddObject(17));
                    }
                    
                }
                {
                    foreach (var value in instance.Field18)
                    {
                        obj.AddEnum(18, (uint) value);
                    }
                    
                }
            }
    
            public static ExhaustiveRepeatedData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new ExhaustiveRepeatedData();
                {
                    instance.Field1 = new global::System.Collections.Generic.List<BlittableBool>();
                    var list = instance.Field1;
                    var listLength = obj.GetBoolCount(1);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexBool(1, (uint) i));
                    }
                    
                }
                {
                    instance.Field2 = new global::System.Collections.Generic.List<float>();
                    var list = instance.Field2;
                    var listLength = obj.GetFloatCount(2);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexFloat(2, (uint) i));
                    }
                    
                }
                {
                    instance.Field3 = new global::System.Collections.Generic.List<byte[]>();
                    var list = instance.Field3;
                    var listLength = obj.GetBytesCount(3);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexBytes(3, (uint) i));
                    }
                    
                }
                {
                    instance.Field4 = new global::System.Collections.Generic.List<int>();
                    var list = instance.Field4;
                    var listLength = obj.GetInt32Count(4);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexInt32(4, (uint) i));
                    }
                    
                }
                {
                    instance.Field5 = new global::System.Collections.Generic.List<long>();
                    var list = instance.Field5;
                    var listLength = obj.GetInt64Count(5);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexInt64(5, (uint) i));
                    }
                    
                }
                {
                    instance.Field6 = new global::System.Collections.Generic.List<double>();
                    var list = instance.Field6;
                    var listLength = obj.GetDoubleCount(6);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexDouble(6, (uint) i));
                    }
                    
                }
                {
                    instance.Field7 = new global::System.Collections.Generic.List<string>();
                    var list = instance.Field7;
                    var listLength = obj.GetStringCount(7);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexString(7, (uint) i));
                    }
                    
                }
                {
                    instance.Field8 = new global::System.Collections.Generic.List<uint>();
                    var list = instance.Field8;
                    var listLength = obj.GetUint32Count(8);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexUint32(8, (uint) i));
                    }
                    
                }
                {
                    instance.Field9 = new global::System.Collections.Generic.List<ulong>();
                    var list = instance.Field9;
                    var listLength = obj.GetUint64Count(9);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexUint64(9, (uint) i));
                    }
                    
                }
                {
                    instance.Field10 = new global::System.Collections.Generic.List<int>();
                    var list = instance.Field10;
                    var listLength = obj.GetSint32Count(10);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSint32(10, (uint) i));
                    }
                    
                }
                {
                    instance.Field11 = new global::System.Collections.Generic.List<long>();
                    var list = instance.Field11;
                    var listLength = obj.GetSint64Count(11);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSint64(11, (uint) i));
                    }
                    
                }
                {
                    instance.Field12 = new global::System.Collections.Generic.List<uint>();
                    var list = instance.Field12;
                    var listLength = obj.GetFixed32Count(12);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexFixed32(12, (uint) i));
                    }
                    
                }
                {
                    instance.Field13 = new global::System.Collections.Generic.List<ulong>();
                    var list = instance.Field13;
                    var listLength = obj.GetFixed64Count(13);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexFixed64(13, (uint) i));
                    }
                    
                }
                {
                    instance.Field14 = new global::System.Collections.Generic.List<int>();
                    var list = instance.Field14;
                    var listLength = obj.GetSfixed32Count(14);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSfixed32(14, (uint) i));
                    }
                    
                }
                {
                    instance.Field15 = new global::System.Collections.Generic.List<long>();
                    var list = instance.Field15;
                    var listLength = obj.GetSfixed64Count(15);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSfixed64(15, (uint) i));
                    }
                    
                }
                {
                    instance.Field16 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId>();
                    var list = instance.Field16;
                    var listLength = obj.GetEntityIdCount(16);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexEntityIdStruct(16, (uint) i));
                    }
                    
                }
                {
                    instance.Field17 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType>();
                    var list = instance.Field17;
                    var listLength = obj.GetObjectCount(17);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.IndexObject(17, (uint) i)));
                    }
                    
                }
                {
                    instance.Field18 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum>();
                    var list = instance.Field18;
                    var listLength = obj.GetEnumCount(18);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add((global::Improbable.Gdk.Tests.SomeEnum) obj.IndexEnum(18, (uint) i));
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
