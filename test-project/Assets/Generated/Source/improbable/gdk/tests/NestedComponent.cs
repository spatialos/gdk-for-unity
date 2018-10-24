// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests
{
    public partial class NestedComponent
    {
        public const uint ComponentId = 20152;

        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 20152;

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
                if (propertyIndex < 0 || propertyIndex >= 1)
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
                if (propertyIndex < 0 || propertyIndex >= 1)
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

            private global::Improbable.Gdk.Tests.TypeName nestedType;

            public global::Improbable.Gdk.Tests.TypeName NestedType
            {
                get => nestedType;
                set
                {
                    MarkDirty(0);
                    nestedType = value;
                }
            }

            public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
                global::Improbable.Gdk.Tests.TypeName nestedType
        )
            {
                var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(20152);
                var obj = schemaComponentData.GetFields();
                {
                    global::Improbable.Gdk.Tests.TypeName.Serialization.Serialize(nestedType, obj.AddObject(1));
                }
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void SerializeUpdate(Improbable.Gdk.Tests.NestedComponent.Component component, global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDirty(0))
                    {
                        global::Improbable.Gdk.Tests.TypeName.Serialization.Serialize(component.NestedType, obj.AddObject(1));
                    }

                }
            }

            public static Improbable.Gdk.Tests.NestedComponent.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.NestedComponent.Component();

                {
                    component.NestedType = global::Improbable.Gdk.Tests.TypeName.Serialization.Deserialize(obj.GetObject(1));
                }
                return component;
            }

            public static Improbable.Gdk.Tests.NestedComponent.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.NestedComponent.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.Gdk.Tests.TypeName.Serialization.Deserialize(obj.GetObject(1));
                        update.NestedType = new global::Improbable.Gdk.Core.Option<global::Improbable.Gdk.Tests.TypeName>(value);
                    }
                    
                }
                return update;
            }

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.NestedComponent.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.Gdk.Tests.TypeName.Serialization.Deserialize(obj.GetObject(1));
                        component.NestedType = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::Improbable.Gdk.Tests.TypeName> NestedType;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.NestedComponent.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class NestedComponentDynamic : IDynamicInvokable
        {
            public uint ComponentId => NestedComponent.ComponentId;

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
                handler.Accept<Component, Update>(NestedComponent.ComponentId, DeserializeData, DeserializeUpdate);
            }
        }
    }
}
