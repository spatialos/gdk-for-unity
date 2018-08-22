// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public class FirstCommand
        {
            /// <summary>
            ///     Please do not use the default constructor. Use CreateRequest instead.
            /// </summary>
            public struct Request
            {
                public EntityId TargetEntityId { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest Payload { get; }
                public uint? TimeoutMillis { get; }
                public bool AllowShortCircuiting { get; }

                internal Request(EntityId targetEntityId,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest request,
                    uint? timeoutMillis = null,
                    bool allowShortCircuiting = false)
                {
                    TargetEntityId = targetEntityId;
                    Payload = request;
                    TimeoutMillis = timeoutMillis;
                    AllowShortCircuiting = allowShortCircuiting;
                }
            }

            public static Request CreateRequest(EntityId targetEntityId,
                global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest request,
                uint? timeoutMillis = null,
                bool allowShortCircuiting = false)
            {
                return new Request(targetEntityId, request, timeoutMillis, allowShortCircuiting);
            }

            public struct ReceivedRequest
            {
                public uint RequestId { get; }
                public string CallerWorkerId { get; }
                public List<string> CallerAttributeSet { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest Payload { get; }

                public ReceivedRequest(uint requestId,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest request)
                {
                    RequestId = requestId;
                    CallerWorkerId = callerWorkerId;
                    CallerAttributeSet = callerAttributeSet;
                    Payload = request;
                }
            }

            /// <summary>
            ///     Please do not use the default constructor. Use CreateResponse or CreateFailure instead.
            /// </summary>
            public struct Response
            {
                public uint RequestId { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse? Payload { get; }
                public string FailureMessage { get; }

                internal Response(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse? payload, string failureMessage)
                {
                    RequestId = req.RequestId;
                    Payload = payload;
                    FailureMessage = failureMessage;
                }
            }

            public static Response CreateResponse(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse payload)
            {
                return new Response(req, payload, null);
            }

            public static Response CreateResponseFailure(ReceivedRequest req, string failureMessage)
            {
                return new Response(req, null, failureMessage);
            }

            public struct ReceivedResponse
            {
                public EntityId EntityId { get; }
                public string Message { get; }
                public StatusCode StatusCode { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse? ResponsePayload { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest RequestPayload { get; }

                public ReceivedResponse(EntityId entityId,
                    string message,
                    StatusCode statusCode,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse? response,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest request)
                {
                    EntityId = entityId;
                    Message = message;
                    StatusCode = statusCode;
                    ResponsePayload = response;
                    RequestPayload = request;
                }
            }
        }
        public class SecondCommand
        {
            /// <summary>
            ///     Please do not use the default constructor. Use CreateRequest instead.
            /// </summary>
            public struct Request
            {
                public EntityId TargetEntityId { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest Payload { get; }
                public uint? TimeoutMillis { get; }
                public bool AllowShortCircuiting { get; }

                internal Request(EntityId targetEntityId,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest request,
                    uint? timeoutMillis = null,
                    bool allowShortCircuiting = false)
                {
                    TargetEntityId = targetEntityId;
                    Payload = request;
                    TimeoutMillis = timeoutMillis;
                    AllowShortCircuiting = allowShortCircuiting;
                }
            }

            public static Request CreateRequest(EntityId targetEntityId,
                global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest request,
                uint? timeoutMillis = null,
                bool allowShortCircuiting = false)
            {
                return new Request(targetEntityId, request, timeoutMillis, allowShortCircuiting);
            }

            public struct ReceivedRequest
            {
                public uint RequestId { get; }
                public string CallerWorkerId { get; }
                public List<string> CallerAttributeSet { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest Payload { get; }

                public ReceivedRequest(uint requestId,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest request)
                {
                    RequestId = requestId;
                    CallerWorkerId = callerWorkerId;
                    CallerAttributeSet = callerAttributeSet;
                    Payload = request;
                }
            }

            /// <summary>
            ///     Please do not use the default constructor. Use CreateResponse or CreateFailure instead.
            /// </summary>
            public struct Response
            {
                public uint RequestId { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse? Payload { get; }
                public string FailureMessage { get; }

                internal Response(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse? payload, string failureMessage)
                {
                    RequestId = req.RequestId;
                    Payload = payload;
                    FailureMessage = failureMessage;
                }
            }

            public static Response CreateResponse(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse payload)
            {
                return new Response(req, payload, null);
            }

            public static Response CreateResponseFailure(ReceivedRequest req, string failureMessage)
            {
                return new Response(req, null, failureMessage);
            }

            public struct ReceivedResponse
            {
                public EntityId EntityId { get; }
                public string Message { get; }
                public StatusCode StatusCode { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse? ResponsePayload { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest RequestPayload { get; }

                public ReceivedResponse(EntityId entityId,
                    string message,
                    StatusCode statusCode,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse? response,
                    global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest request)
                {
                    EntityId = entityId;
                    Message = message;
                    StatusCode = statusCode;
                    ResponsePayload = response;
                    RequestPayload = request;
                }
            }
        }
    }
}
