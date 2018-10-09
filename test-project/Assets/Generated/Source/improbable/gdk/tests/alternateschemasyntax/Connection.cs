// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;
using System.Collections.Generic;

namespace Improbable.Gdk.Tests.AlternateSchemaSyntax
{
    public partial class Connection
    {
        public struct Component : IComponentData, ISpatialComponentData
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
        }

        public static class Serialization
        {
            public static void SerializeUpdate(Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component component, global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
            }

            public static Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component();

                return component;
            }

            public static Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update();
                var obj = updateObj.GetFields();

                return update;
            }

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component component)
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
                get => Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
