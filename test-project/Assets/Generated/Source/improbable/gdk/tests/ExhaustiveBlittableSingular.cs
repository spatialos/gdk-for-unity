// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveBlittableSingular
    {
        public const uint ComponentId = 197720;

        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 197720;

            private BlittableBool isDirty;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            // Each byte tracks 8 component properties.
            private byte dirtyBits0;
            private byte dirtyBits1;

            public bool IsDirty()
            {
                return isDirty;
            }

            /*
            The propertyIndex arguments starts counting from 0. It depends on the order of which you defined
            your component properties in a schema component but is not the schema field number itself. E.g.
            component MyComponent
            {
                id = 1337;
                bool val_a = 1;
                bool val_b = 3;
            }
            In that case, val_a uses propertyIndex 0 and val_b uses propertyIndex 1 in this method.
            */
            public bool IsDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 15)
                {
                    throw new ArgumentException("propertyIndex argument out of range.");
                }

                var byteBatch = propertyIndex / 8;
                switch (byteBatch)
                {
                    case 0:
                        return (dirtyBits0 & (0x1 << propertyIndex % 8)) != 0x0;
                    case 1:
                        return (dirtyBits1 & (0x1 << propertyIndex % 8)) != 0x0;
                    default:
                        throw new ArgumentException("propertyIndex argument out of range.");
                }
            }

            // like the IsDirty() method above, the propertyIndex arguments starts counting from 0.
            public void MarkDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 15)
                {
                    throw new ArgumentException("propertyIndex argument out of range.");
                }

                var byteBatch = propertyIndex / 8;
                switch (byteBatch)
                {
                    case 0:
                        dirtyBits0 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                    case 1:
                        dirtyBits1 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                    default:
                        throw new ArgumentException("propertyIndex argument out of range.");
                }

                isDirty = true;
            }

            public void MarkNotDirty()
            {
                dirtyBits0 = 0x0;
                dirtyBits1 = 0x0;
                isDirty = false;
            }

            private BlittableBool field1;

            public BlittableBool Field1
            {
                get => field1;
                set
                {
                    MarkDirty(0);
                    field1 = value;
                }
            }

            private float field2;

            public float Field2
            {
                get => field2;
                set
                {
                    MarkDirty(1);
                    field2 = value;
                }
            }

            private int field4;

            public int Field4
            {
                get => field4;
                set
                {
                    MarkDirty(2);
                    field4 = value;
                }
            }

            private long field5;

            public long Field5
            {
                get => field5;
                set
                {
                    MarkDirty(3);
                    field5 = value;
                }
            }

            private double field6;

            public double Field6
            {
                get => field6;
                set
                {
                    MarkDirty(4);
                    field6 = value;
                }
            }

            private uint field8;

            public uint Field8
            {
                get => field8;
                set
                {
                    MarkDirty(5);
                    field8 = value;
                }
            }

            private ulong field9;

            public ulong Field9
            {
                get => field9;
                set
                {
                    MarkDirty(6);
                    field9 = value;
                }
            }

            private int field10;

            public int Field10
            {
                get => field10;
                set
                {
                    MarkDirty(7);
                    field10 = value;
                }
            }

            private long field11;

            public long Field11
            {
                get => field11;
                set
                {
                    MarkDirty(8);
                    field11 = value;
                }
            }

            private uint field12;

            public uint Field12
            {
                get => field12;
                set
                {
                    MarkDirty(9);
                    field12 = value;
                }
            }

            private ulong field13;

            public ulong Field13
            {
                get => field13;
                set
                {
                    MarkDirty(10);
                    field13 = value;
                }
            }

            private int field14;

            public int Field14
            {
                get => field14;
                set
                {
                    MarkDirty(11);
                    field14 = value;
                }
            }

            private long field15;

            public long Field15
            {
                get => field15;
                set
                {
                    MarkDirty(12);
                    field15 = value;
                }
            }

            private global::Improbable.Worker.EntityId field16;

            public global::Improbable.Worker.EntityId Field16
            {
                get => field16;
                set
                {
                    MarkDirty(13);
                    field16 = value;
                }
            }

            private global::Improbable.Gdk.Tests.SomeType field17;

            public global::Improbable.Gdk.Tests.SomeType Field17
            {
                get => field17;
                set
                {
                    MarkDirty(14);
                    field17 = value;
                }
            }

            public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
                BlittableBool field1,
                float field2,
                int field4,
                long field5,
                double field6,
                uint field8,
                ulong field9,
                int field10,
                long field11,
                uint field12,
                ulong field13,
                int field14,
                long field15,
                global::Improbable.Worker.EntityId field16,
                global::Improbable.Gdk.Tests.SomeType field17
        )
            {
                var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(197720);
                var obj = schemaComponentData.GetFields();
                {
                    obj.AddBool(1, field1);
                }
                {
                    obj.AddFloat(2, field2);
                }
                {
                    obj.AddInt32(4, field4);
                }
                {
                    obj.AddInt64(5, field5);
                }
                {
                    obj.AddDouble(6, field6);
                }
                {
                    obj.AddUint32(8, field8);
                }
                {
                    obj.AddUint64(9, field9);
                }
                {
                    obj.AddSint32(10, field10);
                }
                {
                    obj.AddSint64(11, field11);
                }
                {
                    obj.AddFixed32(12, field12);
                }
                {
                    obj.AddFixed64(13, field13);
                }
                {
                    obj.AddSfixed32(14, field14);
                }
                {
                    obj.AddSfixed64(15, field15);
                }
                {
                    obj.AddEntityId(16, field16);
                }
                {
                    global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(field17, obj.AddObject(17));
                }
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void SerializeUpdate(Improbable.Gdk.Tests.ExhaustiveBlittableSingular.Component component, global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDirty(0))
                    {
                        obj.AddBool(1, component.Field1);
                    }

                }
                {
                    if (component.IsDirty(1))
                    {
                        obj.AddFloat(2, component.Field2);
                    }

                }
                {
                    if (component.IsDirty(2))
                    {
                        obj.AddInt32(4, component.Field4);
                    }

                }
                {
                    if (component.IsDirty(3))
                    {
                        obj.AddInt64(5, component.Field5);
                    }

                }
                {
                    if (component.IsDirty(4))
                    {
                        obj.AddDouble(6, component.Field6);
                    }

                }
                {
                    if (component.IsDirty(5))
                    {
                        obj.AddUint32(8, component.Field8);
                    }

                }
                {
                    if (component.IsDirty(6))
                    {
                        obj.AddUint64(9, component.Field9);
                    }

                }
                {
                    if (component.IsDirty(7))
                    {
                        obj.AddSint32(10, component.Field10);
                    }

                }
                {
                    if (component.IsDirty(8))
                    {
                        obj.AddSint64(11, component.Field11);
                    }

                }
                {
                    if (component.IsDirty(9))
                    {
                        obj.AddFixed32(12, component.Field12);
                    }

                }
                {
                    if (component.IsDirty(10))
                    {
                        obj.AddFixed64(13, component.Field13);
                    }

                }
                {
                    if (component.IsDirty(11))
                    {
                        obj.AddSfixed32(14, component.Field14);
                    }

                }
                {
                    if (component.IsDirty(12))
                    {
                        obj.AddSfixed64(15, component.Field15);
                    }

                }
                {
                    if (component.IsDirty(13))
                    {
                        obj.AddEntityId(16, component.Field16);
                    }

                }
                {
                    if (component.IsDirty(14))
                    {
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(component.Field17, obj.AddObject(17));
                    }

                }
            }

            public static Improbable.Gdk.Tests.ExhaustiveBlittableSingular.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.ExhaustiveBlittableSingular.Component();

                {
                    component.Field1 = obj.GetBool(1);
                }
                {
                    component.Field2 = obj.GetFloat(2);
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
                    component.Field16 = obj.GetEntityId(16);
                }
                {
                    component.Field17 = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                }
                return component;
            }

            public static Improbable.Gdk.Tests.ExhaustiveBlittableSingular.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.ExhaustiveBlittableSingular.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        update.Field1 = new global::Improbable.Gdk.Core.Option<BlittableBool>(value);
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
                        var value = obj.GetEntityId(16);
                        update.Field16 = new global::Improbable.Gdk.Core.Option<global::Improbable.Worker.EntityId>(value);
                    }
                    
                }
                {
                    if (obj.GetObjectCount(17) == 1)
                    {
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.GetObject(17));
                        update.Field17 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Tests.SomeType>(value);
                    }
                    
                }
                return update;
            }

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.ExhaustiveBlittableSingular.Component component)
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
                        var value = obj.GetEntityId(16);
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
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<BlittableBool> Field1;
            public Option<float> Field2;
            public Option<int> Field4;
            public Option<long> Field5;
            public Option<double> Field6;
            public Option<uint> Field8;
            public Option<ulong> Field9;
            public Option<int> Field10;
            public Option<long> Field11;
            public Option<uint> Field12;
            public Option<ulong> Field13;
            public Option<int> Field14;
            public Option<long> Field15;
            public Option<global::Improbable.Worker.EntityId> Field16;
            public Option<global::Improbable.Gdk.Tests.SomeType> Field17;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.ExhaustiveBlittableSingular.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class ExhaustiveBlittableSingularDynamic : IDynamicInvokable
        {
            public uint ComponentId => ExhaustiveBlittableSingular.ComponentId;

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

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Component, Update>(ExhaustiveBlittableSingular.ComponentId, DeserializeData, DeserializeUpdate);
            }
        }
    }
}
