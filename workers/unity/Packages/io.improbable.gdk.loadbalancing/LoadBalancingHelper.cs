using System;
using System.Collections.Generic;
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
        private readonly World world;

        internal LoadBalancerConfiguration(World world)
        {
            this.world = world;
        }

        public void AddPartitionManagement(params string[] workerTypes)
        {
            var partitionManagementSystem = world.GetOrCreateSystem<PartitionManagementSystem>();
            partitionManagementSystem.WorkerTypes = workerTypes;
        }

#if GDK_PLAYER_LIFECYCLE
        public void AddClientLoadBalancing(string clientEntityType, ComponentSet clientComponentSet)
        {
            var clientLbSystem = world.GetOrCreateSystem<ClientLoadBalancingSystem>();
            clientLbSystem.PlayerEntityType = clientEntityType;
            clientLbSystem.ClientComponentSet = clientComponentSet;
        }
#endif

        public void SetSingletonLoadBalancing(EntityId targetPartition, EntityLoadBalancingMap loadBalancingMap)
        {
            var singletonSystem = world.GetOrCreateSystem<SingletonStrategySystem>();
            singletonSystem.TargetPartition = targetPartition;
            singletonSystem.LoadBalancingMap = loadBalancingMap;
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
