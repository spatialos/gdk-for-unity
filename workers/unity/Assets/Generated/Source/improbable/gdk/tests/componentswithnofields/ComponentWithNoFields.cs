// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public struct SpatialOSComponentWithNoFields : IComponentData, ISpatialComponentData
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


        public static class Serialization
        {
            public static void Serialize(SpatialOSComponentWithNoFields component, global::Improbable.Worker.Core.SchemaObject obj)
            {
            }

            public static SpatialOSComponentWithNoFields Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new SpatialOSComponentWithNoFields();

                return component;
            }

            public static SpatialOSComponentWithNoFields.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref SpatialOSComponentWithNoFields component)
            {
                var update = new SpatialOSComponentWithNoFields.Update();
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
                get => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
