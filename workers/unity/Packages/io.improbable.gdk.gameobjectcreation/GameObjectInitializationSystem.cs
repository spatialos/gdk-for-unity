using System.Collections.Generic;
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

        private readonly ComponentType[] minimumComponentSet =
        {
            ComponentType.ReadOnly<SpatialEntityId>(), ComponentType.ReadOnly<Metadata.Component>()
        };

        private readonly Dictionary<string, EntityTypeRegistration> entityTypeRegistrations
            = new Dictionary<string, EntityTypeRegistration>();

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator, GameObject workerGameObject)
        {
            this.gameObjectCreator = gameObjectCreator;
            this.workerGameObject = workerGameObject;

            var creatorComponentTypes = gameObjectCreator.MinimumComponentTypes;
            if (creatorComponentTypes != null)
            {
                minimumComponentSet = minimumComponentSet
                    .Union(creatorComponentTypes)
                    .ToArray();
            }

            gameObjectCreator.Register(entityTypeRegistrations);
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
                None = new[] { ComponentType.ReadOnly<GameObjectInitSystemStateComponent>() }
            });

            removedEntitiesQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new[] { ComponentType.ReadOnly<GameObjectInitSystemStateComponent>() },
                None = minimumComponentSet
            });
        }

        protected override void OnDestroy()
        {
            EntityManager.RemoveComponent<GameObjectInitSystemStateComponent>(GetEntityQuery(typeof(GameObjectInitSystemStateComponent)));

            Linker.UnlinkAllGameObjects();

            foreach (var entityId in entitySystem.GetEntitiesInView())
            {
                gameObjectCreator.OnEntityRemoved(entityId);
            }

            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
            Entities.With(newEntitiesQuery).ForEach((Entity entity, ref SpatialEntityId spatialEntityId, ref Metadata.Component metadata) =>
            {
                var spatialOsEntity = new SpatialOSEntity(entity, EntityManager);

                if (!entityTypeRegistrations.TryGetValue(metadata.EntityType, out var entityTypeRegistration))
                {
                    gameObjectCreator.OnEntityCreated(spatialOsEntity, Linker);
                    PostUpdateCommands.AddComponent(entity, new GameObjectInitSystemStateComponent
                    {
                        EntityId = spatialEntityId.EntityId
                    });
                    return;
                }

                foreach (var componentType in entityTypeRegistration.ComponentTypes)
                {
                    if (!EntityManager.HasComponent(entity, componentType))
                    {
                        return;
                    }
                }

                entityTypeRegistration.CreateGameObject(spatialOsEntity, Linker);
                PostUpdateCommands.AddComponent(entity, new GameObjectInitSystemStateComponent
                {
                    EntityId = spatialEntityId.EntityId
                });
            });

            Entities.With(removedEntitiesQuery).ForEach((ref GameObjectInitSystemStateComponent state) =>
            {
                Linker.UnlinkAllGameObjectsFromEntityId(state.EntityId);
                gameObjectCreator.OnEntityRemoved(state.EntityId);
            });

            Linker.FlushCommandBuffer();

            EntityManager.RemoveComponent<GameObjectInitSystemStateComponent>(removedEntitiesQuery);
        }
    }
}
