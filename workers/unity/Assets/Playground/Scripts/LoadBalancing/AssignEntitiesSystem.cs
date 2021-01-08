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
        private WorkerSystem workerSystem;

        protected override void OnCreate()
        {
            workerSystem = World.GetExistingSystem<WorkerSystem>();

            unassignedEntities = GetEntityQuery(
                ComponentType.ReadOnly<Metadata.Component>(),
                ComponentType.ReadWrite<AuthorityDelegation.Component>(),
                ComponentType.Exclude<LoadBalancedEntity>());
        }

        protected override void OnUpdate()
        {
            var owningWorkerData = GetComponentDataFromEntity<OwningWorker.Component>(isReadOnly: true);
            var clientWorkerData = GetComponentDataFromEntity<RegisteredClientWorker>(isReadOnly: true);

            Entities.With(unassignedEntities).ForEach(
                (Entity entity, ref Metadata.Component metadata, ref AuthorityDelegation.Component authorityDelegation) =>
                {
                    if (metadata.EntityType == "Character")
                    {
                        authorityDelegation.Delegations[ComponentSets.PlayerServerSet.ComponentSetId] = 1;

                        if (!owningWorkerData.HasComponent(entity))
                        {
                            return;
                        }

                        var owningWorkerEntityId = owningWorkerData[entity].WorkerEntityId;

                        if (!workerSystem.TryGetEntity(owningWorkerEntityId, out var workerEntity))
                        {
                            return;
                        }

                        if (!clientWorkerData.HasComponent(workerEntity))
                        {
                            return;
                        }

                        authorityDelegation.Delegations[ComponentSets.PlayerClientSet.ComponentSetId] = clientWorkerData[workerEntity].PartitionEntityId.Id;
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
