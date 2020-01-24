using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommandDiffStorageGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "System.Collections.Generic",
                    "Improbable.Gdk.Core",
                    "Unity.Entities"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.ComponentName}", partial =>
                    {
                        foreach (var command in componentDetails.CommandDetails)
                        {
                            partial.Text(GenerateCommandStorage(command, qualifiedNamespace, componentDetails.ComponentName));
                            partial.Text(GenerateCommandsToSendStorage(command, qualifiedNamespace, componentDetails.ComponentName));
                        }
                    });
                });
            }).Format();
        }

        private static Text GenerateCommandStorage(UnityCommandDetails command, string qualifiedNamespace, string componentName)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.CommandName}CommandStorage class.");

            var receivedRequestType = $"{command.CommandName}.ReceivedRequest";
            var receivedResponseType = $"{command.CommandName}.ReceivedResponse";

            return Text.New($@"
public class Diff{command.CommandName}CommandStorage : IComponentCommandDiffStorage
    , IDiffCommandRequestStorage<{receivedRequestType}>
    , IDiffCommandResponseStorage<{receivedResponseType}>
{{
    private readonly MessageList<{receivedRequestType}> requestStorage =
        new MessageList<{receivedRequestType}>();

    private readonly MessageList<{receivedResponseType}> responseStorage =
        new MessageList<{receivedResponseType}>();

    private readonly RequestComparer requestComparer = new RequestComparer();
    private readonly ResponseComparer responseComparer = new ResponseComparer();

    private bool requestsSorted;
    private bool responsesSorted;

    public uint GetComponentId()
    {{
        return ComponentId;
    }}

    public uint GetCommandId()
    {{
        return {command.CommandIndex};
    }}

    public Type GetRequestType()
    {{
        return typeof({receivedRequestType});
    }}

    public Type GetResponseType()
    {{
        return typeof({receivedResponseType});
    }}

    public void Clear()
    {{
        requestStorage.Clear();
        responseStorage.Clear();
        requestsSorted = false;
        responsesSorted = false;
    }}

    public void RemoveRequests(long entityId)
    {{
        requestStorage.RemoveAll(request => request.EntityId.Id == entityId);
    }}

    public void AddRequest({receivedRequestType} request)
    {{
        requestStorage.Add(request);
    }}

    public void AddResponse({receivedResponseType} response)
    {{
        responseStorage.Add(response);
    }}

    public MessagesSpan<{receivedRequestType}> GetRequests()
    {{
        return requestStorage.Slice();
    }}

    public MessagesSpan<{receivedRequestType}> GetRequests(EntityId targetEntityId)
    {{
        if (!requestsSorted)
        {{
            requestStorage.Sort(requestComparer);
            requestsSorted = true;
        }}

        var (firstIndex, count) = requestStorage.GetEntityRange(targetEntityId);
        return requestStorage.Slice(firstIndex, count);
    }}

    public MessagesSpan<{receivedResponseType}> GetResponses()
    {{
        return responseStorage.Slice();
    }}

    public MessagesSpan<{receivedResponseType}> GetResponse(long requestId)
    {{
        if (!responsesSorted)
        {{
            responseStorage.Sort(responseComparer);
            responsesSorted = true;
        }}

        var responseIndex = responseStorage.GetResponseIndex(requestId);
        return responseIndex.HasValue
            ? responseStorage.Slice(responseIndex.Value, 1)
            : MessagesSpan<{receivedResponseType}>.Empty();
    }}

    private class RequestComparer : IComparer<{receivedRequestType}>
    {{
        public int Compare({receivedRequestType} x, {receivedRequestType} y)
        {{
            return x.EntityId.Id.CompareTo(y.EntityId.Id);
        }}
    }}

    private class ResponseComparer : IComparer<{receivedResponseType}>
    {{
        public int Compare({receivedResponseType} x, {receivedResponseType} y)
        {{
            return x.RequestId.CompareTo(y.RequestId);
        }}
    }}
}}
");
        }

        private static Text GenerateCommandsToSendStorage(UnityCommandDetails command, string qualifiedNamespace, string componentName)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.CommandName}CommandsToSendStorage class.");

            return Text.New($@"
public class {command.CommandName}CommandsToSendStorage :
    CommandSendStorage<{command.CommandName}.Request, {command.CommandName}.Response>,
    IComponentCommandSendStorage
{{
    public uint GetComponentId()
    {{
        return ComponentId;
    }}

    public uint GetCommandId()
    {{
        return {command.CommandIndex};
    }}
}}
");
        }
    }
}
