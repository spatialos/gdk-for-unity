// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public struct SpatialOSNonBlittableComponent : IComponentData, ISpatialComponentData
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
            get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Get(stringFieldHandle);
            set
            {
                DirtyBit = true;
                Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Set(stringFieldHandle, value);
            }
        }

        internal uint optionalFieldHandle;

        public global::System.Nullable<int> OptionalField
        {
            get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Get(optionalFieldHandle);
            set
            {
                DirtyBit = true;
                Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Set(optionalFieldHandle, value);
            }
        }

        internal uint listFieldHandle;

        public global::System.Collections.Generic.List<int> ListField
        {
            get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Get(listFieldHandle);
            set
            {
                DirtyBit = true;
                Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Set(listFieldHandle, value);
            }
        }

        internal uint mapFieldHandle;

        public global::System.Collections.Generic.Dictionary<int, string> MapField
        {
            get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Get(mapFieldHandle);
            set
            {
                DirtyBit = true;
                Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Set(mapFieldHandle, value);
            }
        }

        public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
            BlittableBool boolField,
            int intField,
            long longField,
            float floatField,
            double doubleField,
            string stringField,
            global::System.Nullable<int> optionalField,
            global::System.Collections.Generic.List<int> listField,
            global::System.Collections.Generic.Dictionary<int, string> mapField
        )
        {
            var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(1002);
            var obj = schemaComponentData.GetFields();

            obj.AddBool(1, boolField);
            obj.AddInt32(2, intField);
            obj.AddInt64(3, longField);
            obj.AddFloat(4, floatField);
            obj.AddDouble(5, doubleField);
            obj.AddString(6, stringField);
            if (optionalField.HasValue)
            {
                obj.AddInt32(7, optionalField.Value);
            }
            foreach (var value in listField)
            {
                obj.AddInt32(8, value);
            }
            foreach (var keyValuePair in mapField)
            {
                var mapObj = obj.AddObject(9);
                mapObj.AddInt32(1, keyValuePair.Key);
                mapObj.AddString(2, keyValuePair.Value);
            }

            return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
        }


        public static class Serialization
        {
            public static void Serialize(SpatialOSNonBlittableComponent component, global::Improbable.Worker.Core.SchemaObject obj)
            {
                obj.AddBool(1, component.BoolField);
                obj.AddInt32(2, component.IntField);
                obj.AddInt64(3, component.LongField);
                obj.AddFloat(4, component.FloatField);
                obj.AddDouble(5, component.DoubleField);
                obj.AddString(6, component.StringField);
                if (component.OptionalField.HasValue)
                {
                    obj.AddInt32(7, component.OptionalField.Value);
                }
                foreach (var value in component.ListField)
                {
                    obj.AddInt32(8, value);
                }
                foreach (var keyValuePair in component.MapField)
                {
                    var mapObj = obj.AddObject(9);
                    mapObj.AddInt32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
            }

            public static SpatialOSNonBlittableComponent Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new SpatialOSNonBlittableComponent();


                component.BoolField = obj.GetBool(1);

                component.IntField = obj.GetInt32(2);

                component.LongField = obj.GetInt64(3);

                component.FloatField = obj.GetFloat(4);

                component.DoubleField = obj.GetDouble(5);
                component.stringFieldHandle = Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Allocate(world);

                component.StringField = obj.GetString(6);
                component.optionalFieldHandle = Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Allocate(world);

                if (obj.GetInt32Count(7) == 1)
                {
                    component.OptionalField = obj.GetInt32(7);
                }
                component.listFieldHandle = Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Allocate(world);

                component.ListField = new global::System.Collections.Generic.List<int>();
                for (var i = 0; i < obj.GetInt32Count(8); i++)
                {
                    component.ListField.Add(obj.IndexInt32(8, (uint) i));
                }

                component.mapFieldHandle = Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Allocate(world);

                {
                    component.MapField = new global::System.Collections.Generic.Dictionary<int,string>();
                    var mapSize = obj.GetObjectCount(9);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        component.MapField.Add(key, value);
                    }
                }
                return component;
            }

            public static SpatialOSNonBlittableComponent.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref SpatialOSNonBlittableComponent component)
            {
                var update = new SpatialOSNonBlittableComponent.Update();
                if (obj.GetBoolCount(1) == 1)
                {
                    var value = obj.GetBool(1);
                    update.BoolField = new Option<BlittableBool>(value);
                    component.BoolField = value;
                }
                if (obj.GetInt32Count(2) == 1)
                {
                    var value = obj.GetInt32(2);
                    update.IntField = new Option<int>(value);
                    component.IntField = value;
                }
                if (obj.GetInt64Count(3) == 1)
                {
                    var value = obj.GetInt64(3);
                    update.LongField = new Option<long>(value);
                    component.LongField = value;
                }
                if (obj.GetFloatCount(4) == 1)
                {
                    var value = obj.GetFloat(4);
                    update.FloatField = new Option<float>(value);
                    component.FloatField = value;
                }
                if (obj.GetDoubleCount(5) == 1)
                {
                    var value = obj.GetDouble(5);
                    update.DoubleField = new Option<double>(value);
                    component.DoubleField = value;
                }
                if (obj.GetStringCount(6) == 1)
                {
                    var value = obj.GetString(6);
                    update.StringField = new Option<string>(value);
                    component.StringField = value;
                }
                if (obj.GetInt32Count(7) == 1)
                {
                    var value = obj.GetInt32(7);
                    update.OptionalField = new Option<global::System.Nullable<int>>(value);
                    component.OptionalField = value;
                }
                {
                    var listSize = obj.GetInt32Count(8);
                    if (listSize > 0)
                    {
                        update.ListField = new Option<global::System.Collections.Generic.List<int>>(new global::System.Collections.Generic.List<int>());
                        component.ListField.Clear();
                    }

                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexInt32(8, (uint) i);
                        update.ListField.Value.Add(value);
                        component.ListField.Add(value);
                    }
                }
                {
                    var mapSize = obj.GetObjectCount(9);
                    if (mapSize > 0)
                    {
                        component.MapField.Clear();
                        update.MapField = new Option<global::System.Collections.Generic.Dictionary<int, string>>(new global::System.Collections.Generic.Dictionary<int, string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        update.MapField.Value.Add(key, value);
                        component.MapField.Add(key, value);
                    }
                }
                return update;
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<BlittableBool> BoolField;
            public Option<int> IntField;
            public Option<long> LongField;
            public Option<float> FloatField;
            public Option<double> DoubleField;
            public Option<string> StringField;
            public Option<global::System.Nullable<int>> OptionalField;
            public Option<global::System.Collections.Generic.List<int>> ListField;
            public Option<global::System.Collections.Generic.Dictionary<int, string>> MapField;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
