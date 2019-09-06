using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Improbable.Gdk.CodeGenerator
{
    public static class PartialDatabase
    {
        public static bool TryGetPartial(string typeName, out string content)
        {
            var resourceName = "GdkCodeGenerator.Partials." + typeName;
            content = null;

            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames();
            if (resources.Contains(resourceName))
            {
                var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName));
                content = AdjustLineEndings(reader.ReadToEnd());
                return true;
            }

            return false;
        }

        private static string AdjustLineEndings(string content)
        {
            if (content.Contains("\r\n"))
            {
                return content.Replace("\r\n", Environment.NewLine);
            }

            return content.Replace("\n", Environment.NewLine);
        }
    }
}
