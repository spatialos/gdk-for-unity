using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommandDiffDeserializerGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate(UnityComponentDetails componentDetails, string package)
        {
            var qualifiedNamespace = package;

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "Improbable.Gdk.Core",
                    "Improbable.Worker.CInterop"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.ComponentName}", partial =>
                    {
                        foreach (var command in componentDetails.CommandDetails)
                        {
                            partial.Text(GenerateDiffCommandDeserializer(command, qualifiedNamespace, componentDetails.ComponentName));
                            partial.Text(GenerateCommandSerializer(command, qualifiedNamespace, componentDetails.ComponentName));
                        }
                    });
                });
            }).Format();
        }

        private static Text GenerateDiffCommandDeserializer(UnityCommandDetails command, string qualifiedNamespace, string componentName)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.CommandName}DiffCommandDeserializer class.");

            return Text.New($@"
private class {command.CommandName}DiffCommandDeserializer : ICommandDiffDeserializer
{{
    public void AddRequestToDiff(CommandRequestOp op, ViewDiff diff)
    {{
        var deserializedRequest = {command.FqnRequestType}.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

        var request = new {command.CommandName}.ReceivedRequest(
            new EntityId(op.EntityId),
            op.RequestId,
            op.CallerWorkerId,
            op.CallerAttributeSet,
            deserializedRequest);

        diff.AddCommandRequest(request, ComponentId, {command.CommandIndex});
    }}

    public void AddResponseToDiff(CommandResponseOp op, ViewDiff diff, CommandMetaDataAggregate commandMetaData)
    {{
        {command.FqnResponseType}? rawResponse = null;
        if (op.StatusCode == StatusCode.Success)
        {{
            rawResponse = {command.FqnResponseType}.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
        }}

        var commandContext = commandMetaData.GetContext<{command.FqnRequestType}>(ComponentId, {command.CommandIndex}, op.RequestId);
        commandMetaData.MarkIdForRemoval(ComponentId, {command.CommandIndex}, op.RequestId);

        var response = new {command.CommandName}.ReceivedResponse(
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
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.CommandName}CommandSerializer class.");

            return Text.New($@"
private class {command.CommandName}CommandSerializer : ICommandSerializer
{{
    public void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
    {{
        var storage = ({command.CommandName}CommandsToSendStorage) messages.GetCommandSendStorage(ComponentId, {command.CommandIndex});

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
                request.Request.TargetEntityId.Id, request.Request.TimeoutMillis, request.RequestId);
        }}

        var responses = storage.GetResponses();
        for (int i = 0; i < responses.Count; ++i)
        {{
            ref readonly var response = ref responses[i];
            if (response.FailureMessage != null)
            {{
                // Send a command failure if the string is non-null.

                serializedMessages.AddFailure(ComponentId, {command.CommandIndex}, response.FailureMessage, (uint) response.RequestId);
                continue;
            }}

            var schemaCommandResponse = global::Improbable.Worker.CInterop.SchemaCommandResponse.Create();
            {command.FqnResponseType}.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

            var serializedResponse = new global::Improbable.Worker.CInterop.CommandResponse(ComponentId, {command.CommandIndex}, schemaCommandResponse);

            serializedMessages.AddResponse(serializedResponse, (uint) response.RequestId);
        }}
    }}
}}
");
        }
    }
}
