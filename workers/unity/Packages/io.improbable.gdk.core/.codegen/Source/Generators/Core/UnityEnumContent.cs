using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityEnumContent
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static EnumBlock Generate(UnityEnumDetails details, string enumNamespace)
        {
            var enumName = details.Name;

            Logger.Trace($"Generating {enumNamespace}.{enumName} enum.");

            return Scope.AnnotatedEnum("global::System.Serializable",
                $"public enum {enumName} : uint", e =>
                {
                    foreach (var (item1, item2) in details.Values)
                    {
                        e.Member($"{item2} = {item1}");
                    }
                });
        }
    }
}
