using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityEnumGenerator
    {
        public static string Generate(UnityEnumDetails details, string package)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.Namespace(package, ns =>
                {
                    ns.Line(UnityEnumContent.Generate(details, package).Format());
                });
            }).Format();
        }
    }
}
