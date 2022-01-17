using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Restricted;
using Improbable.Worker.CInterop;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.LoadBalancing
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    internal class PartitionManagementSystem : SystemBase
    {
        public string[] WorkerTypes { get; internal set; } = { };

        private NativeHashSet<CommandRequestId> pendingPartitionRequests;

        private CommandSystem commandSystem;
        private WorkerSystem workerSystem;

        protected override void OnCreate()
        {
            commandSystem = World.GetExistingSystem<CommandSystem>();
            workerSystem = World.GetExistingSystem<WorkerSystem>();

            pendingPartitionRequests = new NativeHashSet<CommandRequestId>(16, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            pendingPartitionRequests.Dispose();
        }

        protected override void OnUpdate()
        {
            StorePartitionInfo();
            CreateNewPartitions();
            CleanupOldPartitions();
        }

        private void CreateNewPartitions()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var comSystem = commandSystem;
            var requests = pendingPartitionRequests;

            foreach (var workerType in WorkerTypes)
            {
                Entities
                    .WithoutBurst()
                    .WithSharedComponentFilter(new WorkerClassification(workerType))
                    .WithNone<PartitionRequestedForWorker>()
                    .ForEach((Entity entity, in SpatialEntityId spatialEntityId) =>
                {
                    // Request worker entity to be turned into a partition
                    var requestId = comSystem.SendCommand(new Improbable.Restricted.Worker.AssignPartition.Request(
                        spatialEntityId.EntityId, new AssignPartitionRequest(spatialEntityId.EntityId.Id)));
                    requests.Add(requestId);

                    // Mark entity as having an partition requested
                    ecb.AddComponent<PartitionRequestedForWorker>(entity);
                    
                    Debug.Log($"Requested worker {spatialEntityId.EntityId} to be made a partition.");
                }).Run();
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void StorePartitionInfo()
        {
            var responses = commandSystem.GetResponses<Improbable.Restricted.Worker.AssignPartition.ReceivedResponse>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            for (var i = 0; i < responses.Count; i++)
            {
                ref readonly var response = ref responses[i];
                var workerEntityId = new EntityId(response.RequestPayload.PartitionId);

                if (!pendingPartitionRequests.Contains(response.RequestId) || !workerSystem.TryGetEntity(workerEntityId, out var workerEntity))
                {
                    continue;
                }

                pendingPartitionRequests.Remove(response.RequestId);

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
                        Debug.Log($"Worker {workerEntityId} partition failed with status {response.StatusCode}, Retrying");
                        ecb.RemoveComponent<PartitionRequestedForWorker>(workerEntity);
                        continue;
                    case StatusCode.NotFound:
                        // The worker has disconnected, just ignore the result
                        // This should never be reached, as the above worker system lookup should fail first
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(response.StatusCode), response.StatusCode, "Unknown status code");
                }

                // Mark entity as a RegisteredWorker
                ecb.AddComponent(workerEntity, new RegisteredWorker
                {
                    PartitionEntityId = workerEntityId
                });

                Debug.Log($"Worker {workerEntityId} is now a partition.");
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void CleanupOldPartitions()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            // If the worker has disconnected, we need to remove that partition.
            Entities
                .WithNone<Improbable.Restricted.Worker.Component>()
                .ForEach((Entity entity, in RegisteredWorker registeredClientWorker) =>
            {
                Debug.Log($"Removed partition {registeredClientWorker.PartitionEntityId.Id}.");
                ecb.RemoveComponent<RegisteredWorker>(entity);
            }).Run();

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private struct PartitionRequestedForWorker : IComponentData
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
