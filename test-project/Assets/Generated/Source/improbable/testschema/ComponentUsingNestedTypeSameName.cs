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
    public partial class ComponentUsingNestedTypeSameName
    {
        public const uint ComponentId = 198730;

        public unsafe struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 198730;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            private fixed UInt32 dirtyBits[1];

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
                var componentDataSchema = new ComponentData(198730, SchemaComponentData.Create());
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private int nestedField;

            public int NestedField
            {
                get => nestedField;
                set
                {
                    MarkDataDirty(0);
                    this.nestedField = value;
                }
            }

            private global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName other0Field;

            public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName Other0Field
            {
                get => other0Field;
                set
                {
                    MarkDataDirty(1);
                    this.other0Field = value;
                }
            }

            private global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName other1Field;

            public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName Other1Field
            {
                get => other1Field;
                set
                {
                    MarkDataDirty(2);
                    this.other1Field = value;
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
            public uint ComponentId => 198730;

            public int NestedField;
            public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName Other0Field;
            public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName Other1Field;

            public Snapshot(int nestedField, global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName other0Field, global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName other1Field)
            {
                NestedField = nestedField;
                Other0Field = other0Field;
                Other1Field = other1Field;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                obj.AddInt32(1, component.NestedField);

                global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Serialize(component.Other0Field, obj.AddObject(2));

                obj.AddEnum(3, (uint) component.Other1Field);
            }

            public static void SerializeUpdate(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                if (component.IsDataDirty(0))
                {
                    obj.AddInt32(1, component.NestedField);
                }

                if (component.IsDataDirty(1))
                {
                    global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Serialize(component.Other0Field, obj.AddObject(2));
                }

                if (component.IsDataDirty(2))
                {
                    obj.AddEnum(3, (uint) component.Other1Field);
                }
            }

            public static void SerializeUpdate(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                {
                    if (update.NestedField.HasValue)
                    {
                        var field = update.NestedField.Value;

                        obj.AddInt32(1, field);
                    }
                }

                {
                    if (update.Other0Field.HasValue)
                    {
                        var field = update.Other0Field.Value;

                        global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Serialize(field, obj.AddObject(2));
                    }
                }

                {
                    if (update.Other1Field.HasValue)
                    {
                        var field = update.Other1Field.Value;

                        obj.AddEnum(3, (uint) field);
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                obj.AddInt32(1, snapshot.NestedField);

                global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Serialize(snapshot.Other0Field, obj.AddObject(2));

                obj.AddEnum(3, (uint) snapshot.Other1Field);
            }

            public static global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Component();

                component.NestedField = obj.GetInt32(1);

                component.Other0Field = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(2));

                component.Other1Field = (global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName) obj.GetEnum(3);

                return component;
            }

            public static global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Update();
                var obj = updateObj.GetFields();

                if (obj.GetInt32Count(1) == 1)
                {
                    update.NestedField = obj.GetInt32(1);
                }

                if (obj.GetObjectCount(2) == 1)
                {
                    update.Other0Field = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(2));
                }

                if (obj.GetEnumCount(3) == 1)
                {
                    update.Other1Field = (global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName) obj.GetEnum(3);
                }

                return update;
            }

            public static global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Update();
                var obj = data.GetFields();

                update.NestedField = obj.GetInt32(1);

                update.Other0Field = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(2));

                update.Other1Field = (global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName) obj.GetEnum(3);

                return update;
            }

            public static global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Snapshot();

                component.NestedField = obj.GetInt32(1);

                component.Other0Field = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(2));

                component.Other1Field = (global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName) obj.GetEnum(3);

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Component component)
            {
                var obj = updateObj.GetFields();

                if (obj.GetInt32Count(1) == 1)
                {
                    component.NestedField = obj.GetInt32(1);
                }

                if (obj.GetObjectCount(2) == 1)
                {
                    component.Other0Field = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(2));
                }

                if (obj.GetEnumCount(3) == 1)
                {
                    component.Other1Field = (global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName) obj.GetEnum(3);
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.TestSchema.ComponentUsingNestedTypeSameName.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                if (obj.GetInt32Count(1) == 1)
                {
                    snapshot.NestedField = obj.GetInt32(1);
                }

                if (obj.GetObjectCount(2) == 1)
                {
                    snapshot.Other0Field = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(2));
                }

                if (obj.GetEnumCount(3) == 1)
                {
                    snapshot.Other1Field = (global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName) obj.GetEnum(3);
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<int> NestedField;
            public Option<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName> Other0Field;
            public Option<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName> Other1Field;
        }

        internal class ComponentUsingNestedTypeSameNameDynamic : IDynamicInvokable
        {
            public uint ComponentId => ComponentUsingNestedTypeSameName.ComponentId;

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
                    NestedField = snapshot.NestedField,
                    Other0Field = snapshot.Other0Field,
                    Other1Field = snapshot.Other1Field
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
