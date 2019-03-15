// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class DiffFirstCommandCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<FirstCommand.ReceivedRequest>
            , IDiffCommandResponseStorage<FirstCommand.ReceivedResponse>
        {
            private MessageList<FirstCommand.ReceivedRequest> requestStorage =
                new MessageList<FirstCommand.ReceivedRequest>();

            private MessageList<FirstCommand.ReceivedResponse> responseStorage =
                new MessageList<FirstCommand.ReceivedResponse>();

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
                return typeof(FirstCommand.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(FirstCommand.ReceivedResponse);
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

            public void AddRequest(FirstCommand.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(FirstCommand.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<FirstCommand.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<FirstCommand.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<FirstCommand.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<FirstCommand.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<FirstCommand.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<FirstCommand.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<FirstCommand.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<FirstCommand.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<FirstCommand.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<FirstCommand.ReceivedRequest>
            {
                public int Compare(FirstCommand.ReceivedRequest x, FirstCommand.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<FirstCommand.ReceivedResponse>
            {
                public int Compare(FirstCommand.ReceivedResponse x, FirstCommand.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }

        public class DiffSecondCommandCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<SecondCommand.ReceivedRequest>
            , IDiffCommandResponseStorage<SecondCommand.ReceivedResponse>
        {
            private MessageList<SecondCommand.ReceivedRequest> requestStorage =
                new MessageList<SecondCommand.ReceivedRequest>();

            private MessageList<SecondCommand.ReceivedResponse> responseStorage =
                new MessageList<SecondCommand.ReceivedResponse>();

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
                return 2;
            }

            public Type GetRequestType()
            {
                return typeof(SecondCommand.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(SecondCommand.ReceivedResponse);
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

            public void AddRequest(SecondCommand.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(SecondCommand.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<SecondCommand.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<SecondCommand.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<SecondCommand.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<SecondCommand.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<SecondCommand.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<SecondCommand.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<SecondCommand.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<SecondCommand.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<SecondCommand.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<SecondCommand.ReceivedRequest>
            {
                public int Compare(SecondCommand.ReceivedRequest x, SecondCommand.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<SecondCommand.ReceivedResponse>
            {
                public int Compare(SecondCommand.ReceivedResponse x, SecondCommand.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class FirstCommandCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<FirstCommand.Request>
            , ICommandResponseSendStorage<FirstCommand.Response>
        {
            private MessageList<CommandRequestWithMetaData<FirstCommand.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<FirstCommand.Request>>();

            private MessageList<FirstCommand.Response> responseStorage =
                new MessageList<FirstCommand.Response>();

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
                return typeof(FirstCommand.Request);
            }

            public Type GetResponseType()
            {
                return typeof(FirstCommand.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(FirstCommand.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<FirstCommand.Request>(request, entity, requestId));
            }

            public void AddResponse(FirstCommand.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<FirstCommand.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<FirstCommand.Response> GetResponses()
            {
                return responseStorage;
            }
        }

        public class SecondCommandCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<SecondCommand.Request>
            , ICommandResponseSendStorage<SecondCommand.Response>
        {
            private MessageList<CommandRequestWithMetaData<SecondCommand.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<SecondCommand.Request>>();

            private MessageList<SecondCommand.Response> responseStorage =
                new MessageList<SecondCommand.Response>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 2;
            }

            public Type GetRequestType()
            {
                return typeof(SecondCommand.Request);
            }

            public Type GetResponseType()
            {
                return typeof(SecondCommand.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(SecondCommand.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<SecondCommand.Request>(request, entity, requestId));
            }

            public void AddResponse(SecondCommand.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<SecondCommand.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<SecondCommand.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
