// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public struct SpatialOSBlittableComponent : IComponentData, ISpatialComponentData
    {
        public uint ComponentId => 1001;

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

        public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
            BlittableBool boolField,
            int intField,
            long longField,
            float floatField,
            double doubleField
        )
        {
            var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(1001);
            var obj = schemaComponentData.GetFields();

            obj.AddBool(1, boolField);
            obj.AddInt32(2, intField);
            obj.AddInt64(3, longField);
            obj.AddFloat(4, floatField);
            obj.AddDouble(5, doubleField);

            return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
        }


        public static class Serialization
        {
            public static void Serialize(SpatialOSBlittableComponent component, global::Improbable.Worker.Core.SchemaObject obj)
            {
                obj.AddBool(1, component.BoolField);
                obj.AddInt32(2, component.IntField);
                obj.AddInt64(3, component.LongField);
                obj.AddFloat(4, component.FloatField);
                obj.AddDouble(5, component.DoubleField);
            }

            public static SpatialOSBlittableComponent Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new SpatialOSBlittableComponent();


                component.BoolField = obj.GetBool(1);

                component.IntField = obj.GetInt32(2);

                component.LongField = obj.GetInt64(3);

                component.FloatField = obj.GetFloat(4);

                component.DoubleField = obj.GetDouble(5);
                return component;
            }

            public static SpatialOSBlittableComponent.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref SpatialOSBlittableComponent component)
            {
                var update = new SpatialOSBlittableComponent.Update();
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
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
