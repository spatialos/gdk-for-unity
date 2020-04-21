using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.Subscriptions;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Playground
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
    [UpdateAfter(typeof(RequireLifecycleGroup))]
    public class ConversionSystemFromMetadata : ComponentSystem
    {
        private EntityQuery newEntitiesQuery;
        private EntityQuery removedEntitiesQuery;

        private readonly Dictionary<string, Entity> cachedPrefabs
            = new Dictionary<string, Entity>();

        private string workerType;

        private struct SystemStateComponent : ISystemStateComponentData
        {
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            workerType = World.GetExistingSystem<WorkerSystem>().WorkerType;

            var minimumComponentSet = new[]
            {
                ComponentType.ReadOnly<SpatialEntityId>(), ComponentType.ReadOnly<Metadata.Component>()
            };

            newEntitiesQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = minimumComponentSet,
                None = new[] { ComponentType.ReadOnly<SystemStateComponent>() }
            });

            removedEntitiesQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new[] { ComponentType.ReadOnly<SystemStateComponent>() },
                None = minimumComponentSet
            });
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
            Entities.With(removedEntitiesQuery).ForEach((ref SystemStateComponent state) =>
            {
                // TODO
            });

            EntityManager.RemoveComponent<GameObjectInitSystemStateComponent>(removedEntitiesQuery);
        }

        private void ProcessNewEntities()
        {
            Entities.With(newEntitiesQuery).ForEach(
                (Entity entity, ref SpatialEntityId spatialEntityId, ref Metadata.Component metadata) =>
                {
                    var entityType = metadata.EntityType;

#if UNITY_EDITOR
                    EntityManager.SetName(entity, $"{entityType} (SpatialOS: {spatialEntityId.EntityId.Id.ToString()})");
#endif

                    // Load source prefab
                    // TODO Replace with pre-conversion lookup!
                    if (!cachedPrefabs.TryGetValue(entityType, out var prefabEntity))
                    {
                        var workerSpecificPath = Path.Combine("Prefabs", workerType, entityType);
                        var commonPath = Path.Combine("Prefabs", "Common", entityType);

                        // Attempt to load GameObject Prefab
                        var prefab = Resources.Load<GameObject>(workerSpecificPath) ?? Resources.Load<GameObject>(commonPath);
                        if (prefab == null)
                        {
                            cachedPrefabs[entityType] = Entity.Null;
                            return;
                        }

                        // Convert GameObject to Entity Prefab
                        prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab,
                            GameObjectConversionSettings.FromWorld(World, null));

                        // Cache result
                        cachedPrefabs[entityType] = prefabEntity;
                    }

                    if (prefabEntity == Entity.Null)
                    {
                        return;
                    }

                    // Spawn entity
                    var rootEntity = EntityManager.Instantiate(prefabEntity);

                    // Create hybrid archetype
                    var rootTypes = EntityManager.GetComponentTypes(rootEntity);
                    var targetTypes = EntityManager.GetComponentTypes(entity);
                    var hybridTypes = targetTypes.Concat(rootTypes).Distinct().ToArray();
                    var hybridArchetype = EntityManager.CreateArchetype(hybridTypes);

                    // Convert and Copy
                    // TODO Only copy over new data
                    EntityManager.SetArchetype(entity, hybridArchetype);
                    var copyKit = new EntityDataCopyKit(EntityManager);
                    foreach (var componentType in rootTypes)
                    {
                        copyKit.CopyData(rootEntity, entity, componentType);
                    }

                    // Patch Children
                    if (EntityManager.HasComponent<LinkedEntityGroup>(entity))
                    {
                        var groupData = EntityManager.GetBuffer<LinkedEntityGroup>(entity);
                        foreach (var entityGroup in groupData)
                        {
                            if (EntityManager.HasComponent<LinkedEntityGroup>(entityGroup.Value))
                            {
                                var linkedEntityGroups = EntityManager.GetBuffer<LinkedEntityGroup>(entityGroup.Value);
                                for (var i = 0; i < linkedEntityGroups.Length; i++)
                                {
                                    var parent = linkedEntityGroups[i];
                                    if (parent.Value == rootEntity)
                                    {
                                        linkedEntityGroups[i] = entity;
                                    }
                                }
                            }

                            if (EntityManager.HasComponent<Parent>(entityGroup.Value))
                            {
                                EntityManager.SetComponentData(entityGroup.Value, new Parent { Value = entity });
                            }
                        }

                        // Patch Root
                        if (groupData.Length > 0)
                        {
                            groupData[0] = entity;
                        }
                    }

                    // Cleanup Root
                    EntityManager.GetBuffer<LinkedEntityGroup>(rootEntity).Clear();
                    EntityManager.DestroyEntity(rootEntity);
                });

            EntityManager.AddComponent<SystemStateComponent>(newEntitiesQuery);
        }
    }
}
