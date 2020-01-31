using System;
using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public abstract class CommandSendStorage<TRequest, TResponse> :
        ICommandRequestSendStorage<TRequest>,
        ICommandResponseSendStorage<TResponse>
        where TRequest : struct, ICommandRequest
        where TResponse : struct, ICommandResponse
    {
        private readonly MessageList<CommandRequestWithMetaData<TRequest>> requestStorage =
            new MessageList<CommandRequestWithMetaData<TRequest>>();

        private readonly MessageList<TResponse> responseStorage = new MessageList<TResponse>();

        public Type RequestType => typeof(TRequest);
        public Type ResponseType => typeof(TResponse);

        public void Clear()
        {
            requestStorage.Clear();
            responseStorage.Clear();
        }

        public void AddRequest(TRequest request, Entity entity, long requestId)
        {
            requestStorage.Add(new CommandRequestWithMetaData<TRequest>(request, entity, requestId));
        }

        public void AddResponse(TResponse response)
        {
            responseStorage.Add(response);
        }

        internal MessageList<CommandRequestWithMetaData<TRequest>> GetRequests()
        {
            return requestStorage;
        }

        internal MessageList<TResponse> GetResponses()
        {
            return responseStorage;
        }
    }
}
