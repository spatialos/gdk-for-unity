using System.Collections.Generic;
using Generated.Improbable;
using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
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
        private struct AddedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<SpatialOSTransform> Transforms;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> NewlyCreatedEntities;
            [ReadOnly] public ComponentDataArray<SpatialOSMetadata> Metadata;
        }

        private struct RemovedEntitiesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<GameObjectReferenceHandle> GameObjectReferenceHandles;
            public SubtractiveComponent<GameObjectReference> NoGameObjectReference;
        }

        [Inject] private AddedEntitiesData addedEntitiesData;
        [Inject] private RemovedEntitiesData removedEntitiesData;

        private Worker worker;
        private ViewCommandBuffer viewCommandBuffer;
        private EntityGameObjectCreator entityGameObjectCreator;
        private EntityGameObjectLinker entityGameObjectLinker;
        private EntityManager entityManager;
        private readonly Dictionary<int, GameObject> entityGameObjectCache = new Dictionary<int, GameObject>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = Worker.GetWorkerFromWorld(World);
            viewCommandBuffer = new ViewCommandBuffer(EntityManager, worker.LogDispatcher);
            entityGameObjectCreator = new EntityGameObjectCreator(World);
            entityManager = World.GetOrCreateManager<EntityManager>();
            entityGameObjectLinker = World.GetOrCreateManager<EntityGameObjectLinkerSystem>().Linker;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var prefabName = addedEntitiesData.Metadata[i].EntityType;
                var transform = addedEntitiesData.Transforms[i];
                var entity = addedEntitiesData.Entities[i];
                var spatialEntityId = addedEntitiesData.SpatialEntityIds[i].EntityId;

                var position = new Vector3(transform.Location.X, transform.Location.Y, transform.Location.Z) +
                    worker.Origin;
                var rotation = new UnityEngine.Quaternion(transform.Rotation.X, transform.Rotation.Y,
                    transform.Rotation.Z, transform.Rotation.W);

                var gameObject =
                    entityGameObjectCreator.CreateEntityGameObject(entity, prefabName, worker.WorkerType,
                        position, rotation, spatialEntityId);
                var gameObjectReference = new GameObjectReference { GameObject = gameObject };

                var requiresSpatialOSBehaviourManagerComponent = new RequiresMonoBehaviourActivationManager();

                entityGameObjectCache[entity.Index] = gameObject;
                var gameObjectReferenceHandleComponent = new GameObjectReferenceHandle();

                PostUpdateCommands.AddComponent(addedEntitiesData.Entities[i], gameObjectReferenceHandleComponent);

                PostUpdateCommands.AddComponent(addedEntitiesData.Entities[i],
                    requiresSpatialOSBehaviourManagerComponent);

                viewCommandBuffer.AddComponent(entity, gameObjectReference);
                entityGameObjectLinker.LinkGameObjectToEntity(gameObject, entity, spatialEntityId,
                    viewCommandBuffer);
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entity = removedEntitiesData.Entities[i];
                var entityIndex = entity.Index;

                if (!entityGameObjectCache.TryGetValue(entityIndex, out var gameObject))
                {
                    worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "GameObject corresponding to removed entity not found.")
                        .WithField("EntityIndex", entityIndex));
                    continue;
                }

                entityGameObjectCache.Remove(entityIndex);
                UnityObjectDestroyer.Destroy(gameObject);
                PostUpdateCommands.RemoveComponent<GameObjectReferenceHandle>(entity);
            }

            viewCommandBuffer.FlushBuffer();
        }
    }
}
