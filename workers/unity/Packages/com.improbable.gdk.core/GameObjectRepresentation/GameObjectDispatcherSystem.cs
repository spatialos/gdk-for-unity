using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Profiling;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Gathers incoming dispatcher ops and invokes callbacks on relevant GameObjects.
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.GameObjectReceiveGroup))]
    internal class GameObjectDispatcherSystem : ComponentSystem
    {
        private struct HasActivationManagerSystemState : ISystemStateComponentData
        {
        }

        private struct AddedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentArray<GameObjectReference> GameObjectReferences;
            [ReadOnly] public SubtractiveComponent<HasActivationManagerSystemState> DenotesThereIsNoActivationManager;
        }

        private struct RemovedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<HasActivationManagerSystemState> DenotesThereIsAnActivationManager;
            [ReadOnly] public SubtractiveComponent<GameObjectReference> NoGameObjectReference;
        }

        internal readonly Dictionary<Entity, InjectableStore> EntityToReaderWriterStore =
            new Dictionary<Entity, InjectableStore>();

        private readonly List<GameObjectComponentDispatcherBase> gameObjectComponentDispatchers =
            new List<GameObjectComponentDispatcherBase>();

        private readonly Dictionary<Entity, MonoBehaviourActivationManager> entityToActivationManager =
            new Dictionary<Entity, MonoBehaviourActivationManager>();

        [Inject] private AddedEntitiesData addedEntitiesData;
        [Inject] private RemovedEntitiesData removedEntitiesData;

        [Inject] private WorkerSystem worker;

        private RequiredFieldInjector injector;
        private ILogDispatcher logger;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            FindGameObjectComponentDispatchers();
            GenerateComponentGroups();

            logger = worker.LogDispatcher;
            injector = new RequiredFieldInjector(EntityManager, logger);
        }

        protected override void OnUpdate()
        {
            Profiler.BeginSample("CreateActivationManagerAndReaderWriterStore");
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var entity = addedEntitiesData.Entities[i];
                CreateActivationManagerAndReaderWriterStore(entity);
                PostUpdateCommands.AddComponent(entity, new HasActivationManagerSystemState());
            }

            Profiler.EndSample();

            Profiler.BeginSample("RemoveActivationManagerAndReaderWriterStore");
            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entity = removedEntitiesData.Entities[i];
                RemoveActivationManagerAndReaderWriterStore(entity);
                PostUpdateCommands.RemoveComponent<HasActivationManagerSystemState>(entity);
            }

            Profiler.EndSample();

            Profiler.BeginSample("UpdateMonoBehaviours");
            UpdateMonoBehaviours();
            Profiler.EndSample();
        }

        protected override void OnDestroyManager()
        {
            foreach (var entity in new List<Entity>(entityToActivationManager.Keys))
            {
                RemoveActivationManagerAndReaderWriterStore(entity);
            }

            base.OnDestroyManager();
        }

        private void FindGameObjectComponentDispatchers()
        {
            var gameObjectComponentDispatcherTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(GameObjectComponentDispatcherBase).IsAssignableFrom(type) && !type.IsAbstract)
                .ToList();

            gameObjectComponentDispatchers.AddRange(gameObjectComponentDispatcherTypes.Select(type =>
                (GameObjectComponentDispatcherBase) Activator.CreateInstance(type)));
        }

        private void GenerateComponentGroups()
        {
            foreach (var gameObjectComponentDispatcher in gameObjectComponentDispatchers)
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

        private void UpdateMonoBehaviours()
        {
            Profiler.BeginSample("MarkForDeactivation");
            foreach (var gameObjectComponentDispatcher in gameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.MarkComponentsRemovedForDeactivation(entityToActivationManager);
                gameObjectComponentDispatcher.MarkAuthorityLostForDeactivation(entityToActivationManager);
            }

            Profiler.EndSample();
            Profiler.BeginSample("DisableSpatialOSBehaviours");
            foreach (var indexManagerPair in entityToActivationManager)
            {
                indexManagerPair.Value.DisableSpatialOSBehaviours();
            }

            Profiler.EndSample();
            Profiler.BeginSample("InvokeOnAuthorityLostCallbacks");
            foreach (var gameObjectComponentDispatcher in gameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityLostCallbacks(EntityToReaderWriterStore);
            }

            Profiler.EndSample();
            Profiler.BeginSample("InvokeUpdateEventsCallbacks");
            foreach (var gameObjectComponentDispatcher in gameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnComponentUpdateCallbacks(EntityToReaderWriterStore);
                gameObjectComponentDispatcher.InvokeOnEventCallbacks(EntityToReaderWriterStore);
            }

            Profiler.EndSample();
            Profiler.BeginSample("InvokeOnAuthorityGainedCallbacks");
            foreach (var gameObjectComponentDispatcher in gameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityGainedCallbacks(EntityToReaderWriterStore);
            }

            Profiler.EndSample();
            Profiler.BeginSample("MarkForActivation");
            foreach (var gameObjectComponentDispatcher in gameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.MarkAuthorityGainedForActivation(entityToActivationManager);
                gameObjectComponentDispatcher.MarkComponentsAddedForActivation(entityToActivationManager);
            }

            Profiler.EndSample();
            Profiler.BeginSample("EnableSpatialOSBehaviours");
            foreach (var indexManagerPair in entityToActivationManager)
            {
                indexManagerPair.Value.EnableSpatialOSBehaviours();
            }

            Profiler.EndSample();
            Profiler.BeginSample("InvokeAuthorityCommandsCallbacks");
            foreach (var gameObjectComponentDispatcher in gameObjectComponentDispatchers)
            {
                gameObjectComponentDispatcher.InvokeOnAuthorityLossImminentCallbacks(EntityToReaderWriterStore);
                gameObjectComponentDispatcher.InvokeOnCommandRequestCallbacks(EntityToReaderWriterStore);
                gameObjectComponentDispatcher.InvokeOnCommandResponseCallbacks(EntityToReaderWriterStore);
            }

            Profiler.EndSample();
        }

        private void CreateActivationManagerAndReaderWriterStore(Entity entity)
        {
            if (entityToActivationManager.ContainsKey(entity))
            {
                throw new ArgumentException($"{nameof(MonoBehaviourActivationManager)} already exists for entity {entity.Index}.");
            }

            var store = new InjectableStore();
            EntityToReaderWriterStore.Add(entity, store);
            var gameObject = EntityManager.GetComponentObject<GameObjectReference>(entity).GameObject;
            var manager = new MonoBehaviourActivationManager(gameObject, injector, store, logger);
            entityToActivationManager.Add(entity, manager);
        }

        private void RemoveActivationManagerAndReaderWriterStore(Entity entity)
        {
            if (!entityToActivationManager.TryGetValue(entity, out var activationManager))
            {
                throw new KeyNotFoundException($"{nameof(MonoBehaviourActivationManager)} not found for entity {entity.Index}.");
            }

            entityToActivationManager.Remove(entity);
            EntityToReaderWriterStore.Remove(entity);

            // Disable enabled SpatialOSBehaviours and dispose leftover Requirables.
            activationManager.Dispose();
        }
    }
}
