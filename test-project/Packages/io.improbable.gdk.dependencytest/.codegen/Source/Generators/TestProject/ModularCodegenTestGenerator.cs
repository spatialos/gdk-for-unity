using Improbable.Gdk.CodeGeneration.CodeWriter;

namespace Improbable.Gdk.CodeGenerator
{
    public static class ModularCodegenTestGenerator
    {
        public static CodeWriter Generate()
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.Namespace("Improbable.Gdk.ModularCodegenTests", ns =>
                {
                    ns.Type("public class TemplateTest", t =>
                    {
                    });
                });
            });
        }
    }
}
