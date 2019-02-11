using System.Collections.Generic;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class TypeDefinitionRaw
    {
        [JsonProperty("identifier")] public Identifier Identifier;
        [JsonProperty("fieldDefinitions")] public List<Field> Fields;
        [JsonProperty("annotations")] public List<AnnotationRaw> Annotations;
    }
}
