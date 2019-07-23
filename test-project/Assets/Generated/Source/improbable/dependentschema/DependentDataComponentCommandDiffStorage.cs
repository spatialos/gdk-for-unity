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
            private MessageList<BarCommand.ReceivedRequest> requestStorage =
                new MessageList<BarCommand.ReceivedRequest>();

            private MessageList<BarCommand.ReceivedResponse> responseStorage =
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

            public ReceivedMessagesSpan<BarCommand.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<BarCommand.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<BarCommand.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<BarCommand.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<BarCommand.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<BarCommand.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<BarCommand.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<BarCommand.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<BarCommand.ReceivedResponse>(responseStorage, responseIndex, 1);
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
            private MessageList<CommandRequestWithMetaData<BarCommand.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<BarCommand.Request>>();

            private MessageList<BarCommand.Response> responseStorage =
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
