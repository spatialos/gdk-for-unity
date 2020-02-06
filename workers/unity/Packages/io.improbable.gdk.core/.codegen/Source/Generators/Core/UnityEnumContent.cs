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
            Logger.Trace($"Generating {enumNamespace}.{details.Name} enum.");

            return Scope.AnnotatedEnum("global::System.Serializable",
                $"public enum {details.Name} : uint", e =>
                {
                    foreach (var (item1, item2) in details.Values)
                    {
                        e.Member($"{item2} = {item1}");
                    }
                });
        }
    }
}
