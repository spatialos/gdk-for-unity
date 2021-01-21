#if GDK_LOAD_BALANCING
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Unity.Entities;

namespace Improbable.Gdk.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    internal class ClientLoadBalancingSystem : ComponentSystem
    {
        public string PlayerEntityType { get; internal set; }
        public ComponentSet ClientComponentSet { get; internal set; }

        private WorkerSystem workerSystem;
        private EntityQuery unassignedEntities;

        protected override void OnCreate()
        {
            workerSystem = World.GetExistingSystem<WorkerSystem>();

            unassignedEntities = GetEntityQuery(
                ComponentType.ReadOnly<Metadata.Component>(),
                ComponentType.ReadOnly<OwningWorker.Component>(),
                ComponentType.ReadWrite<AuthorityDelegation.Component>(),
                ComponentType.Exclude<LoadBalancedClient>());
        }

        protected override void OnUpdate()
        {
            var clientWorkerData = GetComponentDataFromEntity<RegisteredWorker>(isReadOnly: true);

            Entities.With(unassignedEntities).ForEach(
                (Entity entity, ref Metadata.Component metadata, ref OwningWorker.Component owningWorker, ref AuthorityDelegation.Component authorityDelegation) =>
                {
                    if (metadata.EntityType != PlayerEntityType)
                    {
                        return;
                    }

                    if (!workerSystem.TryGetEntity(owningWorker.WorkerEntityId, out var workerEntity))
                    {
                        return;
                    }

                    if (!clientWorkerData.HasComponent(workerEntity))
                    {
                        return;
                    }

                    authorityDelegation.Delegations[ClientComponentSet.ComponentSetId] = clientWorkerData[workerEntity].PartitionEntityId.Id;
                    authorityDelegation.Delegations = authorityDelegation.Delegations;

                    PostUpdateCommands.AddComponent<LoadBalancedClient>(entity);
                });
        }

        private struct LoadBalancedClient : IComponentData
        {
        }
    }

    public static class LoadBalancingConfigurationExtensions
    {
        public static void AddClientLoadBalancing(this LoadBalancerConfiguration configuration, string clientEntityType, ComponentSet clientComponentSet)
        {
            var clientLbSystem = configuration.World.GetOrCreateSystem<ClientLoadBalancingSystem>();
            clientLbSystem.PlayerEntityType = clientEntityType;
            clientLbSystem.ClientComponentSet = clientComponentSet;
        }
    }
}
#endif
