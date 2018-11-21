using System.Collections.Generic;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class TypeDefinitionRaw
    {
        [JsonProperty("identifier")] public Identifier Identifier;
        [JsonProperty("fieldDefinitions")] public List<Field> Fields;

        public class Field
        {
            [JsonProperty("identifier")] public Identifier Identifier;
            [JsonProperty("fieldId")] public uint FieldId;
            [JsonProperty("transient")] public bool IsTransient;
            [JsonProperty("listType")] public ListType List;
            [JsonProperty("mapType")] public MapType Map;
            [JsonProperty("optionType")] public OptionType Option;
            [JsonProperty("singularType")] public SingularType Singular;

            public class ListType
            {
                [JsonProperty("innerType")] public InnerType InnerType;
            }

            public class MapType
            {
                [JsonProperty("keyType")] public InnerType KeyType;
                [JsonProperty("valueType")] public InnerType ValueType;
            }

            public class OptionType
            {
                [JsonProperty("innerType")] public InnerType InnerType;
            }

            public class SingularType
            {
                [JsonProperty("type")] public InnerType Type;
            }

            // Note: Only one of these fields should ever be non-null.
            // The type is either a primitive or its a user defined type.
            public class InnerType
            {
                [JsonProperty("type")] public UserType UserType;
                [JsonProperty("primitive")] public string Primitive;
            }
        }
    }
}
