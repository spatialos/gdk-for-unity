using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
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
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> NewlyCreatedEntities;
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
        private EntityGameObjectLinker entityGameObjectLinker;
        private EntityManager entityManager;
        private readonly Dictionary<int, GameObject> entityGameObjectCache = new Dictionary<int, GameObject>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            if (EntityGameObjectCreationConfig.EntityGameObjectCreator == null)
            {
                Enabled = false;
                return;
            }

            worker = Worker.GetWorkerFromWorld(World);
            viewCommandBuffer = new ViewCommandBuffer(EntityManager, worker.LogDispatcher);
            entityManager = World.GetOrCreateManager<EntityManager>();
            entityGameObjectLinker = World.GetOrCreateManager<EntityGameObjectLinkerSystem>().Linker;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var entity = addedEntitiesData.Entities[i];
                var spatialEntityId = addedEntitiesData.SpatialEntityIds[i].EntityId;

                var gameObject = EntityGameObjectCreationConfig.EntityGameObjectCreator.CreateGameObjectForEntity(
                    new SpatialOSEntity(entity, entityManager), worker);

                if (gameObject == null)
                {
                    continue;
                }

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
