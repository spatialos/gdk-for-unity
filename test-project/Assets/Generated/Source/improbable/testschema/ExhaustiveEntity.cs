// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveEntity
    {
        public const uint ComponentId = 197720;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 197720;

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
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 4]. " +
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
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 4]. " +
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
                var componentDataSchema = new ComponentData(new SchemaComponentData(197720));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            internal uint field1Handle;

            public global::Improbable.Gdk.Core.EntitySnapshot Field1
            {
                get => global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field1Provider.Get(field1Handle);
                set
                {
                    MarkDataDirty(0);
                    global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field1Provider.Set(field1Handle, value);
                }
            }

            internal uint field2Handle;

            public global::Improbable.Gdk.Core.EntitySnapshot? Field2
            {
                get => global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field2Provider.Get(field2Handle);
                set
                {
                    MarkDataDirty(1);
                    global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field2Provider.Set(field2Handle, value);
                }
            }

            internal uint field3Handle;

            public global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot> Field3
            {
                get => global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field3Provider.Get(field3Handle);
                set
                {
                    MarkDataDirty(2);
                    global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field3Provider.Set(field3Handle, value);
                }
            }

            internal uint field4Handle;

            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string> Field4
            {
                get => global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field4Provider.Get(field4Handle);
                set
                {
                    MarkDataDirty(3);
                    global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field4Provider.Set(field4Handle, value);
                }
            }

            internal uint field5Handle;

            public global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot> Field5
            {
                get => global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field5Provider.Get(field5Handle);
                set
                {
                    MarkDataDirty(4);
                    global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field5Provider.Set(field5Handle, value);
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
            public uint ComponentId => 197720;

            public global::Improbable.Gdk.Core.EntitySnapshot Field1;
            public global::Improbable.Gdk.Core.EntitySnapshot? Field2;
            public global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot> Field3;
            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string> Field4;
            public global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot> Field5;

            public Snapshot(global::Improbable.Gdk.Core.EntitySnapshot field1, global::Improbable.Gdk.Core.EntitySnapshot? field2, global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot> field3, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string> field4, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot> field5)
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
                {
                    obj.AddEntity(1, component.Field1);
                }
                {
                    if (component.Field2.HasValue)
                    {
                        obj.AddEntity(2, component.Field2.Value);
                    }
                    
                }
                {
                    foreach (var value in component.Field3)
                    {
                        obj.AddEntity(3, value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field4)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddEntity(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field5)
                    {
                        var mapObj = obj.AddObject(5);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddEntity(2, keyValuePair.Value);
                    }
                    
                }
            }

            public static void SerializeUpdate(global::Improbable.TestSchema.ExhaustiveEntity.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddEntity(1, component.Field1);
                    }

                    
                }
                {
                    if (component.IsDataDirty(1))
                    {
                        if (component.Field2.HasValue)
                        {
                            obj.AddEntity(2, component.Field2.Value);
                        }
                        
                    }

                    if (!component.Field2.HasValue)
                        {
                            updateObj.AddClearedField(2);
                        }
                        
                }
                {
                    if (component.IsDataDirty(2))
                    {
                        foreach (var value in component.Field3)
                        {
                            obj.AddEntity(3, value);
                        }
                        
                    }

                    if (component.Field3.Count == 0)
                        {
                            updateObj.AddClearedField(3);
                        }
                        
                }
                {
                    if (component.IsDataDirty(3))
                    {
                        foreach (var keyValuePair in component.Field4)
                        {
                            var mapObj = obj.AddObject(4);
                            mapObj.AddEntity(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field4.Count == 0)
                        {
                            updateObj.AddClearedField(4);
                        }
                        
                }
                {
                    if (component.IsDataDirty(4))
                    {
                        foreach (var keyValuePair in component.Field5)
                        {
                            var mapObj = obj.AddObject(5);
                            mapObj.AddString(1, keyValuePair.Key);
                            mapObj.AddEntity(2, keyValuePair.Value);
                        }
                        
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
                {
                    obj.AddEntity(1, snapshot.Field1);
                }
                {
                    if (snapshot.Field2.HasValue)
                {
                    obj.AddEntity(2, snapshot.Field2.Value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field3)
                {
                    obj.AddEntity(3, value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field4)
                {
                    var mapObj = obj.AddObject(4);
                    mapObj.AddEntity(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field5)
                {
                    var mapObj = obj.AddObject(5);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddEntity(2, keyValuePair.Value);
                }
                
                }
            }

            public static global::Improbable.TestSchema.ExhaustiveEntity.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.TestSchema.ExhaustiveEntity.Component();

                component.field1Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field1Provider.Allocate(world);
                {
                    component.Field1 = obj.GetEntity(1);
                }
                component.field2Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field2Provider.Allocate(world);
                {
                    if (obj.GetEntityCount(2) == 1)
                    {
                        component.Field2 = new global::Improbable.Gdk.Core.EntitySnapshot?(obj.GetEntity(2));
                    }
                    
                }
                component.field3Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field3Provider.Allocate(world);
                {
                    component.Field3 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>();
                    var list = component.Field3;
                    var listLength = obj.GetEntityCount(3);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexEntity(3, (uint) i));
                    }
                    
                }
                component.field4Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field4Provider.Allocate(world);
                {
                    component.Field4 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string>();
                    var map = component.Field4;
                    var mapSize = obj.GetObjectCount(4);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetEntity(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field5Handle = global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.Field5Provider.Allocate(world);
                {
                    component.Field5 = new global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot>();
                    var map = component.Field5;
                    var mapSize = obj.GetObjectCount(5);
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

                var clearedFields = updateObj.GetClearedFields();

                {
                    if (obj.GetEntityCount(1) == 1)
                    {
                        var value = obj.GetEntity(1);
                        update.Field1 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntitySnapshot>(value);
                    }
                    
                }
                {
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (isCleared)
                    {
                        update.Field2 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntitySnapshot?>(new global::Improbable.Gdk.Core.EntitySnapshot?());
                    }
                    else if (obj.GetEntityCount(2) == 1)
                    {
                        var value = obj.GetEntity(2);
                        update.Field2 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntitySnapshot?>(new global::Improbable.Gdk.Core.EntitySnapshot?(value));
                    }
                    
                }
                {
                    var listSize = obj.GetEntityCount(3);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 3;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 4;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field4 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string>>(new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string>());
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 5;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field5 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot>>(new global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot>());
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

                {
                    var value = obj.GetEntity(1);
                    update.Field1 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntitySnapshot>(value);
                    
                }
                {
                    if (obj.GetEntityCount(2) == 1)
                    {
                        var value = obj.GetEntity(2);
                        update.Field2 = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Core.EntitySnapshot?>(new global::Improbable.Gdk.Core.EntitySnapshot?(value));
                    }
                    
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
                    var mapSize = obj.GetObjectCount(4);
                    update.Field4 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string>>(new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string>());
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
                    update.Field5 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot>>(new global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot>());
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

            public static global::Improbable.TestSchema.ExhaustiveEntity.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.TestSchema.ExhaustiveEntity.Snapshot();

                {
                    component.Field1 = obj.GetEntity(1);
                }

                {
                    if (obj.GetEntityCount(2) == 1)
                    {
                        component.Field2 = new global::Improbable.Gdk.Core.EntitySnapshot?(obj.GetEntity(2));
                    }
                    
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
                    component.Field4 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string>();
                    var map = component.Field4;
                    var mapSize = obj.GetObjectCount(4);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetEntity(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field5 = new global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot>();
                    var map = component.Field5;
                    var mapSize = obj.GetObjectCount(5);
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

                var clearedFields = updateObj.GetClearedFields();

                {
                    if (obj.GetEntityCount(1) == 1)
                    {
                        var value = obj.GetEntity(1);
                        component.Field1 = value;
                    }
                    
                }
                {
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 3;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 4;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 5;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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

                var clearedFields = updateObj.GetClearedFields();

                {
                    if (obj.GetEntityCount(1) == 1)
                    {
                        var value = obj.GetEntity(1);
                        snapshot.Field1 = value;
                    }
                    
                }
                {
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 3;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 4;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 5;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::Improbable.Gdk.Core.EntitySnapshot> Field1;
            public Option<global::Improbable.Gdk.Core.EntitySnapshot?> Field2;
            public Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>> Field3;
            public Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string>> Field4;
            public Option<global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot>> Field5;
        }

#if USE_LEGACY_REACTIVE_COMPONENTS
        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => global::Improbable.TestSchema.ExhaustiveEntity.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
#endif

        internal class ExhaustiveEntityDynamic : IDynamicInvokable
        {
            public uint ComponentId => ExhaustiveEntity.ComponentId;

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
                update.Field1 = new Option<global::Improbable.Gdk.Core.EntitySnapshot>(snapshot.Field1);
                update.Field2 = new Option<global::Improbable.Gdk.Core.EntitySnapshot?>(snapshot.Field2);
                update.Field3 = new Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>(snapshot.Field3);
                update.Field4 = new Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot,string>>(snapshot.Field4);
                update.Field5 = new Option<global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntitySnapshot>>(snapshot.Field5);
                return update;
            }

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Component, Update, Snapshot>(ComponentId, VTable);
            }
        }
    }
}
