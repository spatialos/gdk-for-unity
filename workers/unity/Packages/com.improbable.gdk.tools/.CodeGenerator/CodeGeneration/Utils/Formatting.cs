using System;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.Utils
{
    public static class Formatting
    {
        public static string PascalCaseToCamelCase(string pascalCase)
        {
            switch (pascalCase.Length)
            {
                case 0:
                    return "";
                case 1:
                    return char.ToLowerInvariant(pascalCase[0]).ToString();
                default:
                    return char.ToLowerInvariant(pascalCase[0]) + pascalCase.Substring(1);
            }
        }

        public static string QualifiedNameToPascalCase(string qualifiedName)
        {
            var parts = qualifiedName.Split('.', StringSplitOptions.RemoveEmptyEntries)
                .Select(SnakeCaseToPascalCase);

            return string.Join(".", parts);
        }

        public static string SnakeCaseToPascalCase(string snakeCase)
        {
            return snakeCase.Split('_', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        public static string FullyQualify(string qualifiedName)
        {
            return "global::" + QualifiedNameToPascalCase(qualifiedName);
        }
    }
}
