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
        private readonly Dictionary<int, SpatialOSBehaviourManager> entityIndexToSpatialOSBehaviourManager =
            new Dictionary<int, SpatialOSBehaviourManager>();
        private readonly List<SpatialOSBehaviourManager> spatialOSBehaviourManagers =
            new Collections.List<SpatialOSBehaviourManager>();

        public readonly List<GameObjectComponentDispatcherBase> GameObjectComponentDispatchers =
            new List<GameObjectComponentDispatcherBase>();

        internal void AddSpatialOSBehaviourManager(int entityIndex, SpatialOSBehaviourManager spatialOSBehaviourManager)
        {
            if (entityIndexToSpatialOSBehaviourManager.ContainsKey(entityIndex))
            {
                throw new SpatialOSBehaviourManagerAlreadyExistsException($"SpatialOSBehaviourManager already exists for entityIndex {entityIndex}.");
            }

            entityIndexToSpatialOSBehaviourManager[entityIndex] = spatialOSBehaviourManager;
            spatialOSBehaviourManagers.Add(spatialOSBehaviourManager);
        }

        internal void RemoveSpatialOSBehaviourManager(int entityIndex)
        {
            if (!entityIndexToSpatialOSBehaviourManager.ContainsKey(entityIndex))
            {
                throw new SpatialOSBehaviourManagerNotFoundException($"SpatialOSBehaviourManager not found for entityIndex {entityIndex}.");
            }

            var spatialOSBehaviourManager = entityIndexToSpatialOSBehaviourManager[entityIndex];
            entityIndexToSpatialOSBehaviourManager.Remove(entityIndex);
            spatialOSBehaviourManagers.Remove(spatialOSBehaviourManager);
        }

        public bool HasSpatialOSBehaviourManager(int entityId)
        {
            return entityIndexToSpatialOSBehaviourManager.ContainsKey(entityId);
        }

        public SpatialOSBehaviourManager GetSpatialOSBehaviourManager(int entityIndex)
        {
            if (!entityIndexToSpatialOSBehaviourManager.ContainsKey(entityIndex))
            {
                throw new SpatialOSBehaviourManagerNotFoundException($"SpatialOSBehaviourManager not found for entityIndex {entityIndex}.");
            }

            return entityIndexToSpatialOSBehaviourManager[entityIndex];
        }

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
                .Where(type => typeof(GameObjectComponentDispatcherBase).IsAssignableFrom(type) && !type.IsAbstract)
                .ToList();

            GameObjectComponentDispatchers.AddRange(gameObjectComponentDispatcherTypes.Select(type =>
                (GameObjectComponentDispatcherBase) Activator.CreateInstance(type)));
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
            }

            foreach (var spatialOSBehaviourManager in spatialOSBehaviourManagers)
            {
                    spatialOSBehaviourManager.EnableSpatialOSBehaviours();
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityChangeUserCallbacks(this);
                gameObjectComponentDispatcher.InvokeOnComponentUpdateUserCallbacks(this);
                gameObjectComponentDispatcher.InvokeOnEventUserCallbacks(this);
                gameObjectComponentDispatcher.InvokeOnCommandRequestUserCallbacks(this);
            }

            foreach (var spatialOSBehaviourManager in spatialOSBehaviourManagers)
            {
                spatialOSBehaviourManager.DisableSpatialOSBehaviours();
            }
        }

        public class SpatialOSBehaviourManagerAlreadyExistsException : Exception
        {
            public SpatialOSBehaviourManagerAlreadyExistsException(string message) : base(message)
            {
            }
        }

        public class SpatialOSBehaviourManagerNotFoundException : Exception
        {
            public SpatialOSBehaviourManagerNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
