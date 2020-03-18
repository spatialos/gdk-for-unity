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
        internal EntityGameObjectLinker Linker;

        private readonly IEntityGameObjectCreator gameObjectCreator;

        private readonly GameObject workerGameObject;

        private EntitySystem entitySystem;
        private WorkerSystem workerSystem;

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator, GameObject workerGameObject)
        {
            this.gameObjectCreator = gameObjectCreator;
            this.workerGameObject = workerGameObject;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            entitySystem = World.GetExistingSystem<EntitySystem>();
            workerSystem = World.GetExistingSystem<WorkerSystem>();

            Linker = new EntityGameObjectLinker(World);

            if (workerGameObject != null)
            {
                Linker.LinkGameObjectToSpatialOSEntity(new EntityId(0), workerGameObject);
            }
        }

        protected override void OnDestroy()
        {
            Linker.UnlinkAllGameObjects();

            foreach (var entityId in entitySystem.GetEntitiesInView())
            {
                gameObjectCreator.OnEntityRemoved(entityId);
            }

            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
            foreach (var entityId in entitySystem.GetEntitiesAdded())
            {
                var entity = workerSystem.GetEntity(entityId);
                gameObjectCreator.OnEntityCreated(new SpatialOSEntity(entity, EntityManager), Linker);
            }

            var removedEntities = entitySystem.GetEntitiesRemoved();
            foreach (var entityId in removedEntities)
            {
                Linker.UnlinkAllGameObjectsFromEntityId(entityId);
            }

            Linker.FlushCommandBuffer();

            foreach (var entityId in removedEntities)
            {
                gameObjectCreator.OnEntityRemoved(entityId);
            }
        }
    }
}
