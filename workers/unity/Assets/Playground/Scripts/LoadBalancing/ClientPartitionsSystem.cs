using System;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Restricted;
using Improbable.Worker.CInterop;
using Unity.Entities;
using Entity = Unity.Entities.Entity;
using Worker = Improbable.Restricted.Worker;

namespace Playground.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    public class ClientPartitionsSystem : ComponentSystem
    {
        private readonly Dictionary<CommandRequestId, PartitionCreationContext> partitionCreationContexts =
            new Dictionary<CommandRequestId, PartitionCreationContext>();

        private readonly Dictionary<CommandRequestId, AssignPartitionContext> assignPartitionContexts =
            new Dictionary<CommandRequestId, AssignPartitionContext>();

        private EntityQuery newClients;
        private EntityQuery removedClients;
        private CommandSystem commandSystem;

        protected override void OnCreate()
        {
            commandSystem = World.GetExistingSystem<CommandSystem>();
            newClients = GetEntityQuery(
                ComponentType.ReadOnly<Improbable.Restricted.Worker.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.Exclude<SeenWorker>());

            removedClients = GetEntityQuery(
                ComponentType.ReadOnly<RegisteredClientWorker>(),
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
                if (worker.WorkerType == "UnityClient" || worker.WorkerType == "MobileClient")
                {
                    var requestId = commandSystem.SendCommand(new WorldCommands.CreateEntity.Request(GetPartitionEntity()));
                    var context = new PartitionCreationContext(spatialEntityId.EntityId, entity);
                    partitionCreationContexts[requestId] = context;
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
                if (!partitionCreationContexts.TryGetValue(response.RequestId, out var context))
                {
                    continue;
                }

                partitionCreationContexts.Remove(response.RequestId);

                if (response.StatusCode != StatusCode.Success)
                {
                    PostUpdateCommands.RemoveComponent<SeenWorker>(context.WorkerEntity);
                    continue;
                }

                AssignPartition(new AssignPartitionContext(context, response.EntityId.Value));
            }
        }

        private void AssignPartition(AssignPartitionContext context)
        {
            var requestId = commandSystem.SendCommand(new Worker.AssignPartition.Request(
                context.WorkerEntityId, new AssignPartitionRequest(context.PartitionEntityId.Id)));
            assignPartitionContexts[requestId] = context;
        }

        private void StorePartitionInfo()
        {
            var responses = commandSystem.GetResponses<Worker.AssignPartition.ReceivedResponse>();

            for (var i = 0; i < responses.Count; i++)
            {
                var response = responses[i];
                if (!assignPartitionContexts.TryGetValue(response.RequestId, out var context))
                {
                    continue;
                }

                assignPartitionContexts.Remove(response.RequestId);

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
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!EntityManager.Exists(context.WorkerEntity))
                {
                    commandSystem.SendCommand(new WorldCommands.DeleteEntity.Request(context.PartitionEntityId));
                    continue;
                }

                PostUpdateCommands.AddComponent(context.WorkerEntity, new RegisteredClientWorker
                {
                    PartitionEntityId = context.PartitionEntityId
                });
            }
        }

        private void CleanupOldPartitions()
        {
            // If the client has disconnected, we need to remove that partition.
            Entities.With(removedClients).ForEach((Entity entity, ref RegisteredClientWorker registeredClientWorker) =>
            {
                commandSystem.SendCommand(new WorldCommands.DeleteEntity.Request(registeredClientWorker.PartitionEntityId));
                PostUpdateCommands.RemoveComponent<RegisteredClientWorker>(entity);
                PostUpdateCommands.DestroyEntity(entity);
            });
        }

        private static EntityTemplate GetPartitionEntity()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new Metadata.Snapshot("ClientPartition"));
            return template;
        }

        private readonly struct PartitionCreationContext
        {
            public readonly EntityId WorkerEntityId;
            public readonly Entity WorkerEntity;

            public PartitionCreationContext(EntityId workerEntityId, Entity workerEntity)
            {
                WorkerEntityId = workerEntityId;
                WorkerEntity = workerEntity;
            }
        }

        private readonly struct AssignPartitionContext
        {
            public readonly EntityId WorkerEntityId;
            public readonly Entity WorkerEntity;
            public readonly EntityId PartitionEntityId;

            public AssignPartitionContext(PartitionCreationContext ctx, EntityId partitionEntityId)
            {
                WorkerEntityId = ctx.WorkerEntityId;
                WorkerEntity = ctx.WorkerEntity;
                PartitionEntityId = partitionEntityId;
            }
        }

        private struct SeenWorker : IComponentData
        {
        }
    }

    public struct RegisteredClientWorker : ISystemStateComponentData
    {
        public EntityId PartitionEntityId;
    }
}
