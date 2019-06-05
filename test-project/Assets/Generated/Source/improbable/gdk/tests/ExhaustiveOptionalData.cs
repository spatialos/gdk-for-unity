// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests
{
    
    [global::System.Serializable]
    public struct ExhaustiveOptionalData
    {
        public bool? Field1;
        public float? Field2;
        public global::Improbable.Gdk.Core.Option<byte[]> Field3;
        public int? Field4;
        public long? Field5;
        public double? Field6;
        public global::Improbable.Gdk.Core.Option<string> Field7;
        public uint? Field8;
        public ulong? Field9;
        public int? Field10;
        public long? Field11;
        public uint? Field12;
        public ulong? Field13;
        public int? Field14;
        public long? Field15;
        public global::Improbable.Gdk.Core.EntityId? Field16;
        public global::Improbable.Gdk.Tests.SomeType? Field17;
        public global::Improbable.Gdk.Tests.SomeEnum? Field18;
    
        public ExhaustiveOptionalData(bool? field1, float? field2, global::Improbable.Gdk.Core.Option<byte[]> field3, int? field4, long? field5, double? field6, global::Improbable.Gdk.Core.Option<string> field7, uint? field8, ulong? field9, int? field10, long? field11, uint? field12, ulong? field13, int? field14, long? field15, global::Improbable.Gdk.Core.EntityId? field16, global::Improbable.Gdk.Tests.SomeType? field17, global::Improbable.Gdk.Tests.SomeEnum? field18)
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
            public static void Serialize(ExhaustiveOptionalData instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    if (instance.Field1.HasValue)
                    {
                        obj.AddBool(1, instance.Field1.Value);
                    }
                    
                }
                {
                    if (instance.Field2.HasValue)
                    {
                        obj.AddFloat(2, instance.Field2.Value);
                    }
                    
                }
                {
                    if (instance.Field3.HasValue)
                    {
                        obj.AddBytes(3, instance.Field3.Value);
                    }
                    
                }
                {
                    if (instance.Field4.HasValue)
                    {
                        obj.AddInt32(4, instance.Field4.Value);
                    }
                    
                }
                {
                    if (instance.Field5.HasValue)
                    {
                        obj.AddInt64(5, instance.Field5.Value);
                    }
                    
                }
                {
                    if (instance.Field6.HasValue)
                    {
                        obj.AddDouble(6, instance.Field6.Value);
                    }
                    
                }
                {
                    if (instance.Field7.HasValue)
                    {
                        obj.AddString(7, instance.Field7.Value);
                    }
                    
                }
                {
                    if (instance.Field8.HasValue)
                    {
                        obj.AddUint32(8, instance.Field8.Value);
                    }
                    
                }
                {
                    if (instance.Field9.HasValue)
                    {
                        obj.AddUint64(9, instance.Field9.Value);
                    }
                    
                }
                {
                    if (instance.Field10.HasValue)
                    {
                        obj.AddSint32(10, instance.Field10.Value);
                    }
                    
                }
                {
                    if (instance.Field11.HasValue)
                    {
                        obj.AddSint64(11, instance.Field11.Value);
                    }
                    
                }
                {
                    if (instance.Field12.HasValue)
                    {
                        obj.AddFixed32(12, instance.Field12.Value);
                    }
                    
                }
                {
                    if (instance.Field13.HasValue)
                    {
                        obj.AddFixed64(13, instance.Field13.Value);
                    }
                    
                }
                {
                    if (instance.Field14.HasValue)
                    {
                        obj.AddSfixed32(14, instance.Field14.Value);
                    }
                    
                }
                {
                    if (instance.Field15.HasValue)
                    {
                        obj.AddSfixed64(15, instance.Field15.Value);
                    }
                    
                }
                {
                    if (instance.Field16.HasValue)
                    {
                        obj.AddEntityId(16, instance.Field16.Value);
                    }
                    
                }
                {
                    if (instance.Field17.HasValue)
                    {
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(instance.Field17.Value, obj.AddObject(17));
                    }
                    
                }
                {
                    if (instance.Field18.HasValue)
                    {
                        obj.AddEnum(18, (uint) instance.Field18.Value);
                    }
                    
                }
            }
    
            public static ExhaustiveOptionalData Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new ExhaustiveOptionalData();
                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        instance.Field1 = new bool?(obj.GetBool(1));
                    }
                    
                }
                {
                    if (obj.GetFloatCount(2) == 1)
                    {
                        instance.Field2 = new float?(obj.GetFloat(2));
                    }
                    
                }
                {
                    if (obj.GetBytesCount(3) == 1)
                    {
                        instance.Field3 = new global::Improbable.Gdk.Core.Option<byte[]>(obj.GetBytes(3));
                    }
                    
                }
                {
                    if (obj.GetInt32Count(4) == 1)
                    {
                        instance.Field4 = new int?(obj.GetInt32(4));
                    }
                    
                }
                {
                    if (obj.GetInt64Count(5) == 1)
                    {
                        instance.Field5 = new long?(obj.GetInt64(5));
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(6) == 1)
                    {
                        instance.Field6 = new double?(obj.GetDouble(6));
                    }
                    
                }
                {
                    if (obj.GetStringCount(7) == 1)
                    {
                        instance.Field7 = new global::Improbable.Gdk.Core.Option<string>(obj.GetString(7));
                    }
                    
                }
                {
                    if (obj.GetUint32Count(8) == 1)
                    {
                        instance.Field8 = new uint?(obj.GetUint32(8));
                    }
                    
                }
                {
                    if (obj.GetUint64Count(9) == 1)
                    {
                        instance.Field9 = new ulong?(obj.GetUint64(9));
                    }
                    
                }
                {
                    if (obj.GetSint32Count(10) == 1)
                    {
                        instance.Field10 = new int?(obj.GetSint32(10));
                    }
                    
                }
                {
                    if (obj.GetSint64Count(11) == 1)
                    {
                        instance.Field11 = new long?(obj.GetSint64(11));
                    }
                    
                }
                {
                    if (obj.GetFixed32Count(12) == 1)
                    {
                        instance.Field12 = new uint?(obj.GetFixed32(12));
                    }
                    
                }
                {
                    if (obj.GetFixed64Count(13) == 1)
                    {
                        instance.Field13 = new ulong?(obj.GetFixed64(13));
                    }
                    
                }
                {
                    if (obj.GetSfixed32Count(14) == 1)
                    {
                        instance.Field14 = new int?(obj.GetSfixed32(14));
                    }
                    
                }
                {
                    if (obj.GetSfixed64Count(15) == 1)
                    {
                        instance.Field15 = new long?(obj.GetSfixed64(15));
                    }
                    
                }
                {
                    if (obj.GetEntityIdCount(16) == 1)
                    {
                        instance.Field16 = new global::Improbable.Gdk.Core.EntityId?(obj.GetEntityIdStruct(16));
                    }
                    
                }
                {
                    if (obj.GetObjectCount(17) == 1)
                    {
                        instance.Field17 = new global::Improbable.Gdk.Tests.SomeType?(global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17)));
                    }
                    
                }
                {
                    if (obj.GetEnumCount(18) == 1)
                    {
                        instance.Field18 = new global::Improbable.Gdk.Tests.SomeEnum?((global::Improbable.Gdk.Tests.SomeEnum) obj.GetEnum(18));
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
