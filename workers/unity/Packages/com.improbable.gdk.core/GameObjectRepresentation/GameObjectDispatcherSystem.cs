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
        private readonly Dictionary<int, InjectableStore> entityIndexToReaderWriterStore =
            new Dictionary<int, InjectableStore>();

        public readonly List<GameObjectComponentDispatcherBase> GameObjectComponentDispatchers =
            new List<GameObjectComponentDispatcherBase>();

        private RequiredFieldInjector injector;
        private ILogDispatcher logger;

        internal void RemoveActivationManagerAndReaderWriterStore(int entityIndex)
        {
            if (!entityIndexToActivationManager.ContainsKey(entityIndex))
            {
                throw new ActivationManagerNotFoundException($"MonoBehaviourActivationManager not found for entityIndex {entityIndex}.");
            }

            entityIndexToActivationManager.Remove(entityIndex);
            entityIndexToReaderWriterStore.Remove(entityIndex);
        }

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            FindGameObjectComponentDispatchers();
            GenerateComponentGroups();

            var entityManager = World.GetOrCreateManager<EntityManager>();
            logger = Worker.GetWorkerFromWorld(World).LogDispatcher;
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
            }
        }

        protected override void OnUpdate()
        {
            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.MarkComponentsRemovedForDeactivation(entityIndexToActivationManager);
                gameObjectComponentDispatcher.MarkAuthorityLostForDeactivation(entityIndexToActivationManager);
            }

            foreach (var indexManagerPair in entityIndexToActivationManager)
            {
                indexManagerPair.Value.DisableSpatialOSBehaviours();
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityLostCallbacks(entityIndexToReaderWriterStore);
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnComponentUpdateCallbacks(entityIndexToReaderWriterStore);
                gameObjectComponentDispatcher.InvokeOnEventCallbacks(entityIndexToReaderWriterStore);
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityGainedCallbacks(entityIndexToReaderWriterStore);
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.MarkAuthorityGainedForActivation(entityIndexToActivationManager);
                gameObjectComponentDispatcher.MarkComponentsAddedForActivation(entityIndexToActivationManager);
            }

            foreach (var indexManagerPair in entityIndexToActivationManager)
            {
                indexManagerPair.Value.EnableSpatialOSBehaviours();
            }

            foreach (var gameObjectComponentDispatcher in GameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityLossImminentCallbacks(entityIndexToReaderWriterStore);
                gameObjectComponentDispatcher.InvokeOnCommandRequestCallbacks(entityIndexToReaderWriterStore);
                gameObjectComponentDispatcher.InvokeOnCommandResponseCallbacks(entityIndexToReaderWriterStore);
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

        public void CreateActivationManagerAndReaderWriterStore(Entity entity)
        {
            if (entityIndexToActivationManager.ContainsKey(entity.Index))
            {
                throw new ActivationManagerAlreadyExistsException($"MonoBehaviourActivationManager already exists for entityIndex {entity.Index}.");
            }

            var gameObject = EntityManager.GetComponentObject<GameObjectReference>(entity).GameObject;
            var store = new InjectableStore();
            entityIndexToReaderWriterStore[entity.Index] = store;
            var manager = new MonoBehaviourActivationManager(gameObject, injector, store, logger);
            entityIndexToActivationManager[entity.Index] = manager;
        }
    }
}
