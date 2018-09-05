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
    [DisableAutoCreation]
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

        private WorkerSystem worker;
        private ViewCommandBuffer viewCommandBuffer;
        private EntityGameObjectLinker entityGameObjectLinker;
        private readonly Dictionary<Entity, GameObject> entityGameObjectCache = new Dictionary<Entity, GameObject>();
        private readonly IEntityGameObjectCreator gameObjectCreator;

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator)
        {
            this.gameObjectCreator = gameObjectCreator;
        }

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = World.GetExistingManager<WorkerSystem>();
            viewCommandBuffer = new ViewCommandBuffer(EntityManager, worker.LogDispatcher);
            entityGameObjectLinker = World.GetOrCreateManager<EntityGameObjectLinkerSystem>().Linker;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var entity = addedEntitiesData.Entities[i];
                var spatialEntityId = addedEntitiesData.SpatialEntityIds[i].EntityId;

                var gameObject = gameObjectCreator.CreateGameObjectForEntityAdded(
                    new SpatialOSEntity(entity, EntityManager), worker);

                if (gameObject == null)
                {
                    continue;
                }

                var gameObjectReference = new GameObjectReference { GameObject = gameObject };

                var requiresSpatialOSBehaviourManagerComponent = new RequiresMonoBehaviourActivationManager();

                entityGameObjectCache.Add(entity, gameObject);
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

                if (!entityGameObjectCache.TryGetValue(entity, out var gameObject))
                {
                    // Entity without linked GameObject removed
                    gameObjectCreator.OnEntityRemoved(new SpatialOSEntity(entity, EntityManager),
                        worker, null);
                }
                else
                {
                    entityGameObjectCache.Remove(entity);
                    gameObjectCreator.OnEntityRemoved(new SpatialOSEntity(entity, EntityManager),
                        worker, gameObject);
                    PostUpdateCommands.RemoveComponent<GameObjectReferenceHandle>(entity);
                }
            }

            viewCommandBuffer.FlushBuffer();
        }

        protected override void OnDestroyManager()
        {
            base.OnDestroyManager();

            foreach (var entityToGameObject in entityGameObjectCache)
            {
                gameObjectCreator.OnEntityRemoved(new SpatialOSEntity(entityToGameObject.Key, EntityManager),
                    worker, entityToGameObject.Value);
            }

            entityGameObjectCache.Clear();
        }
    }
}
