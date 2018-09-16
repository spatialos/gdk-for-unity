using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    [Serializable]
    public class GdkToolsConfiguration
    {
        public string SchemaStdLibDir;
        public List<string> SchemaSourceDirs = new List<string>();
        public string CodegenOutputDir;

        private static string JsonFilePath = Path.GetFullPath("Assets/Config/GdkToolsConfiguration.json");

        private GdkToolsConfiguration()
        {
            ResetToDefault();
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(this, true);
            File.WriteAllText(JsonFilePath, json);
        }

        internal List<string> Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(SchemaStdLibDir))
            {
                errors.Add($"{GdkToolsConfigurationWindow.SchemaStdLibDirLabel} cannot be empty.");
            }

            if (string.IsNullOrEmpty(CodegenOutputDir))
            {
                errors.Add($"{GdkToolsConfigurationWindow.CodegenOutputDirLabel} cannot be empty.");
            }

            if (SchemaSourceDirs.Any(string.IsNullOrEmpty))
            {
                errors.Add($"Cannot have any empty entry in {GdkToolsConfigurationWindow.SchemaSourceDirsLabel}.");
            }

            if (SchemaSourceDirs.Count == 0)
            {
                errors.Add($"You must have at least one item in {GdkToolsConfigurationWindow.SchemaSourceDirsLabel}.");
            }

            return errors;
        }

        internal void ResetToDefault()
        {
            SchemaStdLibDir = DefaultValues.SchemaStdLibDir;
            CodegenOutputDir = DefaultValues.CodegenOutputDir;

            SchemaSourceDirs.Clear();
            SchemaSourceDirs.Add(DefaultValues.SchemaSourceDir);
        }

        public static GdkToolsConfiguration GetOrCreateInstance()
        {
            return File.Exists(JsonFilePath) ? LoadFromFile() : CreateInstance();
        }

        private static GdkToolsConfiguration LoadFromFile()
        {
            return JsonUtility.FromJson<GdkToolsConfiguration>(File.ReadAllText(JsonFilePath));
        }

        private static GdkToolsConfiguration CreateInstance()
        {
            var config = new GdkToolsConfiguration();

            File.WriteAllText(JsonFilePath, JsonUtility.ToJson(config, true));

            return config;
        }

        private static class DefaultValues
        {
            public const string SchemaStdLibDir = "../../build/dependencies/schema/standard_library";
            public const string CodegenOutputDir = "Assets/Generated/Source";
            public const string SchemaSourceDir = "../../schema";
        }
    }
}
