// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public struct SpatialOSComponentWithNoFieldsWithCommands : IComponentData, ISpatialComponentData
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


        public static class Serialization
        {
            public static void Serialize(SpatialOSComponentWithNoFieldsWithCommands component, global::Improbable.Worker.Core.SchemaObject obj)
            {
            }

            public static SpatialOSComponentWithNoFieldsWithCommands Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new SpatialOSComponentWithNoFieldsWithCommands();

                return component;
            }

            public static SpatialOSComponentWithNoFieldsWithCommands.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref SpatialOSComponentWithNoFieldsWithCommands component)
            {
                var update = new SpatialOSComponentWithNoFieldsWithCommands.Update();
                return update;
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
                get => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
