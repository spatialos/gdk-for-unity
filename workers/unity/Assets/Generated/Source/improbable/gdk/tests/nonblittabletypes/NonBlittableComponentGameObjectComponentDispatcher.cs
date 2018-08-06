// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        internal class GameObjectComponentDispatcher : GameObjectComponentDispatcherBase
        {
            public override ComponentType[] ComponentAddedComponentTypes => new ComponentType[]
            {
                typeof(ComponentAdded<SpatialOSNonBlittableComponent>), typeof(GameObjectReference)
            };

            public override ComponentType[] ComponentRemovedComponentTypes => new ComponentType[]
            {
                typeof(ComponentRemoved<SpatialOSNonBlittableComponent>), typeof(GameObjectReference)
            };

            public override ComponentType[] AuthoritiesChangedComponentTypes => new ComponentType[]
            {
                typeof(AuthoritiesChanged<SpatialOSNonBlittableComponent>), typeof(GameObjectReference)
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
                typeof(ComponentsUpdated<SpatialOSNonBlittableComponent.Update>), typeof(GameObjectReference)
            };

            public override ComponentType[][] EventsReceivedComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { typeof(EventsReceived<FirstEventEvent>), typeof(GameObjectReference) },
                new ComponentType[] { typeof(EventsReceived<SecondEventEvent>), typeof(GameObjectReference) },
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { typeof(CommandRequests<FirstCommand.Request>), typeof(GameObjectReference) },
                new ComponentType[] { typeof(CommandRequests<SecondCommand.Request>), typeof(GameObjectReference) },
            };

            public override void InvokeOnAddComponentLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var entities = ComponentAddedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    spatialOSBehaviourManager.AddComponent(1002);
                }
            }

            public override void InvokeOnRemoveComponentLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var entities = ComponentRemovedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    spatialOSBehaviourManager.RemoveComponent(1002);
                }
            }

            public override void InvokeOnAuthorityChangeLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSNonBlittableComponent>>();
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    for (var j = 0; j < authoritiesChangedTags[i].Buffer.Count; j++)
                    {
                        spatialOSBehaviourManager.ChangeAuthority(1002, authoritiesChangedTags[i].Buffer[j]);
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
