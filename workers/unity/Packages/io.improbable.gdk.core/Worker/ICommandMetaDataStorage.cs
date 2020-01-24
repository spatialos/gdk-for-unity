using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public interface ICommandMetaDataStorage
    {
        uint CommandId { get; }

        void RemoveMetaData(long internalRequestId);

        void SetInternalRequestId(long internalRequestId, long requestId);
    }

    public interface ICommandPayloadStorage<T>
    {
        CommandContext<T> GetPayload(long internalRequestId);
        void AddRequest(in CommandContext<T> context);
    }

    public abstract class CommandPayloadStorage<T> : ICommandPayloadStorage<T>
    {
        private readonly Dictionary<long, CommandContext<T>> requestIdToRequest =
            new Dictionary<long, CommandContext<T>>();

        private readonly Dictionary<long, long> internalRequestIdToRequestId = new Dictionary<long, long>();

        public void RemoveMetaData(long internalRequestId)
        {
            var requestId = internalRequestIdToRequestId[internalRequestId];
            internalRequestIdToRequestId.Remove(internalRequestId);
            requestIdToRequest.Remove(requestId);
        }

        public void SetInternalRequestId(long internalRequestId, long requestId)
        {
            internalRequestIdToRequestId.Add(internalRequestId, requestId);
        }

        public void AddRequest(in CommandContext<T> context)
        {
            requestIdToRequest[context.RequestId] = context;
        }

        public CommandContext<T> GetPayload(long internalRequestId)
        {
            var id = internalRequestIdToRequestId[internalRequestId];
            return requestIdToRequest[id];
        }
    }
}
