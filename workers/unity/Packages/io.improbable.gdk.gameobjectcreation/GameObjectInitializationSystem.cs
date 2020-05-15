using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

        private readonly Dictionary<string, (IEntityRepresentationResolver representation, ComponentType[] expectedTypes)> entityTypeExpectations
            = new Dictionary<string, (IEntityRepresentationResolver representation, ComponentType[] expectedTypes)>();

        public GameObjectInitializationSystem(IEntityGameObjectCreator gameObjectCreator, EntityRepresentationMapping entityRepresentationMapping, GameObject workerGameObject)
        {
            if (gameObjectCreator == null)
            {
                throw new ArgumentException("gameObjectCreator can not be Null", nameof(gameObjectCreator));
            }

            if (entityRepresentationMapping == null)
            {
                throw new ArgumentException("entityLinkerDatabase can not be Null", nameof(entityRepresentationMapping));
            }

            this.gameObjectCreator = gameObjectCreator;
            this.workerGameObject = workerGameObject;

            var additionalTypeExpectations = new EntityTypeExpectations();
            gameObjectCreator.PopulateEntityTypeExpectations(additionalTypeExpectations);

            foreach (var entityRepresentation in entityRepresentationMapping.EntityRepresentationResolvers)
            {
                var entityType = entityRepresentation.EntityType;
                var additionalTypes = additionalTypeExpectations.GetExpectedTypes(entityType);
                var componentTypes = entityRepresentation.RequiredComponents
                    .Select(componentId =>
                    {
                        var managedType = ComponentDatabase.GetMetaclass(componentId).Data;
                        return ComponentType.ReadOnly(managedType);
                    })
                    .Concat(additionalTypes)
                    .Distinct()
                    .ToArray();

                entityTypeExpectations.Add(entityType, (entityRepresentation, componentTypes));
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
        }

        protected override void OnDestroy()
        {
            EntityManager.RemoveComponent<GameObjectInitSystemStateComponent>(GetEntityQuery(typeof(GameObjectInitSystemStateComponent)));

            Linker.UnlinkAllGameObjects();

            Entities.ForEach((ref SpatialEntityId entityId) =>
            {
                gameObjectCreator.OnEntityRemoved(entityId.EntityId);
            });

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
                    var registered = entityTypeExpectations.TryGetValue(entityType, out var tuple);
                    var (representation, expectedTypes) = tuple;

                    if (!registered)
                    {
                        SetEntityName(entity, $"{entityType} (SpatialOS: {spatialEntityId.EntityId.Id.ToString()})");
                        PostUpdateCommands.AddComponent(entity, new GameObjectInitSystemStateComponent
                        {
                            EntityId = spatialEntityId.EntityId
                        });
                        return;
                    }

                    foreach (var expectedType in expectedTypes)
                    {
                        if (!EntityManager.HasComponent(entity, expectedType))
                        {
                            return;
                        }
                    }

                    SetEntityName(entity, $"{entityType} (SpatialOS: {spatialEntityId.EntityId.Id.ToString()})");

                    try
                    {
                        var entityInfo = new SpatialOSEntityInfo(entityType, entity, spatialEntityId.EntityId);
                        var prefab = representation.Resolve(entityInfo, EntityManager);
                        gameObjectCreator.OnEntityCreated(entityInfo, prefab, EntityManager, Linker);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

                    PostUpdateCommands.AddComponent(entity, new GameObjectInitSystemStateComponent
                    {
                        EntityId = spatialEntityId.EntityId
                    });
                });
        }

        [Conditional("UNITY_EDITOR")]
        private void SetEntityName(Entity entity, string name)
        {
#if UNITY_EDITOR
            EntityManager.SetName(entity, name);
#endif
        }
    }
}
