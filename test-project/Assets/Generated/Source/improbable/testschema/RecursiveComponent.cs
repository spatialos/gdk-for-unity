// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Entities;

namespace Improbable.TestSchema
{
    public partial class RecursiveComponent
    {
        public const uint ComponentId = 18800;

        public unsafe struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            // Bit masks for tracking which component properties were changed locally and need to be synced.
            private fixed UInt32 dirtyBits[1];

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.TestSchema.TypeA>.ReferenceHandle aHandle;

            public global::Improbable.TestSchema.TypeA A
            {
                get => aHandle.Get();
                set
                {
                    MarkDataDirty(0);
                    aHandle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.TestSchema.TypeB>.ReferenceHandle bHandle;

            public global::Improbable.TestSchema.TypeB B
            {
                get => bHandle.Get();
                set
                {
                    MarkDataDirty(1);
                    bHandle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.TestSchema.TypeC>.ReferenceHandle cHandle;

            public global::Improbable.TestSchema.TypeC C
            {
                get => cHandle.Get();
                set
                {
                    MarkDataDirty(2);
                    cHandle.Set(value);
                }
            }

            public bool IsDataDirty()
            {
                var isDataDirty = false;

                isDataDirty |= (dirtyBits[0] != 0x0);

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
                ValidateFieldIndex(propertyIndex);

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex >> 4;
                return (dirtyBits[dirtyBitsByteIndex] & (0x1 << (propertyIndex & 31))) != 0x0;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                ValidateFieldIndex(propertyIndex);

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex >> 4;
                dirtyBits[dirtyBitsByteIndex] |= (UInt32) (0x1 << (propertyIndex & 31));
            }

            public void MarkDataClean()
            {
                dirtyBits[0] = 0x0;
            }

            [Conditional("DEBUG")]
            private void ValidateFieldIndex(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 3)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 2]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(18800, SchemaComponentData.Create());
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }
        }

        public struct HasAuthority : IComponentData
        {
        }

        [global::System.Serializable]
        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 18800;

            public global::Improbable.TestSchema.TypeA A;
            public global::Improbable.TestSchema.TypeB B;
            public global::Improbable.TestSchema.TypeC C;

            public Snapshot(global::Improbable.TestSchema.TypeA a, global::Improbable.TestSchema.TypeB b, global::Improbable.TestSchema.TypeC c)
            {
                A = a;
                B = b;
                C = c;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.TestSchema.RecursiveComponent.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                global::Improbable.TestSchema.TypeA.Serialization.Serialize(component.A, obj.AddObject(1));

                global::Improbable.TestSchema.TypeB.Serialization.Serialize(component.B, obj.AddObject(2));

                global::Improbable.TestSchema.TypeC.Serialization.Serialize(component.C, obj.AddObject(3));
            }

            public static void SerializeUpdate(global::Improbable.TestSchema.RecursiveComponent.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                if (component.IsDataDirty(0))
                {
                    global::Improbable.TestSchema.TypeA.Serialization.Serialize(component.A, obj.AddObject(1));
                }

                if (component.IsDataDirty(1))
                {
                    global::Improbable.TestSchema.TypeB.Serialization.Serialize(component.B, obj.AddObject(2));
                }

                if (component.IsDataDirty(2))
                {
                    global::Improbable.TestSchema.TypeC.Serialization.Serialize(component.C, obj.AddObject(3));
                }
            }

            public static void SerializeUpdate(global::Improbable.TestSchema.RecursiveComponent.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                {
                    if (update.A.HasValue)
                    {
                        var field = update.A.Value;

                        global::Improbable.TestSchema.TypeA.Serialization.Serialize(field, obj.AddObject(1));
                    }
                }

                {
                    if (update.B.HasValue)
                    {
                        var field = update.B.Value;

                        global::Improbable.TestSchema.TypeB.Serialization.Serialize(field, obj.AddObject(2));
                    }
                }

                {
                    if (update.C.HasValue)
                    {
                        var field = update.C.Value;

                        global::Improbable.TestSchema.TypeC.Serialization.Serialize(field, obj.AddObject(3));
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.TestSchema.RecursiveComponent.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                global::Improbable.TestSchema.TypeA.Serialization.Serialize(snapshot.A, obj.AddObject(1));

                global::Improbable.TestSchema.TypeB.Serialization.Serialize(snapshot.B, obj.AddObject(2));

                global::Improbable.TestSchema.TypeC.Serialization.Serialize(snapshot.C, obj.AddObject(3));
            }

            public static global::Improbable.TestSchema.RecursiveComponent.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.TestSchema.RecursiveComponent.Component();

                component.aHandle = global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.TestSchema.TypeA>.Create();

                component.A = global::Improbable.TestSchema.TypeA.Serialization.Deserialize(obj.GetObject(1));

                component.bHandle = global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.TestSchema.TypeB>.Create();

                component.B = global::Improbable.TestSchema.TypeB.Serialization.Deserialize(obj.GetObject(2));

                component.cHandle = global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.TestSchema.TypeC>.Create();

                component.C = global::Improbable.TestSchema.TypeC.Serialization.Deserialize(obj.GetObject(3));

                return component;
            }

            public static global::Improbable.TestSchema.RecursiveComponent.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.TestSchema.RecursiveComponent.Update();
                var obj = updateObj.GetFields();

                if (obj.GetObjectCount(1) == 1)
                {
                    update.A = global::Improbable.TestSchema.TypeA.Serialization.Deserialize(obj.GetObject(1));
                }

                if (obj.GetObjectCount(2) == 1)
                {
                    update.B = global::Improbable.TestSchema.TypeB.Serialization.Deserialize(obj.GetObject(2));
                }

                if (obj.GetObjectCount(3) == 1)
                {
                    update.C = global::Improbable.TestSchema.TypeC.Serialization.Deserialize(obj.GetObject(3));
                }

                return update;
            }

            public static global::Improbable.TestSchema.RecursiveComponent.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.TestSchema.RecursiveComponent.Update();
                var obj = data.GetFields();

                update.A = global::Improbable.TestSchema.TypeA.Serialization.Deserialize(obj.GetObject(1));

                update.B = global::Improbable.TestSchema.TypeB.Serialization.Deserialize(obj.GetObject(2));

                update.C = global::Improbable.TestSchema.TypeC.Serialization.Deserialize(obj.GetObject(3));

                return update;
            }

            public static global::Improbable.TestSchema.RecursiveComponent.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.TestSchema.RecursiveComponent.Snapshot();

                component.A = global::Improbable.TestSchema.TypeA.Serialization.Deserialize(obj.GetObject(1));

                component.B = global::Improbable.TestSchema.TypeB.Serialization.Deserialize(obj.GetObject(2));

                component.C = global::Improbable.TestSchema.TypeC.Serialization.Deserialize(obj.GetObject(3));

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.TestSchema.RecursiveComponent.Component component)
            {
                var obj = updateObj.GetFields();

                if (obj.GetObjectCount(1) == 1)
                {
                    component.A = global::Improbable.TestSchema.TypeA.Serialization.Deserialize(obj.GetObject(1));
                }

                if (obj.GetObjectCount(2) == 1)
                {
                    component.B = global::Improbable.TestSchema.TypeB.Serialization.Deserialize(obj.GetObject(2));
                }

                if (obj.GetObjectCount(3) == 1)
                {
                    component.C = global::Improbable.TestSchema.TypeC.Serialization.Deserialize(obj.GetObject(3));
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.TestSchema.RecursiveComponent.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                if (obj.GetObjectCount(1) == 1)
                {
                    snapshot.A = global::Improbable.TestSchema.TypeA.Serialization.Deserialize(obj.GetObject(1));
                }

                if (obj.GetObjectCount(2) == 1)
                {
                    snapshot.B = global::Improbable.TestSchema.TypeB.Serialization.Deserialize(obj.GetObject(2));
                }

                if (obj.GetObjectCount(3) == 1)
                {
                    snapshot.C = global::Improbable.TestSchema.TypeC.Serialization.Deserialize(obj.GetObject(3));
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<global::Improbable.TestSchema.TypeA> A;
            public Option<global::Improbable.TestSchema.TypeB> B;
            public Option<global::Improbable.TestSchema.TypeC> C;
        }

        internal class RecursiveComponentDynamic : IDynamicInvokable
        {
            public uint ComponentId => RecursiveComponent.ComponentId;

            internal static Dynamic.VTable<Update, Snapshot> VTable = new Dynamic.VTable<Update, Snapshot>
            {
                DeserializeSnapshot = DeserializeSnapshot,
                SerializeSnapshot = SerializeSnapshot,
                DeserializeSnapshotRaw = Serialization.DeserializeSnapshot,
                SerializeSnapshotRaw = Serialization.SerializeSnapshot,
                ConvertSnapshotToUpdate = SnapshotToUpdate
            };

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
                var update = new Update
                {
                    A = snapshot.A,
                    B = snapshot.B,
                    C = snapshot.C
                };

                return update;
            }

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Update, Snapshot>(ComponentId, VTable);
            }
        }
    }
}
