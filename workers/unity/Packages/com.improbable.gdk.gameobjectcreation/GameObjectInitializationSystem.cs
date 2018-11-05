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

        private readonly IEntityGameObjectCreator gameObjectCreator;

        private EntityGameObjectLinker linker;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            linker = new EntityGameObjectLinker(World);
        }

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator)
        {
            this.gameObjectCreator = gameObjectCreator;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < addedEntitiesData.Length; i++)
            {
                var entity = addedEntitiesData.Entities[i];
                var spatialEntityId = addedEntitiesData.SpatialEntityIds[i].EntityId;
                gameObjectCreator.OnEntityCreated(new SpatialOSEntity(entity, EntityManager), linker);

                PostUpdateCommands.AddComponent<InitializedEntitySystemState>(entity, new InitializedEntitySystemState
                {
                    EntityId = spatialEntityId
                });
            }

            for (var i = 0; i < removedEntitiesData.Length; i++)
            {
                var entity = removedEntitiesData.Entities[i];
                var spatialEntityId = EntityManager.GetComponentData<InitializedEntitySystemState>(entity).EntityId;
                linker.UnlinkAllGameObjectsFromEntity(spatialEntityId);

                PostUpdateCommands.RemoveComponent<InitializedEntitySystemState>(entity);
            }

            linker.FlushCommandBuffer();
        }

        protected override void OnDestroyManager()
        {
        }
    }
}
