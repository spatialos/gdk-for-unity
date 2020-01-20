using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommandMetaDataStorageGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System.Collections.Generic",
                    "Improbable.Gdk.Core"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.ComponentName}", partial =>
                    {
                        foreach (var command in componentDetails.CommandDetails)
                        {
                            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.ComponentName}.{command.CommandName}CommandMetaDataStorage class.");

                            var receivedRequestType = $"{command.CommandName}.ReceivedRequest";
                            var receivedResponseType = $"{command.CommandName}.RawReceivedResponse";

                            partial.Line($@"
public class {command.CommandName}CommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<{command.FqnRequestType}>
{{
    private readonly Dictionary<long, CommandContext<{command.FqnRequestType}>> requestIdToRequest =
        new Dictionary<long, CommandContext<{command.FqnRequestType}>>();

    private readonly Dictionary<long, long> internalRequestIdToRequestId = new Dictionary<long, long>();

    public uint GetComponentId()
    {{
        return ComponentId;
    }}

    public uint GetCommandId()
    {{
        return {command.CommandIndex};
    }}

    public void RemoveMetaData(long internalRequestId)
    {{
        var requestId = internalRequestIdToRequestId[internalRequestId];
        internalRequestIdToRequestId.Remove(internalRequestId);
        requestIdToRequest.Remove(requestId);
    }}

    public void SetInternalRequestId(long internalRequestId, long requestId)
    {{
        internalRequestIdToRequestId.Add(internalRequestId, requestId);
    }}

    public void AddRequest(in CommandContext<{command.FqnRequestType}> context)
    {{
        requestIdToRequest[context.RequestId] = context;
    }}

    public CommandContext<{command.FqnRequestType}> GetPayload(long internalRequestId)
    {{
        var id = internalRequestIdToRequestId[internalRequestId];
        return requestIdToRequest[id];
    }}
}}
");
                        }
                    });
                });
            }).Format();
        }
    }
}
