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
        private readonly Dictionary<CommandRequestId, CommandContext<WorldCommands.CreateEntity.Request>>
            idToCreateEntityRequest =
                new Dictionary<CommandRequestId, CommandContext<WorldCommands.CreateEntity.Request>>();

        private readonly Dictionary<CommandRequestId, CommandContext<WorldCommands.DeleteEntity.Request>>
            idToDeleteEntityRequest =
                new Dictionary<CommandRequestId, CommandContext<WorldCommands.DeleteEntity.Request>>();

        private readonly Dictionary<CommandRequestId, CommandContext<WorldCommands.ReserveEntityIds.Request>>
            idToReserveEntityIdsRequest =
                new Dictionary<CommandRequestId, CommandContext<WorldCommands.ReserveEntityIds.Request>>();

        private readonly Dictionary<CommandRequestId, CommandContext<WorldCommands.EntityQuery.Request>>
            idToEntityQueryRequest =
                new Dictionary<CommandRequestId, CommandContext<WorldCommands.EntityQuery.Request>>();

        private readonly Dictionary<InternalCommandRequestId, CommandRequestId> internalRequestIdToRequestId =
            new Dictionary<InternalCommandRequestId, CommandRequestId>();

        public uint CommandId => 0;

        public void RemoveMetaData(InternalCommandRequestId internalRequestId)
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

        public void SetInternalRequestId(InternalCommandRequestId internalRequestId, CommandRequestId requestId)
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
            GetPayload(InternalCommandRequestId internalRequestId)
        {
            return idToCreateEntityRequest[internalRequestIdToRequestId[internalRequestId]];
        }

        CommandContext<WorldCommands.DeleteEntity.Request> ICommandPayloadStorage<WorldCommands.DeleteEntity.Request>.
            GetPayload(InternalCommandRequestId internalRequestId)
        {
            return idToDeleteEntityRequest[internalRequestIdToRequestId[internalRequestId]];
        }

        CommandContext<WorldCommands.ReserveEntityIds.Request>
            ICommandPayloadStorage<WorldCommands.ReserveEntityIds.Request>.GetPayload(
                InternalCommandRequestId internalRequestId)
        {
            return idToReserveEntityIdsRequest[internalRequestIdToRequestId[internalRequestId]];
        }

        CommandContext<WorldCommands.EntityQuery.Request> ICommandPayloadStorage<WorldCommands.EntityQuery.Request>.
            GetPayload(InternalCommandRequestId internalRequestId)
        {
            return idToEntityQueryRequest[internalRequestIdToRequestId[internalRequestId]];
        }
    }
}
