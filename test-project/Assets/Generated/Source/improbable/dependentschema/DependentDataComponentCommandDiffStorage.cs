// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
    {
        public class DiffBarCommandCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<BarCommand.ReceivedRequest>
            , IDiffCommandResponseStorage<BarCommand.ReceivedResponse>
        {
            private readonly MessageList<BarCommand.ReceivedRequest> requestStorage =
                new MessageList<BarCommand.ReceivedRequest>();

            private readonly MessageList<BarCommand.ReceivedResponse> responseStorage =
                new MessageList<BarCommand.ReceivedResponse>();

            private readonly RequestComparer requestComparer = new RequestComparer();
            private readonly ResponseComparer responseComparer = new ResponseComparer();

            private bool requestsSorted;
            private bool responsesSorted;

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public Type GetRequestType()
            {
                return typeof(BarCommand.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(BarCommand.ReceivedResponse);
            }

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

            public void AddRequest(BarCommand.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(BarCommand.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public MessagesSpan<BarCommand.ReceivedRequest> GetRequests()
            {
                return requestStorage.Slice();
            }

            public MessagesSpan<BarCommand.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);
                return requestStorage.Slice(firstIndex, count);
            }

            public MessagesSpan<BarCommand.ReceivedResponse> GetResponses()
            {
                return responseStorage.Slice();
            }

            public MessagesSpan<BarCommand.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                return responseIndex.HasValue
                    ? responseStorage.Slice(responseIndex.Value, 1)
                    : MessagesSpan<BarCommand.ReceivedResponse>.Empty();
            }

            private class RequestComparer : IComparer<BarCommand.ReceivedRequest>
            {
                public int Compare(BarCommand.ReceivedRequest x, BarCommand.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<BarCommand.ReceivedResponse>
            {
                public int Compare(BarCommand.ReceivedResponse x, BarCommand.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class BarCommandCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<BarCommand.Request>
            , ICommandResponseSendStorage<BarCommand.Response>
        {
            private readonly MessageList<CommandRequestWithMetaData<BarCommand.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<BarCommand.Request>>();

            private readonly MessageList<BarCommand.Response> responseStorage =
                new MessageList<BarCommand.Response>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public Type GetRequestType()
            {
                return typeof(BarCommand.Request);
            }

            public Type GetResponseType()
            {
                return typeof(BarCommand.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(BarCommand.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<BarCommand.Request>(request, entity, requestId));
            }

            public void AddResponse(BarCommand.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<BarCommand.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<BarCommand.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
