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

        [Inject] private AddedEntitiesData addedEntitiesData;
        [Inject] private RemovedEntitiesData removedEntitiesData;

        private MutableView view;
        private WorkerBase worker;
        private Vector3 origin;
        private readonly ViewCommandBuffer viewCommandBuffer = new ViewCommandBuffer();
        private EntityGameObjectCreator entityGameObjectCreator;
        private uint currentHandle;
        private readonly Dictionary<int, GameObject> entityGameObjectCache = new Dictionary<int, GameObject>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;
            origin = worker.Origin;
            entityGameObjectCreator = new EntityGameObjectCreator(World);
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var prefabMapping = PrefabConfig.PrefabMappings[addedEntitiesData.PrefabNames[i].Prefab];
                var transform = addedEntitiesData.Transforms[i];
                var entity = addedEntitiesData.Entities[i];
                var spatialEntityId = addedEntitiesData.SpatialEntityIds[i].EntityId;

                if (!(worker is UnityClient) && !(worker is UnityGameLogic))
                {
                    view.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Worker type isn't supported by the GameObjectInitializationSystem.")
                        .WithField("WorldName", World.Name)
                        .WithField("WorkerType", worker));
                    continue;
                }

                var prefabName = worker is UnityGameLogic
                    ? prefabMapping.UnityGameLogic
                    : prefabMapping.UnityClient;

                var position = new Vector3(transform.Location.X, transform.Location.Y, transform.Location.Z) + origin;
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
                worker.EntityGameObjectLinker.LinkGameObjectToEntity(gameObject, entity, spatialEntityId,
                    viewCommandBuffer);
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entityIndex = removedEntitiesData.Entities[i].Index;
                GameObject gameObject;
                if (!entityGameObjectCache.TryGetValue(entityIndex, out gameObject))
                {
                    view.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "GameObject corresponding to removed entity not found.")
                        .WithField("EntityIndex", entityIndex));
                    continue;
                }

                entityGameObjectCache.Remove(entityIndex);
                UnityObjectDestroyer.Destroy(gameObject);
                PostUpdateCommands.RemoveComponent<GameObjectReferenceHandle>(
                    removedEntitiesData.Entities[i]);
            }

            viewCommandBuffer.FlushBuffer(view);
        }
    }
}
