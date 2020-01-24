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
                    "Improbable.Gdk.Core"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.ComponentName}", partial =>
                    {
                        foreach (var command in componentDetails.CommandDetails)
                        {
                            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.ComponentName}.{command.CommandName}CommandMetaDataStorage class.");

                            partial.Line($@"
public class {command.CommandName}CommandMetaDataStorage :
    CommandPayloadStorage<{command.FqnRequestType}>,
    ICommandMetaDataStorage
{{
    public uint CommandId => {command.CommandIndex};
}}
");
                        }
                    });
                });
            }).Format();
        }
    }
}
