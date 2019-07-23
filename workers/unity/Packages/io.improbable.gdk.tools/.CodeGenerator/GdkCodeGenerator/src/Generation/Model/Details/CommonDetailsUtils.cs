using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommonDetailsUtils
    {
        public static string GetCapitalisedFqnTypename(string qualifiedTypeName)
        {
            return $"global::{Formatting.CapitaliseQualifiedNameParts(qualifiedTypeName)}";
        }
    }
}
