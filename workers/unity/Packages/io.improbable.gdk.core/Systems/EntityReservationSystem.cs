using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    public class EntityReservationSystem : ComponentSystem
    {
        public uint TargetEntityIdCount = 100;
        public uint MinimumReservationCount = 10;

        // Actual entities left on stack
        private long queuedReservationCount;
        private long inFlightCount;

        private readonly HashSet<CommandRequestId> requestIds = new HashSet<CommandRequestId>();

        private readonly Queue<QueuedReservation> queuedReservations = new Queue<QueuedReservation>();

        private readonly EntityRangeCollection entityIdQueue = new EntityRangeCollection();
        private CommandSystem commandSystem;

        protected override void OnCreate()
        {
            commandSystem = World.GetExistingSystem<CommandSystem>();
        }

        protected override void OnUpdate()
        {
            // If there are outstanding requests, receive them
            HandleReservationResponses();

            // If there are queued ranges, attempt to resolve them
            ResolveQueuedRequests();

            // Request new entity id's? (Maybe move to another system)
            RequestQueueRefill();
        }

        private void RequestQueueRefill()
        {
            var requestCount = TargetEntityIdCount + queuedReservationCount - inFlightCount - entityIdQueue.Count;
            if (requestCount > 0)
            {
                requestCount = Math.Max(requestCount, MinimumReservationCount);
                inFlightCount += requestCount;

                var reserveEntityIdsRequest = new WorldCommands.ReserveEntityIds.Request((uint) requestCount);
                var requestId = commandSystem.SendCommand(reserveEntityIdsRequest);
                requestIds.Add(requestId);
            }
        }

        private void HandleReservationResponses()
        {
            var incomingRequests = commandSystem.GetResponses<WorldCommands.ReserveEntityIds.ReceivedResponse>();
            for (var i = 0; i < incomingRequests.Count; i++)
            {
                ref readonly var request = ref incomingRequests[i];
                if (!requestIds.Contains(request.RequestId))
                {
                    continue;
                }

                if (request.StatusCode == StatusCode.Success)
                {
                    //Add range to queue
                    var range = new EntityRangeCollection.EntityIdRange(request.FirstEntityId.Value, (uint) request.NumberOfEntityIds);
                    entityIdQueue.Add(range);
                }

                inFlightCount -= request.RequestPayload.NumberOfEntityIds;

                // Remove ID from the set as it has been handled.
                requestIds.Remove(request.RequestId);
            }
        }

        private void ResolveQueuedRequests()
        {
            while (queuedReservations.Count > 0)
            {
                var queuedReservation = queuedReservations.Peek();
                if (queuedReservation.IsCanceled())
                {
                    // Remove canceled tasks
                    queuedReservationCount -= queuedReservation.Count;
                    queuedReservations.Dequeue();
                }
                else if (entityIdQueue.Count < queuedReservation.Count)
                {
                    // Early out if we don't have enough id's yet
                    break;
                }

                queuedReservationCount -= queuedReservation.Count;
                queuedReservations.Dequeue();

                switch (queuedReservation.Type)
                {
                    case QueuedReservationType.Single:
                        queuedReservation.SingleTcs.TrySetResult(entityIdQueue.Dequeue());
                        break;
                    case QueuedReservationType.Multi:
                        queuedReservation.MultiTcs.TrySetResult(entityIdQueue.Take(queuedReservation.Count));
                        break;
                }
            }
        }

        public bool TryGet(out EntityId entityId)
        {
            if (entityIdQueue.Count >= 1)
            {
                entityId = entityIdQueue.Dequeue();
                return true;
            }

            entityId = default;
            return false;
        }

        public bool TryTake(uint count, out EntityId[] entityIds)
        {
            if (entityIdQueue.Count >= count)
            {
                entityIds = entityIdQueue.Take(count);
                return true;
            }

            entityIds = default;
            return false;
        }

        public Task<EntityId[]> TakeAsync(uint count, CancellationToken cancellationToken = default)
        {
            if (entityIdQueue.Count >= count)
            {
                return Task.FromResult(entityIdQueue.Take(count));
            }
            else
            {
                queuedReservationCount += count;

                var tcs = new TaskCompletionSource<EntityId[]>();
                cancellationToken.Register((source) =>
                {
                    ((TaskCompletionSource<EntityId[]>) source).TrySetCanceled();
                }, tcs);

                queuedReservations.Enqueue(new QueuedReservation(tcs, count));
                return tcs.Task;
            }
        }

        public Task<EntityId> GetAsync(CancellationToken cancellationToken = default)
        {
            if (entityIdQueue.Count >= 1)
            {
                return Task.FromResult(entityIdQueue.Dequeue());
            }
            else
            {
                queuedReservationCount += 1;

                var tcs = new TaskCompletionSource<EntityId>();
                cancellationToken.Register((source) =>
                {
                    ((TaskCompletionSource<EntityId>) source).TrySetCanceled();
                }, tcs);

                queuedReservations.Enqueue(new QueuedReservation(tcs));
                return tcs.Task;
            }
        }

        private readonly struct QueuedReservation
        {
            public readonly QueuedReservationType Type;
            public readonly TaskCompletionSource<EntityId> SingleTcs;
            public readonly TaskCompletionSource<EntityId[]> MultiTcs;
            public readonly uint Count;

            public QueuedReservation(TaskCompletionSource<EntityId[]> taskCompletionSource, uint count)
            {
                MultiTcs = taskCompletionSource;
                SingleTcs = null;
                Count = count;
                Type = QueuedReservationType.Multi;
            }

            public QueuedReservation(TaskCompletionSource<EntityId> taskCompletionSource)
            {
                MultiTcs = null;
                SingleTcs = taskCompletionSource;
                Count = 1;
                Type = QueuedReservationType.Single;
            }

            public bool IsCanceled()
            {
                switch (Type)
                {
                    case QueuedReservationType.Single:
                        return SingleTcs.Task.IsCanceled;
                    case QueuedReservationType.Multi:
                        return MultiTcs.Task.IsCanceled;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private enum QueuedReservationType
        {
            Single,
            Multi
        }
    }
}
