// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public const uint ComponentId = 1001;

        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 1001;

            private BlittableBool isDirty;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            // Each byte tracks 8 component properties.
            private byte dirtyBits0;

            public bool IsDirty()
            {
                return isDirty;
            }

            /*
            The propertyIndex arguments starts counting from 0. It depends on the order of which you defined
            your component properties in a schema component but is not the schema field number itself. E.g.
            component MyComponent
            {
                id = 1337;
                bool val_a = 1;
                bool val_b = 3;
            }
            In that case, val_a uses propertyIndex 0 and val_b uses propertyIndex 1 in this method.
            */
            public bool IsDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("propertyIndex argument out of range.");
                }

                var byteBatch = propertyIndex / 8;
                switch (byteBatch)
                {
                    case 0:
                        return (dirtyBits0 & (0x1 << propertyIndex % 8)) != 0x0;
                    default:
                        throw new ArgumentException("propertyIndex argument out of range.");
                }
            }

            // like the IsDirty() method above, the propertyIndex arguments starts counting from 0.
            public void MarkDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("propertyIndex argument out of range.");
                }

                var byteBatch = propertyIndex / 8;
                switch (byteBatch)
                {
                    case 0:
                        dirtyBits0 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                    default:
                        throw new ArgumentException("propertyIndex argument out of range.");
                }

                isDirty = true;
            }

            public void MarkNotDirty()
            {
                dirtyBits0 = 0x0;
                isDirty = false;
            }

            private BlittableBool boolField;

            public BlittableBool BoolField
            {
                get => boolField;
                set
                {
                    MarkDirty(0);
                    boolField = value;
                }
            }

            private int intField;

            public int IntField
            {
                get => intField;
                set
                {
                    MarkDirty(1);
                    intField = value;
                }
            }

            private long longField;

            public long LongField
            {
                get => longField;
                set
                {
                    MarkDirty(2);
                    longField = value;
                }
            }

            private float floatField;

            public float FloatField
            {
                get => floatField;
                set
                {
                    MarkDirty(3);
                    floatField = value;
                }
            }

            private double doubleField;

            public double DoubleField
            {
                get => doubleField;
                set
                {
                    MarkDirty(4);
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
            public static void SerializeUpdate(Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component component, global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDirty(0))
                    {
                        obj.AddBool(1, component.BoolField);
                    }

                }
                {
                    if (component.IsDirty(1))
                    {
                        obj.AddInt32(2, component.IntField);
                    }

                }
                {
                    if (component.IsDirty(2))
                    {
                        obj.AddInt64(3, component.LongField);
                    }

                }
                {
                    if (component.IsDirty(3))
                    {
                        obj.AddFloat(4, component.FloatField);
                    }

                }
                {
                    if (component.IsDirty(4))
                    {
                        obj.AddDouble(5, component.DoubleField);
                    }

                }
            }

            public static Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component();

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

            public static Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update();
                var obj = updateObj.GetFields();

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
                return update;
            }

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component component)
            {
                var obj = updateObj.GetFields();

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
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class BlittableComponentDynamic : IDynamicInvokable
        {
            public uint ComponentId => BlittableComponent.ComponentId;

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
                handler.Accept<Component, Update>(BlittableComponent.ComponentId, DeserializeData, DeserializeUpdate);
            }
        }
    }
}
