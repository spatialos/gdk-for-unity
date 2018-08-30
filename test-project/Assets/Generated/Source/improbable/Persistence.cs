// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable
{
    public partial class Persistence
    {
        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 55;

            public BlittableBool DirtyBit { get; set; }

        }

        public static class Serialization
        {
            public static void Serialize(Generated.Improbable.Persistence.Component component, global::Improbable.Worker.Core.SchemaObject obj)
            {
            }

            public static Generated.Improbable.Persistence.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Generated.Improbable.Persistence.Component();

                return component;
            }

            public static Generated.Improbable.Persistence.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref Generated.Improbable.Persistence.Component component)
            {
                var update = new Generated.Improbable.Persistence.Update();
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
                get => Generated.Improbable.Persistence.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
