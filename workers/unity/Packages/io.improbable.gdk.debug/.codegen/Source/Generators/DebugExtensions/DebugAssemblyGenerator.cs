namespace Improbable.Gdk.CodeGenerator
{
    public static class DebugAssemblyGenerator
    {
        public static string Generate()
        {
            return @"{
    ""name"": ""Improbable.Gdk.Generated.Debug"",
    ""references"": [
        ""Improbable.Gdk.Generated"",
        ""Improbable.Gdk.Debug.WorkerInspector.Codegen"",
        ""Unity.Entities""
    ],
    ""includePlatforms"": [
        ""Editor""
    ],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": []
}
";
        }
    }
}
