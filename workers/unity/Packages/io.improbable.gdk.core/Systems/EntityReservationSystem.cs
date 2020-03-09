using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public class EntityReservationSystem : ComponentSystem
    {
        // Some sort of range queue
        private EntityRangeCollection queue;

        public uint TargetEntityIdCount = 200;
        public uint MinimumReservationCount = 10;

        // Actual entities left on stack
        private long unreservedCount;
        private long inFlightCount;

        private readonly HashSet<long> requestIds = new HashSet<long>();
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
            RefillQueue();
        }

        private void RefillQueue()
        {
            var requestCount = (uint) (unreservedCount - TargetEntityIdCount + inFlightCount);
            requestCount = Math.Max(requestCount, MinimumReservationCount);
            var reserveEntityIdsRequest = new WorldCommands.ReserveEntityIds.Request(requestCount);
            commandSystem.SendCommand(reserveEntityIdsRequest);
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
                    queue.Add(range);
                }

                inFlightCount += request.RequestPayload.NumberOfEntityIds;

                // Remove ID from the set as it has been handled.
                requestIds.Remove(request.RequestId);
            }
        }

        private void ResolveQueuedRequests()
        {
            // Loop over queued request, check if we have enough, and call them
        }
    }
}
