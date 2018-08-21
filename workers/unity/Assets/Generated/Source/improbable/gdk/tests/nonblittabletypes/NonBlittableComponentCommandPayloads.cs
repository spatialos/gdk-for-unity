// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class FirstCommand
        {
            public struct Request
            {
                public EntityId TargetEntityId { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest RawRequest { get; }
                public uint? TimeoutMillis { get; }
                public bool AllowShortCircuiting { get; }

                public Request(EntityId targetEntityId,
                    global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest request,
                    uint? timeoutMillis = null,
                    bool allowShortCircuiting = false)
                {
                    TargetEntityId = targetEntityId;
                    RawRequest = request;
                    TimeoutMillis = timeoutMillis;
                    AllowShortCircuiting = allowShortCircuiting;
                }
            }

            public struct ReceivedRequest
            {
                public uint RequestId { get; }
                public string CallerWorkerId { get; }
                public List<string> CallerAttributeSet { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest RawRequest { get; }

                public ReceivedRequest(uint requestId,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest request)
                {
                    RequestId = requestId;
                    CallerWorkerId = callerWorkerId;
                    CallerAttributeSet = callerAttributeSet;
                    RawRequest = request;
                }
            }

            public struct Response
            {
                public uint RequestId { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse? RawResponse { get; }
                public string FailureMessage { get; }

                internal Response(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse? payload, string failureMessage)
                {
                    RequestId = req.RequestId;
                    RawResponse = payload;
                    FailureMessage = failureMessage;
                }

                public static Response CreateResponse(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse payload)
                {
                    return new Response(req, payload, null);
                }

                public static Response CreateFailure(ReceivedRequest req, string failureMessage)
                {
                    return new Response(req, null, failureMessage);
                }
            }

            public struct ReceivedResponse
            {
                public EntityId EntityId { get; }
                public string Message { get; }
                public StatusCode StatusCode { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse? RawResponse { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest RawRequest { get; }

                public ReceivedResponse(EntityId entityId,
                    string message,
                    StatusCode statusCode,
                    global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse? response,
                    global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest request)
                {
                    EntityId = entityId;
                    Message = message;
                    StatusCode = statusCode;
                    RawResponse = response;
                    RawRequest = request;
                }
            }
        }
        public class SecondCommand
        {
            public struct Request
            {
                public EntityId TargetEntityId { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest RawRequest { get; }
                public uint? TimeoutMillis { get; }
                public bool AllowShortCircuiting { get; }

                public Request(EntityId targetEntityId,
                    global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest request,
                    uint? timeoutMillis = null,
                    bool allowShortCircuiting = false)
                {
                    TargetEntityId = targetEntityId;
                    RawRequest = request;
                    TimeoutMillis = timeoutMillis;
                    AllowShortCircuiting = allowShortCircuiting;
                }
            }

            public struct ReceivedRequest
            {
                public uint RequestId { get; }
                public string CallerWorkerId { get; }
                public List<string> CallerAttributeSet { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest RawRequest { get; }

                public ReceivedRequest(uint requestId,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest request)
                {
                    RequestId = requestId;
                    CallerWorkerId = callerWorkerId;
                    CallerAttributeSet = callerAttributeSet;
                    RawRequest = request;
                }
            }

            public struct Response
            {
                public uint RequestId { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse? RawResponse { get; }
                public string FailureMessage { get; }

                internal Response(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse? payload, string failureMessage)
                {
                    RequestId = req.RequestId;
                    RawResponse = payload;
                    FailureMessage = failureMessage;
                }

                public static Response CreateResponse(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse payload)
                {
                    return new Response(req, payload, null);
                }

                public static Response CreateFailure(ReceivedRequest req, string failureMessage)
                {
                    return new Response(req, null, failureMessage);
                }
            }

            public struct ReceivedResponse
            {
                public EntityId EntityId { get; }
                public string Message { get; }
                public StatusCode StatusCode { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse? RawResponse { get; }
                public global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest RawRequest { get; }

                public ReceivedResponse(EntityId entityId,
                    string message,
                    StatusCode statusCode,
                    global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse? response,
                    global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest request)
                {
                    EntityId = entityId;
                    Message = message;
                    StatusCode = statusCode;
                    RawResponse = response;
                    RawRequest = request;
                }
            }
        }
    }
}
