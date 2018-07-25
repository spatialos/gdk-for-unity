using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Gathers incoming dispatcher ops and invokes callbacks on relevant GameObjects.
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.GameObjectReceiveGroup))]
    public class GameObjectDispatcherSystem : ComponentSystem
    {
        public readonly Dictionary<int, SpatialOSBehaviourManager> EntityIndexToSpatialOSBehaviourManager = new Dictionary<int, SpatialOSBehaviourManager>();

        public readonly HashSet<GameObjectComponentDispatcherBase> GameObjectComponentDispatchers =
            new HashSet<GameObjectComponentDispatcherBase>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            FindGameObjectComponentDispatchers();
            GenerateComponentGroups();
        }

        private void FindGameObjectComponentDispatchers()
        {
            var gameObjectComponentDispatcherTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(GameObjectComponentDispatcherBase).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var gameObjectComponentDispatcherType in gameObjectComponentDispatcherTypes)
            {
                var gameObjectComponentDispatcher =
                    (GameObjectComponentDispatcherBase)Activator.CreateInstance(gameObjectComponentDispatcherType);
                GameObjectComponentDispatchers.Add(gameObjectComponentDispatcher);
            }
        }

        private void GenerateComponentGroups()
        {
            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.ComponentAddedComponentGroup =
                    GetComponentGroup(gameObjectComponentDispatcher.ComponentAddedComponentTypes);
                gameObjectComponentDispatcher.ComponentRemovedComponentGroup =
                    GetComponentGroup(gameObjectComponentDispatcher.ComponentRemovedComponentTypes);
                gameObjectComponentDispatcher.AuthoritiesChangedComponentGroup =
                    GetComponentGroup(gameObjectComponentDispatcher.AuthoritiesChangedComponentTypes);
                if (gameObjectComponentDispatcher.ComponentsUpdatedComponentTypes.Length > 0)
                {
                    gameObjectComponentDispatcher.ComponentsUpdatedComponentGroup =
                        GetComponentGroup(gameObjectComponentDispatcher.ComponentsUpdatedComponentTypes);
                }

                if (gameObjectComponentDispatcher.EventsReceivedComponentTypeArrays.Length > 0)
                {
                    gameObjectComponentDispatcher.EventsReceivedComponentGroups =
                        new ComponentGroup[gameObjectComponentDispatcher.EventsReceivedComponentTypeArrays.Length];
                    for (var i = 0; i < gameObjectComponentDispatcher.EventsReceivedComponentTypeArrays.Length; i++)
                    {
                        gameObjectComponentDispatcher.EventsReceivedComponentGroups[i] =
                            GetComponentGroup(gameObjectComponentDispatcher.EventsReceivedComponentTypeArrays[i]);
                    }
                }

                if (gameObjectComponentDispatcher.CommandRequestsComponentTypeArrays.Length > 0)
                {
                    gameObjectComponentDispatcher.CommandRequestsComponentGroups =
                        new ComponentGroup[gameObjectComponentDispatcher.CommandRequestsComponentTypeArrays.Length];
                    for (var i = 0; i < gameObjectComponentDispatcher.CommandRequestsComponentTypeArrays.Length; i++)
                    {
                        gameObjectComponentDispatcher.CommandRequestsComponentGroups[i] =
                            GetComponentGroup(gameObjectComponentDispatcher.CommandRequestsComponentTypeArrays[i]);
                    }
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAddComponentLifecycleCallbacks(this);
                gameObjectComponentDispatcher.InvokeOnRemoveComponentLifecycleCallbacks(this);
                gameObjectComponentDispatcher.InvokeOnAuthorityChangeLifecycleCallbacks(this);

                foreach (var spatialOSBehaviourManager in EntityIndexToSpatialOSBehaviourManager.Values)
                {
                    spatialOSBehaviourManager.EnableSpatialOSBehaviours();
                }

                gameObjectComponentDispatcher.InvokeOnAuthorityChangeUserCallbacks(this);
                gameObjectComponentDispatcher.InvokeOnComponentUpdateUserCallbacks(this);
                gameObjectComponentDispatcher.InvokeOnEventUserCallbacks(this);
                gameObjectComponentDispatcher.InvokeOnCommandRequestUserCallbacks(this);

                foreach (var spatialOSBehaviourManager in EntityIndexToSpatialOSBehaviourManager.Values)
                {
                    spatialOSBehaviourManager.DisableSpatialOSBehaviours();
                }
            }
        }
    }
}
