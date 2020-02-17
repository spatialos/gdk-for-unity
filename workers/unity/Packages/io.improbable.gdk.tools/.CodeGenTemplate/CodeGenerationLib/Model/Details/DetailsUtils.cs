using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public static class DetailsUtils
    {
        public static string GetCapitalisedFqnTypename(string qualifiedTypeName)
        {
            return $"global::{Formatting.CapitaliseQualifiedNameParts(qualifiedTypeName)}";
        }
    }
}
