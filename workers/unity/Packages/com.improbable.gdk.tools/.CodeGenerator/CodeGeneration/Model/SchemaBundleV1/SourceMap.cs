using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class SourceMap
    {
        [JsonProperty("sourceReferences")] public Dictionary<string, SourceReference> SourceReferences;
    }

    public class SourceReference
    {
        [JsonProperty("filePath")] public string FilePath;
        [JsonProperty("line")] public uint Line;
        [JsonProperty("column")] public uint Column;
    }
}
