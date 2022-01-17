using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityCommandPayloadGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails componentDetails)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System.Collections.Generic",
                    "Improbable.Worker.CInterop",
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Core.Commands"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        foreach (var commandDetails in componentDetails.CommandDetails)
                        {
                            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.{commandDetails.PascalCaseName} partial class.");

                            partial.Line($@"
public partial class {commandDetails.PascalCaseName}
{{
    public struct Request : ICommandRequest
    {{
        public EntityId TargetEntityId;
        public {commandDetails.FqnRequestType} Payload;
        public uint? TimeoutMillis;
        public bool AllowShortCircuiting;
        public object Context;

        public Request(
            EntityId targetEntityId,
            {commandDetails.FqnRequestType} request,
            uint? timeoutMillis = null,
            bool allowShortCircuiting = false,
            object context = null)
        {{
            TargetEntityId = targetEntityId;
            Payload = request;
            TimeoutMillis = timeoutMillis;
            AllowShortCircuiting = allowShortCircuiting;
            Context = context;
        }}
    }}

    public readonly struct ReceivedRequest : IReceivedCommandRequest
    {{
        public readonly EntityId EntityId;
        public readonly long RequestId;
        public readonly EntityId CallerWorkerEntityId;
        public readonly {commandDetails.FqnRequestType} Payload;

        public ReceivedRequest(
            EntityId entityId,
            long requestId,
            EntityId callerWorkerEntityId,
            {commandDetails.FqnRequestType} request)
        {{
            EntityId = entityId;
            RequestId = requestId;
            CallerWorkerEntityId = callerWorkerEntityId;
            Payload = request;
        }}

        long IReceivedCommandRequest.RequestId => RequestId;
        EntityId IReceivedCommandRequest.CallerWorkerEntityId => CallerWorkerEntityId;

        EntityId IReceivedEntityMessage.EntityId => EntityId;
    }}

    /// <summary>
    ///     A Response will be considered a failure if FailureMessage is not null;
    /// </summary>
    // todo Should consider making this a union of a failure and a response
    // todo consider making this readonly
    public struct Response : ICommandResponse
    {{
        public long RequestId;
        public {commandDetails.FqnResponseType}? Payload;
        public string FailureMessage;

        public Response(long requestId, {commandDetails.FqnResponseType} payload)
        {{
            RequestId = requestId;
            Payload = payload;
            FailureMessage = null;
        }}

        public Response(long requestId, string failureMessage)
        {{
            RequestId = requestId;
            Payload = null;
            FailureMessage = failureMessage;
        }}
    }}

    public readonly struct ReceivedResponse : IReceivedCommandResponse
    {{
        public readonly Unity.Entities.Entity SendingEntity;
        public readonly EntityId EntityId;
        public readonly string Message;
        public readonly StatusCode StatusCode;
        public readonly {commandDetails.FqnResponseType}? ResponsePayload;
        public readonly {commandDetails.FqnRequestType} RequestPayload;
        public readonly global::System.Object Context;
        public readonly CommandRequestId RequestId;

        public ReceivedResponse(
            Unity.Entities.Entity sendingEntity,
            EntityId entityId,
            string message,
            StatusCode statusCode,
            {commandDetails.FqnResponseType}? response,
            {commandDetails.FqnRequestType} request,
            global::System.Object context,
            CommandRequestId requestId)
        {{
            SendingEntity = sendingEntity;
            EntityId = entityId;
            Message = message;
            StatusCode = statusCode;
            ResponsePayload = response;
            RequestPayload = request;
            Context = context;
            RequestId = requestId;
        }}

        CommandRequestId IReceivedCommandResponse.RequestId => RequestId;
    }}

    public readonly struct RawReceivedResponse : IRawReceivedCommandResponse
    {{
        public readonly EntityId EntityId;
        public readonly string Message;
        public readonly StatusCode StatusCode;
        public readonly {commandDetails.FqnResponseType}? ResponsePayload;
        public readonly long RequestId;

        public RawReceivedResponse(
            EntityId entityId,
            string message,
            StatusCode statusCode,
            {commandDetails.FqnResponseType}? response,
            long requestId)
        {{
            EntityId = entityId;
            Message = message;
            StatusCode = statusCode;
            ResponsePayload = response;
            RequestId = requestId;
        }}
    }}
}}
");
                        }
                    });
                });
            });
        }
    }
}
