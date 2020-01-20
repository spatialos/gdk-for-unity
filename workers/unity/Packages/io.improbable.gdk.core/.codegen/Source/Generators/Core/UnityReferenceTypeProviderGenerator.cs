using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityReferenceTypeProviderGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "System.Collections.Generic",
                    "System.Linq",
                    "Improbable.Gdk.Core"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.ComponentName}", partial =>
                    {
                        partial.Type("internal static class ReferenceTypeProviders", providers =>
                        {
                            foreach (var fieldDetails in componentDetails.FieldDetails.Where(fd => !fd.IsBlittable))
                            {
                                providers.Type(UnityReferenceTypeProviderContent.Generate(fieldDetails, qualifiedNamespace, componentDetails.ComponentName));
                            }
                        });
                    });
                });
            }).Format();
        }
    }
}
