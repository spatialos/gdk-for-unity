using System.Linq;
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

        private EntityQuery newEntitiesQuery;
        private EntityQuery removedEntitiesQuery;

        private ComponentType[] minimumComponentSet = new[]
        {
            ComponentType.ReadOnly<SpatialEntityId>()
        };

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator, GameObject workerGameObject)
        {
            this.gameObjectCreator = gameObjectCreator;
            this.workerGameObject = workerGameObject;

            var minCreatorComponentSet = gameObjectCreator.MinimumComponentTypes;
            if (minCreatorComponentSet != null)
            {
                minimumComponentSet = minimumComponentSet
                    .Concat(minCreatorComponentSet)
                    .ToArray();
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            entitySystem = World.GetExistingSystem<EntitySystem>();

            Linker = new EntityGameObjectLinker(World);

            if (workerGameObject != null)
            {
                Linker.LinkGameObjectToSpatialOSEntity(new EntityId(0), workerGameObject);
            }

            newEntitiesQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = minimumComponentSet,
                None = new[] { ComponentType.ReadOnly<GameObjectInitializationComponent>() }
            });

            removedEntitiesQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new[] { ComponentType.ReadOnly<GameObjectInitializationComponent>() },
                None = minimumComponentSet
            });
        }

        protected override void OnDestroy()
        {
            EntityManager.RemoveComponent<GameObjectInitializationComponent>(GetEntityQuery(typeof(GameObjectInitializationComponent)));

            Linker.UnlinkAllGameObjects();

            foreach (var entityId in entitySystem.GetEntitiesInView())
            {
                gameObjectCreator.OnEntityRemoved(entityId);
            }

            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
            Entities.With(newEntitiesQuery).ForEach((Entity entity, ref SpatialEntityId spatialEntityId) =>
            {
                gameObjectCreator.OnEntityCreated(new SpatialOSEntity(entity, EntityManager), Linker);
                PostUpdateCommands.AddComponent(entity, new GameObjectInitializationComponent
                {
                    EntityId = spatialEntityId.EntityId
                });
            });

            Entities.With(removedEntitiesQuery).ForEach((ref GameObjectInitializationComponent state) =>
            {
                Linker.UnlinkAllGameObjectsFromEntityId(state.EntityId);
                gameObjectCreator.OnEntityRemoved(state.EntityId);
            });

            Linker.FlushCommandBuffer();

            EntityManager.RemoveComponent<GameObjectInitializationComponent>(removedEntitiesQuery);
        }
    }
}
