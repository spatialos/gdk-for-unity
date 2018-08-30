// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests
{
    public partial class ExhaustiveOptional
    {
        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 197716;

            public BlittableBool DirtyBit { get; set; }

            internal uint field1Handle;

            public BlittableBool? Field1
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field1Provider.Get(field1Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field1Provider.Set(field1Handle, value);
                }
            }

            internal uint field2Handle;

            public float? Field2
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field2Provider.Get(field2Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field2Provider.Set(field2Handle, value);
                }
            }

            internal uint field3Handle;

            public global::Improbable.Gdk.Core.Option<byte[]> Field3
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field3Provider.Get(field3Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field3Provider.Set(field3Handle, value);
                }
            }

            internal uint field4Handle;

            public int? Field4
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field4Provider.Get(field4Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field4Provider.Set(field4Handle, value);
                }
            }

            internal uint field5Handle;

            public long? Field5
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field5Provider.Get(field5Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field5Provider.Set(field5Handle, value);
                }
            }

            internal uint field6Handle;

            public double? Field6
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field6Provider.Get(field6Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field6Provider.Set(field6Handle, value);
                }
            }

            internal uint field7Handle;

            public global::Improbable.Gdk.Core.Option<string> Field7
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field7Provider.Get(field7Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field7Provider.Set(field7Handle, value);
                }
            }

            internal uint field8Handle;

            public uint? Field8
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field8Provider.Get(field8Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field8Provider.Set(field8Handle, value);
                }
            }

            internal uint field9Handle;

            public ulong? Field9
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field9Provider.Get(field9Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field9Provider.Set(field9Handle, value);
                }
            }

            internal uint field10Handle;

            public int? Field10
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field10Provider.Get(field10Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field10Provider.Set(field10Handle, value);
                }
            }

            internal uint field11Handle;

            public long? Field11
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field11Provider.Get(field11Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field11Provider.Set(field11Handle, value);
                }
            }

            internal uint field12Handle;

            public uint? Field12
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field12Provider.Get(field12Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field12Provider.Set(field12Handle, value);
                }
            }

            internal uint field13Handle;

            public ulong? Field13
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field13Provider.Get(field13Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field13Provider.Set(field13Handle, value);
                }
            }

            internal uint field14Handle;

            public int? Field14
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field14Provider.Get(field14Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field14Provider.Set(field14Handle, value);
                }
            }

            internal uint field15Handle;

            public long? Field15
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field15Provider.Get(field15Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field15Provider.Set(field15Handle, value);
                }
            }

            internal uint field16Handle;

            public global::Improbable.Worker.EntityId? Field16
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field16Provider.Get(field16Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field16Provider.Set(field16Handle, value);
                }
            }

            internal uint field17Handle;

            public global::Generated.Improbable.Gdk.Tests.SomeType? Field17
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field17Provider.Get(field17Handle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field17Provider.Set(field17Handle, value);
                }
            }

            public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
                BlittableBool? field1,
                float? field2,
                global::Improbable.Gdk.Core.Option<byte[]> field3,
                int? field4,
                long? field5,
                double? field6,
                global::Improbable.Gdk.Core.Option<string> field7,
                uint? field8,
                ulong? field9,
                int? field10,
                long? field11,
                uint? field12,
                ulong? field13,
                int? field14,
                long? field15,
                global::Improbable.Worker.EntityId? field16,
                global::Generated.Improbable.Gdk.Tests.SomeType? field17
        )
            {
                var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(197716);
                var obj = schemaComponentData.GetFields();
                {
                    if (field1.HasValue)
                {
                    obj.AddBool(1, field1.Value);
                }
                
                }
                {
                    if (field2.HasValue)
                {
                    obj.AddFloat(2, field2.Value);
                }
                
                }
                {
                    if (field3.HasValue)
                {
                    obj.AddBytes(3, field3.Value);
                }
                
                }
                {
                    if (field4.HasValue)
                {
                    obj.AddInt32(4, field4.Value);
                }
                
                }
                {
                    if (field5.HasValue)
                {
                    obj.AddInt64(5, field5.Value);
                }
                
                }
                {
                    if (field6.HasValue)
                {
                    obj.AddDouble(6, field6.Value);
                }
                
                }
                {
                    if (field7.HasValue)
                {
                    obj.AddString(7, field7.Value);
                }
                
                }
                {
                    if (field8.HasValue)
                {
                    obj.AddUint32(8, field8.Value);
                }
                
                }
                {
                    if (field9.HasValue)
                {
                    obj.AddUint64(9, field9.Value);
                }
                
                }
                {
                    if (field10.HasValue)
                {
                    obj.AddSint32(10, field10.Value);
                }
                
                }
                {
                    if (field11.HasValue)
                {
                    obj.AddSint64(11, field11.Value);
                }
                
                }
                {
                    if (field12.HasValue)
                {
                    obj.AddFixed32(12, field12.Value);
                }
                
                }
                {
                    if (field13.HasValue)
                {
                    obj.AddFixed64(13, field13.Value);
                }
                
                }
                {
                    if (field14.HasValue)
                {
                    obj.AddSfixed32(14, field14.Value);
                }
                
                }
                {
                    if (field15.HasValue)
                {
                    obj.AddSfixed64(15, field15.Value);
                }
                
                }
                {
                    if (field16.HasValue)
                {
                    obj.AddEntityId(16, field16.Value);
                }
                
                }
                {
                    if (field17.HasValue)
                {
                    global::Generated.Improbable.Gdk.Tests.SomeType.Serialization.Serialize(field17.Value, obj.AddObject(17));
                }
                
                }
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void Serialize(Generated.Improbable.Gdk.Tests.ExhaustiveOptional.Component component, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    if (component.Field1.HasValue)
                    {
                        obj.AddBool(1, component.Field1.Value);
                    }
                    
                }
                {
                    if (component.Field2.HasValue)
                    {
                        obj.AddFloat(2, component.Field2.Value);
                    }
                    
                }
                {
                    if (component.Field3.HasValue)
                    {
                        obj.AddBytes(3, component.Field3.Value);
                    }
                    
                }
                {
                    if (component.Field4.HasValue)
                    {
                        obj.AddInt32(4, component.Field4.Value);
                    }
                    
                }
                {
                    if (component.Field5.HasValue)
                    {
                        obj.AddInt64(5, component.Field5.Value);
                    }
                    
                }
                {
                    if (component.Field6.HasValue)
                    {
                        obj.AddDouble(6, component.Field6.Value);
                    }
                    
                }
                {
                    if (component.Field7.HasValue)
                    {
                        obj.AddString(7, component.Field7.Value);
                    }
                    
                }
                {
                    if (component.Field8.HasValue)
                    {
                        obj.AddUint32(8, component.Field8.Value);
                    }
                    
                }
                {
                    if (component.Field9.HasValue)
                    {
                        obj.AddUint64(9, component.Field9.Value);
                    }
                    
                }
                {
                    if (component.Field10.HasValue)
                    {
                        obj.AddSint32(10, component.Field10.Value);
                    }
                    
                }
                {
                    if (component.Field11.HasValue)
                    {
                        obj.AddSint64(11, component.Field11.Value);
                    }
                    
                }
                {
                    if (component.Field12.HasValue)
                    {
                        obj.AddFixed32(12, component.Field12.Value);
                    }
                    
                }
                {
                    if (component.Field13.HasValue)
                    {
                        obj.AddFixed64(13, component.Field13.Value);
                    }
                    
                }
                {
                    if (component.Field14.HasValue)
                    {
                        obj.AddSfixed32(14, component.Field14.Value);
                    }
                    
                }
                {
                    if (component.Field15.HasValue)
                    {
                        obj.AddSfixed64(15, component.Field15.Value);
                    }
                    
                }
                {
                    if (component.Field16.HasValue)
                    {
                        obj.AddEntityId(16, component.Field16.Value);
                    }
                    
                }
                {
                    if (component.Field17.HasValue)
                    {
                        global::Generated.Improbable.Gdk.Tests.SomeType.Serialization.Serialize(component.Field17.Value, obj.AddObject(17));
                    }
                    
                }
            }

            public static Generated.Improbable.Gdk.Tests.ExhaustiveOptional.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Generated.Improbable.Gdk.Tests.ExhaustiveOptional.Component();

                component.field1Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field1Provider.Allocate(world);
                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        component.Field1 = new BlittableBool?(obj.GetBool(1));
                    }
                    
                }
                component.field2Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field2Provider.Allocate(world);
                {
                    if (obj.GetFloatCount(2) == 1)
                    {
                        component.Field2 = new float?(obj.GetFloat(2));
                    }
                    
                }
                component.field3Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field3Provider.Allocate(world);
                {
                    if (obj.GetBytesCount(3) == 1)
                    {
                        component.Field3 = new global::Improbable.Gdk.Core.Option<byte[]>(obj.GetBytes(3));
                    }
                    
                }
                component.field4Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field4Provider.Allocate(world);
                {
                    if (obj.GetInt32Count(4) == 1)
                    {
                        component.Field4 = new int?(obj.GetInt32(4));
                    }
                    
                }
                component.field5Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field5Provider.Allocate(world);
                {
                    if (obj.GetInt64Count(5) == 1)
                    {
                        component.Field5 = new long?(obj.GetInt64(5));
                    }
                    
                }
                component.field6Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field6Provider.Allocate(world);
                {
                    if (obj.GetDoubleCount(6) == 1)
                    {
                        component.Field6 = new double?(obj.GetDouble(6));
                    }
                    
                }
                component.field7Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field7Provider.Allocate(world);
                {
                    if (obj.GetStringCount(7) == 1)
                    {
                        component.Field7 = new global::Improbable.Gdk.Core.Option<string>(obj.GetString(7));
                    }
                    
                }
                component.field8Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field8Provider.Allocate(world);
                {
                    if (obj.GetUint32Count(8) == 1)
                    {
                        component.Field8 = new uint?(obj.GetUint32(8));
                    }
                    
                }
                component.field9Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field9Provider.Allocate(world);
                {
                    if (obj.GetUint64Count(9) == 1)
                    {
                        component.Field9 = new ulong?(obj.GetUint64(9));
                    }
                    
                }
                component.field10Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field10Provider.Allocate(world);
                {
                    if (obj.GetSint32Count(10) == 1)
                    {
                        component.Field10 = new int?(obj.GetSint32(10));
                    }
                    
                }
                component.field11Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field11Provider.Allocate(world);
                {
                    if (obj.GetSint64Count(11) == 1)
                    {
                        component.Field11 = new long?(obj.GetSint64(11));
                    }
                    
                }
                component.field12Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field12Provider.Allocate(world);
                {
                    if (obj.GetFixed32Count(12) == 1)
                    {
                        component.Field12 = new uint?(obj.GetFixed32(12));
                    }
                    
                }
                component.field13Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field13Provider.Allocate(world);
                {
                    if (obj.GetFixed64Count(13) == 1)
                    {
                        component.Field13 = new ulong?(obj.GetFixed64(13));
                    }
                    
                }
                component.field14Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field14Provider.Allocate(world);
                {
                    if (obj.GetSfixed32Count(14) == 1)
                    {
                        component.Field14 = new int?(obj.GetSfixed32(14));
                    }
                    
                }
                component.field15Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field15Provider.Allocate(world);
                {
                    if (obj.GetSfixed64Count(15) == 1)
                    {
                        component.Field15 = new long?(obj.GetSfixed64(15));
                    }
                    
                }
                component.field16Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field16Provider.Allocate(world);
                {
                    if (obj.GetEntityIdCount(16) == 1)
                    {
                        component.Field16 = new global::Improbable.Worker.EntityId?(obj.GetEntityId(16));
                    }
                    
                }
                component.field17Handle = Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.Field17Provider.Allocate(world);
                {
                    if (obj.GetObjectCount(17) == 1)
                    {
                        component.Field17 = new global::Generated.Improbable.Gdk.Tests.SomeType?(global::Generated.Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17)));
                    }
                    
                }
                return component;
            }

            public static Generated.Improbable.Gdk.Tests.ExhaustiveOptional.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref Generated.Improbable.Gdk.Tests.ExhaustiveOptional.Component component)
            {
                var update = new Generated.Improbable.Gdk.Tests.ExhaustiveOptional.Update();
                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        component.Field1 = new BlittableBool?(value);
                        update.Field1 = new global::Improbable.Gdk.Core.Option<BlittableBool?>(new BlittableBool?(value));
                    }
                    
                }
                {
                    if (obj.GetFloatCount(2) == 1)
                    {
                        var value = obj.GetFloat(2);
                        component.Field2 = new float?(value);
                        update.Field2 = new global::Improbable.Gdk.Core.Option<float?>(new float?(value));
                    }
                    
                }
                {
                    if (obj.GetBytesCount(3) == 1)
                    {
                        var value = obj.GetBytes(3);
                        component.Field3 = new global::Improbable.Gdk.Core.Option<byte[]>(value);
                        update.Field3 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.Option<byte[]>>(new global::Improbable.Gdk.Core.Option<byte[]>(value));
                    }
                    
                }
                {
                    if (obj.GetInt32Count(4) == 1)
                    {
                        var value = obj.GetInt32(4);
                        component.Field4 = new int?(value);
                        update.Field4 = new global::Improbable.Gdk.Core.Option<int?>(new int?(value));
                    }
                    
                }
                {
                    if (obj.GetInt64Count(5) == 1)
                    {
                        var value = obj.GetInt64(5);
                        component.Field5 = new long?(value);
                        update.Field5 = new global::Improbable.Gdk.Core.Option<long?>(new long?(value));
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(6) == 1)
                    {
                        var value = obj.GetDouble(6);
                        component.Field6 = new double?(value);
                        update.Field6 = new global::Improbable.Gdk.Core.Option<double?>(new double?(value));
                    }
                    
                }
                {
                    if (obj.GetStringCount(7) == 1)
                    {
                        var value = obj.GetString(7);
                        component.Field7 = new global::Improbable.Gdk.Core.Option<string>(value);
                        update.Field7 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.Option<string>>(new global::Improbable.Gdk.Core.Option<string>(value));
                    }
                    
                }
                {
                    if (obj.GetUint32Count(8) == 1)
                    {
                        var value = obj.GetUint32(8);
                        component.Field8 = new uint?(value);
                        update.Field8 = new global::Improbable.Gdk.Core.Option<uint?>(new uint?(value));
                    }
                    
                }
                {
                    if (obj.GetUint64Count(9) == 1)
                    {
                        var value = obj.GetUint64(9);
                        component.Field9 = new ulong?(value);
                        update.Field9 = new global::Improbable.Gdk.Core.Option<ulong?>(new ulong?(value));
                    }
                    
                }
                {
                    if (obj.GetSint32Count(10) == 1)
                    {
                        var value = obj.GetSint32(10);
                        component.Field10 = new int?(value);
                        update.Field10 = new global::Improbable.Gdk.Core.Option<int?>(new int?(value));
                    }
                    
                }
                {
                    if (obj.GetSint64Count(11) == 1)
                    {
                        var value = obj.GetSint64(11);
                        component.Field11 = new long?(value);
                        update.Field11 = new global::Improbable.Gdk.Core.Option<long?>(new long?(value));
                    }
                    
                }
                {
                    if (obj.GetFixed32Count(12) == 1)
                    {
                        var value = obj.GetFixed32(12);
                        component.Field12 = new uint?(value);
                        update.Field12 = new global::Improbable.Gdk.Core.Option<uint?>(new uint?(value));
                    }
                    
                }
                {
                    if (obj.GetFixed64Count(13) == 1)
                    {
                        var value = obj.GetFixed64(13);
                        component.Field13 = new ulong?(value);
                        update.Field13 = new global::Improbable.Gdk.Core.Option<ulong?>(new ulong?(value));
                    }
                    
                }
                {
                    if (obj.GetSfixed32Count(14) == 1)
                    {
                        var value = obj.GetSfixed32(14);
                        component.Field14 = new int?(value);
                        update.Field14 = new global::Improbable.Gdk.Core.Option<int?>(new int?(value));
                    }
                    
                }
                {
                    if (obj.GetSfixed64Count(15) == 1)
                    {
                        var value = obj.GetSfixed64(15);
                        component.Field15 = new long?(value);
                        update.Field15 = new global::Improbable.Gdk.Core.Option<long?>(new long?(value));
                    }
                    
                }
                {
                    if (obj.GetEntityIdCount(16) == 1)
                    {
                        var value = obj.GetEntityId(16);
                        component.Field16 = new global::Improbable.Worker.EntityId?(value);
                        update.Field16 = new global::Improbable.Gdk.Core.Option<global::Improbable.Worker.EntityId?>(new global::Improbable.Worker.EntityId?(value));
                    }
                    
                }
                {
                    if (obj.GetObjectCount(17) == 1)
                    {
                        var value = global::Generated.Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                        component.Field17 = new global::Generated.Improbable.Gdk.Tests.SomeType?(value);
                        update.Field17 = new global::Improbable.Gdk.Core.Option<global::Generated.Improbable.Gdk.Tests.SomeType?>(new global::Generated.Improbable.Gdk.Tests.SomeType?(value));
                    }
                    
                }
                return update;
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<BlittableBool?> Field1;
            public Option<float?> Field2;
            public Option<global::Improbable.Gdk.Core.Option<byte[]>> Field3;
            public Option<int?> Field4;
            public Option<long?> Field5;
            public Option<double?> Field6;
            public Option<global::Improbable.Gdk.Core.Option<string>> Field7;
            public Option<uint?> Field8;
            public Option<ulong?> Field9;
            public Option<int?> Field10;
            public Option<long?> Field11;
            public Option<uint?> Field12;
            public Option<ulong?> Field13;
            public Option<int?> Field14;
            public Option<long?> Field15;
            public Option<global::Improbable.Worker.EntityId?> Field16;
            public Option<global::Generated.Improbable.Gdk.Tests.SomeType?> Field17;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Generated.Improbable.Gdk.Tests.ExhaustiveOptional.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
