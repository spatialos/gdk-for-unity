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
            ///     Using CreateRequest will ensure a correctly formed structure.
            /// </summary>
            public struct Request
            {
                public EntityId TargetEntityId { get; internal set; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest Payload { get; internal set; }
                public uint? TimeoutMillis { get; internal set; }
                public bool AllowShortCircuiting { get; internal set; }
            }

            public static Request CreateRequest(EntityId targetEntityId,
                global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest request,
                uint? timeoutMillis = null,
                bool allowShortCircuiting = false)
            {
                return new Request
                {
                    TargetEntityId = targetEntityId,
                    Payload = request,
                    TimeoutMillis = timeoutMillis,
                    AllowShortCircuiting = allowShortCircuiting,
                };
            }

            public struct ReceivedRequest
            {
                public long RequestId { get; }
                public string CallerWorkerId { get; }
                public List<string> CallerAttributeSet { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest Payload { get; }

                public ReceivedRequest(long requestId,
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
            ///     Using CreateResponse or CreateFailure will ensure a correctly formed structure.
            /// </summary>
            public struct Response
            {
                public long RequestId { get; internal set; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse? Payload { get; internal set; }
                public string FailureMessage { get; internal set; }
            }

            public static Response CreateResponse(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse payload)
            {
                return new Response
                {
                    RequestId = req.RequestId,
                    Payload = payload,
                    FailureMessage = null,
                };
            }

            public static Response CreateResponseFailure(ReceivedRequest req, string failureMessage)
            {
                return new Response
                {
                    RequestId = req.RequestId,
                    Payload = null,
                    FailureMessage = failureMessage,
                };
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
            ///     Using CreateRequest will ensure a correctly formed structure.
            /// </summary>
            public struct Request
            {
                public EntityId TargetEntityId { get; internal set; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest Payload { get; internal set; }
                public uint? TimeoutMillis { get; internal set; }
                public bool AllowShortCircuiting { get; internal set; }
            }

            public static Request CreateRequest(EntityId targetEntityId,
                global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest request,
                uint? timeoutMillis = null,
                bool allowShortCircuiting = false)
            {
                return new Request
                {
                    TargetEntityId = targetEntityId,
                    Payload = request,
                    TimeoutMillis = timeoutMillis,
                    AllowShortCircuiting = allowShortCircuiting,
                };
            }

            public struct ReceivedRequest
            {
                public long RequestId { get; }
                public string CallerWorkerId { get; }
                public List<string> CallerAttributeSet { get; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest Payload { get; }

                public ReceivedRequest(long requestId,
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
            ///     Using CreateResponse or CreateFailure will ensure a correctly formed structure.
            /// </summary>
            public struct Response
            {
                public long RequestId { get; internal set; }
                public global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse? Payload { get; internal set; }
                public string FailureMessage { get; internal set; }
            }

            public static Response CreateResponse(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse payload)
            {
                return new Response
                {
                    RequestId = req.RequestId,
                    Payload = payload,
                    FailureMessage = null,
                };
            }

            public static Response CreateResponseFailure(ReceivedRequest req, string failureMessage)
            {
                return new Response
                {
                    RequestId = req.RequestId,
                    Payload = null,
                    FailureMessage = failureMessage,
                };
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
