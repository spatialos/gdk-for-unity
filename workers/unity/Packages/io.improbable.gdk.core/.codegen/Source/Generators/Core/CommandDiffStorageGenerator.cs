using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommandDiffStorageGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails componentDetails)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Core.Commands"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        foreach (var command in componentDetails.CommandDetails)
                        {
                            partial.Text(GenerateCommandStorage(command, componentDetails.Namespace, componentDetails.Name));
                            partial.Text(GenerateCommandsToSendStorage(command, componentDetails.Namespace, componentDetails.Name));
                        }
                    });
                });
            });
        }

        private static Text GenerateCommandStorage(UnityCommandDetails command, string qualifiedNamespace, string componentName)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.PascalCaseName}CommandStorage class.");

            var receivedRequestType = $"{command.PascalCaseName}.ReceivedRequest";
            var receivedResponseType = $"{command.PascalCaseName}.ReceivedResponse";

            return Text.New($@"
private class Diff{command.PascalCaseName}CommandStorage
    : CommandDiffStorageBase<{receivedRequestType}, {receivedResponseType}>
{{
    public override uint ComponentId => {qualifiedNamespace}.{componentName}.ComponentId;
    public override uint CommandId => {command.CommandIndex};
}}
");
        }

        private static Text GenerateCommandsToSendStorage(UnityCommandDetails command, string qualifiedNamespace, string componentName)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.PascalCaseName}CommandsToSendStorage class.");

            return Text.New($@"
private class {command.PascalCaseName}CommandsToSendStorage :
    CommandSendStorage<{command.PascalCaseName}.Request, {command.PascalCaseName}.Response>,
    IComponentCommandSendStorage
{{
    uint IComponentCommandSendStorage.ComponentId => ComponentId;
    uint IComponentCommandSendStorage.CommandId => {command.CommandIndex};
}}
");
        }
    }
}
