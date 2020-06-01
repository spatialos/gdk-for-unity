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
    public partial class ExhaustiveEntity
    {
        public const uint ComponentId = 197720;

        public unsafe struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            // Bit masks for tracking which component properties were changed locally and need to be synced.
            private fixed UInt32 dirtyBits[1];

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.Gdk.Core.EntitySnapshot>.ReferenceHandle field1Handle;

            public global::Improbable.Gdk.Core.EntitySnapshot Field1
            {
                get => field1Handle.Get();
                set
                {
                    MarkDataDirty(0);
                    field1Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.Gdk.Core.EntitySnapshot?>.ReferenceHandle field2Handle;

            public global::Improbable.Gdk.Core.EntitySnapshot? Field2
            {
                get => field2Handle.Get();
                set
                {
                    MarkDataDirty(1);
                    field2Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>.ReferenceHandle field3Handle;

            public global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot> Field3
            {
                get => field3Handle.Get();
                set
                {
                    MarkDataDirty(2);
                    field3Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>>.ReferenceHandle field4Handle;

            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string> Field4
            {
                get => field4Handle.Get();
                set
                {
                    MarkDataDirty(3);
                    field4Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>>.ReferenceHandle field5Handle;

            public global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot> Field5
            {
                get => field5Handle.Get();
                set
                {
                    MarkDataDirty(4);
                    field5Handle.Set(value);
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
                var dirtyBitsByteIndex = propertyIndex >> 5;
                return (dirtyBits[dirtyBitsByteIndex] & (0x1 << (propertyIndex & 31))) != 0x0;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                ValidateFieldIndex(propertyIndex);

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex >> 5;
                dirtyBits[dirtyBitsByteIndex] |= (UInt32) (0x1 << (propertyIndex & 31));
            }

            public void MarkDataClean()
            {
                dirtyBits[0] = 0x0;
            }

            [Conditional("DEBUG")]
            private void ValidateFieldIndex(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 4]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(197720, SchemaComponentData.Create());
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
            public uint ComponentId => 197720;

            public global::Improbable.Gdk.Core.EntitySnapshot Field1;
            public global::Improbable.Gdk.Core.EntitySnapshot? Field2;
            public global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot> Field3;
            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string> Field4;
            public global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot> Field5;

            public Snapshot(global::Improbable.Gdk.Core.EntitySnapshot field1, global::Improbable.Gdk.Core.EntitySnapshot? field2, global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot> field3, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string> field4, global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot> field5)
            {
                Field1 = field1;
                Field2 = field2;
                Field3 = field3;
                Field4 = field4;
                Field5 = field5;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.TestSchema.ExhaustiveEntity.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                obj.AddEntity(1, component.Field1);

                if (component.Field2.HasValue)
                {
                    obj.AddEntity(2, component.Field2.Value);
                }

                foreach (var value in component.Field3)
                {
                    obj.AddEntity(3, value);
                }

                foreach (var keyValuePair in component.Field4)
                {
                    var mapObj = obj.AddObject(4);
                    mapObj.AddEntity(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field5)
                {
                    var mapObj = obj.AddObject(5);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddEntity(2, keyValuePair.Value);
                }
            }

            public static void SerializeUpdate(global::Improbable.TestSchema.ExhaustiveEntity.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                if (component.IsDataDirty(0))
                {
                    obj.AddEntity(1, component.Field1);

                    
                }

                if (component.IsDataDirty(1))
                {
                    if (component.Field2.HasValue)
                    {
                        obj.AddEntity(2, component.Field2.Value);
                    }

                    if (!component.Field2.HasValue)
                    {
                        updateObj.AddClearedField(2);
                    }
                }

                if (component.IsDataDirty(2))
                {
                    foreach (var value in component.Field3)
                    {
                        obj.AddEntity(3, value);
                    }

                    if (component.Field3.Count == 0)
                    {
                        updateObj.AddClearedField(3);
                    }
                }

                if (component.IsDataDirty(3))
                {
                    foreach (var keyValuePair in component.Field4)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddEntity(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field4.Count == 0)
                    {
                        updateObj.AddClearedField(4);
                    }
                }

                if (component.IsDataDirty(4))
                {
                    foreach (var keyValuePair in component.Field5)
                    {
                        var mapObj = obj.AddObject(5);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddEntity(2, keyValuePair.Value);
                    }

                    if (component.Field5.Count == 0)
                    {
                        updateObj.AddClearedField(5);
                    }
                }
            }

            public static void SerializeUpdate(global::Improbable.TestSchema.ExhaustiveEntity.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                {
                    if (update.Field1.HasValue)
                    {
                        var field = update.Field1.Value;

                        obj.AddEntity(1, field);

                        
                    }
                }

                {
                    if (update.Field2.HasValue)
                    {
                        var field = update.Field2.Value;

                        if (field.HasValue)
                        {
                            obj.AddEntity(2, field.Value);
                        }

                        if (!field.HasValue)
                        {
                            updateObj.AddClearedField(2);
                        }
                    }
                }

                {
                    if (update.Field3.HasValue)
                    {
                        var field = update.Field3.Value;

                        foreach (var value in field)
                        {
                            obj.AddEntity(3, value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(3);
                        }
                    }
                }

                {
                    if (update.Field4.HasValue)
                    {
                        var field = update.Field4.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(4);
                            mapObj.AddEntity(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(4);
                        }
                    }
                }

                {
                    if (update.Field5.HasValue)
                    {
                        var field = update.Field5.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(5);
                            mapObj.AddString(1, keyValuePair.Key);
                            mapObj.AddEntity(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(5);
                        }
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.TestSchema.ExhaustiveEntity.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                obj.AddEntity(1, snapshot.Field1);

                if (snapshot.Field2.HasValue)
                {
                    obj.AddEntity(2, snapshot.Field2.Value);
                }

                foreach (var value in snapshot.Field3)
                {
                    obj.AddEntity(3, value);
                }

                foreach (var keyValuePair in snapshot.Field4)
                {
                    var mapObj = obj.AddObject(4);
                    mapObj.AddEntity(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field5)
                {
                    var mapObj = obj.AddObject(5);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddEntity(2, keyValuePair.Value);
                }
            }

            public static global::Improbable.TestSchema.ExhaustiveEntity.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.TestSchema.ExhaustiveEntity.Component();

                component.field1Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.Gdk.Core.EntitySnapshot>.Create();

                component.Field1 = obj.GetEntity(1);

                component.field2Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::Improbable.Gdk.Core.EntitySnapshot?>.Create();

                if (obj.GetEntityCount(2) == 1)
                {
                    component.Field2 = new global::Improbable.Gdk.Core.EntitySnapshot?(obj.GetEntity(2));
                }

                component.field3Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>.Create();

                {
                    component.Field3 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>();
                    var list = component.Field3;
                    var listLength = obj.GetEntityCount(3);

                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexEntity(3, (uint) i));
                    }
                }

                component.field4Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>();
                    var mapSize = obj.GetObjectCount(4);
                    component.Field4 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetEntity(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field5Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>();
                    var mapSize = obj.GetObjectCount(5);
                    component.Field5 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntity(2);
                        map.Add(key, value);
                    }
                }

                return component;
            }

            public static global::Improbable.TestSchema.ExhaustiveEntity.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.TestSchema.ExhaustiveEntity.Update();
                var obj = updateObj.GetFields();

                if (obj.GetEntityCount(1) == 1)
                {
                    update.Field1 = obj.GetEntity(1);
                }

                {
                    var isCleared = updateObj.IsFieldCleared(2);

                    if (isCleared)
                    {
                        update.Field2 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntitySnapshot?>(new global::Improbable.Gdk.Core.EntitySnapshot?());
                    }
                    else if (obj.GetEntityCount(2) == 1)
                    {
                        update.Field2 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntitySnapshot?>(obj.GetEntity(2));
                    }
                }

                {
                    var listSize = obj.GetEntityCount(3);

                    var isCleared = updateObj.IsFieldCleared(3);

                    if (listSize > 0 || isCleared)
                    {
                        update.Field3 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>(new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>());
                    }

                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexEntity(3, (uint) i);
                        update.Field3.Value.Add(value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(4);

                    var isCleared = updateObj.IsFieldCleared(4);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field4 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>>(new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetEntity(1);
                        var value = mapObj.GetString(2);
                        update.Field4.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(5);

                    var isCleared = updateObj.IsFieldCleared(5);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field5 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>>(new global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntity(2);
                        update.Field5.Value.Add(key, value);
                    }
                }

                return update;
            }

            public static global::Improbable.TestSchema.ExhaustiveEntity.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.TestSchema.ExhaustiveEntity.Update();
                var obj = data.GetFields();

                update.Field1 = obj.GetEntity(1);

                if (obj.GetEntityCount(2) == 1)
                {
                    update.Field2 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntitySnapshot?>(obj.GetEntity(2));
                }

                {
                    var listSize = obj.GetEntityCount(3);
                    update.Field3 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>(new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>());

                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexEntity(3, (uint) i);
                        update.Field3.Value.Add(value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>();
                    var mapSize = obj.GetObjectCount(4);
                    update.Field4 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetEntity(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>();
                    var mapSize = obj.GetObjectCount(5);
                    update.Field5 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntity(2);
                        map.Add(key, value);
                    }
                }

                return update;
            }

            public static global::Improbable.TestSchema.ExhaustiveEntity.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.TestSchema.ExhaustiveEntity.Snapshot();

                component.Field1 = obj.GetEntity(1);

                if (obj.GetEntityCount(2) == 1)
                {
                    component.Field2 = new global::Improbable.Gdk.Core.EntitySnapshot?(obj.GetEntity(2));
                }

                {
                    component.Field3 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>();
                    var list = component.Field3;
                    var listLength = obj.GetEntityCount(3);

                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexEntity(3, (uint) i));
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>();
                    var mapSize = obj.GetObjectCount(4);
                    component.Field4 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetEntity(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>();
                    var mapSize = obj.GetObjectCount(5);
                    component.Field5 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntity(2);
                        map.Add(key, value);
                    }
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.TestSchema.ExhaustiveEntity.Component component)
            {
                var obj = updateObj.GetFields();

                if (obj.GetEntityCount(1) == 1)
                {
                    component.Field1 = obj.GetEntity(1);
                }

                {
                    var isCleared = updateObj.IsFieldCleared(2);

                    if (isCleared)
                    {
                        component.Field2 = new global::Improbable.Gdk.Core.EntitySnapshot?();
                    }
                    else if (obj.GetEntityCount(2) == 1)
                    {
                        var value = obj.GetEntity(2);
                        component.Field2 = new global::Improbable.Gdk.Core.EntitySnapshot?(value);
                    }
                }

                {
                    var listSize = obj.GetEntityCount(3);

                    var isCleared = updateObj.IsFieldCleared(3);

                    if (listSize > 0 || isCleared)
                    {
                        component.Field3.Clear();
                    }

                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexEntity(3, (uint) i);
                        component.Field3.Add(value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(4);

                    var isCleared = updateObj.IsFieldCleared(4);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field4.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetEntity(1);
                        var value = mapObj.GetString(2);
                        component.Field4.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(5);

                    var isCleared = updateObj.IsFieldCleared(5);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field5.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntity(2);
                        component.Field5.Add(key, value);
                    }
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.TestSchema.ExhaustiveEntity.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                if (obj.GetEntityCount(1) == 1)
                {
                    snapshot.Field1 = obj.GetEntity(1);
                }

                {
                    var isCleared = updateObj.IsFieldCleared(2);

                    if (isCleared)
                    {
                        snapshot.Field2 = new global::Improbable.Gdk.Core.EntitySnapshot?();
                    }
                    else if (obj.GetEntityCount(2) == 1)
                    {
                        var value = obj.GetEntity(2);
                        snapshot.Field2 = new global::Improbable.Gdk.Core.EntitySnapshot?(value);
                    }
                }

                {
                    var listSize = obj.GetEntityCount(3);

                    var isCleared = updateObj.IsFieldCleared(3);

                    if (listSize > 0 || isCleared)
                    {
                        snapshot.Field3.Clear();
                    }

                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexEntity(3, (uint) i);
                        snapshot.Field3.Add(value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(4);

                    var isCleared = updateObj.IsFieldCleared(4);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field4.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetEntity(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field4.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(5);

                    var isCleared = updateObj.IsFieldCleared(5);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field5.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntity(2);
                        snapshot.Field5.Add(key, value);
                    }
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<global::Improbable.Gdk.Core.EntitySnapshot> Field1;
            public Option<global::Improbable.Gdk.Core.EntitySnapshot?> Field2;
            public Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>> Field3;
            public Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>> Field4;
            public Option<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>> Field5;
        }

        internal class ExhaustiveEntityDynamic : IDynamicInvokable
        {
            public uint ComponentId => ExhaustiveEntity.ComponentId;

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
                    Field1 = snapshot.Field1,
                    Field2 = snapshot.Field2,
                    Field3 = snapshot.Field3,
                    Field4 = snapshot.Field4,
                    Field5 = snapshot.Field5
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
