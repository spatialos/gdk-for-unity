// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public struct Component : IComponentData, ISpatialComponentData
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
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void Serialize(Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component component, global::Improbable.Worker.Core.SchemaObject obj)
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
            }

            public static Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component();

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
                return component;
            }

            public static Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component component)
            {
                var update = new Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update();
                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        component.BoolField = value;
                        update.BoolField = new global::Improbable.Gdk.Core.Option<BlittableBool>(value);
                    }
                    
                }
                {
                    if (obj.GetInt32Count(2) == 1)
                    {
                        var value = obj.GetInt32(2);
                        component.IntField = value;
                        update.IntField = new global::Improbable.Gdk.Core.Option<int>(value);
                    }
                    
                }
                {
                    if (obj.GetInt64Count(3) == 1)
                    {
                        var value = obj.GetInt64(3);
                        component.LongField = value;
                        update.LongField = new global::Improbable.Gdk.Core.Option<long>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        component.FloatField = value;
                        update.FloatField = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(5) == 1)
                    {
                        var value = obj.GetDouble(5);
                        component.DoubleField = value;
                        update.DoubleField = new global::Improbable.Gdk.Core.Option<double>(value);
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
