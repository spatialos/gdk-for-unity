// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public struct SpatialOSComponentWithNoFieldsWithEvents : IComponentData, ISpatialComponentData
    {
        public uint ComponentId => 1004;

        public BlittableBool DirtyBit { get; set; }

        public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
        )
        {
            var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(1004);
            var obj = schemaComponentData.GetFields();


            return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
        }


        public static class Serialization
        {
            public static void Serialize(SpatialOSComponentWithNoFieldsWithEvents component, global::Improbable.Worker.Core.SchemaObject obj)
            {
            }

            public static SpatialOSComponentWithNoFieldsWithEvents Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new SpatialOSComponentWithNoFieldsWithEvents();

                return component;
            }

            public static SpatialOSComponentWithNoFieldsWithEvents.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref SpatialOSComponentWithNoFieldsWithEvents component)
            {
                var update = new SpatialOSComponentWithNoFieldsWithEvents.Update();
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
                get => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
