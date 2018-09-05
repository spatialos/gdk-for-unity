using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public class ScriptableGdkToolsConfiguration : ScriptableObject
    {
        public string SchemaStdLibDir;
        public List<string> SchemaSourceDirs = new List<string>();
        public string CodegenOutputDir;

        private const string AssetPath = "Assets/Config/GdkToolsConfiguration.asset";

        public ScriptableGdkToolsConfiguration()
        {
            ResetToDefault();
        }

        internal List<string> Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(SchemaStdLibDir))
            {
                errors.Add($"{GdkToolsConfigurationInspector.SchemaStdLibDirLabel} cannot be empty.");
            }

            if (string.IsNullOrEmpty(CodegenOutputDir))
            {
                errors.Add($"{GdkToolsConfigurationInspector.CodegenOutputDirLabel} cannot be empty.");
            }

            if (SchemaSourceDirs.Any(string.IsNullOrEmpty))
            {
                errors.Add($"Cannot have any empty entry in {GdkToolsConfigurationInspector.SchemaSourceDirsLabel}.");
            }

            if (SchemaSourceDirs.Count == 0)
            {
                errors.Add($"You must have at least one item in {GdkToolsConfigurationInspector.SchemaSourceDirsLabel}.");
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

        public static ScriptableGdkToolsConfiguration GetOrCreateInstance()
        {
            return AssetDatabase.LoadAssetAtPath<ScriptableGdkToolsConfiguration>(AssetPath) ?? CreateInstance();
        }

        private static ScriptableGdkToolsConfiguration CreateInstance()
        {
            var config = CreateInstance<ScriptableGdkToolsConfiguration>();
            CreateFoldersFromAssetPath(AssetPath);
            AssetDatabase.CreateAsset(config, AssetPath);
            EditorUtility.SetDirty(config);

            return config;
        }

        private static void CreateFoldersFromAssetPath(string path)
        {
            var fullDir = Path.GetDirectoryName(Path.GetFullPath(path));

            if (Directory.Exists(fullDir))
            {
                return;
            }

            Directory.CreateDirectory(fullDir);

            AssetDatabase.ImportAsset(Path.GetDirectoryName(path));
        }

        private static class DefaultValues
        {
            public const string SchemaStdLibDir = "../../build/dependencies/schema/standard_library";
            public const string CodegenOutputDir = "Assets/Generated/Source";
            public const string SchemaSourceDir = "../../schema";
        }
    }
}
