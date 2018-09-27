using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    ///     For each newly added SpatialOS entity, calls IEntityGameObjectCreator to get an associated GameObject
    ///     and links it to the entity via the EntityGameObjectLinker. Also checks for entity removal and calls the
    ///     IEntityGameObjectCreator for cleanup.
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.GameObjectInitializationGroup))]
    internal class GameObjectInitializationSystem : ComponentSystem
    {
        private struct InitializedEntitySystemState : ISystemStateComponentData
        {
            public EntityId EntityId;
        }

        private struct AddedEntitiesData
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> DenotesNewSpatialOSEntity;
            [ReadOnly] public SubtractiveComponent<InitializedEntitySystemState> DenotesEntityIsNotInitialized;
        }

        private struct RemovedEntitiesData
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<InitializedEntitySystemState> DenotesEntityIsInitialized;
            [ReadOnly] public SubtractiveComponent<SpatialEntityId> NoSpatialEntityIds;
        }

        [Inject] private AddedEntitiesData addedEntitiesData;
        [Inject] private RemovedEntitiesData removedEntitiesData;

        [Inject] private EntityGameObjectLinkerSystem linkerSystem;
        [Inject] private WorkerSystem worker;

        private readonly IEntityGameObjectCreator gameObjectCreator;
        private readonly Dictionary<Entity, GameObject> entityToGameObjects = new Dictionary<Entity, GameObject>();
        private ViewCommandBuffer viewCommandBuffer;

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator)
        {
            this.gameObjectCreator = gameObjectCreator;
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            viewCommandBuffer = new ViewCommandBuffer(EntityManager, worker.LogDispatcher);
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var entity = addedEntitiesData.Entities[i];
                var spatialEntityId = addedEntitiesData.SpatialEntityIds[i].EntityId;
                var gameObject = gameObjectCreator.OnEntityCreated(new SpatialOSEntity(entity, EntityManager));
                PostUpdateCommands.AddComponent(entity, new InitializedEntitySystemState { EntityId = spatialEntityId });
                if (gameObject == null)
                {
                    continue;
                }

                linkerSystem.Linker.LinkGameObjectToEntity(gameObject, entity, viewCommandBuffer);
                entityToGameObjects.Add(entity, gameObject);
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entity = removedEntitiesData.Entities[i];
                var spatialEntityId = EntityManager.GetComponentData<InitializedEntitySystemState>(entity).EntityId;
                if (entityToGameObjects.TryGetValue(entity, out var gameObject))
                {
                    linkerSystem.Linker.UnlinkGameObjectFromEntity(gameObject, entity, viewCommandBuffer);
                }

                gameObjectCreator.OnEntityRemoved(spatialEntityId, gameObject);
                entityToGameObjects.Remove(entity);
                PostUpdateCommands.RemoveComponent<InitializedEntitySystemState>(entity);
            }

            viewCommandBuffer.FlushBuffer();
        }

        protected override void OnDestroyManager()
        {
            if (entityToGameObjects.Count > 0)
            {
                foreach (var entityToGameObject in entityToGameObjects)
                {
                    var entity = entityToGameObject.Key;
                    var gameObject = entityToGameObject.Value;
                    var spatialEntityId = EntityManager.GetComponentData<InitializedEntitySystemState>(entity).EntityId;
                    linkerSystem.Linker.UnlinkGameObjectFromEntity(gameObject, entity, viewCommandBuffer);
                    gameObjectCreator.OnEntityRemoved(spatialEntityId, gameObject);
                }

                viewCommandBuffer.FlushBuffer();
                entityToGameObjects.Clear();
            }

            base.OnDestroyManager();
        }
    }
}
