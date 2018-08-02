// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests
{
    public partial class ExhaustiveMapKey
    {
        internal class GameObjectComponentDispatcher : GameObjectComponentDispatcherBase
        {
            public override ComponentType[] ComponentAddedComponentTypes => new ComponentType[]
            {
                typeof(ComponentAdded<SpatialOSExhaustiveMapKey>), typeof(GameObjectReference)
            };

            public override ComponentType[] ComponentRemovedComponentTypes => new ComponentType[]
            {
                typeof(ComponentRemoved<SpatialOSExhaustiveMapKey>), typeof(GameObjectReference)
            };

            public override ComponentType[] AuthoritiesChangedComponentTypes => new ComponentType[]
            {
                typeof(AuthoritiesChanged<SpatialOSExhaustiveMapKey>), typeof(GameObjectReference)
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
                typeof(ComponentsUpdated<SpatialOSExhaustiveMapKey.Update>), typeof(GameObjectReference)
            };

            public override ComponentType[][] EventsReceivedComponentTypeArrays => new ComponentType[][]
            {
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
            };

            public override void InvokeOnAddComponentLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var entities = ComponentAddedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    spatialOSBehaviourManager.AddComponent(197719);
                }
            }

            public override void InvokeOnRemoveComponentLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var entities = ComponentRemovedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    spatialOSBehaviourManager.RemoveComponent(197719);
                }
            }

            public override void InvokeOnAuthorityChangeLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSExhaustiveMapKey>>();
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    for (var j = 0; j < authoritiesChangedTags[i].Buffer.Count; j++)
                    {
                        spatialOSBehaviourManager.ChangeAuthority(197719, authoritiesChangedTags[i].Buffer[j]);
                    }
                }
            }

            public override void InvokeOnAuthorityChangeUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
            }

            public override void InvokeOnComponentUpdateUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
            }

            public override void InvokeOnEventUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
            }

            public override void InvokeOnCommandRequestUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
            }
        }
    }
}
