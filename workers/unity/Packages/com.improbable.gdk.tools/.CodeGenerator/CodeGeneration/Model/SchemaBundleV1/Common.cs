using System.Collections.Generic;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class Identifier
    {
        [JsonProperty("qualifiedName")] public string QualifiedName;
        [JsonProperty("name")] public string Name;
        [JsonProperty("path")] public List<string> Path;
    }

    public class UserType
    {
        [JsonProperty("qualifiedName")] public string QualifiedName;
    }
}
