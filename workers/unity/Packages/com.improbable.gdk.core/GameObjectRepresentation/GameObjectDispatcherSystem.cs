using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Gathers incoming dispatcher ops and invokes callbacks on relevant GameObjects.
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.GameObjectReceiveGroup))]
    internal class GameObjectDispatcherSystem : ComponentSystem
    {
        private readonly Dictionary<int, MonoBehaviourActivationManager> entityIndexToActivationManager =
            new Dictionary<int, MonoBehaviourActivationManager>();
        private readonly List<MonoBehaviourActivationManager> activationManagers =
            new Collections.List<MonoBehaviourActivationManager>();

        public readonly List<GameObjectComponentDispatcherBase> GameObjectComponentDispatchers =
            new List<GameObjectComponentDispatcherBase>();

        private ReaderWriterInjector injector;
        private ReaderWriterStore readerWriterStore;
        private ILogDispatcher logger;

        internal void AddSpatialOSBehaviourManager(int entityIndex, MonoBehaviourActivationManager activationManager)
        {
            if (entityIndexToActivationManager.ContainsKey(entityIndex))
            {
                throw new ActivationManagerAlreadyExistsException($"SpatialOSBehaviourManager already exists for entityIndex {entityIndex}.");
            }

            entityIndexToActivationManager[entityIndex] = activationManager;
            activationManagers.Add(activationManager);
        }

        internal void RemoveActivationManager(int entityIndex)
        {
            if (!entityIndexToActivationManager.ContainsKey(entityIndex))
            {
                throw new ActivationManagerNotFoundException($"SpatialOSBehaviourManager not found for entityIndex {entityIndex}.");
            }

            var spatialOSBehaviourManager = entityIndexToActivationManager[entityIndex];
            entityIndexToActivationManager.Remove(entityIndex);
            activationManagers.Remove(spatialOSBehaviourManager);
        }

        public bool HasSpatialOSBehaviourManager(int entityId)
        {
            return entityIndexToActivationManager.ContainsKey(entityId);
        }

        public MonoBehaviourActivationManager GetSpatialOSBehaviourManager(int entityIndex)
        {
            if (!entityIndexToActivationManager.ContainsKey(entityIndex))
            {
                throw new ActivationManagerNotFoundException($"SpatialOSBehaviourManager not found for entityIndex {entityIndex}.");
            }

            return entityIndexToActivationManager[entityIndex];
        }

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            FindGameObjectComponentDispatchers();
            GenerateComponentGroups();

            var entityManager = World.GetOrCreateManager<EntityManager>();
            logger = WorkerRegistry.GetWorkerForWorld(World).View.LogDispatcher;
            injector = new ReaderWriterInjector(entityManager, logger);
            readerWriterStore = new ReaderWriterStore();
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
                gameObjectComponentDispatcher.InvokeOnAddComponentLifecycleCallbacks(entityIndexToActivationManager);
                gameObjectComponentDispatcher.InvokeOnRemoveComponentLifecycleCallbacks(entityIndexToActivationManager);
                gameObjectComponentDispatcher.InvokeOnAuthorityChangeLifecycleCallbacks(entityIndexToActivationManager);
            }

            foreach (var activationManager in activationManagers)
            {
                activationManager.EnableSpatialOSBehaviours();
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityChangeUserCallbacks(readerWriterStore);
                gameObjectComponentDispatcher.InvokeOnComponentUpdateUserCallbacks(readerWriterStore);
                gameObjectComponentDispatcher.InvokeOnEventUserCallbacks(readerWriterStore);
                gameObjectComponentDispatcher.InvokeOnCommandRequestUserCallbacks(readerWriterStore);
            }

            foreach (var activationManager in activationManagers)
            {
                activationManager.DisableSpatialOSBehaviours();
            }
        }

        public class ActivationManagerAlreadyExistsException : Exception
        {
            public ActivationManagerAlreadyExistsException(string message) : base(message)
            {
            }
        }

        public class ActivationManagerNotFoundException : Exception
        {
            public ActivationManagerNotFoundException(string message) : base(message)
            {
            }
        }

        public void CreateActivationManager(Entity entity)
        {
            var gameObject = EntityManager.GetComponentObject<GameObjectReference>(entity).GameObject;
            var manager = new MonoBehaviourActivationManager(gameObject, injector, readerWriterStore, logger);
            if (entityIndexToActivationManager.ContainsKey(entity.Index))
            {
                throw new ActivationManagerAlreadyExistsException($"SpatialOSBehaviourManager already exists for entityIndex {entity.Index}.");
            }

            entityIndexToActivationManager[entity.Index] = manager;
            activationManagers.Add(manager);
        }
    }
}
