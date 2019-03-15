// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public class DiffCmdCommandStorage : IComponentCommandDiffStorage
            , IDiffCommandRequestStorage<Cmd.ReceivedRequest>
            , IDiffCommandResponseStorage<Cmd.ReceivedResponse>
        {
            private MessageList<Cmd.ReceivedRequest> requestStorage =
                new MessageList<Cmd.ReceivedRequest>();

            private MessageList<Cmd.ReceivedResponse> responseStorage =
                new MessageList<Cmd.ReceivedResponse>();

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
                return typeof(Cmd.ReceivedRequest);
            }

            public Type GetResponseType()
            {
                return typeof(Cmd.ReceivedResponse);
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

            public void AddRequest(Cmd.ReceivedRequest request)
            {
                requestStorage.Add(request);
            }

            public void AddResponse(Cmd.ReceivedResponse response)
            {
                responseStorage.Add(response);
            }

            public ReceivedMessagesSpan<Cmd.ReceivedRequest> GetRequests()
            {
                return new ReceivedMessagesSpan<Cmd.ReceivedRequest>(requestStorage);
            }

            public ReceivedMessagesSpan<Cmd.ReceivedRequest> GetRequests(EntityId targetEntityId)
            {
                if (!requestsSorted)
                {
                    requestStorage.Sort(requestComparer);
                    requestsSorted = true;
                }

                var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);

                return new ReceivedMessagesSpan<Cmd.ReceivedRequest>(requestStorage, firstIndex, count);
            }

            public ReceivedMessagesSpan<Cmd.ReceivedResponse> GetResponses()
            {
                return new ReceivedMessagesSpan<Cmd.ReceivedResponse>(responseStorage);
            }

            public ReceivedMessagesSpan<Cmd.ReceivedResponse> GetResponse(long requestId)
            {
                if (!responsesSorted)
                {
                    responseStorage.Sort(responseComparer);
                    responsesSorted = true;
                }

                var responseIndex = responseStorage.GetResponseIndex(requestId);
                if (responseIndex < 0)
                {
                    return ReceivedMessagesSpan<Cmd.ReceivedResponse>.Empty();
                }

                return new ReceivedMessagesSpan<Cmd.ReceivedResponse>(responseStorage, responseIndex, 1);
            }

            private class RequestComparer : IComparer<Cmd.ReceivedRequest>
            {
                public int Compare(Cmd.ReceivedRequest x, Cmd.ReceivedRequest y)
                {
                    return x.EntityId.Id.CompareTo(y.EntityId.Id);
                }
            }

            private class ResponseComparer : IComparer<Cmd.ReceivedResponse>
            {
                public int Compare(Cmd.ReceivedResponse x, Cmd.ReceivedResponse y)
                {
                    return x.RequestId.CompareTo(y.RequestId);
                }
            }
        }


        public class CmdCommandsToSendStorage : ICommandSendStorage, IComponentCommandSendStorage
            , ICommandRequestSendStorage<Cmd.Request>
            , ICommandResponseSendStorage<Cmd.Response>
        {
            private MessageList<CommandRequestWithMetaData<Cmd.Request>> requestStorage =
                new MessageList<CommandRequestWithMetaData<Cmd.Request>>();

            private MessageList<Cmd.Response> responseStorage =
                new MessageList<Cmd.Response>();

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
                return typeof(Cmd.Request);
            }

            public Type GetResponseType()
            {
                return typeof(Cmd.Response);
            }

            public void Clear()
            {
                requestStorage.Clear();
                responseStorage.Clear();
            }

            public void AddRequest(Cmd.Request request, Entity entity, long requestId)
            {
                requestStorage.Add(new CommandRequestWithMetaData<Cmd.Request>(request, entity, requestId));
            }

            public void AddResponse(Cmd.Response response)
            {
                responseStorage.Add(response);
            }

            internal MessageList<CommandRequestWithMetaData<Cmd.Request>> GetRequests()
            {
                return requestStorage;
            }

            internal MessageList<Cmd.Response> GetResponses()
            {
                return responseStorage;
            }
        }

    }
}
