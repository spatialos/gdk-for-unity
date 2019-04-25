// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests
{
    
    [global::System.Serializable]
    public struct ExhaustiveSingularData
    {
        public BlittableBool Field1;
        public float Field2;
        public byte[] Field3;
        public int Field4;
        public long Field5;
        public double Field6;
        public string Field7;
        public uint Field8;
        public ulong Field9;
        public int Field10;
        public long Field11;
        public uint Field12;
        public ulong Field13;
        public int Field14;
        public long Field15;
        public global::Improbable.Gdk.Core.EntityId Field16;
        public global::Improbable.Gdk.Tests.SomeType Field17;
        public global::Improbable.Gdk.Tests.SomeEnum Field18;
    
        public ExhaustiveSingularData(BlittableBool field1, float field2, byte[] field3, int field4, long field5, double field6, string field7, uint field8, ulong field9, int field10, long field11, uint field12, ulong field13, int field14, long field15, global::Improbable.Gdk.Core.EntityId field16, global::Improbable.Gdk.Tests.SomeType field17, global::Improbable.Gdk.Tests.SomeEnum field18)
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
            public static void Serialize(ExhaustiveSingularData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, instance.Field1);
                }
                {
                    obj.AddFloat(2, instance.Field2);
                }
                {
                    obj.AddBytes(3, instance.Field3);
                }
                {
                    obj.AddInt32(4, instance.Field4);
                }
                {
                    obj.AddInt64(5, instance.Field5);
                }
                {
                    obj.AddDouble(6, instance.Field6);
                }
                {
                    obj.AddString(7, instance.Field7);
                }
                {
                    obj.AddUint32(8, instance.Field8);
                }
                {
                    obj.AddUint64(9, instance.Field9);
                }
                {
                    obj.AddSint32(10, instance.Field10);
                }
                {
                    obj.AddSint64(11, instance.Field11);
                }
                {
                    obj.AddFixed32(12, instance.Field12);
                }
                {
                    obj.AddFixed64(13, instance.Field13);
                }
                {
                    obj.AddSfixed32(14, instance.Field14);
                }
                {
                    obj.AddSfixed64(15, instance.Field15);
                }
                {
                    obj.AddEntityId(16, instance.Field16);
                }
                {
                    global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(instance.Field17, obj.AddObject(17));
                }
                {
                    obj.AddEnum(18, (uint) instance.Field18);
                }
            }
    
            public static ExhaustiveSingularData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new ExhaustiveSingularData();
                {
                    instance.Field1 = obj.GetBool(1);
                }
                {
                    instance.Field2 = obj.GetFloat(2);
                }
                {
                    instance.Field3 = obj.GetBytes(3);
                }
                {
                    instance.Field4 = obj.GetInt32(4);
                }
                {
                    instance.Field5 = obj.GetInt64(5);
                }
                {
                    instance.Field6 = obj.GetDouble(6);
                }
                {
                    instance.Field7 = obj.GetString(7);
                }
                {
                    instance.Field8 = obj.GetUint32(8);
                }
                {
                    instance.Field9 = obj.GetUint64(9);
                }
                {
                    instance.Field10 = obj.GetSint32(10);
                }
                {
                    instance.Field11 = obj.GetSint64(11);
                }
                {
                    instance.Field12 = obj.GetFixed32(12);
                }
                {
                    instance.Field13 = obj.GetFixed64(13);
                }
                {
                    instance.Field14 = obj.GetSfixed32(14);
                }
                {
                    instance.Field15 = obj.GetSfixed64(15);
                }
                {
                    instance.Field16 = obj.GetEntityIdStruct(16);
                }
                {
                    instance.Field17 = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                }
                {
                    instance.Field18 = (global::Improbable.Gdk.Tests.SomeEnum) obj.GetEnum(18);
                }
                return instance;
            }
        }
    }
    
}
