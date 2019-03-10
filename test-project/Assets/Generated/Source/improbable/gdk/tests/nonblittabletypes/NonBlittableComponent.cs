// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public const uint ComponentId = 1002;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 1002;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            // Each byte tracks 8 component properties.
            private byte dirtyBits0;
            private byte dirtyBits1;

            public bool IsDataDirty()
            {
                var isDataDirty = false;
                isDataDirty |= (dirtyBits0 != 0x0);
                isDataDirty |= (dirtyBits1 != 0x0);
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
                if (propertyIndex < 0 || propertyIndex >= 9)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 8]. " +
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
                }

                return false;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 9)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 8]. " +
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
                }
            }

            public void MarkDataClean()
            {
                dirtyBits0 = 0x0;
                dirtyBits1 = 0x0;
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(new SchemaComponentData(1002));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private BlittableBool boolField;

            public BlittableBool BoolField
            {
                get => boolField;
                set
                {
                    MarkDataDirty(0);
                    this.boolField = value;
                }
            }

            private int intField;

            public int IntField
            {
                get => intField;
                set
                {
                    MarkDataDirty(1);
                    this.intField = value;
                }
            }

            private long longField;

            public long LongField
            {
                get => longField;
                set
                {
                    MarkDataDirty(2);
                    this.longField = value;
                }
            }

            private float floatField;

            public float FloatField
            {
                get => floatField;
                set
                {
                    MarkDataDirty(3);
                    this.floatField = value;
                }
            }

            private double doubleField;

            public double DoubleField
            {
                get => doubleField;
                set
                {
                    MarkDataDirty(4);
                    this.doubleField = value;
                }
            }

            internal uint stringFieldHandle;

            public string StringField
            {
                get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Get(stringFieldHandle);
                set
                {
                    MarkDataDirty(5);
                    Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Set(stringFieldHandle, value);
                }
            }

            internal uint optionalFieldHandle;

            public int? OptionalField
            {
                get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Get(optionalFieldHandle);
                set
                {
                    MarkDataDirty(6);
                    Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Set(optionalFieldHandle, value);
                }
            }

            internal uint listFieldHandle;

            public global::System.Collections.Generic.List<int> ListField
            {
                get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Get(listFieldHandle);
                set
                {
                    MarkDataDirty(7);
                    Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Set(listFieldHandle, value);
                }
            }

            internal uint mapFieldHandle;

            public global::System.Collections.Generic.Dictionary<int,string> MapField
            {
                get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Get(mapFieldHandle);
                set
                {
                    MarkDataDirty(8);
                    Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Set(mapFieldHandle, value);
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

        [System.Serializable]
        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 1002;

            public BlittableBool BoolField;
            public int IntField;
            public long LongField;
            public float FloatField;
            public double DoubleField;
            public string StringField;
            public int? OptionalField;
            public global::System.Collections.Generic.List<int> ListField;
            public global::System.Collections.Generic.Dictionary<int,string> MapField;

            public Snapshot(BlittableBool boolField, int intField, long longField, float floatField, double doubleField, string stringField, int? optionalField, global::System.Collections.Generic.List<int> listField, global::System.Collections.Generic.Dictionary<int,string> mapField)
            {
                BoolField = boolField;
                IntField = intField;
                LongField = longField;
                FloatField = floatField;
                DoubleField = doubleField;
                StringField = stringField;
                OptionalField = optionalField;
                ListField = listField;
                MapField = mapField;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddBool(1, component.BoolField);
                }
                {
                    obj.AddInt32(2, component.IntField);
                }
                {
                    obj.AddInt64(3, component.LongField);
                }
                {
                    obj.AddFloat(4, component.FloatField);
                }
                {
                    obj.AddDouble(5, component.DoubleField);
                }
                {
                    obj.AddString(6, component.StringField);
                }
                {
                    if (component.OptionalField.HasValue)
                    {
                        obj.AddInt32(7, component.OptionalField.Value);
                    }
                    
                }
                {
                    foreach (var value in component.ListField)
                    {
                        obj.AddInt32(8, value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.MapField)
                    {
                        var mapObj = obj.AddObject(9);
                        mapObj.AddInt32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
            }

            public static void SerializeUpdate(Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddBool(1, component.BoolField);
                    }

                    
                }
                {
                    if (component.IsDataDirty(1))
                    {
                        obj.AddInt32(2, component.IntField);
                    }

                    
                }
                {
                    if (component.IsDataDirty(2))
                    {
                        obj.AddInt64(3, component.LongField);
                    }

                    
                }
                {
                    if (component.IsDataDirty(3))
                    {
                        obj.AddFloat(4, component.FloatField);
                    }

                    
                }
                {
                    if (component.IsDataDirty(4))
                    {
                        obj.AddDouble(5, component.DoubleField);
                    }

                    
                }
                {
                    if (component.IsDataDirty(5))
                    {
                        obj.AddString(6, component.StringField);
                    }

                    
                }
                {
                    if (component.IsDataDirty(6))
                    {
                        if (component.OptionalField.HasValue)
                        {
                            obj.AddInt32(7, component.OptionalField.Value);
                        }
                        
                    }

                    if (!component.OptionalField.HasValue)
                        {
                            updateObj.AddClearedField(7);
                        }
                        
                }
                {
                    if (component.IsDataDirty(7))
                    {
                        foreach (var value in component.ListField)
                        {
                            obj.AddInt32(8, value);
                        }
                        
                    }

                    if (component.ListField.Count == 0)
                        {
                            updateObj.AddClearedField(8);
                        }
                        
                }
                {
                    if (component.IsDataDirty(8))
                    {
                        foreach (var keyValuePair in component.MapField)
                        {
                            var mapObj = obj.AddObject(9);
                            mapObj.AddInt32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.MapField.Count == 0)
                        {
                            updateObj.AddClearedField(9);
                        }
                        
                }
            }

            public static void SerializeUpdate(Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (update.BoolField.HasValue)
                    {
                        var field = update.BoolField.Value;
                        obj.AddBool(1, field);
                        
                    }
                }
                {
                    if (update.IntField.HasValue)
                    {
                        var field = update.IntField.Value;
                        obj.AddInt32(2, field);
                        
                    }
                }
                {
                    if (update.LongField.HasValue)
                    {
                        var field = update.LongField.Value;
                        obj.AddInt64(3, field);
                        
                    }
                }
                {
                    if (update.FloatField.HasValue)
                    {
                        var field = update.FloatField.Value;
                        obj.AddFloat(4, field);
                        
                    }
                }
                {
                    if (update.DoubleField.HasValue)
                    {
                        var field = update.DoubleField.Value;
                        obj.AddDouble(5, field);
                        
                    }
                }
                {
                    if (update.StringField.HasValue)
                    {
                        var field = update.StringField.Value;
                        obj.AddString(6, field);
                        
                    }
                }
                {
                    if (update.OptionalField.HasValue)
                    {
                        var field = update.OptionalField.Value;
                        if (field.HasValue)
                        {
                            obj.AddInt32(7, field.Value);
                        }
                        
                        if (!field.HasValue)
                        {
                            updateObj.AddClearedField(7);
                        }
                        
                    }
                }
                {
                    if (update.ListField.HasValue)
                    {
                        var field = update.ListField.Value;
                        foreach (var value in field)
                        {
                            obj.AddInt32(8, value);
                        }
                        
                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(8);
                        }
                        
                    }
                }
                {
                    if (update.MapField.HasValue)
                    {
                        var field = update.MapField.Value;
                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(9);
                            mapObj.AddInt32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(9);
                        }
                        
                    }
                }
            }

            public static void SerializeSnapshot(Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, snapshot.BoolField);
                }
                {
                    obj.AddInt32(2, snapshot.IntField);
                }
                {
                    obj.AddInt64(3, snapshot.LongField);
                }
                {
                    obj.AddFloat(4, snapshot.FloatField);
                }
                {
                    obj.AddDouble(5, snapshot.DoubleField);
                }
                {
                    obj.AddString(6, snapshot.StringField);
                }
                {
                    if (snapshot.OptionalField.HasValue)
                {
                    obj.AddInt32(7, snapshot.OptionalField.Value);
                }
                
                }
                {
                    foreach (var value in snapshot.ListField)
                {
                    obj.AddInt32(8, value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.MapField)
                {
                    var mapObj = obj.AddObject(9);
                    mapObj.AddInt32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
            }

            public static Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component();

                {
                    component.BoolField = obj.GetBool(1);
                }
                {
                    component.IntField = obj.GetInt32(2);
                }
                {
                    component.LongField = obj.GetInt64(3);
                }
                {
                    component.FloatField = obj.GetFloat(4);
                }
                {
                    component.DoubleField = obj.GetDouble(5);
                }
                component.stringFieldHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Allocate(world);
                {
                    component.StringField = obj.GetString(6);
                }
                component.optionalFieldHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Allocate(world);
                {
                    if (obj.GetInt32Count(7) == 1)
                    {
                        component.OptionalField = new int?(obj.GetInt32(7));
                    }
                    
                }
                component.listFieldHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Allocate(world);
                {
                    component.ListField = new global::System.Collections.Generic.List<int>();
                    var list = component.ListField;
                    var listLength = obj.GetInt32Count(8);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexInt32(8, (uint) i));
                    }
                    
                }
                component.mapFieldHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Allocate(world);
                {
                    component.MapField = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = component.MapField;
                    var mapSize = obj.GetObjectCount(9);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                return component;
            }

            public static Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update();
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        update.BoolField = new global::Improbable.Gdk.Core.Option<BlittableBool>(value);
                    }
                    
                }
                {
                    if (obj.GetInt32Count(2) == 1)
                    {
                        var value = obj.GetInt32(2);
                        update.IntField = new global::Improbable.Gdk.Core.Option<int>(value);
                    }
                    
                }
                {
                    if (obj.GetInt64Count(3) == 1)
                    {
                        var value = obj.GetInt64(3);
                        update.LongField = new global::Improbable.Gdk.Core.Option<long>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        update.FloatField = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(5) == 1)
                    {
                        var value = obj.GetDouble(5);
                        update.DoubleField = new global::Improbable.Gdk.Core.Option<double>(value);
                    }
                    
                }
                {
                    if (obj.GetStringCount(6) == 1)
                    {
                        var value = obj.GetString(6);
                        update.StringField = new global::Improbable.Gdk.Core.Option<string>(value);
                    }
                    
                }
                {
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 7;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (isCleared)
                    {
                        update.OptionalField = new global::Improbable.Gdk.Core.Option<int?>(new int?());
                    }
                    else if (obj.GetInt32Count(7) == 1)
                    {
                        var value = obj.GetInt32(7);
                        update.OptionalField = new global::Improbable.Gdk.Core.Option<int?>(new int?(value));
                    }
                    
                }
                {
                    var listSize = obj.GetInt32Count(8);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 8;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.ListField = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<int>>(new global::System.Collections.Generic.List<int>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexInt32(8, (uint) i);
                        update.ListField.Value.Add(value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(9);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 9;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.MapField = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<int,string>>(new global::System.Collections.Generic.Dictionary<int,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        update.MapField.Value.Add(key, value);
                    }
                    
                }
                return update;
            }

            public static Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update();
                var obj = data.GetFields();

                {
                    var value = obj.GetBool(1);
                    update.BoolField = new global::Improbable.Gdk.Core.Option<BlittableBool>(value);
                    
                }
                {
                    var value = obj.GetInt32(2);
                    update.IntField = new global::Improbable.Gdk.Core.Option<int>(value);
                    
                }
                {
                    var value = obj.GetInt64(3);
                    update.LongField = new global::Improbable.Gdk.Core.Option<long>(value);
                    
                }
                {
                    var value = obj.GetFloat(4);
                    update.FloatField = new global::Improbable.Gdk.Core.Option<float>(value);
                    
                }
                {
                    var value = obj.GetDouble(5);
                    update.DoubleField = new global::Improbable.Gdk.Core.Option<double>(value);
                    
                }
                {
                    var value = obj.GetString(6);
                    update.StringField = new global::Improbable.Gdk.Core.Option<string>(value);
                    
                }
                {
                    if (obj.GetInt32Count(7) == 1)
                    {
                        var value = obj.GetInt32(7);
                        update.OptionalField = new global::Improbable.Gdk.Core.Option<int?>(new int?(value));
                    }
                    
                }
                {
                    var listSize = obj.GetInt32Count(8);
                    update.ListField = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<int>>(new global::System.Collections.Generic.List<int>());
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexInt32(8, (uint) i);
                        update.ListField.Value.Add(value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(9);
                    update.MapField = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<int,string>>(new global::System.Collections.Generic.Dictionary<int,string>());
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        update.MapField.Value.Add(key, value);
                    }
                    
                }
                return update;
            }

            public static Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Snapshot();

                {
                    component.BoolField = obj.GetBool(1);
                }

                {
                    component.IntField = obj.GetInt32(2);
                }

                {
                    component.LongField = obj.GetInt64(3);
                }

                {
                    component.FloatField = obj.GetFloat(4);
                }

                {
                    component.DoubleField = obj.GetDouble(5);
                }

                {
                    component.StringField = obj.GetString(6);
                }

                {
                    if (obj.GetInt32Count(7) == 1)
                    {
                        component.OptionalField = new int?(obj.GetInt32(7));
                    }
                    
                }

                {
                    component.ListField = new global::System.Collections.Generic.List<int>();
                    var list = component.ListField;
                    var listLength = obj.GetInt32Count(8);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexInt32(8, (uint) i));
                    }
                    
                }

                {
                    component.MapField = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = component.MapField;
                    var mapSize = obj.GetObjectCount(9);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component component)
            {
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        component.BoolField = value;
                    }
                    
                }
                {
                    if (obj.GetInt32Count(2) == 1)
                    {
                        var value = obj.GetInt32(2);
                        component.IntField = value;
                    }
                    
                }
                {
                    if (obj.GetInt64Count(3) == 1)
                    {
                        var value = obj.GetInt64(3);
                        component.LongField = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        component.FloatField = value;
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(5) == 1)
                    {
                        var value = obj.GetDouble(5);
                        component.DoubleField = value;
                    }
                    
                }
                {
                    if (obj.GetStringCount(6) == 1)
                    {
                        var value = obj.GetString(6);
                        component.StringField = value;
                    }
                    
                }
                {
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 7;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (isCleared)
                    {
                        component.OptionalField = new int?();
                    }
                    else if (obj.GetInt32Count(7) == 1)
                    {
                        var value = obj.GetInt32(7);
                        component.OptionalField = new int?(value);
                    }
                    
                }
                {
                    var listSize = obj.GetInt32Count(8);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 8;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.ListField.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexInt32(8, (uint) i);
                        component.ListField.Add(value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(9);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 9;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.MapField.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        component.MapField.Add(key, value);
                    }
                    
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        snapshot.BoolField = value;
                    }
                    
                }
                {
                    if (obj.GetInt32Count(2) == 1)
                    {
                        var value = obj.GetInt32(2);
                        snapshot.IntField = value;
                    }
                    
                }
                {
                    if (obj.GetInt64Count(3) == 1)
                    {
                        var value = obj.GetInt64(3);
                        snapshot.LongField = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        snapshot.FloatField = value;
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(5) == 1)
                    {
                        var value = obj.GetDouble(5);
                        snapshot.DoubleField = value;
                    }
                    
                }
                {
                    if (obj.GetStringCount(6) == 1)
                    {
                        var value = obj.GetString(6);
                        snapshot.StringField = value;
                    }
                    
                }
                {
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 7;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (isCleared)
                    {
                        snapshot.OptionalField = new int?();
                    }
                    else if (obj.GetInt32Count(7) == 1)
                    {
                        var value = obj.GetInt32(7);
                        snapshot.OptionalField = new int?(value);
                    }
                    
                }
                {
                    var listSize = obj.GetInt32Count(8);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 8;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        snapshot.ListField.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexInt32(8, (uint) i);
                        snapshot.ListField.Add(value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(9);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 9;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.MapField.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        snapshot.MapField.Add(key, value);
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<BlittableBool> BoolField;
            public Option<int> IntField;
            public Option<long> LongField;
            public Option<float> FloatField;
            public Option<double> DoubleField;
            public Option<string> StringField;
            public Option<int?> OptionalField;
            public Option<global::System.Collections.Generic.List<int>> ListField;
            public Option<global::System.Collections.Generic.Dictionary<int,string>> MapField;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class NonBlittableComponentDynamic : IDynamicInvokable
        {
            public uint ComponentId => NonBlittableComponent.ComponentId;

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
                update.BoolField = new Option<BlittableBool>(snapshot.BoolField);
                update.IntField = new Option<int>(snapshot.IntField);
                update.LongField = new Option<long>(snapshot.LongField);
                update.FloatField = new Option<float>(snapshot.FloatField);
                update.DoubleField = new Option<double>(snapshot.DoubleField);
                update.StringField = new Option<string>(snapshot.StringField);
                update.OptionalField = new Option<int?>(snapshot.OptionalField);
                update.ListField = new Option<global::System.Collections.Generic.List<int>>(snapshot.ListField);
                update.MapField = new Option<global::System.Collections.Generic.Dictionary<int,string>>(snapshot.MapField);
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
