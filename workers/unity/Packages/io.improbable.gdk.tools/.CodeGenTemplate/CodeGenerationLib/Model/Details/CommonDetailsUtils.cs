using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public static class CommonDetailsUtils
    {
        public static string GetCapitalisedFqnTypename(string qualifiedTypeName)
        {
            return $"global::{Formatting.CapitaliseQualifiedNameParts(qualifiedTypeName)}";
        }

        public static string WriteCheckIsCleared(uint fieldNumber)
        {
            return $"var isCleared = updateObj.IsFieldCleared({fieldNumber});";
        }
    }
}
