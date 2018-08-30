// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable
{
    public partial class EntityAcl
    {
        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 50;

            public BlittableBool DirtyBit { get; set; }

            internal uint readAclHandle;

            public global::Generated.Improbable.WorkerRequirementSet ReadAcl
            {
                get => Generated.Improbable.EntityAcl.ReferenceTypeProviders.ReadAclProvider.Get(readAclHandle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.EntityAcl.ReferenceTypeProviders.ReadAclProvider.Set(readAclHandle, value);
                }
            }

            internal uint componentWriteAclHandle;

            public global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet> ComponentWriteAcl
            {
                get => Generated.Improbable.EntityAcl.ReferenceTypeProviders.ComponentWriteAclProvider.Get(componentWriteAclHandle);
                set
                {
                    DirtyBit = true;
                    Generated.Improbable.EntityAcl.ReferenceTypeProviders.ComponentWriteAclProvider.Set(componentWriteAclHandle, value);
                }
            }

        }

        public static class Serialization
        {
            public static void Serialize(Generated.Improbable.EntityAcl.Component component, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    global::Generated.Improbable.WorkerRequirementSet.Serialization.Serialize(component.ReadAcl, obj.AddObject(1));
                }
                {
                    foreach (var keyValuePair in component.ComponentWriteAcl)
                    {
                        var mapObj = obj.AddObject(2);
                        mapObj.AddUint32(1, keyValuePair.Key);
                        global::Generated.Improbable.WorkerRequirementSet.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                    }
                    
                }
            }

            public static Generated.Improbable.EntityAcl.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Generated.Improbable.EntityAcl.Component();

                component.readAclHandle = Generated.Improbable.EntityAcl.ReferenceTypeProviders.ReadAclProvider.Allocate(world);
                {
                    component.ReadAcl = global::Generated.Improbable.WorkerRequirementSet.Serialization.Deserialize(obj.GetObject(1));
                }
                component.componentWriteAclHandle = Generated.Improbable.EntityAcl.ReferenceTypeProviders.ComponentWriteAclProvider.Allocate(world);
                {
                    var map = component.ComponentWriteAcl = new global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>();
                    var mapSize = obj.GetObjectCount(2);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Generated.Improbable.WorkerRequirementSet.Serialization.Deserialize(mapObj.GetObject(2));
                        map.Add(key, value);
                    }
                    
                }
                return component;
            }

            public static Generated.Improbable.EntityAcl.Update GetAndApplyUpdate(global::Improbable.Worker.Core.SchemaObject obj, ref Generated.Improbable.EntityAcl.Component component)
            {
                var update = new Generated.Improbable.EntityAcl.Update();
                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Generated.Improbable.WorkerRequirementSet.Serialization.Deserialize(obj.GetObject(1));
                        component.ReadAcl = value;
                        update.ReadAcl = new global::Improbable.Gdk.Core.Option<global::Generated.Improbable.WorkerRequirementSet>(value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(2);
                    if (mapSize > 0)
                    {
                        update.ComponentWriteAcl = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>>(new global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>());
                        component.ComponentWriteAcl.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = global::Generated.Improbable.WorkerRequirementSet.Serialization.Deserialize(mapObj.GetObject(2));
                        update.ComponentWriteAcl.Value.Add(key, value);
                        component.ComponentWriteAcl.Add(key, value);
                    }
                    
                }
                return update;
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<global::Generated.Improbable.WorkerRequirementSet> ReadAcl;
            public Option<global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>> ComponentWriteAcl;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Generated.Improbable.EntityAcl.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }
    }
}
