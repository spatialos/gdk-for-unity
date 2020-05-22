using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommandMetaDataStorageGenerator
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
                            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.{command.PascalCaseName}CommandMetaDataStorage class.");

                            partial.Line($@"
private class {command.PascalCaseName}CommandMetaDataStorage :
    CommandPayloadStorage<{command.FqnRequestType}>,
    ICommandMetaDataStorage
{{
    public uint CommandId => {command.CommandIndex};
}}
");
                        }
                    });
                });
            });
        }
    }
}
