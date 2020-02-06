using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityEnumGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityEnumDetails details)
        {
            Logger.Trace($"Generating code for {details.QualifiedName}.");

            return CodeWriter.Populate(cgw =>
            {
                cgw.Namespace(details.Namespace, ns =>
                {
                    ns.Enum(UnityEnumContent.Generate(details, details.Namespace));
                });
            });
        }
    }
}
