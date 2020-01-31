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
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Core.Commands"
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
public class Diff{command.CommandName}CommandStorage
    : DiffSpawnCubeCommandStorage<{receivedRequestType}, {receivedResponseType}>
{{
    public override uint ComponentId => ComponentId;
    public override uint CommandId => {command.CommandIndex};
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
