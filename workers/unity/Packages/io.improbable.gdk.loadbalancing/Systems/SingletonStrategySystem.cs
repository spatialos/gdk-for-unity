using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    internal class SingletonStrategySystem : ComponentSystem
    {
        internal EntityLoadBalancingMap LoadBalancingMap;
        internal EntityId TargetPartition;

        private EntityQuery unassignedEntities;

        protected override void OnCreate()
        {
            unassignedEntities = GetEntityQuery(
                ComponentType.ReadOnly<Metadata.Component>(),
                ComponentType.ReadWrite<AuthorityDelegation.Component>(),
                ComponentType.Exclude<LoadBalancedEntity>());
        }

        protected override void OnUpdate()
        {
            Entities.With(unassignedEntities).ForEach(
                (Entity entity, ref Metadata.Component metadata, ref AuthorityDelegation.Component authorityDelegation) =>
                {
                    authorityDelegation.Delegations[LoadBalancingMap.Resolve(metadata.EntityType).ComponentSetId] = TargetPartition.Id;
                    authorityDelegation.Delegations = authorityDelegation.Delegations;

                    PostUpdateCommands.AddComponent<LoadBalancedEntity>(entity);
                });
        }

        private struct LoadBalancedEntity : IComponentData
        {
        }
    }
}
