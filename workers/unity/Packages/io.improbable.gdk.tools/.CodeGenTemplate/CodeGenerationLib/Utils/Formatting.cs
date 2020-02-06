using System;
using System.IO;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.Utils
{
    public static class Formatting
    {
        public static string CapitaliseQualifiedNameParts(string text)
        {
            var parts = text.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                    SnakeCaseToPascalCase(char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)))
                .ToArray();
            return string.Join(".", parts);
        }

        public static string SnakeCaseToPascalCase(string text)
        {
            return text.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        public static string GetNamespacePath(string packageName)
        {
            var capitalisedPackageName = CapitaliseQualifiedNameParts(packageName);
            var packageParts = capitalisedPackageName.Split('.').Select(p => p.ToLowerInvariant()).ToList();

            var outputPath = string.Empty;
            foreach (var segment in packageParts)
            {
                outputPath = Path.Combine(outputPath, segment);
            }

            return outputPath;
        }

        public static string PascalCaseToCamelCase(string pascalCase)
        {
            if (string.IsNullOrEmpty(pascalCase))
            {
                return pascalCase;
            }

            if (pascalCase.Length > 1)
            {
                return char.ToLowerInvariant(pascalCase[0]) + pascalCase.Substring(1);
            }

            return pascalCase.ToLowerInvariant();
        }
    }
}
