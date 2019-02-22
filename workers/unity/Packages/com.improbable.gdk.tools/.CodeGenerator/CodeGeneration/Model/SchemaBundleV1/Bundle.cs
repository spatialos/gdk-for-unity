using System.Collections.Generic;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class SchemaBundle
    {
        [JsonProperty("v1")] public Bundle BundleContents;
        [JsonProperty("sourceMapV1")] public SourceMap SourceMap;

        public static SchemaBundle FromJson(string json)
        {
            return JsonConvert.DeserializeObject<SchemaBundle>(json);
        }

        public class Bundle
        {
            [JsonProperty("enumDefinitions")] public List<EnumDefinitionRaw> EnumDefinitions;
            [JsonProperty("typeDefinitions")] public List<TypeDefinitionRaw> TypeDefinitions;
            [JsonProperty("componentDefinitions")] public List<ComponentDefinitionRaw> ComponentDefinitions;
        }
    }
}
