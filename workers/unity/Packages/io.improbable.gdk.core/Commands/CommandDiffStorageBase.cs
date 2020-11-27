using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Commands
{
    public abstract class CommandDiffStorageBase<TRequest, TResponse> : IComponentCommandDiffStorage
        , IDiffCommandRequestStorage<TRequest>
        , IDiffCommandResponseStorage<TResponse>
        where TRequest : struct, IReceivedCommandRequest
        where TResponse : struct, IReceivedCommandResponse
    {
        private readonly MessageList<TRequest> requestStorage = new MessageList<TRequest>(new RequestComparer<TRequest>());
        private readonly MessageList<TResponse> responseStorage = new MessageList<TResponse>(new ResponseComparer<TResponse>());

        public abstract uint ComponentId { get; }
        public abstract uint CommandId { get; }

        public Type RequestType => typeof(TRequest);
        public Type ResponseType => typeof(TResponse);

        public void Clear()
        {
            requestStorage.Clear();
            responseStorage.Clear();
        }

        public void RemoveRequests(long entityId)
        {
            requestStorage.RemoveAll(request => request.EntityId.Id == entityId);
        }

        public void AddRequest(TRequest request)
        {
            requestStorage.Add(request);
        }

        public void AddResponse(TResponse response)
        {
            responseStorage.Add(response);
        }

        public MessagesSpan<TRequest> GetRequests()
        {
            return requestStorage.Slice();
        }

        public MessagesSpan<TRequest> GetRequests(EntityId targetEntityId)
        {
            return requestStorage.GetEntityRange(targetEntityId);
        }

        public MessagesSpan<TResponse> GetResponses()
        {
            return responseStorage.Slice();
        }

        public TResponse? GetResponse(CommandRequestId requestId)
        {
            return responseStorage.GetResponse(requestId);
        }

        private sealed class RequestComparer<T> : IComparer<T> where T : struct, IReceivedCommandRequest
        {
            public int Compare(T x, T y)
            {
                return x.EntityId.Id.CompareTo(y.EntityId.Id);
            }
        }

        private sealed class ResponseComparer<T> : IComparer<T> where T : struct, IReceivedCommandResponse
        {
            public int Compare(T x, T y)
            {
                return x.RequestId.CompareTo(y.RequestId);
            }
        }
    }
}
