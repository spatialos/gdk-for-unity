using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public class ScriptableGdkToolsConfiguration : ScriptableObject
    {
        public string SchemaStdLibDir = DefaultValues.SchemaStdLibDir;
        public List<string> SchemaSourceDirs = new List<string> { DefaultValues.SchemaSourceDir };
        public string CodegenOutputDir = DefaultValues.CodegenOutputDir;

        public const string AssetPath = "Assets/Config/GdkToolsConfiguration.asset";

        private const string DownloadCoreSdkLabel = "Core SDK Options";
        private const string SchemaStdLibDirLabel = "Schema Standard Library Directory";

        private const string CodeGeneratorLabel = "Code Generator Options";
        private const string CodegenOutputDirLabel = "Code Generator Output Directory";
        private const string SchemaSourceDirsLabel = "Schema Source Directories";

        private const string AddSchemaDirButtonText = "Add Schema Source Directory";
        private const string RemoveSchemaDirButtonText = "Remove";

        private const string SaveConfigurationButtonText = "Save GDK Tools Configuration";
        private const string ResetConfigurationButtonText = "Reset GDK Tools Configuration to Default";

        private Vector2 scrollPosition = Vector2.zero;

        public void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            GUILayout.Label(DownloadCoreSdkLabel);
            GUILayout.Space(5);
            GUILayout.Label(SchemaStdLibDirLabel);
            SchemaStdLibDir = GUILayout.TextField(SchemaStdLibDir);

            GUILayout.Space(10);
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);

            GUILayout.Label(CodeGeneratorLabel);
            GUILayout.Space(5);
            GUILayout.Label(CodegenOutputDirLabel);
            CodegenOutputDir = GUILayout.TextField(CodegenOutputDir);

            GUILayout.Space(5);
            GUILayout.Label(SchemaSourceDirsLabel);
            for (var i = 0; i < SchemaSourceDirs.Count; i++)
            {
                GUILayout.BeginHorizontal();
                SchemaSourceDirs[i] = GUILayout.TextField(SchemaSourceDirs[i]);

                if (GUILayout.Button(RemoveSchemaDirButtonText, GUILayout.Width(100)))
                {
                    SchemaSourceDirs.RemoveAt(i);
                }

                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button(AddSchemaDirButtonText, GUILayout.Width(250)))
            {
                SchemaSourceDirs.Add("");
            }

            GUILayout.Space(10);
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);

            if (GUILayout.Button(SaveConfigurationButtonText, GUILayout.Width(250)))
            {
                Save();
            }

            GUILayout.Space(15);

            if (GUILayout.Button(ResetConfigurationButtonText, GUILayout.Width(250)))
            {
                ResetToDefault();
            }

            GUILayout.EndScrollView();
        }

        private void Save()
        {
            Validate();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(SchemaStdLibDir))
            {
                errors.Add($"{SchemaStdLibDirLabel} cannot be empty!");
            }

            if (string.IsNullOrEmpty(CodegenOutputDir))
            {
                errors.Add($"{CodegenOutputDirLabel} cannot be empty!");
            }

            if (SchemaSourceDirs.Any(dir => string.IsNullOrEmpty(dir)))
            {
                errors.Add($"Cannot have any empty entry in {SchemaSourceDirsLabel}");
            }

            if (SchemaSourceDirs.Count == 0)
            {
                errors.Add($"Cannot have no entrys in {SchemaSourceDirsLabel}");
            }

            if (errors.Count != 0)
            {
                throw new InvalidOperationException("GDK Tools Configuration Validation failed with the following error(s): " + string.Join("\n", errors));
            }
        }

        private void ResetToDefault()
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
            var folders = path.Split('/');

            var prevPath = "";
            // Ignore last element as it is the asset name.
            for (var i = 0; i < folders.Length - 1; i++)
            {
                var testPath = prevPath + (i == 0 ? "" : "/") + folders[i];
                if (!AssetDatabase.IsValidFolder(testPath))
                {
                    Debug.Log($"Creating folder at: {prevPath} {folders[i]}");
                    AssetDatabase.CreateFolder(prevPath, folders[i]);
                }

                prevPath = testPath;
            }
        }

        private static class DefaultValues
        {
            public const string SchemaStdLibDir = "../../build/dependencies/schema/standard_library";
            public const string CodegenOutputDir = "Assets/Generated/Source";
            public const string SchemaSourceDir = "../../schema";
        }
    }
}
