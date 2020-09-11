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
            public NativeList<EntityId> EntityIds;
            public int EntityCount { get; private set; }
            public JobHandle JobHandle { get; set; }

            public EntityCollection(int size)
            {
                EntityIds = new NativeList<EntityId>(size, Allocator.Persistent);
                EntityCount = 0;
                JobHandle = default;
            }

            public void PrepareList(in EntityQuery query)
            {
                EntityIds.Clear();
                EntityCount = query.CalculateEntityCount();
                if (EntityIds.Capacity < EntityCount)
                {
                    EntityIds.Capacity = EntityCount; // Capacity will be >= EntityCount
                }
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

        public NativeList<EntityId> EntitiesRemoved
        {
            get
            {
                removed.JobHandle.Complete();
                return removed.EntityIds;
            }
        }

        public NativeList<EntityId> EntitiesAdded
        {
            get
            {
                added.JobHandle.Complete();
                return added.EntityIds;
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            bufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            removed = new EntityCollection(1);
            added = new EntityCollection(1);
        }

        protected override void OnUpdate()
        {
            var addedJob = UpdateEntitiesAdded();
            var removedJob = UpdateEntitiesRemoved();

            Dependency = JobHandle.CombineDependencies(addedJob, removedJob);
            bufferSystem.AddJobHandleForProducer(Dependency);

            if (added.EntityCount != 0 || removed.EntityCount != 0)
            {
                ViewVersion += 1;
            }
        }

        private JobHandle UpdateEntitiesAdded()
        {
            added.PrepareList(addedQuery);
            var buffer = bufferSystem.CreateCommandBuffer().AsParallelWriter();
            var list = added.EntityIds.AsParallelWriter();
            added.JobHandle = Entities.WithName("EntitiesAdded")
                .WithNone<EntitySystemStateComponent>()
                .WithStoreEntityQueryInField(ref addedQuery)
                .ForEach((int entityInQueryIndex, in Entity entity, in SpatialEntityId entityId) =>
                {
                    buffer.AddComponent(entityInQueryIndex, entity, (EntitySystemStateComponent) entityId);
                    list.AddNoResize(entityId.EntityId);
                }).ScheduleParallel(Dependency);
            return added.JobHandle;
        }

        private JobHandle UpdateEntitiesRemoved()
        {
            removed.PrepareList(removedQuery);
            var buffer = bufferSystem.CreateCommandBuffer().AsParallelWriter();
            var list = removed.EntityIds.AsParallelWriter();
            removed.JobHandle = Entities.WithName("EntitiesRemoved")
                .WithNone<SpatialEntityId>()
                .WithStoreEntityQueryInField(ref removedQuery)
                .ForEach((int entityInQueryIndex, in Entity entity, in EntitySystemStateComponent entityId) =>
                {
                    buffer.RemoveComponent<EntitySystemStateComponent>(entityInQueryIndex, entity);
                    list.AddNoResize(entityId.EntityId);
                }).ScheduleParallel(Dependency);
            return removed.JobHandle;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            removed.Dispose();
            added.Dispose();
        }
    }
}
