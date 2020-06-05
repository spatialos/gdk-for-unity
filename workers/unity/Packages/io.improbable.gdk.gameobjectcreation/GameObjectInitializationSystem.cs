using System;
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

        private readonly EntityTypeExpectations entityTypeExpectations = new EntityTypeExpectations();

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator, GameObject workerGameObject)
        {
            this.gameObjectCreator = gameObjectCreator;
            this.workerGameObject = workerGameObject;
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

            var minimumComponentSet = new[]
            {
                ComponentType.ReadOnly<SpatialEntityId>(), ComponentType.ReadOnly<Metadata.Component>()
            };

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

            gameObjectCreator.PopulateEntityTypeExpectations(entityTypeExpectations);
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
            if (!newEntitiesQuery.IsEmptyIgnoreFilter)
            {
                ProcessNewEntities();
            }

            if (!removedEntitiesQuery.IsEmptyIgnoreFilter)
            {
                ProcessRemovedEntities();
            }
        }

        private void ProcessRemovedEntities()
        {
            Entities.With(removedEntitiesQuery).ForEach((ref GameObjectInitSystemStateComponent state) =>
            {
                Linker.UnlinkAllGameObjectsFromEntityId(state.EntityId);

                try
                {
                    gameObjectCreator.OnEntityRemoved(state.EntityId);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            });

            EntityManager.RemoveComponent<GameObjectInitSystemStateComponent>(removedEntitiesQuery);
        }

        private void ProcessNewEntities()
        {
            Entities.With(newEntitiesQuery).ForEach(
                (Entity entity, ref SpatialEntityId spatialEntityId, ref Metadata.Component metadata) =>
                {
                    var entityType = metadata.EntityType;
                    var expectedTypes = entityTypeExpectations.GetExpectedTypes(entityType);

                    foreach (var expectedType in expectedTypes)
                    {
                        if (!EntityManager.HasComponent(entity, expectedType))
                        {
                            return;
                        }
                    }

#if UNITY_EDITOR
                    EntityManager.SetName(entity, $"{entityType} (SpatialOS: {spatialEntityId.EntityId.Id.ToString()})");
#endif

                    try
                    {
                        gameObjectCreator.OnEntityCreated(entityType, new SpatialOSEntity(entity, EntityManager), Linker);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }

                    PostUpdateCommands.AddComponent(entity, new GameObjectInitSystemStateComponent
                    {
                        EntityId = spatialEntityId.EntityId
                    });
                });
        }
    }
}
