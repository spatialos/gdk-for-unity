using System.Collections.Generic;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandMetaDataStorage
    {
        uint CommandId { get; }

        void RemoveMetaData(InternalCommandRequestId internalRequestId);

        void SetInternalRequestId(InternalCommandRequestId internalRequestId, CommandRequestId requestId);
    }

    public interface ICommandPayloadStorage<T>
    {
        CommandContext<T> GetPayload(InternalCommandRequestId internalRequestId);
        void AddRequest(in CommandContext<T> context);
    }

    public abstract class CommandPayloadStorage<T> : ICommandPayloadStorage<T>
    {
        private readonly Dictionary<CommandRequestId, CommandContext<T>> requestIdToRequest =
            new Dictionary<CommandRequestId, CommandContext<T>>();

        private readonly Dictionary<InternalCommandRequestId, CommandRequestId> internalRequestIdToRequestId = new Dictionary<InternalCommandRequestId, CommandRequestId>();

        public void RemoveMetaData(InternalCommandRequestId internalRequestId)
        {
            var requestId = internalRequestIdToRequestId[internalRequestId];
            internalRequestIdToRequestId.Remove(internalRequestId);
            requestIdToRequest.Remove(requestId);
        }

        public void SetInternalRequestId(InternalCommandRequestId internalRequestId, CommandRequestId requestId)
        {
            internalRequestIdToRequestId.Add(internalRequestId, requestId);
        }

        public void AddRequest(in CommandContext<T> context)
        {
            requestIdToRequest[context.RequestId] = context;
        }

        public CommandContext<T> GetPayload(InternalCommandRequestId internalRequestId)
        {
            var id = internalRequestIdToRequestId[internalRequestId];
            return requestIdToRequest[id];
        }
    }
}
