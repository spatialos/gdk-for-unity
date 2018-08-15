using System.Collections.Generic;
using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    /// <summary>
    ///     Creates a companion gameobject for newly spawned entities according to a prefab definition.
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.EntityInitialisationGroup))]
    internal class GameObjectInitializationSystem : ComponentSystem
    {
        public struct AddedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentArray<SpatialOSPrefab> PrefabNames;
            [ReadOnly] public ComponentDataArray<SpatialOSTransform> Transforms;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> NewlyCreatedEntities;
        }

        public struct RemovedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<GameObjectReferenceHandle> GameObjectReferenceHandles;
            public SubtractiveComponent<GameObjectReference> NoGameObjectReference;
        }

        public struct WorkerData
        {
            public readonly int Length;
            [ReadOnly] public SharedComponentDataArray<WorkerConfig> WorkerConfigs;
        }

        [Inject] private AddedEntitiesData addedEntitiesData;
        [Inject] private RemovedEntitiesData removedEntitiesData;
        [Inject] private WorkerData workerData;

        private ViewCommandBuffer viewCommandBuffer;
        private EntityGameObjectCreator entityGameObjectCreator;
        private EntityGameObjectLinker entityGameObjectLinker;
        private uint currentHandle;
        private readonly Dictionary<int, GameObject> entityGameObjectCache = new Dictionary<int, GameObject>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            var worker = workerData.WorkerConfigs[0].Worker;
            viewCommandBuffer = new ViewCommandBuffer(worker.LogDispatcher);
            entityGameObjectLinker = new EntityGameObjectLinker(World, worker.LogDispatcher);
            entityGameObjectCreator = new EntityGameObjectCreator(World);
        }

        protected override void OnUpdate()
        {
            if (workerData.Length == 0)
            {
                new LoggingDispatcher().HandleLog(LogType.Error, new LogEvent("This system should not have been run without a worker entity"));
            }

            var worker = workerData.WorkerConfigs[0].Worker;

            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var prefabMapping = PrefabConfig.PrefabMappings[addedEntitiesData.PrefabNames[i].Prefab];
                var transform = addedEntitiesData.Transforms[i];
                var entity = addedEntitiesData.Entities[i];
                var spatialEntityId = addedEntitiesData.SpatialEntityIds[i].EntityId;

                if (!(SystemConfig.UnityGameLogic.Equals(worker.WorkerType)) && !(SystemConfig.UnityClient.Equals(worker.WorkerType)))
                {
                    worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Worker type isn't supported by the GameObjectInitializationSystem.")
                        .WithField("WorldName", World.Name)
                        .WithField("WorkerType", worker.WorkerType));
                    continue;
                }

                var prefabName = SystemConfig.UnityGameLogic.Equals(worker.WorkerType)
                    ? prefabMapping.UnityGameLogic
                    : prefabMapping.UnityClient;

                var position = new Vector3(transform.Location.X, transform.Location.Y, transform.Location.Z) + worker.Origin;
                var rotation = new UnityEngine.Quaternion(transform.Rotation.X, transform.Rotation.Y,
                    transform.Rotation.Z, transform.Rotation.W);

                var gameObject =
                    entityGameObjectCreator.CreateEntityGameObject(entity, prefabName, position, rotation,
                        spatialEntityId);
                var gameObjectReference = new GameObjectReference { GameObject = gameObject };

                entityGameObjectCache[entity.Index] = gameObject;
                var gameObjectReferenceHandleComponent = new GameObjectReferenceHandle();

                PostUpdateCommands.AddComponent(addedEntitiesData.Entities[i], gameObjectReferenceHandleComponent);
                viewCommandBuffer.AddComponent(entity, gameObjectReference);
                entityGameObjectLinker.LinkGameObjectToEntity(gameObject, entity, spatialEntityId,
                    viewCommandBuffer);
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entityIndex = removedEntitiesData.Entities[i].Index;
                if (!entityGameObjectCache.TryGetValue(entityIndex, out var gameObject))
                {
                    worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "GameObject corresponding to removed entity not found.")
                        .WithField("EntityIndex", entityIndex));
                    continue;
                }

                entityGameObjectCache.Remove(entityIndex);
                UnityObjectDestroyer.Destroy(gameObject);
                PostUpdateCommands.RemoveComponent<GameObjectReferenceHandle>(
                    removedEntitiesData.Entities[i]);
            }

            viewCommandBuffer.FlushBuffer(EntityManager);
        }
    }
}
