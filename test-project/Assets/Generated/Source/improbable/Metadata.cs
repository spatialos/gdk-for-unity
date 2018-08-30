// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable
{
    public partial class Metadata
    {
        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 53;

            public BlittableBool DirtyBit { get; set; }

            internal uint entityTypeHandle;

            public string EntityType
            {
                get => Generated.Improbable.Metadata.ReferenceTypeProviders.EntityTypeProvider.Get(entityTypeHandle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.Metadata.ReferenceTypeProviders.EntityTypeProvider.Set(entityTypeHandle, value);
                }
            }

        }

        public static class Serialization
        {
            public static void Serialize(Generated.Improbable.Metadata.Component component, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    obj.AddString(1, component.EntityType);
                }
            }

            public static Generated.Improbable.Metadata.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Generated.Improbable.Metadata.Component();

                component.entityTypeHandle = Generated.Improbable.Metadata.ReferenceTypeProviders.EntityTypeProvider.Allocate(world);
                {
                    component.EntityType = obj.GetString(1);
                }
                return component;
            }

            public static Generated.Improbable.Metadata.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref Generated.Improbable.Metadata.Component component)
            {
                var update = new Generated.Improbable.Metadata.Update();
                {
                    if (obj.GetStringCount(1) == 1)
                    {
                        var value = obj.GetString(1);
                        component.EntityType = value;
                        update.EntityType = new global::Improbable.Gdk.Core.Option<string>(value);
                    }
                    
                }
                return update;
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<string> EntityType;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Generated.Improbable.Metadata.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
