namespace Improbable.Gdk.CodeGenerator
{
    public static class BuildSystemAssemblyGenerator
    {
        public static string Generate()
        {
            return @"{
    ""name"": ""Improbable.Gdk.Generated.BuildSystem"",
    ""references"": [
        ""Improbable.Gdk.BuildSystem"",
        ""Improbable.Gdk.Tools""
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
