using System.Collections.Generic;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class Identifier
    {
        [JsonProperty("qualifiedName")] public string QualifiedName;
        [JsonProperty("name")] public string Name;
        [JsonProperty("path")] public List<string> Path;

        public (string PascalCaseName, string CamelCaseName, string FullyQualifiedName) GetNameSet()
        {
            var pascalCaseName = Utils.Formatting.SnakeCaseToPascalCase(Name);
            var camelCaseName = Utils.Formatting.PascalCaseToCamelCase(pascalCaseName);
            var fullyQualifiedName = Utils.Formatting.FullyQualify(QualifiedName);

            return (pascalCaseName, camelCaseName, fullyQualifiedName);
        }
    }

    public class UserType
    {
        [JsonProperty("qualifiedName")] public string QualifiedName;
    }
}
