using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Restricted;
using Improbable.Worker.CInterop;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    internal class PartitionManagementSystem : ComponentSystem
    {
        public string[] WorkerTypes { get; internal set; } = { };

        private readonly Dictionary<CommandRequestId, PartitionCreationContext> partitionCreationContexts =
            new Dictionary<CommandRequestId, PartitionCreationContext>();

        private readonly Dictionary<CommandRequestId, AssignPartitionContext> assignPartitionContexts =
            new Dictionary<CommandRequestId, AssignPartitionContext>();

        private CommandSystem commandSystem;

        private EntityQuery newWorkers;
        private EntityQuery workersWithoutPartitions;
        private EntityQuery removedWorkers;

        protected override void OnCreate()
        {
            commandSystem = World.GetExistingSystem<CommandSystem>();

            newWorkers = GetEntityQuery(ComponentType.ReadOnly<Improbable.Restricted.Worker.Component>(),
                ComponentType.Exclude<WorkerClassification>());

            workersWithoutPartitions = GetEntityQuery(
                ComponentType.ReadOnly<Improbable.Restricted.Worker.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.Exclude<SeenWorker>());

            removedWorkers = GetEntityQuery(
                ComponentType.ReadOnly<RegisteredWorker>(),
                ComponentType.Exclude<Improbable.Restricted.Worker.Component>());
        }

        protected override void OnUpdate()
        {
            ClassifyWorkers();
            CreateNewPartitions();
            AssignPartitions();
            StorePartitionInfo();
            CleanupOldPartitions();
        }

        private void ClassifyWorkers()
        {
            Entities.With(newWorkers).ForEach((Entity entity, ref Improbable.Restricted.Worker.Component worker) =>
            {
                PostUpdateCommands.AddSharedComponent(entity, new WorkerClassification(worker.WorkerType));
            });
        }

        private void CreateNewPartitions()
        {
            Entities.With(workersWithoutPartitions).ForEach((Entity entity, ref SpatialEntityId spatialEntityId, ref Improbable.Restricted.Worker.Component worker) =>
            {
                if (Array.IndexOf(WorkerTypes, worker.WorkerType) != -1)
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
            var requestId = commandSystem.SendCommand(new Improbable.Restricted.Worker.AssignPartition.Request(
                context.WorkerEntityId, new AssignPartitionRequest(context.PartitionEntityId.Id)));
            assignPartitionContexts[requestId] = context;
        }

        private void StorePartitionInfo()
        {
            var responses = commandSystem.GetResponses<Improbable.Restricted.Worker.AssignPartition.ReceivedResponse>();

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

                PostUpdateCommands.AddComponent(context.WorkerEntity, new RegisteredWorker
                {
                    PartitionEntityId = context.PartitionEntityId
                });
            }
        }

        private void CleanupOldPartitions()
        {
            // If the client has disconnected, we need to remove that partition.
            Entities.With(removedWorkers).ForEach((Entity entity, ref RegisteredWorker registeredClientWorker) =>
            {
                commandSystem.SendCommand(new WorldCommands.DeleteEntity.Request(registeredClientWorker.PartitionEntityId));
                PostUpdateCommands.RemoveComponent<RegisteredWorker>(entity);
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

    public struct RegisteredWorker : ISystemStateComponentData, IEquatable<RegisteredWorker>
    {
        public EntityId PartitionEntityId;

        public bool Equals(RegisteredWorker other)
        {
            return PartitionEntityId.Equals(other.PartitionEntityId);
        }

        public override bool Equals(object obj)
        {
            return obj is RegisteredWorker other && Equals(other);
        }

        public override int GetHashCode()
        {
            return PartitionEntityId.GetHashCode();
        }

        public static bool operator ==(RegisteredWorker left, RegisteredWorker right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RegisteredWorker left, RegisteredWorker right)
        {
            return !left.Equals(right);
        }
    }

    public struct WorkerClassification : ISharedComponentData, IEquatable<WorkerClassification>
    {
        public string WorkerType;

        public WorkerClassification(string workerType)
        {
            WorkerType = workerType;
        }

        public bool Equals(WorkerClassification other)
        {
            return WorkerType == other.WorkerType;
        }

        public override bool Equals(object obj)
        {
            return obj is WorkerClassification other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (WorkerType != null ? WorkerType.GetHashCode() : 0);
        }

        public static bool operator ==(WorkerClassification left, WorkerClassification right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WorkerClassification left, WorkerClassification right)
        {
            return !left.Equals(right);
        }
    }
}
