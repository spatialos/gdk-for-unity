using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    public class WorldCommandMetaDataStorage : ICommandMetaDataStorage
        , ICommandPayloadStorage<WorldCommands.CreateEntity.Request>
        , ICommandPayloadStorage<WorldCommands.DeleteEntity.Request>
        , ICommandPayloadStorage<WorldCommands.ReserveEntityIds.Request>
        , ICommandPayloadStorage<WorldCommands.EntityQuery.Request>
    {
        private readonly Dictionary<long, CommandContext<WorldCommands.CreateEntity.Request>> idToCreateEntityRequest =
            new Dictionary<long, CommandContext<WorldCommands.CreateEntity.Request>>();

        private readonly Dictionary<long, CommandContext<WorldCommands.DeleteEntity.Request>> idToDeleteEntityRequest =
            new Dictionary<long, CommandContext<WorldCommands.DeleteEntity.Request>>();

        private readonly Dictionary<long, CommandContext<WorldCommands.ReserveEntityIds.Request>>
            idToReserveEntityIdsRequest =
                new Dictionary<long, CommandContext<WorldCommands.ReserveEntityIds.Request>>();

        private readonly Dictionary<long, CommandContext<WorldCommands.EntityQuery.Request>> idToEntityQueryRequest =
            new Dictionary<long, CommandContext<WorldCommands.EntityQuery.Request>>();

        private readonly Dictionary<uint, long> internalRequestIdToRequestId = new Dictionary<uint, long>();

        public uint GetComponentId()
        {
            return 0;
        }

        public uint GetCommandId()
        {
            return 0;
        }

        public void RemoveMetaData(uint internalRequestId)
        {
            var requestId = internalRequestIdToRequestId[internalRequestId];
            internalRequestIdToRequestId.Remove(internalRequestId);

            if (!idToCreateEntityRequest.Remove(requestId) &&
                !idToDeleteEntityRequest.Remove(requestId) &&
                !idToReserveEntityIdsRequest.Remove(requestId) &&
                !idToEntityQueryRequest.Remove(requestId))
            {
                throw new ArgumentException($"Can not remove non-existent command metadata for request ID {requestId}",
                    nameof(internalRequestId));
            }
        }

        public void AddRequestId(uint internalRequestId, long requestId)
        {
            internalRequestIdToRequestId.Add(internalRequestId, requestId);
        }

        public long GetRequestId(uint internalRequestId)
        {
            return internalRequestIdToRequestId[internalRequestId];
        }

        public void AddRequest(CommandContext<WorldCommands.CreateEntity.Request> context, long requestId)
        {
            idToCreateEntityRequest.Add(requestId, context);
        }

        public void AddRequest(CommandContext<WorldCommands.DeleteEntity.Request> context, long requestId)
        {
            idToDeleteEntityRequest.Add(requestId, context);
        }

        public void AddRequest(CommandContext<WorldCommands.ReserveEntityIds.Request> context, long requestId)
        {
            idToReserveEntityIdsRequest.Add(requestId, context);
        }

        public void AddRequest(CommandContext<WorldCommands.EntityQuery.Request> context, long requestId)
        {
            idToEntityQueryRequest.Add(requestId, context);
        }

        CommandContext<WorldCommands.CreateEntity.Request> ICommandPayloadStorage<WorldCommands.CreateEntity.Request>.
            GetPayload(long requestId)
        {
            return idToCreateEntityRequest[requestId];
        }

        CommandContext<WorldCommands.DeleteEntity.Request> ICommandPayloadStorage<WorldCommands.DeleteEntity.Request>.
            GetPayload(long requestId)
        {
            return idToDeleteEntityRequest[requestId];
        }

        CommandContext<WorldCommands.ReserveEntityIds.Request>
            ICommandPayloadStorage<WorldCommands.ReserveEntityIds.Request>.GetPayload(long requestId)
        {
            return idToReserveEntityIdsRequest[requestId];
        }

        CommandContext<WorldCommands.EntityQuery.Request> ICommandPayloadStorage<WorldCommands.EntityQuery.Request>.
            GetPayload(long requestId)
        {
            return idToEntityQueryRequest[requestId];
        }
    }
}
