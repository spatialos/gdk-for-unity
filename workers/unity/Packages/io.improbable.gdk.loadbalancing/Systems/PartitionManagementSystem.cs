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
        private WorkerSystem workerSystem;


        private EntityQuery workersWithoutPartitions;

        private EntityQuery removedWorkers;
        private EntityQuery unknownPartitions;

        protected override void OnCreate()
        {
            commandSystem = World.GetExistingSystem<CommandSystem>();
            workerSystem = World.GetExistingSystem<WorkerSystem>();

            workersWithoutPartitions = GetEntityQuery(
                ComponentType.ReadOnly<WorkerClassification>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.Exclude<PartitionCreatedForWorker>());

            removedWorkers = GetEntityQuery(
                ComponentType.ReadOnly<RegisteredWorker>(),
                ComponentType.Exclude<Improbable.Restricted.Worker.Component>());

            unknownPartitions = GetEntityQuery(
                ComponentType.ReadOnly<Improbable.Gdk.Loadbalancing.Partition.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.Exclude<KnownPartition>());
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
            foreach (var workerType in WorkerTypes)
            {
                workersWithoutPartitions.SetSharedComponentFilter(new WorkerClassification(workerType));

                Entities.With(workersWithoutPartitions).ForEach((Entity entity, ref SpatialEntityId spatialEntityId) =>
                {
                    var requestId = commandSystem.SendCommand(new WorldCommands.CreateEntity.Request(GetPartitionEntity(workerType, spatialEntityId.EntityId)));
                    var context = new PartitionCreationContext(spatialEntityId.EntityId, entity);
                    partitionCreationContexts[requestId] = context;

                    PostUpdateCommands.AddComponent<PartitionCreatedForWorker>(entity);
                });
            }
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
                    PostUpdateCommands.RemoveComponent<PartitionCreatedForWorker>(context.WorkerEntity);
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
            // If the worker has disconnected, we need to remove that partition.
            Entities.With(removedWorkers).ForEach((Entity entity, ref RegisteredWorker registeredClientWorker) =>
            {
                commandSystem.SendCommand(new WorldCommands.DeleteEntity.Request(registeredClientWorker.PartitionEntityId));
                PostUpdateCommands.RemoveComponent<RegisteredWorker>(entity);
                PostUpdateCommands.DestroyEntity(entity);
            });

            // If the partition is checked out, but the worker entity isn't.. it must be an old one ==> delete it.
            Entities.With(unknownPartitions).ForEach(
                (Entity entity, ref SpatialEntityId spatialEntityId, ref Improbable.Gdk.Loadbalancing.Partition.Component partition) =>
                {
                    if (!workerSystem.TryGetEntity(partition.WorkerEntityId, out _))
                    {
                        commandSystem.SendCommand(new WorldCommands.DeleteEntity.Request(spatialEntityId.EntityId));
                    }
                    else
                    {
                        PostUpdateCommands.AddComponent<KnownPartition>(entity);
                    }
                });
        }

        internal static EntityTemplate GetPartitionEntity(string workerType, EntityId workerEntityId)
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new Metadata.Snapshot($"{workerType} Partition"));
            template.AddComponent(new Improbable.Gdk.Loadbalancing.Partition.Snapshot(workerType, workerEntityId));
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

        private struct PartitionCreatedForWorker : IComponentData
        {
        }

        private struct KnownPartition : IComponentData
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
}
