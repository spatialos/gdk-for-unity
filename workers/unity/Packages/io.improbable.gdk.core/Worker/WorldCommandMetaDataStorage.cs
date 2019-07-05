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

        public void SetInternalRequestId(uint internalRequestId, long requestId)
        {
            internalRequestIdToRequestId.Add(internalRequestId, requestId);
        }

        public void AddRequest(in CommandContext<WorldCommands.CreateEntity.Request> context)
        {
            idToCreateEntityRequest.Add(context.RequestId, context);
        }

        public void AddRequest(in CommandContext<WorldCommands.DeleteEntity.Request> context)
        {
            idToDeleteEntityRequest.Add(context.RequestId, context);
        }

        public void AddRequest(in CommandContext<WorldCommands.ReserveEntityIds.Request> context)
        {
            idToReserveEntityIdsRequest.Add(context.RequestId, context);
        }

        public void AddRequest(in CommandContext<WorldCommands.EntityQuery.Request> context)
        {
            idToEntityQueryRequest.Add(context.RequestId, context);
        }

        CommandContext<WorldCommands.CreateEntity.Request> ICommandPayloadStorage<WorldCommands.CreateEntity.Request>.
            GetPayload(uint internalRequestId)
        {
            return idToCreateEntityRequest[internalRequestIdToRequestId[internalRequestId]];
        }

        CommandContext<WorldCommands.DeleteEntity.Request> ICommandPayloadStorage<WorldCommands.DeleteEntity.Request>.
            GetPayload(uint internalRequestId)
        {
            return idToDeleteEntityRequest[internalRequestIdToRequestId[internalRequestId]];
        }

        CommandContext<WorldCommands.ReserveEntityIds.Request>
            ICommandPayloadStorage<WorldCommands.ReserveEntityIds.Request>.GetPayload(uint internalRequestId)
        {
            return idToReserveEntityIdsRequest[internalRequestIdToRequestId[internalRequestId]];
        }

        CommandContext<WorldCommands.EntityQuery.Request> ICommandPayloadStorage<WorldCommands.EntityQuery.Request>.
            GetPayload(uint internalRequestId)
        {
            return idToEntityQueryRequest[internalRequestIdToRequestId[internalRequestId]];
        }
    }
}
