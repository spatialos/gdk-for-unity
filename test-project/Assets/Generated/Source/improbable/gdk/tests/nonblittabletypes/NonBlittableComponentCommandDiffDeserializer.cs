// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class FirstCommandDiffCommandDeserializer : ICommandDiffDeserializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public void AddRequestToDiff(CommandRequestOp op, ViewDiff diff)
            {
                var deserializedRequest = global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                var request = new FirstCommand.ReceivedRequest(
                    new EntityId(op.EntityId),
                    op.RequestId,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest);

                diff.AddCommandRequest(request, ComponentId, 1);
            }

            public void AddResponseToDiff(CommandResponseOp op, ViewDiff diff, CommandMetaDataAggregate commandMetaData)
            {
                global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse? rawResponse = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    rawResponse = global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                var commandContext = commandMetaData.GetContext<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>(ComponentId, 1, op.RequestId);
                commandMetaData.MarkIdForRemoval(ComponentId, 1, op.RequestId);

                var response = new FirstCommand.ReceivedResponse(
                    commandContext.SendingEntity,
                    new EntityId(op.EntityId),
                    op.Message,
                    op.StatusCode,
                    rawResponse,
                    commandContext.Request,
                    commandContext.Context,
                    commandContext.RequestId);

                diff.AddCommandResponse(response, ComponentId, 1);
            }
        }

        public class FirstCommandCommandSerializer : ICommandSerializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
            {
                var storage = (FirstCommandCommandsToSendStorage) messages.GetCommandSendStorage(ComponentId, 1);

                var requests = storage.GetRequests();

                for (int i = 0; i < requests.Count; ++i)
                {
                    ref readonly var request = ref requests[i];
                    var context = new CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>(request.SendingEntity, request.Request.Payload, request.Request.Context, request.RequestId);
                    commandMetaData.AddRequest<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>(ComponentId, 1, in context);

                    var schemaCommandRequest = new global::Improbable.Worker.CInterop.SchemaCommandRequest(ComponentId, 1);
                    global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest.Serialization.Serialize(request.Request.Payload, schemaCommandRequest.GetObject());
                    var serializedRequest = new global::Improbable.Worker.CInterop.CommandRequest(schemaCommandRequest);

                    serializedMessages.AddRequest(serializedRequest, 1,
                        request.Request.TargetEntityId.Id, request.Request.TimeoutMillis, request.RequestId);
                }

                var responses = storage.GetResponses();
                for (int i = 0; i < responses.Count; ++i)
                {
                    ref readonly var response = ref responses[i];
                    if (response.FailureMessage != null)
                    {
                        // Send a command failure if the string is non-null.

                        serializedMessages.AddFailure(response.FailureMessage, (uint) response.RequestId);
                        continue;
                    }

                    var schemaCommandResponse = new global::Improbable.Worker.CInterop.SchemaCommandResponse(ComponentId, 1);
                    global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

                    var serializedResponse = new global::Improbable.Worker.CInterop.CommandResponse(schemaCommandResponse);

                    serializedMessages.AddResponse(serializedResponse, (uint) response.RequestId);
                }
            }
        }

        public class SecondCommandDiffCommandDeserializer : ICommandDiffDeserializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 2;
            }

            public void AddRequestToDiff(CommandRequestOp op, ViewDiff diff)
            {
                var deserializedRequest = global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                var request = new SecondCommand.ReceivedRequest(
                    new EntityId(op.EntityId),
                    op.RequestId,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest);

                diff.AddCommandRequest(request, ComponentId, 2);
            }

            public void AddResponseToDiff(CommandResponseOp op, ViewDiff diff, CommandMetaDataAggregate commandMetaData)
            {
                global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse? rawResponse = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    rawResponse = global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                var commandContext = commandMetaData.GetContext<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>(ComponentId, 2, op.RequestId);
                commandMetaData.MarkIdForRemoval(ComponentId, 2, op.RequestId);

                var response = new SecondCommand.ReceivedResponse(
                    commandContext.SendingEntity,
                    new EntityId(op.EntityId),
                    op.Message,
                    op.StatusCode,
                    rawResponse,
                    commandContext.Request,
                    commandContext.Context,
                    commandContext.RequestId);

                diff.AddCommandResponse(response, ComponentId, 2);
            }
        }

        public class SecondCommandCommandSerializer : ICommandSerializer
        {
            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 2;
            }

            public void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
            {
                var storage = (SecondCommandCommandsToSendStorage) messages.GetCommandSendStorage(ComponentId, 2);

                var requests = storage.GetRequests();

                for (int i = 0; i < requests.Count; ++i)
                {
                    ref readonly var request = ref requests[i];
                    var context = new CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>(request.SendingEntity, request.Request.Payload, request.Request.Context, request.RequestId);
                    commandMetaData.AddRequest<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>(ComponentId, 2, in context);

                    var schemaCommandRequest = new global::Improbable.Worker.CInterop.SchemaCommandRequest(ComponentId, 2);
                    global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest.Serialization.Serialize(request.Request.Payload, schemaCommandRequest.GetObject());
                    var serializedRequest = new global::Improbable.Worker.CInterop.CommandRequest(schemaCommandRequest);

                    serializedMessages.AddRequest(serializedRequest, 2,
                        request.Request.TargetEntityId.Id, request.Request.TimeoutMillis, request.RequestId);
                }

                var responses = storage.GetResponses();
                for (int i = 0; i < responses.Count; ++i)
                {
                    ref readonly var response = ref responses[i];
                    if (response.FailureMessage != null)
                    {
                        // Send a command failure if the string is non-null.

                        serializedMessages.AddFailure(response.FailureMessage, (uint) response.RequestId);
                        continue;
                    }

                    var schemaCommandResponse = new global::Improbable.Worker.CInterop.SchemaCommandResponse(ComponentId, 2);
                    global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

                    var serializedResponse = new global::Improbable.Worker.CInterop.CommandResponse(schemaCommandResponse);

                    serializedMessages.AddResponse(serializedResponse, (uint) response.RequestId);
                }
            }
        }

    }
}
