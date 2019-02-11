using System.Collections.Generic;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class ComponentDefinitionRaw
    {
        [JsonProperty("identifier")] public Identifier Identifier;
        [JsonProperty("componentId")] public uint ComponentId;
        [JsonProperty("fieldDefinitions")] public List<Field> Fields;
        [JsonProperty("dataDefinition")] public UserType Data;

        [JsonProperty("eventDefinitions")] public List<EventDefinitionRaw> Events;
        [JsonProperty("commandDefinitions")] public List<CommandDefinitionRaw> Commands;

        public class WrappedType
        {
            [JsonProperty("type")] public UserType Type;
        }

        public class EventDefinitionRaw
        {
            [JsonProperty("identifier")] public Identifier Identifier;
            [JsonProperty("eventIndex")] public uint EventIndex;
            [JsonProperty("type")] public WrappedType Type;
        }

        public class CommandDefinitionRaw
        {
            [JsonProperty("identifier")] public Identifier Identifier;
            [JsonProperty("commandIndex")] public uint CommandIndex;
            [JsonProperty("requestType")] public WrappedType RequestType;
            [JsonProperty("responseType")] public WrappedType ResponseType;
        }
    }
}
