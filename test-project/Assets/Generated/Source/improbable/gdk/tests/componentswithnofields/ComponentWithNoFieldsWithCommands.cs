// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 1005;

            public BlittableBool DirtyBit { get; set; }

            public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
        )
            {
                var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(1005);
                var obj = schemaComponentData.GetFields();
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void SerializeUpdate(Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component component, global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
            }

            public static Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component();

                return component;
            }

            public static Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update();
                var obj = updateObj.GetFields();

                return update;
            }

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component component)
            {
                var obj = updateObj.GetFields();

            }
        }

        public struct Update : ISpatialComponentUpdate
        {
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
