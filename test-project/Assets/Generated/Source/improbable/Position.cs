// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable
{
    public partial class Position
    {
        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 54;

            public BlittableBool DirtyBit { get; set; }

            private global::Generated.Improbable.Coordinates coords;

            public global::Generated.Improbable.Coordinates Coords
            {
                get => coords;
                set
                {
                    DirtyBit = true;
                    coords = value;
                }
            }

        }

        public static class Serialization
        {
            public static void Serialize(Generated.Improbable.Position.Component component, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    global::Generated.Improbable.Coordinates.Serialization.Serialize(component.Coords, obj.AddObject(1));
                }
            }

            public static Generated.Improbable.Position.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Generated.Improbable.Position.Component();

                {
                    component.Coords = global::Generated.Improbable.Coordinates.Serialization.Deserialize(obj.GetObject(1));
                }
                return component;
            }

            public static Generated.Improbable.Position.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref Generated.Improbable.Position.Component component)
            {
                var update = new Generated.Improbable.Position.Update();
                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Generated.Improbable.Coordinates.Serialization.Deserialize(obj.GetObject(1));
                        component.Coords = value;
                        update.Coords = new global::Improbable.Gdk.Core.Option<global::Generated.Improbable.Coordinates>(value);
                    }
                    
                }
                return update;
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<global::Generated.Improbable.Coordinates> Coords;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Generated.Improbable.Position.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
