using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class EntitySystem : SystemBase
    {
        private struct EntityCollection : IDisposable
        {
            public NativeArray<EntityId> EntityIds { get; private set; }
            public int EntityCount { get; private set; }
            public JobHandle JobHandle { get; set; }

            public EntityCollection(int size)
            {
                EntityIds = new NativeArray<EntityId>(size, Allocator.Persistent);
                EntityCount = 0;
                JobHandle = default;
            }

            public NativeSlice<EntityId> Slice
            {
                get
                {
                    JobHandle.Complete();
                    return new NativeSlice<EntityId>(EntityIds, 0, EntityCount);
                }
            }

            public void EnsureSize(ref EntityQuery query)
            {
                EntityCount = query.CalculateEntityCount();
                if (EntityIds.Length >= EntityCount)
                {
                    return;
                }

                EntityIds.Dispose();
                EntityIds = new NativeArray<EntityId>(EntityCount << 1, Allocator.Persistent);
            }

            public void Dispose()
            {
                if (EntityIds.IsCreated)
                {
                    EntityIds.Dispose();
                }
            }
        }

        private EndSimulationEntityCommandBufferSystem bufferSystem;
        private EntityCollection added;
        private EntityQuery addedQuery; // This query needs to be a member of SystemBase
        private EntityCollection removed;
        private EntityQuery removedQuery; // This query needs to be a member of SystemBase
        public int ViewVersion { get; private set; }
        public NativeSlice<EntityId> EntitiesRemoved => removed.Slice;
        public NativeSlice<EntityId> EntitiesAdded => added.Slice;

        protected override void OnCreate()
        {
            base.OnCreate();
            bufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            removed = new EntityCollection(1);
            added = new EntityCollection(1);
        }

        protected override void OnUpdate()
        {
            UpdateEntitiesAdded();
            UpdateEntitiesRemoved();

            Dependency = JobHandle.CombineDependencies(added.JobHandle, removed.JobHandle);
            bufferSystem.AddJobHandleForProducer(Dependency);

            if (added.EntityCount != 0 || removed.EntityCount != 0)
            {
                ViewVersion += 1;
            }
        }

        private void UpdateEntitiesAdded()
        {
            added.EnsureSize(ref addedQuery);
            var buffer = bufferSystem.CreateCommandBuffer().AsParallelWriter();
            var array = added.EntityIds;
            added.JobHandle = Entities.WithName("EntitiesAdded")
                .WithNone<EntitySystemStateComponent>()
                .WithAll<SpatialEntityId>()
                .WithStoreEntityQueryInField(ref addedQuery)
                .ForEach((int entityInQueryIndex, in Entity entity, in SpatialEntityId entityId) =>
                {
                    buffer.AddComponent(entityInQueryIndex, entity, (EntitySystemStateComponent) entityId);
                    array[entityInQueryIndex] = entityId.EntityId;
                }).ScheduleParallel(Dependency);
        }

        private void UpdateEntitiesRemoved()
        {
            removed.EnsureSize(ref removedQuery);
            var buffer = bufferSystem.CreateCommandBuffer().AsParallelWriter();
            var array = removed.EntityIds;
            removed.JobHandle = Entities.WithName("EntitiesRemoved")
                .WithAll<EntitySystemStateComponent>()
                .WithNone<SpatialEntityId>()
                .WithStoreEntityQueryInField(ref removedQuery)
                .ForEach((int entityInQueryIndex, in Entity entity, in EntitySystemStateComponent entityId) =>
                {
                    buffer.RemoveComponent<EntitySystemStateComponent>(entityInQueryIndex, entity);
                    array[entityInQueryIndex] = entityId.EntityId;
                }).ScheduleParallel(Dependency);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            removed.Dispose();
            added.Dispose();
        }
    }
}
