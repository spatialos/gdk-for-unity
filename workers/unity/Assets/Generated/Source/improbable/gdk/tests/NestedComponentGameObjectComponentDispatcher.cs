// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests
{
    public partial class NestedComponent
    {
        internal class GameObjectComponentDispatcher : GameObjectComponentDispatcherBase
        {
            public override ComponentType[] ComponentAddedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentAdded<SpatialOSNestedComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentRemovedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentRemoved<SpatialOSNestedComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] AuthoritiesChangedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthoritiesChanged<SpatialOSNestedComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentsUpdated<SpatialOSNestedComponent.Update>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[][] EventsReceivedComponentTypeArrays => new ComponentType[][]
            {
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
            };

            protected override uint getComponentId()
            {
                return 20152;
            }

            public override void InvokeOnComponentUpdateUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var entities = ComponentsUpdatedComponentGroup.GetEntityArray();
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentArray<ComponentsUpdated<SpatialOSNestedComponent.Update>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var readers = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index)
                        .GetReadersWriters(getComponentId());
                    var updateList = updateLists[i];
                    foreach (var reader in readers)
                    {
                        foreach (var update in updateList.Buffer)
                        {
                            ((SpatialOSNestedComponent.ReaderWriterImpl) reader).OnComponentUpdate(update);
                        }
                    }
                }
            }

            public override void InvokeOnEventUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
            }

            public override void InvokeOnCommandRequestUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
            }

            public override void InvokeOnAuthorityChangeLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSNestedComponent>>();
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    for (var j = 0; j < authoritiesChangedTags[i].Buffer.Count; j++)
                    {
                        spatialOSBehaviourManager.ChangeAuthority(20152, authoritiesChangedTags[i].Buffer[j]);
                    }
                }
            }

            public override void InvokeOnAuthorityChangeUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                var authChangeLists = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSNestedComponent>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var readers = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index)
                        .GetReadersWriters(getComponentId());
                    var authChanges = authChangeLists[i];
                    foreach (var reader in readers)
                    {
                        foreach (var auth in authChanges.Buffer)
                        {
                            ((SpatialOSNestedComponent.ReaderWriterImpl) reader).OnAuthorityChange(auth);
                        }
                    }
                }
            }
        }
    }
}
