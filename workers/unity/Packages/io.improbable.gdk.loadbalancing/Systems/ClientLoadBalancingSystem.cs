#if GDK_PLAYER_LIFECYCLE
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Unity.Entities;

namespace Improbable.Gdk.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
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
                ComponentType.ReadWrite<AuthorityDelegation.Component>(),
                ComponentType.Exclude<LoadBalancedClient>());
        }

        protected override void OnUpdate()
        {
            var owningWorkerData = GetComponentDataFromEntity<OwningWorker.Component>(isReadOnly: true);
            var clientWorkerData = GetComponentDataFromEntity<RegisteredWorker>(isReadOnly: true);

            Entities.With(unassignedEntities).ForEach(
                (Entity entity, ref Metadata.Component metadata, ref AuthorityDelegation.Component authorityDelegation) =>
                {
                    if (metadata.EntityType != PlayerEntityType)
                    {
                        return;
                    }

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

                    authorityDelegation.Delegations[ClientComponentSet.ComponentSetId] = clientWorkerData[workerEntity].PartitionEntityId.Id;
                    authorityDelegation.Delegations = authorityDelegation.Delegations;

                    PostUpdateCommands.AddComponent<LoadBalancedClient>(entity);
                });
        }

        private struct LoadBalancedClient : IComponentData
        {
        }
    }
}
#endif
