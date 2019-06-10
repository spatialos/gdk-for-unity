// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveSingular
    {
        public const uint ComponentId = 197715;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 197715;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            // Each byte tracks 8 component properties.
            private byte dirtyBits0;
            private byte dirtyBits1;
            private byte dirtyBits2;

            public bool IsDataDirty()
            {
                var isDataDirty = false;
                isDataDirty |= (dirtyBits0 != 0x0);
                isDataDirty |= (dirtyBits1 != 0x0);
                isDataDirty |= (dirtyBits2 != 0x0);
                return isDataDirty;
            }

            /*
            The propertyIndex argument counts up from 0 in the order defined in your schema component.
            It is not the schema field number itself. For example:
            component MyComponent
            {
                id = 1337;
                bool val_a = 1;
                bool val_b = 3;
            }
            In that case, val_a corresponds to propertyIndex 0 and val_b corresponds to propertyIndex 1 in this method.
            This method throws an InvalidOperationException in case your component doesn't contain properties.
            */
            public bool IsDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 18)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 17]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        return (dirtyBits0 & (0x1 << propertyIndex % 8)) != 0x0;
                    case 1:
                        return (dirtyBits1 & (0x1 << propertyIndex % 8)) != 0x0;
                    case 2:
                        return (dirtyBits2 & (0x1 << propertyIndex % 8)) != 0x0;
                }

                return false;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 18)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 17]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        dirtyBits0 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                    case 1:
                        dirtyBits1 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                    case 2:
                        dirtyBits2 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                }
            }

            public void MarkDataClean()
            {
                dirtyBits0 = 0x0;
                dirtyBits1 = 0x0;
                dirtyBits2 = 0x0;
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(new SchemaComponentData(197715));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private bool field1;

            public bool Field1
            {
                get => field1;
                set
                {
                    MarkDataDirty(0);
                    this.field1 = value;
                }
            }

            private float field2;

            public float Field2
            {
                get => field2;
                set
                {
                    MarkDataDirty(1);
                    this.field2 = value;
                }
            }

            internal uint field3Handle;

            public byte[] Field3
            {
                get => Improbable.Gdk.Tests.ExhaustiveSingular.ReferenceTypeProviders.Field3Provider.Get(field3Handle);
                set
                {
                    MarkDataDirty(2);
                    Improbable.Gdk.Tests.ExhaustiveSingular.ReferenceTypeProviders.Field3Provider.Set(field3Handle, value);
                }
            }

            private int field4;

            public int Field4
            {
                get => field4;
                set
                {
                    MarkDataDirty(3);
                    this.field4 = value;
                }
            }

            private long field5;

            public long Field5
            {
                get => field5;
                set
                {
                    MarkDataDirty(4);
                    this.field5 = value;
                }
            }

            private double field6;

            public double Field6
            {
                get => field6;
                set
                {
                    MarkDataDirty(5);
                    this.field6 = value;
                }
            }

            internal uint field7Handle;

            public string Field7
            {
                get => Improbable.Gdk.Tests.ExhaustiveSingular.ReferenceTypeProviders.Field7Provider.Get(field7Handle);
                set
                {
                    MarkDataDirty(6);
                    Improbable.Gdk.Tests.ExhaustiveSingular.ReferenceTypeProviders.Field7Provider.Set(field7Handle, value);
                }
            }

            private uint field8;

            public uint Field8
            {
                get => field8;
                set
                {
                    MarkDataDirty(7);
                    this.field8 = value;
                }
            }

            private ulong field9;

            public ulong Field9
            {
                get => field9;
                set
                {
                    MarkDataDirty(8);
                    this.field9 = value;
                }
            }

            private int field10;

            public int Field10
            {
                get => field10;
                set
                {
                    MarkDataDirty(9);
                    this.field10 = value;
                }
            }

            private long field11;

            public long Field11
            {
                get => field11;
                set
                {
                    MarkDataDirty(10);
                    this.field11 = value;
                }
            }

            private uint field12;

            public uint Field12
            {
                get => field12;
                set
                {
                    MarkDataDirty(11);
                    this.field12 = value;
                }
            }

            private ulong field13;

            public ulong Field13
            {
                get => field13;
                set
                {
                    MarkDataDirty(12);
                    this.field13 = value;
                }
            }

            private int field14;

            public int Field14
            {
                get => field14;
                set
                {
                    MarkDataDirty(13);
                    this.field14 = value;
                }
            }

            private long field15;

            public long Field15
            {
                get => field15;
                set
                {
                    MarkDataDirty(14);
                    this.field15 = value;
                }
            }

            private global::Improbable.Gdk.Core.EntityId field16;

            public global::Improbable.Gdk.Core.EntityId Field16
            {
                get => field16;
                set
                {
                    MarkDataDirty(15);
                    this.field16 = value;
                }
            }

            private global::Improbable.Gdk.Tests.SomeType field17;

            public global::Improbable.Gdk.Tests.SomeType Field17
            {
                get => field17;
                set
                {
                    MarkDataDirty(16);
                    this.field17 = value;
                }
            }

            private global::Improbable.Gdk.Tests.SomeEnum field18;

            public global::Improbable.Gdk.Tests.SomeEnum Field18
            {
                get => field18;
                set
                {
                    MarkDataDirty(17);
                    this.field18 = value;
                }
            }
        }

        public struct ComponentAuthority : ISharedComponentData, IEquatable<ComponentAuthority>
        {
            public bool HasAuthority;

            public ComponentAuthority(bool hasAuthority)
            {
                HasAuthority = hasAuthority;
            }

            // todo think about whether any of this is necessary
            // Unity does a bitwise equality check so this is just for users reading the struct
            public static readonly ComponentAuthority NotAuthoritative = new ComponentAuthority(false);
            public static readonly ComponentAuthority Authoritative = new ComponentAuthority(true);

            public bool Equals(ComponentAuthority other)
            {
                return this == other;
            }

            public override bool Equals(object obj)
            {
                return obj is ComponentAuthority auth && this == auth;
            }

            public override int GetHashCode()
            {
                return HasAuthority.GetHashCode();
            }

            public static bool operator ==(ComponentAuthority a, ComponentAuthority b)
            {
                return a.HasAuthority == b.HasAuthority;
            }

            public static bool operator !=(ComponentAuthority a, ComponentAuthority b)
            {
                return !(a == b);
            }
        }

        [global::System.Serializable]
        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 197715;

            public bool Field1;
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

            public Snapshot(bool field1, float field2, byte[] field3, int field4, long field5, double field6, string field7, uint field8, ulong field9, int field10, long field11, uint field12, ulong field13, int field14, long field15, global::Improbable.Gdk.Core.EntityId field16, global::Improbable.Gdk.Tests.SomeType field17, global::Improbable.Gdk.Tests.SomeEnum field18)
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
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.Gdk.Tests.ExhaustiveSingular.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddBool(1, component.Field1);
                }
                {
                    obj.AddFloat(2, component.Field2);
                }
                {
                    obj.AddBytes(3, component.Field3);
                }
                {
                    obj.AddInt32(4, component.Field4);
                }
                {
                    obj.AddInt64(5, component.Field5);
                }
                {
                    obj.AddDouble(6, component.Field6);
                }
                {
                    obj.AddString(7, component.Field7);
                }
                {
                    obj.AddUint32(8, component.Field8);
                }
                {
                    obj.AddUint64(9, component.Field9);
                }
                {
                    obj.AddSint32(10, component.Field10);
                }
                {
                    obj.AddSint64(11, component.Field11);
                }
                {
                    obj.AddFixed32(12, component.Field12);
                }
                {
                    obj.AddFixed64(13, component.Field13);
                }
                {
                    obj.AddSfixed32(14, component.Field14);
                }
                {
                    obj.AddSfixed64(15, component.Field15);
                }
                {
                    obj.AddEntityId(16, component.Field16);
                }
                {
                    global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(component.Field17, obj.AddObject(17));
                }
                {
                    obj.AddEnum(18, (uint) component.Field18);
                }
            }

            public static void SerializeUpdate(global::Improbable.Gdk.Tests.ExhaustiveSingular.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddBool(1, component.Field1);
                    }

                }
                {
                    if (component.IsDataDirty(1))
                    {
                        obj.AddFloat(2, component.Field2);
                    }

                }
                {
                    if (component.IsDataDirty(2))
                    {
                        obj.AddBytes(3, component.Field3);
                    }

                }
                {
                    if (component.IsDataDirty(3))
                    {
                        obj.AddInt32(4, component.Field4);
                    }

                }
                {
                    if (component.IsDataDirty(4))
                    {
                        obj.AddInt64(5, component.Field5);
                    }

                }
                {
                    if (component.IsDataDirty(5))
                    {
                        obj.AddDouble(6, component.Field6);
                    }

                }
                {
                    if (component.IsDataDirty(6))
                    {
                        obj.AddString(7, component.Field7);
                    }

                }
                {
                    if (component.IsDataDirty(7))
                    {
                        obj.AddUint32(8, component.Field8);
                    }

                }
                {
                    if (component.IsDataDirty(8))
                    {
                        obj.AddUint64(9, component.Field9);
                    }

                }
                {
                    if (component.IsDataDirty(9))
                    {
                        obj.AddSint32(10, component.Field10);
                    }

                }
                {
                    if (component.IsDataDirty(10))
                    {
                        obj.AddSint64(11, component.Field11);
                    }

                }
                {
                    if (component.IsDataDirty(11))
                    {
                        obj.AddFixed32(12, component.Field12);
                    }

                }
                {
                    if (component.IsDataDirty(12))
                    {
                        obj.AddFixed64(13, component.Field13);
                    }

                }
                {
                    if (component.IsDataDirty(13))
                    {
                        obj.AddSfixed32(14, component.Field14);
                    }

                }
                {
                    if (component.IsDataDirty(14))
                    {
                        obj.AddSfixed64(15, component.Field15);
                    }

                }
                {
                    if (component.IsDataDirty(15))
                    {
                        obj.AddEntityId(16, component.Field16);
                    }

                }
                {
                    if (component.IsDataDirty(16))
                    {
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(component.Field17, obj.AddObject(17));
                    }

                }
                {
                    if (component.IsDataDirty(17))
                    {
                        obj.AddEnum(18, (uint) component.Field18);
                    }

                }
            }

            public static void SerializeUpdate(global::Improbable.Gdk.Tests.ExhaustiveSingular.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.Field1.HasValue)
                    {
                        var field = update.Field1.Value;
                        obj.AddBool(1, field);
                    }
                }
                {
                    if (update.Field2.HasValue)
                    {
                        var field = update.Field2.Value;
                        obj.AddFloat(2, field);
                    }
                }
                {
                    if (update.Field3.HasValue)
                    {
                        var field = update.Field3.Value;
                        obj.AddBytes(3, field);
                    }
                }
                {
                    if (update.Field4.HasValue)
                    {
                        var field = update.Field4.Value;
                        obj.AddInt32(4, field);
                    }
                }
                {
                    if (update.Field5.HasValue)
                    {
                        var field = update.Field5.Value;
                        obj.AddInt64(5, field);
                    }
                }
                {
                    if (update.Field6.HasValue)
                    {
                        var field = update.Field6.Value;
                        obj.AddDouble(6, field);
                    }
                }
                {
                    if (update.Field7.HasValue)
                    {
                        var field = update.Field7.Value;
                        obj.AddString(7, field);
                    }
                }
                {
                    if (update.Field8.HasValue)
                    {
                        var field = update.Field8.Value;
                        obj.AddUint32(8, field);
                    }
                }
                {
                    if (update.Field9.HasValue)
                    {
                        var field = update.Field9.Value;
                        obj.AddUint64(9, field);
                    }
                }
                {
                    if (update.Field10.HasValue)
                    {
                        var field = update.Field10.Value;
                        obj.AddSint32(10, field);
                    }
                }
                {
                    if (update.Field11.HasValue)
                    {
                        var field = update.Field11.Value;
                        obj.AddSint64(11, field);
                    }
                }
                {
                    if (update.Field12.HasValue)
                    {
                        var field = update.Field12.Value;
                        obj.AddFixed32(12, field);
                    }
                }
                {
                    if (update.Field13.HasValue)
                    {
                        var field = update.Field13.Value;
                        obj.AddFixed64(13, field);
                    }
                }
                {
                    if (update.Field14.HasValue)
                    {
                        var field = update.Field14.Value;
                        obj.AddSfixed32(14, field);
                    }
                }
                {
                    if (update.Field15.HasValue)
                    {
                        var field = update.Field15.Value;
                        obj.AddSfixed64(15, field);
                    }
                }
                {
                    if (update.Field16.HasValue)
                    {
                        var field = update.Field16.Value;
                        obj.AddEntityId(16, field);
                    }
                }
                {
                    if (update.Field17.HasValue)
                    {
                        var field = update.Field17.Value;
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(field, obj.AddObject(17));
                    }
                }
                {
                    if (update.Field18.HasValue)
                    {
                        var field = update.Field18.Value;
                        obj.AddEnum(18, (uint) field);
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.Gdk.Tests.ExhaustiveSingular.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, snapshot.Field1);
                }
                {
                    obj.AddFloat(2, snapshot.Field2);
                }
                {
                    obj.AddBytes(3, snapshot.Field3);
                }
                {
                    obj.AddInt32(4, snapshot.Field4);
                }
                {
                    obj.AddInt64(5, snapshot.Field5);
                }
                {
                    obj.AddDouble(6, snapshot.Field6);
                }
                {
                    obj.AddString(7, snapshot.Field7);
                }
                {
                    obj.AddUint32(8, snapshot.Field8);
                }
                {
                    obj.AddUint64(9, snapshot.Field9);
                }
                {
                    obj.AddSint32(10, snapshot.Field10);
                }
                {
                    obj.AddSint64(11, snapshot.Field11);
                }
                {
                    obj.AddFixed32(12, snapshot.Field12);
                }
                {
                    obj.AddFixed64(13, snapshot.Field13);
                }
                {
                    obj.AddSfixed32(14, snapshot.Field14);
                }
                {
                    obj.AddSfixed64(15, snapshot.Field15);
                }
                {
                    obj.AddEntityId(16, snapshot.Field16);
                }
                {
                    global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(snapshot.Field17, obj.AddObject(17));
                }
                {
                    obj.AddEnum(18, (uint) snapshot.Field18);
                }
            }

            public static global::Improbable.Gdk.Tests.ExhaustiveSingular.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.Gdk.Tests.ExhaustiveSingular.Component();

                {
                    component.Field1 = obj.GetBool(1);
                }
                {
                    component.Field2 = obj.GetFloat(2);
                }
                component.field3Handle = Improbable.Gdk.Tests.ExhaustiveSingular.ReferenceTypeProviders.Field3Provider.Allocate(world);
                {
                    component.Field3 = obj.GetBytes(3);
                }
                {
                    component.Field4 = obj.GetInt32(4);
                }
                {
                    component.Field5 = obj.GetInt64(5);
                }
                {
                    component.Field6 = obj.GetDouble(6);
                }
                component.field7Handle = Improbable.Gdk.Tests.ExhaustiveSingular.ReferenceTypeProviders.Field7Provider.Allocate(world);
                {
                    component.Field7 = obj.GetString(7);
                }
                {
                    component.Field8 = obj.GetUint32(8);
                }
                {
                    component.Field9 = obj.GetUint64(9);
                }
                {
                    component.Field10 = obj.GetSint32(10);
                }
                {
                    component.Field11 = obj.GetSint64(11);
                }
                {
                    component.Field12 = obj.GetFixed32(12);
                }
                {
                    component.Field13 = obj.GetFixed64(13);
                }
                {
                    component.Field14 = obj.GetSfixed32(14);
                }
                {
                    component.Field15 = obj.GetSfixed64(15);
                }
                {
                    component.Field16 = obj.GetEntityIdStruct(16);
                }
                {
                    component.Field17 = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                }
                {
                    component.Field18 = (global::Improbable.Gdk.Tests.SomeEnum) obj.GetEnum(18);
                }
                return component;
            }

            public static global::Improbable.Gdk.Tests.ExhaustiveSingular.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.Gdk.Tests.ExhaustiveSingular.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        update.Field1 = new global::Improbable.Gdk.Core.Option<bool>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(2) == 1)
                    {
                        var value = obj.GetFloat(2);
                        update.Field2 = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                {
                    if (obj.GetBytesCount(3) == 1)
                    {
                        var value = obj.GetBytes(3);
                        update.Field3 = new global::Improbable.Gdk.Core.Option<byte[]>(value);
                    }
                    
                }
                {
                    if (obj.GetInt32Count(4) == 1)
                    {
                        var value = obj.GetInt32(4);
                        update.Field4 = new global::Improbable.Gdk.Core.Option<int>(value);
                    }
                    
                }
                {
                    if (obj.GetInt64Count(5) == 1)
                    {
                        var value = obj.GetInt64(5);
                        update.Field5 = new global::Improbable.Gdk.Core.Option<long>(value);
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(6) == 1)
                    {
                        var value = obj.GetDouble(6);
                        update.Field6 = new global::Improbable.Gdk.Core.Option<double>(value);
                    }
                    
                }
                {
                    if (obj.GetStringCount(7) == 1)
                    {
                        var value = obj.GetString(7);
                        update.Field7 = new global::Improbable.Gdk.Core.Option<string>(value);
                    }
                    
                }
                {
                    if (obj.GetUint32Count(8) == 1)
                    {
                        var value = obj.GetUint32(8);
                        update.Field8 = new global::Improbable.Gdk.Core.Option<uint>(value);
                    }
                    
                }
                {
                    if (obj.GetUint64Count(9) == 1)
                    {
                        var value = obj.GetUint64(9);
                        update.Field9 = new global::Improbable.Gdk.Core.Option<ulong>(value);
                    }
                    
                }
                {
                    if (obj.GetSint32Count(10) == 1)
                    {
                        var value = obj.GetSint32(10);
                        update.Field10 = new global::Improbable.Gdk.Core.Option<int>(value);
                    }
                    
                }
                {
                    if (obj.GetSint64Count(11) == 1)
                    {
                        var value = obj.GetSint64(11);
                        update.Field11 = new global::Improbable.Gdk.Core.Option<long>(value);
                    }
                    
                }
                {
                    if (obj.GetFixed32Count(12) == 1)
                    {
                        var value = obj.GetFixed32(12);
                        update.Field12 = new global::Improbable.Gdk.Core.Option<uint>(value);
                    }
                    
                }
                {
                    if (obj.GetFixed64Count(13) == 1)
                    {
                        var value = obj.GetFixed64(13);
                        update.Field13 = new global::Improbable.Gdk.Core.Option<ulong>(value);
                    }
                    
                }
                {
                    if (obj.GetSfixed32Count(14) == 1)
                    {
                        var value = obj.GetSfixed32(14);
                        update.Field14 = new global::Improbable.Gdk.Core.Option<int>(value);
                    }
                    
                }
                {
                    if (obj.GetSfixed64Count(15) == 1)
                    {
                        var value = obj.GetSfixed64(15);
                        update.Field15 = new global::Improbable.Gdk.Core.Option<long>(value);
                    }
                    
                }
                {
                    if (obj.GetEntityIdCount(16) == 1)
                    {
                        var value = obj.GetEntityIdStruct(16);
                        update.Field16 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntityId>(value);
                    }
                    
                }
                {
                    if (obj.GetObjectCount(17) == 1)
                    {
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                        update.Field17 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Tests.SomeType>(value);
                    }
                    
                }
                {
                    if (obj.GetEnumCount(18) == 1)
                    {
                        var value = (global::Improbable.Gdk.Tests.SomeEnum) obj.GetEnum(18);
                        update.Field18 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Tests.SomeEnum>(value);
                    }
                    
                }
                return update;
            }

            public static global::Improbable.Gdk.Tests.ExhaustiveSingular.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.Gdk.Tests.ExhaustiveSingular.Update();
                var obj = data.GetFields();

                {
                    var value = obj.GetBool(1);
                    update.Field1 = new global::Improbable.Gdk.Core.Option<bool>(value);
                    
                }
                {
                    var value = obj.GetFloat(2);
                    update.Field2 = new global::Improbable.Gdk.Core.Option<float>(value);
                    
                }
                {
                    var value = obj.GetBytes(3);
                    update.Field3 = new global::Improbable.Gdk.Core.Option<byte[]>(value);
                    
                }
                {
                    var value = obj.GetInt32(4);
                    update.Field4 = new global::Improbable.Gdk.Core.Option<int>(value);
                    
                }
                {
                    var value = obj.GetInt64(5);
                    update.Field5 = new global::Improbable.Gdk.Core.Option<long>(value);
                    
                }
                {
                    var value = obj.GetDouble(6);
                    update.Field6 = new global::Improbable.Gdk.Core.Option<double>(value);
                    
                }
                {
                    var value = obj.GetString(7);
                    update.Field7 = new global::Improbable.Gdk.Core.Option<string>(value);
                    
                }
                {
                    var value = obj.GetUint32(8);
                    update.Field8 = new global::Improbable.Gdk.Core.Option<uint>(value);
                    
                }
                {
                    var value = obj.GetUint64(9);
                    update.Field9 = new global::Improbable.Gdk.Core.Option<ulong>(value);
                    
                }
                {
                    var value = obj.GetSint32(10);
                    update.Field10 = new global::Improbable.Gdk.Core.Option<int>(value);
                    
                }
                {
                    var value = obj.GetSint64(11);
                    update.Field11 = new global::Improbable.Gdk.Core.Option<long>(value);
                    
                }
                {
                    var value = obj.GetFixed32(12);
                    update.Field12 = new global::Improbable.Gdk.Core.Option<uint>(value);
                    
                }
                {
                    var value = obj.GetFixed64(13);
                    update.Field13 = new global::Improbable.Gdk.Core.Option<ulong>(value);
                    
                }
                {
                    var value = obj.GetSfixed32(14);
                    update.Field14 = new global::Improbable.Gdk.Core.Option<int>(value);
                    
                }
                {
                    var value = obj.GetSfixed64(15);
                    update.Field15 = new global::Improbable.Gdk.Core.Option<long>(value);
                    
                }
                {
                    var value = obj.GetEntityIdStruct(16);
                    update.Field16 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntityId>(value);
                    
                }
                {
                    var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                    update.Field17 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Tests.SomeType>(value);
                    
                }
                {
                    var value = (global::Improbable.Gdk.Tests.SomeEnum) obj.GetEnum(18);
                    update.Field18 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Tests.SomeEnum>(value);
                    
                }
                return update;
            }

            public static global::Improbable.Gdk.Tests.ExhaustiveSingular.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.Gdk.Tests.ExhaustiveSingular.Snapshot();

                {
                    component.Field1 = obj.GetBool(1);
                }

                {
                    component.Field2 = obj.GetFloat(2);
                }

                {
                    component.Field3 = obj.GetBytes(3);
                }

                {
                    component.Field4 = obj.GetInt32(4);
                }

                {
                    component.Field5 = obj.GetInt64(5);
                }

                {
                    component.Field6 = obj.GetDouble(6);
                }

                {
                    component.Field7 = obj.GetString(7);
                }

                {
                    component.Field8 = obj.GetUint32(8);
                }

                {
                    component.Field9 = obj.GetUint64(9);
                }

                {
                    component.Field10 = obj.GetSint32(10);
                }

                {
                    component.Field11 = obj.GetSint64(11);
                }

                {
                    component.Field12 = obj.GetFixed32(12);
                }

                {
                    component.Field13 = obj.GetFixed64(13);
                }

                {
                    component.Field14 = obj.GetSfixed32(14);
                }

                {
                    component.Field15 = obj.GetSfixed64(15);
                }

                {
                    component.Field16 = obj.GetEntityIdStruct(16);
                }

                {
                    component.Field17 = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                }

                {
                    component.Field18 = (global::Improbable.Gdk.Tests.SomeEnum) obj.GetEnum(18);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Gdk.Tests.ExhaustiveSingular.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        component.Field1 = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(2) == 1)
                    {
                        var value = obj.GetFloat(2);
                        component.Field2 = value;
                    }
                    
                }
                {
                    if (obj.GetBytesCount(3) == 1)
                    {
                        var value = obj.GetBytes(3);
                        component.Field3 = value;
                    }
                    
                }
                {
                    if (obj.GetInt32Count(4) == 1)
                    {
                        var value = obj.GetInt32(4);
                        component.Field4 = value;
                    }
                    
                }
                {
                    if (obj.GetInt64Count(5) == 1)
                    {
                        var value = obj.GetInt64(5);
                        component.Field5 = value;
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(6) == 1)
                    {
                        var value = obj.GetDouble(6);
                        component.Field6 = value;
                    }
                    
                }
                {
                    if (obj.GetStringCount(7) == 1)
                    {
                        var value = obj.GetString(7);
                        component.Field7 = value;
                    }
                    
                }
                {
                    if (obj.GetUint32Count(8) == 1)
                    {
                        var value = obj.GetUint32(8);
                        component.Field8 = value;
                    }
                    
                }
                {
                    if (obj.GetUint64Count(9) == 1)
                    {
                        var value = obj.GetUint64(9);
                        component.Field9 = value;
                    }
                    
                }
                {
                    if (obj.GetSint32Count(10) == 1)
                    {
                        var value = obj.GetSint32(10);
                        component.Field10 = value;
                    }
                    
                }
                {
                    if (obj.GetSint64Count(11) == 1)
                    {
                        var value = obj.GetSint64(11);
                        component.Field11 = value;
                    }
                    
                }
                {
                    if (obj.GetFixed32Count(12) == 1)
                    {
                        var value = obj.GetFixed32(12);
                        component.Field12 = value;
                    }
                    
                }
                {
                    if (obj.GetFixed64Count(13) == 1)
                    {
                        var value = obj.GetFixed64(13);
                        component.Field13 = value;
                    }
                    
                }
                {
                    if (obj.GetSfixed32Count(14) == 1)
                    {
                        var value = obj.GetSfixed32(14);
                        component.Field14 = value;
                    }
                    
                }
                {
                    if (obj.GetSfixed64Count(15) == 1)
                    {
                        var value = obj.GetSfixed64(15);
                        component.Field15 = value;
                    }
                    
                }
                {
                    if (obj.GetEntityIdCount(16) == 1)
                    {
                        var value = obj.GetEntityIdStruct(16);
                        component.Field16 = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(17) == 1)
                    {
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                        component.Field17 = value;
                    }
                    
                }
                {
                    if (obj.GetEnumCount(18) == 1)
                    {
                        var value = (global::Improbable.Gdk.Tests.SomeEnum) obj.GetEnum(18);
                        component.Field18 = value;
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Gdk.Tests.ExhaustiveSingular.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        snapshot.Field1 = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(2) == 1)
                    {
                        var value = obj.GetFloat(2);
                        snapshot.Field2 = value;
                    }
                    
                }
                {
                    if (obj.GetBytesCount(3) == 1)
                    {
                        var value = obj.GetBytes(3);
                        snapshot.Field3 = value;
                    }
                    
                }
                {
                    if (obj.GetInt32Count(4) == 1)
                    {
                        var value = obj.GetInt32(4);
                        snapshot.Field4 = value;
                    }
                    
                }
                {
                    if (obj.GetInt64Count(5) == 1)
                    {
                        var value = obj.GetInt64(5);
                        snapshot.Field5 = value;
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(6) == 1)
                    {
                        var value = obj.GetDouble(6);
                        snapshot.Field6 = value;
                    }
                    
                }
                {
                    if (obj.GetStringCount(7) == 1)
                    {
                        var value = obj.GetString(7);
                        snapshot.Field7 = value;
                    }
                    
                }
                {
                    if (obj.GetUint32Count(8) == 1)
                    {
                        var value = obj.GetUint32(8);
                        snapshot.Field8 = value;
                    }
                    
                }
                {
                    if (obj.GetUint64Count(9) == 1)
                    {
                        var value = obj.GetUint64(9);
                        snapshot.Field9 = value;
                    }
                    
                }
                {
                    if (obj.GetSint32Count(10) == 1)
                    {
                        var value = obj.GetSint32(10);
                        snapshot.Field10 = value;
                    }
                    
                }
                {
                    if (obj.GetSint64Count(11) == 1)
                    {
                        var value = obj.GetSint64(11);
                        snapshot.Field11 = value;
                    }
                    
                }
                {
                    if (obj.GetFixed32Count(12) == 1)
                    {
                        var value = obj.GetFixed32(12);
                        snapshot.Field12 = value;
                    }
                    
                }
                {
                    if (obj.GetFixed64Count(13) == 1)
                    {
                        var value = obj.GetFixed64(13);
                        snapshot.Field13 = value;
                    }
                    
                }
                {
                    if (obj.GetSfixed32Count(14) == 1)
                    {
                        var value = obj.GetSfixed32(14);
                        snapshot.Field14 = value;
                    }
                    
                }
                {
                    if (obj.GetSfixed64Count(15) == 1)
                    {
                        var value = obj.GetSfixed64(15);
                        snapshot.Field15 = value;
                    }
                    
                }
                {
                    if (obj.GetEntityIdCount(16) == 1)
                    {
                        var value = obj.GetEntityIdStruct(16);
                        snapshot.Field16 = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(17) == 1)
                    {
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                        snapshot.Field17 = value;
                    }
                    
                }
                {
                    if (obj.GetEnumCount(18) == 1)
                    {
                        var value = (global::Improbable.Gdk.Tests.SomeEnum) obj.GetEnum(18);
                        snapshot.Field18 = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<bool> Field1;
            public Option<float> Field2;
            public Option<byte[]> Field3;
            public Option<int> Field4;
            public Option<long> Field5;
            public Option<double> Field6;
            public Option<string> Field7;
            public Option<uint> Field8;
            public Option<ulong> Field9;
            public Option<int> Field10;
            public Option<long> Field11;
            public Option<uint> Field12;
            public Option<ulong> Field13;
            public Option<int> Field14;
            public Option<long> Field15;
            public Option<global::Improbable.Gdk.Core.EntityId> Field16;
            public Option<global::Improbable.Gdk.Tests.SomeType> Field17;
            public Option<global::Improbable.Gdk.Tests.SomeEnum> Field18;
        }

#if !DISABLE_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Improbable.Gdk.Tests.ExhaustiveSingular.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class ExhaustiveSingularDynamic : IDynamicInvokable
        {
            public uint ComponentId => ExhaustiveSingular.ComponentId;

            private static Component DeserializeData(ComponentData data, World world)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.Deserialize(schemaDataOpt.Value.GetFields(), world);
            }

            private static Update DeserializeUpdate(ComponentUpdate update, World world)
            {
                var schemaDataOpt = update.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentUpdate)}");
                }

                return Serialization.DeserializeUpdate(schemaDataOpt.Value);
            }

            private static Snapshot DeserializeSnapshot(ComponentData snapshot)
            {
                var schemaDataOpt = snapshot.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.DeserializeSnapshot(schemaDataOpt.Value.GetFields());
            }

            private static void SerializeSnapshot(Snapshot snapshot, ComponentData data)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not serialise an empty {nameof(ComponentData)}");
                }

                Serialization.SerializeSnapshot(snapshot, data.SchemaData.Value.GetFields());
            }

            private static Update SnapshotToUpdate(in Snapshot snapshot)
            {
                var update = new Update();
                update.Field1 = new Option<bool>(snapshot.Field1);
                update.Field2 = new Option<float>(snapshot.Field2);
                update.Field3 = new Option<byte[]>(snapshot.Field3);
                update.Field4 = new Option<int>(snapshot.Field4);
                update.Field5 = new Option<long>(snapshot.Field5);
                update.Field6 = new Option<double>(snapshot.Field6);
                update.Field7 = new Option<string>(snapshot.Field7);
                update.Field8 = new Option<uint>(snapshot.Field8);
                update.Field9 = new Option<ulong>(snapshot.Field9);
                update.Field10 = new Option<int>(snapshot.Field10);
                update.Field11 = new Option<long>(snapshot.Field11);
                update.Field12 = new Option<uint>(snapshot.Field12);
                update.Field13 = new Option<ulong>(snapshot.Field13);
                update.Field14 = new Option<int>(snapshot.Field14);
                update.Field15 = new Option<long>(snapshot.Field15);
                update.Field16 = new Option<global::Improbable.Gdk.Core.EntityId>(snapshot.Field16);
                update.Field17 = new Option<global::Improbable.Gdk.Tests.SomeType>(snapshot.Field17);
                update.Field18 = new Option<global::Improbable.Gdk.Tests.SomeEnum>(snapshot.Field18);
                return update;
            }

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Component, Update>(ComponentId, DeserializeData, DeserializeUpdate);
            }

            public void InvokeSnapshotHandler(DynamicSnapshot.ISnapshotHandler handler)
            {
                handler.Accept<Snapshot>(ComponentId, DeserializeSnapshot, SerializeSnapshot);
            }

            public void InvokeConvertHandler(DynamicConverter.IConverterHandler handler)
            {
                handler.Accept<Snapshot, Update>(ComponentId, SnapshotToUpdate);
            }
        }
    }
}
