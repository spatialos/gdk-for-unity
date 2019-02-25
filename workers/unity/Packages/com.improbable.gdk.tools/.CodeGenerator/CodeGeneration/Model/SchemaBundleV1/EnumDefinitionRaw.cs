using System.Collections.Generic;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class EnumDefinitionRaw
    {
        [JsonProperty("identifier")] public Identifier EnumIdentifier;
        [JsonProperty("valueDefinitions")] public List<Value> Values;
        [JsonProperty("annotations")] public List<AnnotationRaw> Annotations;

        public class Value
        {
            [JsonProperty("identifier")] public Identifier Identifier;
            [JsonProperty("value")] public uint EnumValue;
            [JsonProperty("annotations")] public List<AnnotationRaw> Annotations;
        }
    }
}
