using System.Collections.Generic;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class AnnotationRaw
    {
        [JsonProperty("value")] public TypeValueData Value;
    }

    public class TypeValueData
    {
        [JsonProperty("type")] public FullyQualifiedReference Type;
        [JsonProperty("fields")] public List<FieldValue> Fields;

        public class FieldValue
        {
            [JsonProperty("field")] public FullyQualifiedReference Field;
            [JsonProperty("name")] public string Name;
            [JsonProperty("number")] public uint Number;
            [JsonProperty("value")] public Value Value;
        }
    }

    public class OptionValueData
    {
        [JsonProperty("value")] public Value Value;
    }

    public class ListValueData
    {
        [JsonProperty("values")] public List<Value> Values;
    }

    public class MapValueData
    {
        [JsonProperty("values")] public List<MapPair> Values;

        public class MapPair
        {
            [JsonProperty("value")] public Value Value;
            [JsonProperty("key ")] public Value Key;
        }
    }

    public class EnumDataValue
    {
        [JsonProperty("enum")] public FullyQualifiedReference EnumReference;
        [JsonProperty("enum_value")] public FullyQualifiedReference EnumValueReference;
        [JsonProperty("name")] public string Name;
        [JsonProperty("value")] public uint Value;
    }

    public class Value
    {
        [JsonProperty("option_value")] public OptionValueData OptionValue;
        [JsonProperty("list_value")] public ListValueData ListValue;
        [JsonProperty("map_value")] public MapValueData MapValue;

        [JsonProperty("enum_value")] public EnumDataValue EnumValue;
        [JsonProperty("type_value")] public TypeValueData TypeValue;

        [JsonProperty("bool_value")] public bool BoolValue;
        [JsonProperty("uint32_value")] public uint Uint32Value;
        [JsonProperty("uint64_value")] public ulong Uint64Value;
        [JsonProperty("int32_value")] public int Int32Value;
        [JsonProperty("int64_value")] public long Int64Value;
        [JsonProperty("float_value")] public float FloatValue;
        [JsonProperty("double_value")] public double DoubleValue;
        [JsonProperty("string_value")] public string StringValue;
        [JsonProperty("bytes_value")] public byte[] BytesValue;
        [JsonProperty("entity_id_value")] public long EntityIdValue;
    }
}
