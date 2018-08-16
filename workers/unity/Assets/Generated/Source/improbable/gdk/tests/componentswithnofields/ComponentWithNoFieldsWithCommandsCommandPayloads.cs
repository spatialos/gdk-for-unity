// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public class Cmd
        {
            public struct Request
            {
                public EntityId TargetEntityId { get; }
                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty RawRequest { get; }
                public uint? TimeoutMillis { get; }
                public bool AllowShortCircuiting { get; }

                public Request(EntityId targetEntityId,
                    global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request,
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
                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty RawRequest { get; }

                public ReceivedRequest(uint requestId,
                    string callerWorkerId,
                    List<string> callerAttributeSet,
                    global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request)
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
                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty? RawResponse { get; }
                public string FailureMessage { get; }

                internal Response(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty? payload, string failureMessage)
                {
                    RequestId = req.RequestId;
                    RawResponse = payload;
                    FailureMessage = failureMessage;
                }

                public static Response CreateResponse(ReceivedRequest req, global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload)
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
                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty? RawResponse { get; }
                public global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty RawRequest { get; }

                public ReceivedResponse(EntityId entityId,
                    string message,
                    StatusCode statusCode,
                    global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty? response,
                    global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request)
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
