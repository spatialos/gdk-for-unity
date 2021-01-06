using System;
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
        private EntityQuery removedClients;
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

            removedClients = GetEntityQuery(
                ComponentType.ReadOnly<RegisteredWorker>(),
                ComponentType.Exclude<Improbable.Restricted.Worker.Component>());
        }

        protected override void OnUpdate()
        {
            CreateNewPartitions();
            AssignPartitions();
            StorePartitionInfo();
            CleanupOldPartitions();
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
                    PartitionEntityId = response.EntityId.Value,
                    ClientEcsEntity = context.ClientEcsEntity
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

                switch (response.StatusCode)
                {
                    case StatusCode.Success:
                        break;
                    case StatusCode.Timeout:
                    case StatusCode.AuthorityLost:
                    case StatusCode.PermissionDenied:
                    case StatusCode.ApplicationError:
                    case StatusCode.InternalError:
                        // Retry in these failure modes.
                        AssignPartition(context);
                        continue;
                    case StatusCode.NotFound:
                        // In this case, the client entity no longer exists. We need to delete the partition
                        commandSystem.SendCommand(new WorldCommands.DeleteEntity.Request(context.PartitionEntityId));
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                clientPartitionMap[context.ClientWorkerEntityId] = context.PartitionEntityId;

                if (EntityManager.Exists(context.ClientEcsEntity))
                {
                    PostUpdateCommands.AddComponent(context.ClientEcsEntity, new RegisteredWorker
                    {
                        PartitionEntityId = context.PartitionEntityId
                    });
                }
            }
        }

        private void CleanupOldPartitions()
        {
            // If the client has disconnected, we need to remove that partition.
            Entities.With(removedClients).ForEach((Entity entity, ref RegisteredWorker registerWorker) =>
            {
                commandSystem.SendCommand(new WorldCommands.DeleteEntity.Request(registerWorker.PartitionEntityId));
                PostUpdateCommands.RemoveComponent<RegisteredWorker>(entity);
                PostUpdateCommands.DestroyEntity(entity);
            });
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
            public Entity ClientEcsEntity;
        }

        private struct SeenWorker : IComponentData
        {
        }

        private struct RegisteredWorker : ISystemStateComponentData
        {
            public EntityId PartitionEntityId;
        }
    }
}
