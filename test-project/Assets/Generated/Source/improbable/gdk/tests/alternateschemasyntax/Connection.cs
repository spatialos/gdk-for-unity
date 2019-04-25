// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.AlternateSchemaSyntax
{
    public partial class Connection
    {
        public const uint ComponentId = 1105;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 1105;

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
                var componentDataSchema = new ComponentData(new SchemaComponentData(1105));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private int value;

            public int Value
            {
                get => value;
                set
                {
                    MarkDataDirty(0);
                    this.value = value;
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
            public uint ComponentId => 1105;

            public int Value;

            public Snapshot(int value)
            {
                Value = value;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddInt32(1, component.Value);
                }
            }

            public static void SerializeUpdate(global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddInt32(1, component.Value);
                    }

                }
            }

            public static void SerializeUpdate(global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.Value.HasValue)
                    {
                        var field = update.Value.Value;
                        obj.AddInt32(1, field);
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddInt32(1, snapshot.Value);
                }
            }

            public static global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component();

                {
                    component.Value = obj.GetInt32(1);
                }
                return component;
            }

            public static global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetInt32Count(1) == 1)
                    {
                        var value = obj.GetInt32(1);
                        update.Value = new global::Improbable.Gdk.Core.Option<int>(value);
                    }
                    
                }
                return update;
            }

            public static global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update();
                var obj = data.GetFields();

                {
                    var value = obj.GetInt32(1);
                    update.Value = new global::Improbable.Gdk.Core.Option<int>(value);
                    
                }
                return update;
            }

            public static global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Snapshot();

                {
                    component.Value = obj.GetInt32(1);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetInt32Count(1) == 1)
                    {
                        var value = obj.GetInt32(1);
                        component.Value = value;
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetInt32Count(1) == 1)
                    {
                        var value = obj.GetInt32(1);
                        snapshot.Value = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<int> Value;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class ConnectionDynamic : IDynamicInvokable
        {
            public uint ComponentId => Connection.ComponentId;

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
                update.Value = new Option<int>(snapshot.Value);
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
