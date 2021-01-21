using System.Linq;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    internal class SingletonStrategySystem : ComponentSystem
    {
        private readonly EntityLoadBalancingMap entityLoadBalancingMap;
        private readonly string workerType;

        private ILogDispatcher logger;
        private EntityQuery unassignedEntities;
        private EntityQuery workers;

        private RegisteredWorker targetWorker;

        public SingletonStrategySystem(string workerType, EntityLoadBalancingMap entityLoadBalancingMap)
        {
            this.entityLoadBalancingMap = entityLoadBalancingMap;
            this.workerType = workerType;
            targetWorker = default;
        }

        protected override void OnCreate()
        {
            logger = World.GetExistingSystem<WorkerSystem>().LogDispatcher;

            unassignedEntities = GetEntityQuery(
                ComponentType.ReadOnly<Metadata.Component>(),
                ComponentType.ReadWrite<AuthorityDelegation.Component>(),
                ComponentType.Exclude<LoadBalancedEntity>());

            workers = GetEntityQuery(ComponentType.ReadOnly<RegisteredWorker>(),
                ComponentType.ReadOnly<WorkerClassification>());
            workers.SetSharedComponentFilter(new WorkerClassification(workerType));
        }

        protected override void OnUpdate()
        {
            FindTargetWorker();
            UpdateEntities();
        }

        private void FindTargetWorker()
        {
            var workerRegistrations = workers.ToComponentDataArray<RegisteredWorker>(Allocator.Temp);

            if (workerRegistrations.Length == 0)
            {
                logger.Warn($"There are no {workerType} workers for satisfying the singleton load balancing strategy.");
                targetWorker = default;
                return;
            }

            if (workerRegistrations.Length > 1)
            {
                logger.Warn($"There is an excess of {workerType} workers for satisfying the singleton load balancing strategy.");
            }

            if (targetWorker.PartitionEntityId.IsValid() && workerRegistrations.Contains(targetWorker))
            {
                // The current target worker is valid and still exists.
                return;
            }

            targetWorker = workerRegistrations[0];
        }

        private void UpdateEntities()
        {
            if (!targetWorker.PartitionEntityId.IsValid())
            {
                return;
            }

            Entities.With(unassignedEntities).ForEach(
                (Entity entity, ref Metadata.Component metadata, ref AuthorityDelegation.Component authorityDelegation) =>
                {
                    authorityDelegation.Delegations[entityLoadBalancingMap.Resolve(metadata.EntityType).ComponentSetId] = targetWorker.PartitionEntityId.Id;
                    authorityDelegation.Delegations = authorityDelegation.Delegations;
                });

            PostUpdateCommands.AddComponent<LoadBalancedEntity>(unassignedEntities);
        }

        private struct LoadBalancedEntity : IComponentData
        {
        }
    }
}
