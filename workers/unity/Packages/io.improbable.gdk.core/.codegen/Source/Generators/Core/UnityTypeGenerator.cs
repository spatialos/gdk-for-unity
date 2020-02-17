using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityTypeGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityTypeDetails details)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System.Linq",
                    "Improbable.Gdk.Core",
                    "UnityEngine"
                );

                cgw.Namespace(details.Namespace, ns =>
                {
                    ns.Type(UnityTypeContent.Generate(details, details.Namespace));
                });
            });
        }
    }
}
