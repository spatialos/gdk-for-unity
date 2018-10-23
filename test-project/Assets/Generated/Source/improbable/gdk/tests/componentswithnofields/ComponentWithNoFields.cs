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
    public partial class ComponentWithNoFields
    {
        public const uint ComponentId = 1003;

        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 1003;

            public BlittableBool DirtyBit { get; set; }

            public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
        )
            {
                var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(1003);
                var obj = schemaComponentData.GetFields();
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void SerializeUpdate(Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Component component, global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
            }

            public static Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Component();

                return component;
            }

            public static Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Update();
                var obj = updateObj.GetFields();

                return update;
            }

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Component component)
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
                get => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class ComponentWithNoFieldsDynamic : IDynamicInvokable
        {
            public uint ComponentId => ComponentWithNoFields.ComponentId;

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
                handler.Accept<Component, Update>(ComponentWithNoFields.ComponentId, DeserializeData, DeserializeUpdate);
            }
        }
    }
}
