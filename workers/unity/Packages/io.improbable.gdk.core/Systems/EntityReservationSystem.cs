using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public class EntityReservationSystem : ComponentSystem
    {
        public uint TargetEntityIdCount = 200;
        public uint MinimumReservationCount = 10;

        // Actual entities left on stack
        private long unreservedCount;
        private long inFlightCount;

        private readonly HashSet<long> requestIds = new HashSet<long>();

        private readonly Queue<(TaskCompletionSource<EntityRangeCollection> taskCompletionSource, uint count)> queuedReservations =
            new Queue<(TaskCompletionSource<EntityRangeCollection> taskCompletionSource, uint count)>();

        private readonly EntityRangeCollection entityIdQueue = new EntityRangeCollection();
        private CommandSystem commandSystem;

        protected override void OnCreate()
        {
            commandSystem = World.GetExistingSystem<CommandSystem>();
        }

        protected override void OnUpdate()
        {
            // If there are outstanding requests, receive them
            HandleReservationRequests();

            // If there are queued ranges, attempt to resolve them
            ResolveQueuedRequests();

            // Request new entity id's? (Maybe move to another system)
            RequestQueueRefill();
        }

        private void RequestQueueRefill()
        {
            var requestCount = -(unreservedCount - TargetEntityIdCount + inFlightCount);
            if (requestCount > 0)
            {
                requestCount = Math.Max(requestCount, MinimumReservationCount);
                var reserveEntityIdsRequest = new WorldCommands.ReserveEntityIds.Request((uint) requestCount);
                commandSystem.SendCommand(reserveEntityIdsRequest);
            }
        }

        private void HandleReservationRequests()
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
                    unreservedCount += range.Count;
                }

                inFlightCount += request.RequestPayload.NumberOfEntityIds;

                // Remove ID from the set as it has been handled.
                requestIds.Remove(request.RequestId);
            }
        }

        private void ResolveQueuedRequests()
        {
            // Remove canceled reservations from the queue
            while (queuedReservations.Count > 0)
            {
                var (taskCompletionSource, count) = queuedReservations.Peek();
                if (!taskCompletionSource.Task.IsCanceled)
                {
                    break;
                }

                unreservedCount += count;
                queuedReservations.Dequeue();
            }

            // Loop over queued request, check if we have enough, and call them
            foreach (var (taskCompletionSource, count) in queuedReservations)
            {
                if (entityIdQueue.Count < count)
                {
                    break;
                }

                taskCompletionSource.SetResult(entityIdQueue.Take(count));
            }
        }

        public Task<EntityRangeCollection> Take(uint count, CancellationToken cancellationToken = default)
        {
            unreservedCount -= count;

            if (unreservedCount >= 0)
            {
                return Task.FromResult(entityIdQueue.Take(count));
            }
            else
            {
                var tcs = new TaskCompletionSource<EntityRangeCollection>();
                cancellationToken.Register((source) =>
                {
                    ((TaskCompletionSource<EntityRangeCollection>) source).TrySetCanceled();
                }, tcs);

                queuedReservations.Enqueue((tcs, count));
                return tcs.Task;
            }
        }
    }
}
