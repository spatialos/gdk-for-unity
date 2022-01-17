using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommandDiffDeserializerGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails componentDetails)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Core.Commands",
                    "Improbable.Worker.CInterop"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        foreach (var command in componentDetails.CommandDetails)
                        {
                            partial.Text(GenerateDiffCommandDeserializer(command, componentDetails.Namespace, componentDetails.Name));
                            partial.Text(GenerateCommandSerializer(command, componentDetails.Namespace, componentDetails.Name));
                        }
                    });
                });
            });
        }

        private static Text GenerateDiffCommandDeserializer(UnityCommandDetails command, string qualifiedNamespace, string componentName)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.PascalCaseName}DiffCommandDeserializer class.");

            return Text.New($@"
private class {command.PascalCaseName}DiffCommandDeserializer : ICommandDiffDeserializer
{{
    public void AddRequestToDiff(CommandRequestOp op, ViewDiff diff)
    {{
        var deserializedRequest = {command.FqnRequestType}.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

        var request = new {command.PascalCaseName}.ReceivedRequest(
            new EntityId(op.EntityId),
            op.RequestId,
            new EntityId(op.CallerWorkerEntityId),
            deserializedRequest);

        diff.AddCommandRequest(request, ComponentId, {command.CommandIndex});
    }}

    public void AddResponseToDiff(CommandResponseOp op, ViewDiff diff, CommandMetaData commandMetaData)
    {{
        {command.FqnResponseType}? rawResponse = null;
        if (op.StatusCode == StatusCode.Success)
        {{
            rawResponse = {command.FqnResponseType}.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
        }}

        var internalRequestId = new InternalCommandRequestId(op.RequestId);
        var commandContext = commandMetaData.GetContext<{command.FqnRequestType}>(ComponentId, {command.CommandIndex}, internalRequestId);
        commandMetaData.RemoveRequest(ComponentId, {command.CommandIndex}, internalRequestId);

        var response = new {command.PascalCaseName}.ReceivedResponse(
            commandContext.SendingEntity,
            new EntityId(op.EntityId),
            op.Message,
            op.StatusCode,
            rawResponse,
            commandContext.Request,
            commandContext.Context,
            commandContext.RequestId);

        diff.AddCommandResponse(response, ComponentId, {command.CommandIndex});
    }}
}}
");
        }

        private static Text GenerateCommandSerializer(UnityCommandDetails command, string qualifiedNamespace,
            string componentName)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.PascalCaseName}CommandSerializer class.");

            return Text.New($@"
private class {command.PascalCaseName}CommandSerializer : ICommandSerializer
{{
    public void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
    {{
        var storage = ({command.PascalCaseName}CommandsToSendStorage) messages.GetCommandSendStorage(ComponentId, {command.CommandIndex});

        if (!storage.Dirty)
        {{
            return;
        }}

        var requests = storage.GetRequests();

        for (int i = 0; i < requests.Count; ++i)
        {{
            ref readonly var request = ref requests[i];
            var context = new CommandContext<{command.FqnRequestType}>(request.SendingEntity, request.Request.Payload, request.Request.Context, request.RequestId);
            commandMetaData.AddRequest<{command.FqnRequestType}>(ComponentId, {command.CommandIndex}, in context);

            var schemaCommandRequest = global::Improbable.Worker.CInterop.SchemaCommandRequest.Create();
            {command.FqnRequestType}.Serialization.Serialize(request.Request.Payload, schemaCommandRequest.GetObject());
            var serializedRequest = new global::Improbable.Worker.CInterop.CommandRequest(ComponentId, {command.CommandIndex}, schemaCommandRequest);

            serializedMessages.AddRequest(serializedRequest, {command.CommandIndex},
                request.Request.TargetEntityId.Id, request.Request.TimeoutMillis, request.Request.AllowShortCircuiting, request.RequestId);
        }}

        var responses = storage.GetResponses();
        for (int i = 0; i < responses.Count; ++i)
        {{
            ref readonly var response = ref responses[i];
            if (response.FailureMessage != null)
            {{
                // Send a command failure if the string is non-null.

                serializedMessages.AddFailure(ComponentId, {command.CommandIndex}, response.FailureMessage, response.RequestId);
                continue;
            }}

            var schemaCommandResponse = global::Improbable.Worker.CInterop.SchemaCommandResponse.Create();
            {command.FqnResponseType}.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

            var serializedResponse = new global::Improbable.Worker.CInterop.CommandResponse(ComponentId, {command.CommandIndex}, schemaCommandResponse);

            serializedMessages.AddResponse(serializedResponse, response.RequestId);
        }}
    }}
}}
");
        }
    }
}
