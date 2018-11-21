using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class SchemaBundle
    {
        [JsonProperty("v1")] public Bundle BundleContents;

        public static Bundle FromJson(string json)
        {
            var schemaBundle = JsonConvert.DeserializeObject<SchemaBundle>(json);

            return schemaBundle.BundleContents;
        }

        public class Bundle
        {
            [JsonProperty("enumDefinitions")] public List<EnumDefinitionRaw> EnumDefinitions;
            [JsonProperty("typeDefinitions")] public List<TypeDefinitionRaw> TypeDefinitions;
            [JsonProperty("componentDefinitions")] public List<ComponentDefinitionRaw> ComponentDefinitions;

            public TypeDefinitionRaw GetSchemaType(string qualifiedName)
            {
                return TypeDefinitions.First(def => def.Identifier.QualifiedName == qualifiedName);
            }
        }
    }
}
