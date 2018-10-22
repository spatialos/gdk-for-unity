// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public const uint ComponentId = 1002;

        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 1002;

            public BlittableBool DirtyBit { get; set; }

            private BlittableBool boolField;

            public BlittableBool BoolField
            {
                get => boolField;
                set
                {
                    DirtyBit = true;
                    boolField = value;
                }
            }

            private int intField;

            public int IntField
            {
                get => intField;
                set
                {
                    DirtyBit = true;
                    intField = value;
                }
            }

            private long longField;

            public long LongField
            {
                get => longField;
                set
                {
                    DirtyBit = true;
                    longField = value;
                }
            }

            private float floatField;

            public float FloatField
            {
                get => floatField;
                set
                {
                    DirtyBit = true;
                    floatField = value;
                }
            }

            private double doubleField;

            public double DoubleField
            {
                get => doubleField;
                set
                {
                    DirtyBit = true;
                    doubleField = value;
                }
            }

            internal uint stringFieldHandle;

            public string StringField
            {
                get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Get(stringFieldHandle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Set(stringFieldHandle, value);
                }
            }

            internal uint optionalFieldHandle;

            public int? OptionalField
            {
                get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Get(optionalFieldHandle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Set(optionalFieldHandle, value);
                }
            }

            internal uint listFieldHandle;

            public global::System.Collections.Generic.List<int> ListField
            {
                get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Get(listFieldHandle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Set(listFieldHandle, value);
                }
            }

            internal uint mapFieldHandle;

            public global::System.Collections.Generic.Dictionary<int,string> MapField
            {
                get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Get(mapFieldHandle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Set(mapFieldHandle, value);
                }
            }

            public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
                BlittableBool boolField,
                int intField,
                long longField,
                float floatField,
                double doubleField,
                string stringField,
                int? optionalField,
                global::System.Collections.Generic.List<int> listField,
                global::System.Collections.Generic.Dictionary<int,string> mapField
        )
            {
                var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(1002);
                var obj = schemaComponentData.GetFields();
                {
                    obj.AddBool(1, boolField);
                }
                {
                    obj.AddInt32(2, intField);
                }
                {
                    obj.AddInt64(3, longField);
                }
                {
                    obj.AddFloat(4, floatField);
                }
                {
                    obj.AddDouble(5, doubleField);
                }
                {
                    obj.AddString(6, stringField);
                }
                {
                    if (optionalField.HasValue)
                {
                    obj.AddInt32(7, optionalField.Value);
                }
                
                }
                {
                    foreach (var value in listField)
                {
                    obj.AddInt32(8, value);
                }
                
                }
                {
                    foreach (var keyValuePair in mapField)
                {
                    var mapObj = obj.AddObject(9);
                    mapObj.AddInt32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void SerializeUpdate(Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component component, global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
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
                    
                    if (!component.OptionalField.HasValue)
                    {
                        updateObj.AddClearedField(7);
                    }
                    
                }
                {
                    foreach (var value in component.ListField)
                    {
                        obj.AddInt32(8, value);
                    }
                    
                    if (component.ListField.Count == 0)
                    {
                        updateObj.AddClearedField(8);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.MapField)
                    {
                        var mapObj = obj.AddObject(9);
                        mapObj.AddInt32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                    if (component.MapField.Count == 0)
                    {
                        updateObj.AddClearedField(9);
                    }
                    
                }
            }

            public static Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
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
                    var list = component.ListField = new global::System.Collections.Generic.List<int>();
                    var listLength = obj.GetInt32Count(8);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexInt32(8, (uint) i));
                    }
                    
                }
                component.mapFieldHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Allocate(world);
                {
                    var map = component.MapField = new global::System.Collections.Generic.Dictionary<int,string>();
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

            public static Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
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

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component component)
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

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Component, Update>(NonBlittableComponent.ComponentId, DeserializeData, DeserializeUpdate);
            }
        }
    }
}
