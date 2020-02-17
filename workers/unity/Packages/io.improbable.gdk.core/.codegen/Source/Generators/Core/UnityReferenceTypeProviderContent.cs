using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityReferenceTypeProviderContent
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static TypeBlock Generate(UnityFieldDetails fieldDetails, string qualifiedNamespace, string componentName)
        {
            var fieldName = fieldDetails.PascalCaseName;
            var typeName = fieldDetails.Type;

            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.ReferenceTypeProviders.{fieldName}Provider static class.");

            return Scope.Type($"public static class {fieldName}Provider", provider =>
            {
                provider.Line($@"
private static readonly Dictionary<uint, {typeName}> Storage = new Dictionary<uint, {typeName}>();
private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, global::Unity.Entities.World>();

private static uint nextHandle = 0;

public static uint Allocate(global::Unity.Entities.World world)
{{
    var handle = GetNextHandle();

    Storage.Add(handle, default({typeName}));
    WorldMapping.Add(handle, world);

    return handle;
}}

public static {typeName} Get(uint handle)
{{
    if (!Storage.TryGetValue(handle, out var value))
    {{
        throw new ArgumentException($""{fieldName}Provider does not contain handle {{handle}}"");
    }}

    return value;
}}

public static void Set(uint handle, {typeName} value)
{{
    if (!Storage.ContainsKey(handle))
    {{
        throw new ArgumentException($""{fieldName}Provider does not contain handle {{handle}}"");
    }}

    Storage[handle] = value;
}}

public static void Free(uint handle)
{{
    Storage.Remove(handle);
    WorldMapping.Remove(handle);
}}

public static void CleanDataInWorld(global::Unity.Entities.World world)
{{
    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();

    foreach (var handle in handles)
    {{
        Free(handle);
    }}
}}

private static uint GetNextHandle()
{{
    nextHandle++;

    while (Storage.ContainsKey(nextHandle))
    {{
        nextHandle++;
    }}

    return nextHandle;
}}
");
            });
        }
    }
}
