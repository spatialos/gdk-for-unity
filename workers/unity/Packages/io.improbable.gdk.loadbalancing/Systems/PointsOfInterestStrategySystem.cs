using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    internal class PointsOfInterestStrategySystem : ComponentSystem
    {
        private readonly EntityLoadBalancingMap entityLoadBalancingMap;
        private readonly string workerType;
        private readonly List<PointOfInterest> pointsOfInterest;

        private ILogDispatcher logDispatcher;

        private EntityQuery entities;
        private EntityQuery workers;

        private bool didWorkersChange;

        public PointsOfInterestStrategySystem(string workerType, IEnumerable<Coordinates> pointsOfInterest, EntityLoadBalancingMap entityLoadBalancingMap)
        {
            this.entityLoadBalancingMap = entityLoadBalancingMap;
            this.workerType = workerType;
            this.pointsOfInterest = pointsOfInterest.Select(coord => new PointOfInterest(coord)).ToList();
        }

        protected override void OnCreate()
        {
            logDispatcher = World.GetExistingSystem<WorkerSystem>().LogDispatcher;

            entities = GetEntityQuery(ComponentType.ReadOnly<Position.Component>(),
                ComponentType.ReadOnly<Metadata.Component>(),
                ComponentType.ReadWrite<AuthorityDelegation.Component>());

            workers = GetEntityQuery(ComponentType.ReadOnly<RegisteredWorker>(),
                ComponentType.ReadOnly<WorkerClassification>());
            workers.SetSharedComponentFilter(new WorkerClassification(workerType));
        }

        protected override void OnUpdate()
        {
            UpdatePartitionAssignments();
            UpdateEntityAssignments();
        }

        private void UpdatePartitionAssignments()
        {
            var workerRegistrations = workers.ToComponentDataArray<RegisteredWorker>(Allocator.Temp);
            var diff = pointsOfInterest.Count - workerRegistrations.Length;

            // Warns if there aren't enough workers for the given POI.
            if (diff > 0)
            {
                logDispatcher.HandleLog(LogType.Warning, new LogEvent("There are not enough workers to satisfy the current points of interest configuration."));
            }
            else if (diff < 0)
            {
                logDispatcher.HandleLog(LogType.Warning, new LogEvent("There are more workers than required to satisfy the current point of interest configuration."));
            }

            int idx = 0;

            foreach (var pointOfInterest in pointsOfInterest)
            {
                // Any assigned POI with workers that no longer exist get removed.
                if (pointOfInterest.OwningPartition.HasValue && !workerRegistrations.Contains(pointOfInterest.OwningPartition.Value))
                {
                    didWorkersChange = true;
                    pointOfInterest.OwningPartition = null;
                }

                if (pointOfInterest.OwningPartition != null)
                {
                    continue;
                }

                // Any unassigned POI gets assigned to workers.
                while (idx < workerRegistrations.Length)
                {
                    if (pointsOfInterest.All(poi => poi.OwningPartition != workerRegistrations[idx]))
                    {
                        didWorkersChange = true;
                        pointOfInterest.OwningPartition = workerRegistrations[idx];
                    }

                    idx++;
                }
            }
        }

        private void UpdateEntityAssignments()
        {
            if (didWorkersChange)
            {
                entities.ResetFilter();
            }
            else if (!entities.HasFilter())
            {
                entities.AddChangedVersionFilter(ComponentType.ReadOnly<Position.Component>());
            }

            Entities.With(entities).ForEach((ref AuthorityDelegation.Component authorityDelegation, ref Position.Component position, ref Metadata.Component component) =>
            {
                var closestDist = double.MaxValue;
                RegisteredWorker? closestWorker = null;

                foreach (var poi in pointsOfInterest)
                {
                    if (!poi.OwningPartition.HasValue)
                    {
                        continue;
                    }

                    var dist = Dist(position.Coords, poi.Point);

                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestWorker = poi.OwningPartition.Value;
                    }
                }

                if (closestWorker.HasValue)
                {
                    authorityDelegation.Delegations[entityLoadBalancingMap.Resolve(component.EntityType).ComponentSetId]
                        = closestWorker.Value.PartitionEntityId.Id;
                    authorityDelegation.Delegations = authorityDelegation.Delegations;
                }
            });
        }


        private class PointOfInterest
        {
            public readonly Coordinates Point;
            public RegisteredWorker? OwningPartition;

            public PointOfInterest(Coordinates point)
            {
                Point = point;
                OwningPartition = null;
            }
        }

        private static double Dist(Coordinates first, Coordinates second)
        {
            return Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2) + Math.Pow(first.Z - second.Z, 2);
        }
    }
}
