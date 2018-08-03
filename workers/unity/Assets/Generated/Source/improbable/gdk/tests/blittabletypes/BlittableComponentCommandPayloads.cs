// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using System.Collections.Generic;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public class FirstCommand
        {
            public struct Request : IIncomingCommandRequest
            {
                public uint RequestId { get; }

                internal Translation Translation { get; }

                public string CallerWorkerId {get; }

                public List<string> CallerAttributeSet { get; }

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest RawRequest { get; }

                internal Request(uint requestId, 
                    Translation translation,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest request)
                {
                    this.RequestId = requestId;
                    this.Translation = translation;
                    this.CallerWorkerId = callerWorkerId;
                    this.CallerAttributeSet = callerAttributeSet;
                    this.RawRequest = request;
                }

                public void SendFirstCommandResponse(global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse response)
                {
                    Translation.FirstCommandResponses.Add(
                        new OutgoingResponse(RequestId, response));
                }
                
                public void SendFirstCommandFailure(string message)
                {
                    Translation.FirstCommandFailure.Add(
                        new CommandFailure(RequestId, message));
                }
            }

            internal struct OutgoingRequest : IOutgoingCommandRequest
            {
                public long TargetEntityId { get; }

                public long SenderEntityId { get; }

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest RawRequest { get; }

                internal OutgoingRequest(long targetEntityId, long senderEntityId,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest request)
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

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse? RawResponse { get; }

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest RawRequest { get; }

                internal Response(long entityId, 
                    string message,
                    CommandStatusCode statusCode, 
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse? response,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest request)
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

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse RawResponse { get; }

                internal OutgoingResponse(uint requestId, 
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse response)
                {
                    this.RequestId = requestId;
                    this.RawResponse = response;
                }
            }
        }
        public class SecondCommand
        {
            public struct Request : IIncomingCommandRequest
            {
                public uint RequestId { get; }

                internal Translation Translation { get; }

                public string CallerWorkerId {get; }

                public List<string> CallerAttributeSet { get; }

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest RawRequest { get; }

                internal Request(uint requestId, 
                    Translation translation,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest request)
                {
                    this.RequestId = requestId;
                    this.Translation = translation;
                    this.CallerWorkerId = callerWorkerId;
                    this.CallerAttributeSet = callerAttributeSet;
                    this.RawRequest = request;
                }

                public void SendSecondCommandResponse(global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse response)
                {
                    Translation.SecondCommandResponses.Add(
                        new OutgoingResponse(RequestId, response));
                }
                
                public void SendSecondCommandFailure(string message)
                {
                    Translation.SecondCommandFailure.Add(
                        new CommandFailure(RequestId, message));
                }
            }

            internal struct OutgoingRequest : IOutgoingCommandRequest
            {
                public long TargetEntityId { get; }

                public long SenderEntityId { get; }

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest RawRequest { get; }

                internal OutgoingRequest(long targetEntityId, long senderEntityId,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest request)
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

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse? RawResponse { get; }

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest RawRequest { get; }

                internal Response(long entityId, 
                    string message,
                    CommandStatusCode statusCode, 
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse? response,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest request)
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

                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse RawResponse { get; }

                internal OutgoingResponse(uint requestId, 
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse response)
                {
                    this.RequestId = requestId;
                    this.RawResponse = response;
                }
            }
        }
    }
}
