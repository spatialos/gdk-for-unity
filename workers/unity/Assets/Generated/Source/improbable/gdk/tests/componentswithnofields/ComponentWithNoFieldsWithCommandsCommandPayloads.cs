// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using System.Collections.Generic;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public class Cmd
        {
            public struct Request : IIncomingCommandRequest
            {
                public uint RequestId { get; }

                internal Translation Translation { get; }

                public string CallerWorkerId {get; }

                public List<string> CallerAttributeSet { get; }

                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty RawRequest { get; }

                internal Request(uint requestId, 
                    Translation translation,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request)
                {
                    this.RequestId = requestId;
                    this.Translation = translation;
                    this.CallerWorkerId = callerWorkerId;
                    this.CallerAttributeSet = callerAttributeSet;
                    this.RawRequest = request;
                }

                public void SendCmdResponse(global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty response)
                {
                    Translation.CmdResponses.Add(
                        new OutgoingResponse(RequestId, response));
                }
                
                public void SendCmdFailure(string message)
                {
                    Translation.CmdFailure.Add(
                        new CommandFailure(RequestId, message));
                }
            }

            internal struct OutgoingRequest : IOutgoingCommandRequest
            {
                public long TargetEntityId { get; }

                public long SenderEntityId { get; }

                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty RawRequest { get; }

                internal OutgoingRequest(long targetEntityId, long senderEntityId,
                    global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request)
                {
                    this.TargetEntityId = targetEntityId;
                    this.SenderEntityId = senderEntityId;
                    this.RawRequest = request;
                }
            }

            public struct Response : IIncomingCommandResponse
            {
                public long EntityId { get; }

                public string Message { get; }

                public CommandStatusCode StatusCode { get; }

                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty? RawResponse { get; }

                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty RawRequest { get; }

                internal Response(long entityId, 
                    string message,
                    CommandStatusCode statusCode, 
                    global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty? response,
                    global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request)
                {
                    this.EntityId = entityId;
                    this.Message = message;
                    this.StatusCode = statusCode;
                    this.RawResponse = response;
                    this.RawRequest = request;
                }
            }

            internal struct OutgoingResponse : IOutgoingCommandResponse
            {
                public uint RequestId { get; }

                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty RawResponse { get; }

                internal OutgoingResponse(uint requestId, 
                    global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty response)
                {
                    this.RequestId = requestId;
                    this.RawResponse = response;
                }
            }
        }
    }
}
