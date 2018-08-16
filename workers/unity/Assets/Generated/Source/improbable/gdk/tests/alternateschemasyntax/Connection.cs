// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax
{
    public struct SpatialOSConnection : IComponentData, ISpatialComponentData
    {
        public uint ComponentId => 1105;

        public BlittableBool DirtyBit { get; set; }

        public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
        )
        {
            var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(1105);
            var obj = schemaComponentData.GetFields();


            return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
        }


        public static class Serialization
        {
            public static void Serialize(SpatialOSConnection component, global::Improbable.Worker.Core.SchemaObject obj)
            {
            }

            public static SpatialOSConnection Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new SpatialOSConnection();

                return component;
            }

            public static SpatialOSConnection.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref SpatialOSConnection component)
            {
                var update = new SpatialOSConnection.Update();
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
                get => Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
