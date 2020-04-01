// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Entities;

namespace Improbable.Tests
{
    public partial class DependencyTest
    {
        public const uint ComponentId = 11111;

        public unsafe struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            // Bit masks for tracking which component properties were changed locally and need to be synced.
            private fixed UInt32 dirtyBits[1];

            private uint root;

            public uint Root
            {
                get => root;
                set
                {
                    MarkDataDirty(0);
                    this.root = value;
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
                if (propertyIndex < 0 || propertyIndex >= 1)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 0]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(11111, SchemaComponentData.Create());
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
            public uint ComponentId => 11111;

            public uint Root;

            public Snapshot(uint root)
            {
                Root = root;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.Tests.DependencyTest.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                obj.AddUint32(1, component.Root);
            }

            public static void SerializeUpdate(global::Improbable.Tests.DependencyTest.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                if (component.IsDataDirty(0))
                {
                    obj.AddUint32(1, component.Root);
                }
            }

            public static void SerializeUpdate(global::Improbable.Tests.DependencyTest.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                {
                    if (update.Root.HasValue)
                    {
                        var field = update.Root.Value;

                        obj.AddUint32(1, field);
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.Tests.DependencyTest.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                obj.AddUint32(1, snapshot.Root);
            }

            public static global::Improbable.Tests.DependencyTest.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.Tests.DependencyTest.Component();

                component.Root = obj.GetUint32(1);

                return component;
            }

            public static global::Improbable.Tests.DependencyTest.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.Tests.DependencyTest.Update();
                var obj = updateObj.GetFields();

                if (obj.GetUint32Count(1) == 1)
                {
                    update.Root = obj.GetUint32(1);
                }

                return update;
            }

            public static global::Improbable.Tests.DependencyTest.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.Tests.DependencyTest.Update();
                var obj = data.GetFields();

                update.Root = obj.GetUint32(1);

                return update;
            }

            public static global::Improbable.Tests.DependencyTest.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.Tests.DependencyTest.Snapshot();

                component.Root = obj.GetUint32(1);

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Tests.DependencyTest.Component component)
            {
                var obj = updateObj.GetFields();

                if (obj.GetUint32Count(1) == 1)
                {
                    component.Root = obj.GetUint32(1);
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Tests.DependencyTest.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                if (obj.GetUint32Count(1) == 1)
                {
                    snapshot.Root = obj.GetUint32(1);
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<uint> Root;
        }

        internal class DependencyTestDynamic : IDynamicInvokable
        {
            public uint ComponentId => DependencyTest.ComponentId;

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
                    Root = snapshot.Root
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
