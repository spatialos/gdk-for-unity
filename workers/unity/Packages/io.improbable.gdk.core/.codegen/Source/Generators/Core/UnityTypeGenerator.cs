using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityTypeGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate(UnityTypeDetails details, string package)
        {
            var qualifiedNamespace = package;

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System.Linq",
                    "Improbable.Gdk.Core",
                    "UnityEngine"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type(UnityTypeContent.Generate(details, qualifiedNamespace));
                });
            }).Format();
        }
    }
}
