// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests
{
    public partial class NestedComponent
    {
        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 20152;

            public BlittableBool DirtyBit { get; set; }

            private global::Generated.Improbable.Gdk.Tests.TypeName nestedType;

            public global::Generated.Improbable.Gdk.Tests.TypeName NestedType
            {
                get => nestedType;
                set
                {
                    DirtyBit = true;
                    nestedType = value;
                }
            }

            public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
                global::Generated.Improbable.Gdk.Tests.TypeName nestedType
        )
            {
                var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(20152);
                var obj = schemaComponentData.GetFields();
                {
                    global::Generated.Improbable.Gdk.Tests.TypeName.Serialization.Serialize(nestedType, obj.AddObject(1));
                }
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void Serialize(Generated.Improbable.Gdk.Tests.NestedComponent.Component component, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    global::Generated.Improbable.Gdk.Tests.TypeName.Serialization.Serialize(component.NestedType, obj.AddObject(1));
                }
            }

            public static Generated.Improbable.Gdk.Tests.NestedComponent.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Generated.Improbable.Gdk.Tests.NestedComponent.Component();

                {
                    component.NestedType = global::Generated.Improbable.Gdk.Tests.TypeName.Serialization.Deserialize(obj.GetObject(1));
                }
                return component;
            }

            public static Generated.Improbable.Gdk.Tests.NestedComponent.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var update = new Generated.Improbable.Gdk.Tests.NestedComponent.Update();
                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Generated.Improbable.Gdk.Tests.TypeName.Serialization.Deserialize(obj.GetObject(1));
                        update.NestedType = new global::Improbable.Gdk.Core.Option<global::Generated.Improbable.Gdk.Tests.TypeName>(value);
                    }
                    
                }
                return update;
            }

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref Generated.Improbable.Gdk.Tests.NestedComponent.Component component)
            {
                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Generated.Improbable.Gdk.Tests.TypeName.Serialization.Deserialize(obj.GetObject(1));
                        component.NestedType = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<global::Generated.Improbable.Gdk.Tests.TypeName> NestedType;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Generated.Improbable.Gdk.Tests.NestedComponent.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
