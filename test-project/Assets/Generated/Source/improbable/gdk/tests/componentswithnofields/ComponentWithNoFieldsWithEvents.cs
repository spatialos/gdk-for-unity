// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithEvents
    {
        public const uint ComponentId = 1004;

        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 1004;

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
                if (propertyIndex < 0 || propertyIndex >= 0)
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
                if (propertyIndex < 0 || propertyIndex >= 0)
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

            public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
        )
            {
                var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(1004);
                var obj = schemaComponentData.GetFields();
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void SerializeUpdate(Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Component component, global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
            }

            public static Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Component();

                return component;
            }

            public static Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Update();
                var obj = updateObj.GetFields();

                return update;
            }

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Component component)
            {
                var obj = updateObj.GetFields();

            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class ComponentWithNoFieldsWithEventsDynamic : IDynamicInvokable
        {
            public uint ComponentId => ComponentWithNoFieldsWithEvents.ComponentId;

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
                handler.Accept<Component, Update>(ComponentWithNoFieldsWithEvents.ComponentId, DeserializeData, DeserializeUpdate);
            }
        }
    }
}
