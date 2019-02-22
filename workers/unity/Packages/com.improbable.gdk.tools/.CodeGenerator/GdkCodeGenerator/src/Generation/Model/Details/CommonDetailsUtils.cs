using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
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

        public static Identifier CreateIdentifier(string qualifiedName)
        {
            return new Identifier
            {
                QualifiedName = qualifiedName,
                Name = qualifiedName.Split(".").Last(),
                Path = qualifiedName.Split(".").ToList()
            };
        }
    }
}
