using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;

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

        private readonly GameObject workerGameObject;

        private EntityGameObjectLinker linker;

        private EntitySystem entitySystem;
        private WorkerSystem workerSystem;

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator, GameObject workerGameObject)
        {
            this.gameObjectCreator = gameObjectCreator;
            this.workerGameObject = workerGameObject;
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            entitySystem = World.GetExistingManager<EntitySystem>();
            workerSystem = World.GetExistingManager<WorkerSystem>();

            linker = new EntityGameObjectLinker(World);

            if (workerGameObject != null)
            {
                linker.LinkGameObjectToSpatialOSEntity(new EntityId(0), workerGameObject);
            }
        }

        protected override void OnDestroyManager()
        {
            linker.UnlinkAllGameObjects();

            foreach (var entityId in entitySystem.GetEntitiesInView())
            {
                gameObjectCreator.OnEntityRemoved(entityId);
            }

            base.OnDestroyManager();
        }

        protected override void OnUpdate()
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
