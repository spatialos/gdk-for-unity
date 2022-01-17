using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Improbable.Gdk.LoadBalancing
{
    [UpdateInGroup(typeof(LoadBalanceStrategySystemGroup))]
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    internal class PointsOfInterestStrategySystem : SystemBase
    {
        private readonly string workerType;

        private NativeHashSet<uint> componentSetIdWhitelist;
        private NativeArray<PointOfInterest> pointsOfInterest;

        private ILogDispatcher logDispatcher;

        private EntityQuery entitiesQuery;
        private EntityQuery workers;

        private bool forceRecalculation;
        private int knownWorkerCount = 0;

        public PointsOfInterestStrategySystem(string workerType, IEnumerable<Coordinates> pointsOfInterest, ICollection<uint> componentSetIds)
        {
            this.workerType = workerType;
            this.pointsOfInterest = new NativeArray<PointOfInterest>(
                pointsOfInterest.Select(coord => new PointOfInterest(coord)).ToArray(),
                Allocator.Persistent);

            componentSetIdWhitelist = new NativeHashSet<uint>(componentSetIds.Count, Allocator.Persistent);
            foreach (var componentSetId in componentSetIds)
            {
                componentSetIdWhitelist.Add(componentSetId);
            }

        }

        protected override void OnCreate()
        {
            logDispatcher = World.GetExistingSystem<WorkerSystem>().LogDispatcher;

            workers = GetEntityQuery(ComponentType.ReadOnly<RegisteredWorker>(),
                ComponentType.ReadOnly<WorkerClassification>());
            workers.SetSharedComponentFilter(new WorkerClassification(workerType));
        }

        protected override void OnDestroy()
        {
            pointsOfInterest.Dispose();
            componentSetIdWhitelist.Dispose();
        }

        protected override void OnUpdate()
        {
            InitializeNewEntities();
            UpdatePartitionAssignments();
            UpdateEntityAssignments();
        }

        private void InitializeNewEntities()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var recoveredPartitions = new NativeQueue<RecoverPartition>(Allocator.Temp);
            var points = pointsOfInterest.AsReadOnly();

            Entities
                .WithoutBurst()
                .WithReadOnly(points)
                .WithNone<AssignedPartition>()
                .ForEach((Entity entity, in AuthorityDelegation.Component authority, in Position.Component position) =>
            {
                var delegations = authority.Delegations;
                foreach (var delegation in delegations)
                {
                    if (componentSetIdWhitelist.Contains(delegation.Key))
                    {
                        var partitionId = new EntityId(delegation.Value);

                        // Skip invalid partitions
                        if (!partitionId.IsValid())
                        {
                            break;
                        }

                        // Assign to previous partition, even if this does not technically exist
                        ecb.AddComponent(entity, new AssignedPartition(partitionId));

                        // Queue partition recovery
                        var entitypos = position.Coords;
                        var mathpos = new double3(entitypos.X, entitypos.Y, entitypos.Z);
                        var index = GetClosestPOIIndex(mathpos, points);
                        recoveredPartitions.Enqueue(new RecoverPartition(index, partitionId));
                        return;
                    }
                }
                
                // Entity has no existing authority delegation, assign to invalid partition
                ecb.AddComponent(entity, new AssignedPartition());
            }).Run();

            // If a recovered partition isn't overwritten by another partition yet, set it
            while (recoveredPartitions.TryDequeue(out var recovery))
            {
                var pointOfInterest = pointsOfInterest[recovery.Index];
                if (pointOfInterest.OwningPartition.HasValue)
                {
                    continue;
                }

                Debug.Log($"Recovered partition {recovery.Partition.Id} for POI {recovery.Index}");
                pointOfInterest.OwningPartition = new RegisteredWorker
                {
                    PartitionEntityId = recovery.Partition,
                };
                pointsOfInterest[recovery.Index] = pointOfInterest;
            }

            recoveredPartitions.Dispose();

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void UpdatePartitionAssignments()
        {
            // If the amount of workers hasn't changed, no need to update partitions
            if (knownWorkerCount == workers.CalculateEntityCount())
            {
                return;
            }

            var workerRegistrations = workers.ToComponentDataArray<RegisteredWorker>(Allocator.Temp);
            var diff = pointsOfInterest.Length - workerRegistrations.Length;
            knownWorkerCount = workerRegistrations.Length;

            // Warns if there aren't enough workers for the given POI.
            if (diff > 0)
            {
                logDispatcher.HandleLog(LogType.Warning, new LogEvent("There are not enough workers to satisfy the current points of interest configuration."));
                return;
            }

            if (diff < 0)
            {
                logDispatcher.HandleLog(LogType.Warning, new LogEvent("There are more workers than required to satisfy the current point of interest configuration."));
                return;
            }

            logDispatcher.HandleLog(LogType.Log, new LogEvent("The correct amount of workers have been connected to satisfy the current point of interest configuration."));
            forceRecalculation = true;

            int idx = 0;

            for (var i = 0; i < pointsOfInterest.Length; i++)
            {
                var pointOfInterest = pointsOfInterest[i];
                // Any assigned POI with workers that no longer exist get removed.
                if (pointOfInterest.OwningPartition.HasValue &&
                    !workerRegistrations.Contains(pointOfInterest.OwningPartition.Value))
                {
                    pointOfInterest.OwningPartition = null;
                    pointsOfInterest[i] = pointOfInterest;
                }

                if (pointOfInterest.OwningPartition != null)
                {
                    continue;
                }

                // Any unassigned POI gets assigned to workers.
                while (idx < workerRegistrations.Length)
                {
                    var registeredWorker = workerRegistrations[idx];
                    idx++;
                    
                    if (!IsWorkerInPoints(registeredWorker, pointsOfInterest))
                    {
                        pointOfInterest.OwningPartition = registeredWorker;
                        pointsOfInterest[i] = pointOfInterest;
                        break;
                    }
                    
                }
            }
        }

        private static bool IsWorkerInPoints(RegisteredWorker registeredWorker, NativeArray<PointOfInterest> pointsOfInterest)
        {
            foreach (var point in pointsOfInterest)
            {
                if (point.OwningPartition == registeredWorker)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateEntityAssignments()
        {
            if (forceRecalculation)
            {
                entitiesQuery.ResetFilter();
                forceRecalculation = false;
            }
            else if (!entitiesQuery.HasFilter())
            {
                entitiesQuery.AddChangedVersionFilter(ComponentType.ReadOnly<Position.Component>());
            }

            var changedEntities = new NativeQueue<ChangedPartition>(Allocator.Temp);
            var points = pointsOfInterest.AsReadOnly();

            Entities
                .WithReadOnly(points)
                .WithStoreEntityQueryInField(ref entitiesQuery)
                .ForEach((Entity entity, ref AssignedPartition assignedPartition, in Position.Component position) =>
            {
                // Get Partition id of closestWorker, or 0
                var entitypos = position.Coords;
                var mathpos = new double3(entitypos.X, entitypos.Y, entitypos.Z);
                var index = GetClosestPOIIndex(mathpos, points);
                var closestPartitionId = points[index].OwningPartition.GetValueOrDefault().PartitionEntityId;
                if (closestPartitionId != assignedPartition.Partition)
                {
                    assignedPartition.Partition = closestPartitionId;
                    changedEntities.Enqueue(new ChangedPartition(entity, closestPartitionId));
                }
            }).Run();

            if (changedEntities.IsEmpty())
            {
                changedEntities.Dispose();
                return;
            }

            var authorityDelegationData = GetComponentDataFromEntity<AuthorityDelegation.Component>();

            while (changedEntities.TryDequeue(out var changed))
            {
                var (entity, newPartition) = changed;
                var authorityDelegation = authorityDelegationData[entity];

                // TODO Somehow fix this allocation
                var keys = authorityDelegation.Delegations.Keys.ToList();
                foreach (var componentSetId in keys)
                {
                    if (!componentSetIdWhitelist.Contains(componentSetId))
                    {
                        continue;
                    }

                    authorityDelegation.Delegations[componentSetId] = newPartition.Id;
                }

                // Set dirty flag and write back
                authorityDelegation.Delegations = authorityDelegation.Delegations;
                authorityDelegationData[entity] = authorityDelegation;
            }

            changedEntities.Dispose();
        }

        [BurstCompatible]
        private static int GetClosestPOIIndex(double3 position, NativeArray<PointOfInterest>.ReadOnly points)
        {
            var closestDist = double.MaxValue;
            int closestIndex = 0;

            for (var i = 0; i < points.Length; i++)
            {
                var poi = points[i];
                var dist = math.distancesq(position, poi.Point);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }


        private struct PointOfInterest
        {
            public readonly double3 Point;
            public RegisteredWorker? OwningPartition;

            public PointOfInterest(Coordinates point)
            {
                Point = new double3(point.X, point.Y, point.Z);
                OwningPartition = null;
            }
        }

        private readonly struct ChangedPartition
        {
            public readonly Entity Entity;
            public readonly EntityId Partition;

            public ChangedPartition(Entity entity, EntityId partition)
            {
                Entity = entity;
                Partition = partition;
            }

            public void Deconstruct(out Entity entity, out EntityId partition)
            {
                entity = Entity;
                partition = Partition;
            }
        }
        
        private readonly struct RecoverPartition
        {
            public readonly int Index;
            public readonly EntityId Partition;

            public RecoverPartition(int index, EntityId partition)
            {
                Index = index;
                Partition = partition;
            }

            public void Deconstruct(out int index, out EntityId partition)
            {
                index = Index;
                partition = Partition;
            }
        }
    }
}
