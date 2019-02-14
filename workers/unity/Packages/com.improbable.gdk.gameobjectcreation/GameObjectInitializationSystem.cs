using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    ///     For each newly added SpatialOS entity, calls IEntityGameObjectCreator to get an associated GameObject
    ///     and links it to the entity via the EntityGameObjectLinker. Also checks for entity removal and calls the
    ///     IEntityGameObjectCreator for cleanup.
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(GameObjectInitializationGroup))]
    internal class GameObjectInitializationSystem : ComponentSystem
    {
        private readonly IEntityGameObjectCreator gameObjectCreator;
        private EntityGameObjectLinker linker;

        private EntitySystem entitySystem;
        private WorkerSystem workerSystem;

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator)
        {
            this.gameObjectCreator = gameObjectCreator;
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            entitySystem = World.GetExistingManager<EntitySystem>();
            workerSystem = World.GetExistingManager<WorkerSystem>();

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
            foreach (var entityId in entitySystem.GetEntitiesAdded())
            {
                workerSystem.TryGetEntity(entityId, out var entity);
                gameObjectCreator.OnEntityCreated(new SpatialOSEntity(entity, EntityManager), linker);
            }

            var removedEntities = entitySystem.GetEntitiesRemoved();
            foreach (var entityId in removedEntities)
            {
                linker.UnlinkAllGameObjectsFromEntityId(entityId);
            }

            linker.FlushCommandBuffer();

            foreach (var entityId in removedEntities)
            {
                gameObjectCreator.OnEntityRemoved(entityId);
            }
        }
    }
}
