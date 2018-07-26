using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Creates and removes SpatialOSBehaviourManager object for EntityGameObjects.
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.GameObjectInitialisationGroup))]
    public class SpatialOSBehaviourManagerInitializationSystem : ComponentSystem
    {
        public struct AddedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentArray<GameObjectReference> GameObjectReferences;
            [ReadOnly] public ComponentDataArray<GameObjectReferenceHandle> GameObjectReferenceHandles;

            [ReadOnly]
            public ComponentDataArray<RequiresSpatialOSBehaviourManager> RequiresSpatialOSBehaviourManagerTags;
        }

        public struct RemovedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<GameObjectReferenceHandle> GameObjectReferenceHandles;
            public SubtractiveComponent<GameObjectReference> NoGameObjectReference;
        }

        [Inject] private AddedEntitiesData addedEntitiesData;
        [Inject] private RemovedEntitiesData removedEntitiesData;

        private GameObjectDispatcherSystem gameObjectDispatcherSystem;
        private ILogDispatcher logDispatcher;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            gameObjectDispatcherSystem = World.GetOrCreateManager<GameObjectDispatcherSystem>();
            logDispatcher = WorkerRegistry.GetWorkerForWorld(World).View.LogDispatcher;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var entityIndex = addedEntitiesData.Entities[i].Index;
                if (gameObjectDispatcherSystem.EntityIndexToSpatialOSBehaviourManager.ContainsKey(entityIndex))
                {
                    logDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Entity already has a SpatialOSBehaviourManager.")
                        .WithField("EntityIndex", entityIndex));
                    continue;
                }

                gameObjectDispatcherSystem.EntityIndexToSpatialOSBehaviourManager[entityIndex] =
                    new SpatialOSBehaviourManager(addedEntitiesData.GameObjectReferences[i].GameObject);
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entityIndex = removedEntitiesData.Entities[i].Index;
                if (!gameObjectDispatcherSystem.EntityIndexToSpatialOSBehaviourManager.ContainsKey(entityIndex))
                {
                    logDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "SpatialOSBehaviourManager corresponding to removed entity not found.")
                        .WithField("EntityIndex", entityIndex));
                    continue;
                }

                gameObjectDispatcherSystem.EntityIndexToSpatialOSBehaviourManager.Remove(entityIndex);
            }
        }

        public class GameObjectDispatcherSystemNotFoundException : Exception
        {
            public GameObjectDispatcherSystemNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
