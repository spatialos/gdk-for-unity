using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Restricted;
using Improbable.TestSchema;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;
using Worker = Improbable.Restricted.Worker;

namespace Playground.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    public class ClientPartitionsSystem : ComponentSystem
    {
        private EntityQuery newClients;
        private CommandSystem commandSystem;

        private readonly Dictionary<CommandRequestId, PendingPartitionCreationContext> pendingPartitionCreationContexts =
            new Dictionary<CommandRequestId, PendingPartitionCreationContext>();

        private readonly Dictionary<CommandRequestId, PendingAssignPartitionContext> pendingAssignPartitionContexts =
            new Dictionary<CommandRequestId, PendingAssignPartitionContext>();

        private readonly Dictionary<EntityId, EntityId> clientPartitionMap = new Dictionary<EntityId, EntityId>();

        public bool TryGetPartitionEntityId(EntityId clientWorkerId, out EntityId partitionEntityId)
        {
            return clientPartitionMap.TryGetValue(clientWorkerId, out partitionEntityId);
        }

        protected override void OnCreate()
        {
            commandSystem = World.GetExistingSystem<CommandSystem>();
            newClients = GetEntityQuery(
                ComponentType.ReadOnly<Improbable.Restricted.Worker.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.Exclude<SeenWorker>());
        }

        protected override void OnUpdate()
        {
            CreateNewPartitions();
            AssignPartitions();
            StorePartitionInfo();
            // TODO: CleanupOldPartitions();
        }

        private void CreateNewPartitions()
        {
            Entities.With(newClients).ForEach((Entity entity, ref SpatialEntityId spatialEntityId, ref Improbable.Restricted.Worker.Component worker) =>
            {
                if (worker.WorkerType == "UnityClient")
                {
                    var requestId = commandSystem.SendCommand(new WorldCommands.CreateEntity.Request(GetPartitionEntity()));
                    var context = new PendingPartitionCreationContext { ClientWorkerEntityId = spatialEntityId.EntityId, ClientEcsEntity = entity };
                    pendingPartitionCreationContexts[requestId] = context;
                }

                PostUpdateCommands.AddComponent<SeenWorker>(entity);
            });
        }

        private void AssignPartitions()
        {
            var responses = commandSystem.GetResponses<WorldCommands.CreateEntity.ReceivedResponse>();

            for (var i = 0; i < responses.Count; i++)
            {
                var response = responses[i];
                if (!pendingPartitionCreationContexts.TryGetValue(response.RequestId, out var context))
                {
                    continue;
                }

                pendingPartitionCreationContexts.Remove(response.RequestId);

                if (response.StatusCode != StatusCode.Success)
                {
                    PostUpdateCommands.RemoveComponent<SeenWorker>(context.ClientEcsEntity);
                    continue;
                }

                var nextContext = new PendingAssignPartitionContext
                {
                    ClientWorkerEntityId = context.ClientWorkerEntityId,
                    PartitionEntityId = response.EntityId.Value
                };

                AssignPartition(nextContext);
            }
        }

        private void AssignPartition(PendingAssignPartitionContext context)
        {
            var requestId = commandSystem.SendCommand(new Worker.AssignPartition.Request(
                context.ClientWorkerEntityId, new AssignPartitionRequest(context.PartitionEntityId.Id)));
            pendingAssignPartitionContexts[requestId] = context;
        }

        private void StorePartitionInfo()
        {
            var responses = commandSystem.GetResponses<Worker.AssignPartition.ReceivedResponse>();

            for (var i = 0; i < responses.Count; i++)
            {
                var response = responses[i];
                if (!pendingAssignPartitionContexts.TryGetValue(response.RequestId, out var context))
                {
                    continue;
                }

                pendingAssignPartitionContexts.Remove(response.RequestId);

                if (response.StatusCode != StatusCode.Success)
                {
                    AssignPartition(context);
                    continue;
                }

                clientPartitionMap[context.ClientWorkerEntityId] = context.PartitionEntityId;
            }
        }

        private EntityTemplate GetPartitionEntity()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new Metadata.Snapshot("ClientPartition"));
            return template;
        }

        private struct PendingPartitionCreationContext
        {
            public EntityId ClientWorkerEntityId;
            public Entity ClientEcsEntity;
        }

        private struct PendingAssignPartitionContext
        {
            public EntityId ClientWorkerEntityId;
            public EntityId PartitionEntityId;
        }

        private struct SeenWorker : IComponentData
        {
        }
    }
}
