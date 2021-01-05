using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Generated;
using Unity.Entities;

namespace Playground.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    public class AssignEntitiesSystem : ComponentSystem
    {
        private EntityQuery unassignedEntities;
        private ClientPartitionsSystem clientPartitionsSystem;

        protected override void OnCreate()
        {
            clientPartitionsSystem = World.GetOrCreateSystem<ClientPartitionsSystem>();
            unassignedEntities = GetEntityQuery(
                ComponentType.ReadOnly<Metadata.Component>(),
                ComponentType.ReadWrite<AuthorityDelegation.Component>(),
                ComponentType.Exclude<LoadBalancedEntity>());
        }

        protected override void OnUpdate()
        {
            var owningWorkerData = GetComponentDataFromEntity<OwningWorker.Component>();
            Entities.With(unassignedEntities).ForEach(
                (Entity entity, ref Metadata.Component metadata, ref AuthorityDelegation.Component authorityDelegation) =>
                {
                    if (metadata.EntityType == "Character")
                    {
                        // Server component.
                        authorityDelegation.Delegations[ComponentSets.PlayerServerSet.ComponentSetId] = 1;

                        if (!owningWorkerData.HasComponent(entity))
                        {
                            return;
                        }

                        var owningWorkerEntityId = owningWorkerData[entity].WorkerEntityId;

                        if (!clientPartitionsSystem.TryGetPartitionEntityId(owningWorkerEntityId, out var partitionEntityId))
                        {
                            return;
                        }

                        authorityDelegation.Delegations[ComponentSets.PlayerClientSet.ComponentSetId] = partitionEntityId.Id;
                    }
                    else
                    {
                        authorityDelegation.Delegations[ComponentSets.DefaultServerSet.ComponentSetId] = 1;
                    }

                    authorityDelegation.Delegations = authorityDelegation.Delegations;

                    PostUpdateCommands.AddComponent<LoadBalancedEntity>(entity);
                });
        }

        private struct LoadBalancedEntity : IComponentData
        {
        }
    }
}
