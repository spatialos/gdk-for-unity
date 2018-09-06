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
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.GameObjectReceiveGroup))]
    internal class GameObjectDispatcherSystem : ComponentSystem
    {
        private readonly Dictionary<Entity, MonoBehaviourActivationManager> entityToActivationManager =
            new Dictionary<Entity, MonoBehaviourActivationManager>();
        private readonly Dictionary<Entity, InjectableStore> entityToReaderWriterStore =
            new Dictionary<Entity, InjectableStore>();

        public readonly List<GameObjectComponentDispatcherBase> GameObjectComponentDispatchers =
            new List<GameObjectComponentDispatcherBase>();

        private RequiredFieldInjector injector;
        private ILogDispatcher logger;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            FindGameObjectComponentDispatchers();
            GenerateComponentGroups();

            var entityManager = World.GetOrCreateManager<EntityManager>();
            logger = World.GetExistingManager<WorkerSystem>().LogDispatcher;
            injector = new RequiredFieldInjector(entityManager, logger);
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
                gameObjectComponentDispatcher.AuthorityGainedComponentGroup =
                    GetComponentGroup(gameObjectComponentDispatcher.AuthorityGainedComponentTypes);
                gameObjectComponentDispatcher.AuthorityLostComponentGroup =
                    GetComponentGroup(gameObjectComponentDispatcher.AuthorityLostComponentTypes);
                gameObjectComponentDispatcher.AuthorityLossImminentComponentGroup =
                    GetComponentGroup(gameObjectComponentDispatcher.AuthorityLossImminentComponentTypes);

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

                if (gameObjectComponentDispatcher.CommandResponsesComponentTypeArrays.Length > 0)
                {
                    gameObjectComponentDispatcher.CommandResponsesComponentGroups =
                        new ComponentGroup[gameObjectComponentDispatcher.CommandResponsesComponentTypeArrays.Length];
                    for (var i = 0; i < gameObjectComponentDispatcher.CommandResponsesComponentTypeArrays.Length; i++)
                    {
                        gameObjectComponentDispatcher.CommandResponsesComponentGroups[i] =
                            GetComponentGroup(gameObjectComponentDispatcher.CommandResponsesComponentTypeArrays[i]);
                    }
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.MarkComponentsRemovedForDeactivation(entityToActivationManager);
                gameObjectComponentDispatcher.MarkAuthorityLostForDeactivation(entityToActivationManager);
            }

            foreach (var indexManagerPair in entityToActivationManager)
            {
                indexManagerPair.Value.DisableSpatialOSBehaviours();
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityLostCallbacks(entityToReaderWriterStore);
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnComponentUpdateCallbacks(entityToReaderWriterStore);
                gameObjectComponentDispatcher.InvokeOnEventCallbacks(entityToReaderWriterStore);
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityGainedCallbacks(entityToReaderWriterStore);
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.MarkAuthorityGainedForActivation(entityToActivationManager);
                gameObjectComponentDispatcher.MarkComponentsAddedForActivation(entityToActivationManager);
            }

            foreach (var indexManagerPair in entityToActivationManager)
            {
                indexManagerPair.Value.EnableSpatialOSBehaviours();
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityLossImminentCallbacks(entityToReaderWriterStore);
                gameObjectComponentDispatcher.InvokeOnCommandRequestCallbacks(entityToReaderWriterStore);
                gameObjectComponentDispatcher.InvokeOnCommandResponseCallbacks(entityToReaderWriterStore);
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

        internal void CreateActivationManagerAndReaderWriterStore(Entity entity, GameObject gameObject)
        {
            if (entityToActivationManager.ContainsKey(entity))
            {
                throw new ActivationManagerAlreadyExistsException($"MonoBehaviourActivationManager already exists for entity {entity.Index}.");
            }

            var store = new InjectableStore();
            entityToReaderWriterStore.Add(entity, store);
            var manager = new MonoBehaviourActivationManager(gameObject, injector, store, logger);
            entityToActivationManager.Add(entity, manager);
        }

        internal void RemoveActivationManagerAndReaderWriterStore(Entity entity)
        {
            if (!entityToActivationManager.TryGetValue(entity, out var activationManager))
            {
                throw new ActivationManagerNotFoundException($"MonoBehaviourActivationManager not found for entity {entity.Index}.");
            }

            entityToActivationManager.Remove(entity);
            entityToReaderWriterStore.Remove(entity);

            // Disable enabled SpatialOSBehaviours and dispose leftover Requirables.
            activationManager.Dispose();
        }

        protected override void OnDestroyManager()
        {
            foreach (var entity in new List<Entity>(entityToActivationManager.Keys))
            {
                RemoveActivationManagerAndReaderWriterStore(entity);
            }

            base.OnDestroyManager();
        }
    }
}
