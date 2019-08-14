// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Tests
{
    public partial class DependencyTestChild
    {
        public const uint ComponentId = 11112;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 11112;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            // Each byte tracks 8 component properties.
            private byte dirtyBits0;

            public bool IsDataDirty()
            {
                var isDataDirty = false;
                isDataDirty |= (dirtyBits0 != 0x0);
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
                if (propertyIndex < 0 || propertyIndex >= 1)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 0]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        return (dirtyBits0 & (0x1 << propertyIndex % 8)) != 0x0;
                }

                return false;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 1)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 0]. " +
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
                }
            }

            public void MarkDataClean()
            {
                dirtyBits0 = 0x0;
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(11112, SchemaComponentData.Create());
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private uint child;

            public uint Child
            {
                get => child;
                set
                {
                    MarkDataDirty(0);
                    this.child = value;
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
            public uint ComponentId => 11112;

            public uint Child;

            public Snapshot(uint child)
            {
                Child = child;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.Tests.DependencyTestChild.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddUint32(1, component.Child);
                }
            }

            public static void SerializeUpdate(global::Improbable.Tests.DependencyTestChild.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddUint32(1, component.Child);
                    }

                }
            }

            public static void SerializeUpdate(global::Improbable.Tests.DependencyTestChild.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.Child.HasValue)
                    {
                        var field = update.Child.Value;
                        obj.AddUint32(1, field);
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.Tests.DependencyTestChild.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddUint32(1, snapshot.Child);
                }
            }

            public static global::Improbable.Tests.DependencyTestChild.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.Tests.DependencyTestChild.Component();

                {
                    component.Child = obj.GetUint32(1);
                }
                return component;
            }

            public static global::Improbable.Tests.DependencyTestChild.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.Tests.DependencyTestChild.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetUint32Count(1) == 1)
                    {
                        var value = obj.GetUint32(1);
                        update.Child = new global::Improbable.Gdk.Core.Option<uint>(value);
                    }
                    
                }
                return update;
            }

            public static global::Improbable.Tests.DependencyTestChild.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.Tests.DependencyTestChild.Update();
                var obj = data.GetFields();

                {
                    var value = obj.GetUint32(1);
                    update.Child = new global::Improbable.Gdk.Core.Option<uint>(value);
                    
                }
                return update;
            }

            public static global::Improbable.Tests.DependencyTestChild.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.Tests.DependencyTestChild.Snapshot();

                {
                    component.Child = obj.GetUint32(1);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Tests.DependencyTestChild.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetUint32Count(1) == 1)
                    {
                        var value = obj.GetUint32(1);
                        component.Child = value;
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Tests.DependencyTestChild.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetUint32Count(1) == 1)
                    {
                        var value = obj.GetUint32(1);
                        snapshot.Child = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
#if USE_LEGACY_REACTIVE_COMPONENTS
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();
#endif

            public Option<uint> Child;
        }

#if USE_LEGACY_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Improbable.Tests.DependencyTestChild.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class DependencyTestChildDynamic : IDynamicInvokable
        {
            public uint ComponentId => DependencyTestChild.ComponentId;

            internal static Dynamic.VTable<Component, Update, Snapshot> VTable = new Dynamic.VTable<Component, Update, Snapshot>
            {
                DeserializeComponent = DeserializeData,
                DeserializeUpdate = DeserializeUpdate,
                DeserializeSnapshot = DeserializeSnapshot,
                SerializeSnapshot = SerializeSnapshot,
                DeserializeSnapshotRaw = Serialization.DeserializeSnapshot,
                SerializeSnapshotRaw = Serialization.SerializeSnapshot,
                ConvertSnapshotToUpdate = SnapshotToUpdate
            };

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
                update.Child = new Option<uint>(snapshot.Child);
                return update;
            }

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Component, Update, Snapshot>(ComponentId, VTable);
            }
        }
    }
}
