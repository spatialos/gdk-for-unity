using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Commands
{
    public abstract class DiffSpawnCubeCommandStorage<TRequest, TResponse> : IComponentCommandDiffStorage
        , IDiffCommandRequestStorage<TRequest>
        , IDiffCommandResponseStorage<TResponse>
        where TRequest : struct, IReceivedCommandRequest
        where TResponse : struct, IReceivedCommandResponse
    {
        private readonly MessageList<TRequest> requestStorage = new MessageList<TRequest>();
        private readonly MessageList<TResponse> responseStorage = new MessageList<TResponse>();

        private readonly RequestComparer<TRequest> requestComparer = new RequestComparer<TRequest>();
        private readonly ResponseComparer<TResponse> responseComparer = new ResponseComparer<TResponse>();

        private bool requestsSorted;
        private bool responsesSorted;

        public abstract uint ComponentId { get; }
        public abstract uint CommandId { get; }

        public Type RequestType => typeof(TRequest);
        public Type ResponseType => typeof(TResponse);

        public void Clear()
        {
            requestStorage.Clear();
            responseStorage.Clear();
            requestsSorted = false;
            responsesSorted = false;
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
            if (!requestsSorted)
            {
                requestStorage.Sort(requestComparer);
                requestsSorted = true;
            }

            var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);
            return requestStorage.Slice(firstIndex, count);
        }

        public MessagesSpan<TResponse> GetResponses()
        {
            return responseStorage.Slice();
        }

        public MessagesSpan<TResponse> GetResponse(long requestId)
        {
            if (!responsesSorted)
            {
                responseStorage.Sort(responseComparer);
                responsesSorted = true;
            }

            var responseIndex = responseStorage.GetResponseIndex(requestId);
            return responseIndex.HasValue
                ? responseStorage.Slice(responseIndex.Value, 1)
                : MessagesSpan<TResponse>.Empty();
        }

        private class RequestComparer<T> : IComparer<T> where T : struct, IReceivedCommandRequest
        {
            public int Compare(T x, T y)
            {
                return x.EntityId.Id.CompareTo(y.EntityId.Id);
            }
        }

        private class ResponseComparer<T> : IComparer<T> where T : struct, IReceivedCommandResponse
        {
            public int Compare(T x, T y)
            {
                return x.RequestId.CompareTo(y.RequestId);
            }
        }
    }
}
