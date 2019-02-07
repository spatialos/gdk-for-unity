using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommonDetailsUtils
    {
        public static string GetCapitalisedFqnTypename(string qualifiedTypeName)
        {
            if (UnityTypeMappings.BuiltInSchemaTypeToUnityNativeType.TryGetValue(qualifiedTypeName, out var fqn))
            {
                return fqn;
            }

            return
                $"global::{Formatting.CapitaliseQualifiedNameParts(qualifiedTypeName)}";
        }
    }
}
