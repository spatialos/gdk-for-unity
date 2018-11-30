using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Subscriptions;
using Unity.Collections;
using Unity.Entities;

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

        [Inject] private WorkerSystem worker;
        [Inject] private EntitySystem entitySystem;

        private readonly IEntityGameObjectCreator gameObjectCreator;
        private EntityGameObjectLinker linker;

        private List<EntityId> entitiesRemoved = new List<EntityId>();

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator)
        {
            this.gameObjectCreator = gameObjectCreator;
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            linker = new EntityGameObjectLinker(World);
        }

        protected override void OnDestroyManager()
        {
            var ids = linker.GetLinkedEntityIds();
            foreach (var id in ids)
            {
                linker.UnlinkAllGameObjectsFromEntityId(id);
                gameObjectCreator.OnEntityRemoved(id);
            }

            base.OnDestroyManager();
        }

        protected override unsafe void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var entity = addedEntitiesData.Entities[i];
                var spatialEntityId = addedEntitiesData.SpatialEntityIds[i].EntityId;
                gameObjectCreator.OnEntityCreated(new SpatialOSEntity(entity, EntityManager), linker);

                PostUpdateCommands.AddComponent(entity, new InitializedEntitySystemState
                {
                    EntityId = spatialEntityId
                });
            }

            EntityId* entitiesToRemove = stackalloc EntityId[removedEntitiesData.Length];
            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entity = removedEntitiesData.Entities[i];
                var spatialEntityId = EntityManager.GetComponentData<InitializedEntitySystemState>(entity).EntityId;
                entitiesToRemove[i] = spatialEntityId;
                linker.UnlinkAllGameObjectsFromEntityId(spatialEntityId);

                PostUpdateCommands.RemoveComponent<InitializedEntitySystemState>(entity);
            }

            linker.FlushCommandBuffer();

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                gameObjectCreator.OnEntityRemoved(entitiesToRemove[i]);
            }
        }
    }
}
