using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.LoadBalancing
{
    public static class LoadBalancingHelper
    {
        public static void AddLoadBalancingSystems(this WorkerInWorld worker, Action<LoadBalancerConfiguration> configure)
        {
            var configuration = new LoadBalancerConfiguration(worker.World);
            configure(configuration);
        }
    }

    public class LoadBalancerConfiguration
    {
        public readonly World World;

        internal LoadBalancerConfiguration(World world)
        {
            World = world;
            World.GetOrCreateSystem<ClassifyWorkersSystem>();
        }

        public void AddPartitionManagement(string workerType, params string[] workerTypes)
        {
            var partitionManagementSystem = World.GetOrCreateSystem<PartitionManagementSystem>();
            partitionManagementSystem.WorkerTypes = workerTypes.Append(workerType).ToArray();
        }

        public void SetSingletonLoadBalancing(string workerType, EntityLoadBalancingMap loadBalancingMap)
        {
            World.AddSystem(new SingletonStrategySystem(workerType, loadBalancingMap));
        }

        public void SetPointOfInterestLoadBalancing(string workerType, IEnumerable<Coordinates> pointsOfInterest,
            ICollection<uint> componentSetIds)
        {
            World.AddSystem(new PointsOfInterestStrategySystem(workerType, pointsOfInterest, componentSetIds));
        }
    }

    public class EntityLoadBalancingMap
    {
        private readonly ComponentSet defaultComponentSet;
        private readonly Dictionary<string, ComponentSet> componentSetOverrides = new Dictionary<string, ComponentSet>();

        public EntityLoadBalancingMap(ComponentSet defaultComponentSet)
        {
            this.defaultComponentSet = defaultComponentSet;
        }

        public EntityLoadBalancingMap AddOverride(string entityType, ComponentSet componentSet)
        {
            componentSetOverrides[entityType] = componentSet;
            return this;
        }

        internal ComponentSet Resolve(string entityType)
        {
            return componentSetOverrides.TryGetValue(entityType, out var componentSet)
                ? componentSet
                : defaultComponentSet;
        }
    }
}
