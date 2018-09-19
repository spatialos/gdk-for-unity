using Improbable.CodeGeneration.Model;
using Improbable.CodeGeneration.Utils;

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

        public static string GetFqnTypeFromTypeReference(TypeReferenceRaw typeReferenceRaw)
        {
            return typeReferenceRaw.IsBuiltInType
                ? UnityTypeMappings.BuiltInSchemaTypeToUnityNativeType[typeReferenceRaw.TypeName]
                : GetCapitalisedFqnTypename(typeReferenceRaw.TypeName);
        }
    }
}
