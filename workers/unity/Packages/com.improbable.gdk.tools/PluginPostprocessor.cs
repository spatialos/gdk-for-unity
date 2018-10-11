using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    internal class PluginPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (importedAssets.Any(DownloadCoreSdk.IsImprobablePlugin))
            {
                SetPluginsCompatibility();
            }
        }

        private static readonly List<PluginDirectoryCompatibility> PluginsCompatibilityList = new List<PluginDirectoryCompatibility>
        {
            PluginDirectoryCompatibility.CreateWithCompatiblePlatforms("Assets/Plugins/Improbable/Core/OSX", new List<BuildTarget> { BuildTarget.StandaloneOSX }, editorCompatible: true),
            PluginDirectoryCompatibility.CreateWithCompatiblePlatforms("Assets/Plugins/Improbable/Core/Linux", new List<BuildTarget> { BuildTarget.StandaloneLinux64 }, editorCompatible: true),
            PluginDirectoryCompatibility.CreateWithCompatiblePlatforms("Assets/Plugins/Improbable/Core/Windows/x86_64", new List<BuildTarget> { BuildTarget.StandaloneWindows64 }, editorCompatible: true),
            PluginDirectoryCompatibility.CreateWithCompatiblePlatforms("Assets/Plugins/Improbable/Core/Windows/x86", new List<BuildTarget> { BuildTarget.StandaloneWindows }, editorCompatible: false),
            PluginDirectoryCompatibility.CreateWithCompatiblePlatforms("Assets/Plugins/Improbable/Core/Android/arm64", new List<BuildTarget> { BuildTarget.Android }, editorCompatible: false, extraPlatformData: new List<PluginPlatformData> { new PluginPlatformData(BuildTarget.Android, "CPU", "ARM64") }),
            PluginDirectoryCompatibility.CreateWithCompatiblePlatforms("Assets/Plugins/Improbable/Core/Android/armeabi-v7a", new List<BuildTarget> { BuildTarget.Android }, editorCompatible: false, extraPlatformData: new List<PluginPlatformData> { new PluginPlatformData(BuildTarget.Android, "CPU", "ARMv7") }),
            PluginDirectoryCompatibility.CreateWithCompatiblePlatforms("Assets/Plugins/Improbable/Core/Android/x86", new List<BuildTarget> { BuildTarget.Android }, editorCompatible: false, extraPlatformData: new List<PluginPlatformData> { new PluginPlatformData(BuildTarget.Android, "CPU", "x86") }),
            PluginDirectoryCompatibility.CreateWithCompatiblePlatforms("Assets/Plugins/Improbable/Core/iOS", new List<BuildTarget> { BuildTarget.iOS }, editorCompatible: false),
            PluginDirectoryCompatibility.CreateWithIncompatiblePlatforms("Assets/Plugins/Improbable/Sdk/Common", new List<BuildTarget> { BuildTarget.iOS }, editorCompatible: true),
            PluginDirectoryCompatibility.CreateWithCompatiblePlatforms("Assets/Plugins/Improbable/Sdk/iOS", new List<BuildTarget> { BuildTarget.iOS }, editorCompatible: false),
        };

        /// <summary>
        ///     Sets plugin platform compatibility based on directory structure
        /// </summary>
        internal static void SetPluginsCompatibility()
        {
            AssetDatabase.StartAssetEditing();
            foreach (var pluginDirectoryCompatibility in PluginsCompatibilityList)
            {
                if (!Directory.Exists(pluginDirectoryCompatibility.Path))
                {
                    continue;
                }

                var pluginPaths = AssetDatabase.FindAssets("", new[] { pluginDirectoryCompatibility.Path });
                foreach (var pluginPath in pluginPaths)
                {
                    var plugin = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(pluginPath)) as PluginImporter;
                    if (plugin == null)
                    {
                        continue;
                    }

                    // We only update options that needs to be updated to avoid reloading plugins that have correct settings
                    bool pluginCompatibilityUpdated = false;
                    if (plugin.GetCompatibleWithAnyPlatform() != pluginDirectoryCompatibility.AnyPlatformCompatible)
                    {
                        plugin.SetCompatibleWithAnyPlatform(pluginDirectoryCompatibility.AnyPlatformCompatible);
                        pluginCompatibilityUpdated = true;
                    }

                    if (plugin.GetCompatibleWithEditor() != pluginDirectoryCompatibility.EditorCompatible)
                    {
                        plugin.SetCompatibleWithEditor(pluginDirectoryCompatibility.EditorCompatible);
                        pluginCompatibilityUpdated = true;
                    }

                    if (pluginDirectoryCompatibility.AnyPlatformCompatible)
                    {
                        foreach (var buildTarget in pluginDirectoryCompatibility.IncompatiblePlatforms)
                        {
                            if (!plugin.GetExcludeFromAnyPlatform(buildTarget))
                            {
                                plugin.SetExcludeFromAnyPlatform(buildTarget, true);
                                pluginCompatibilityUpdated = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var buildTarget in pluginDirectoryCompatibility.CompatiblePlatforms)
                        {
                            if (!plugin.GetCompatibleWithPlatform(buildTarget))
                            {
                                plugin.SetCompatibleWithPlatform(buildTarget, true);
                                pluginCompatibilityUpdated = true;
                            }
                        }
                    }

                    foreach (var pluginPlaformData in pluginDirectoryCompatibility.ExtraPlatformData)
                    {
                        if (plugin.GetPlatformData(pluginPlaformData.Platform, pluginPlaformData.Key) != pluginPlaformData.Value)
                        {
                            plugin.SetPlatformData(pluginPlaformData.Platform, pluginPlaformData.Key, pluginPlaformData.Value);
                            pluginCompatibilityUpdated = true;
                        }
                    }

                    if (pluginCompatibilityUpdated)
                    {
                        plugin.SaveAndReimport();
                    }
                }
            }

            AssetDatabase.StopAssetEditing();
        }

        private class PluginPlatformData
        {
            public PluginPlatformData(BuildTarget platform, string key, string value)
            {
                Platform = platform;
                Key = key;
                Value = value;
            }

            public BuildTarget Platform { get; }
            public string Key { get; }
            public string Value { get; }
        }

        private class PluginDirectoryCompatibility
        {
            public static PluginDirectoryCompatibility CreateWithCompatiblePlatforms(string path,
                List<BuildTarget> compatiblePlatforms,
                bool editorCompatible,
                List<PluginPlatformData> extraPlatformData = null)
            {
                return new PluginDirectoryCompatibility(path, false, compatiblePlatforms, null, editorCompatible, extraPlatformData);
            }

            public static PluginDirectoryCompatibility CreateWithIncompatiblePlatforms(string path,
                List<BuildTarget> incompatiblePlatforms,
                bool editorCompatible)
            {
                return new PluginDirectoryCompatibility(path, true, null, incompatiblePlatforms, editorCompatible, null);
            }

            public static PluginDirectoryCompatibility CreateAllCompatible(string path)
            {
                return new PluginDirectoryCompatibility(path, true, null, null, true, null);
            }

            private PluginDirectoryCompatibility(string path,
                bool anyPlatformCompatible,
                List<BuildTarget> compatiblePlatforms,
                List<BuildTarget> incompatiblePlatforms,
                bool editorCompatible,
                List<PluginPlatformData> extraPlatformData)
            {
                Path = path;
                AnyPlatformCompatible = anyPlatformCompatible;
                CompatiblePlatforms = compatiblePlatforms ?? new List<BuildTarget>();
                IncompatiblePlatforms = incompatiblePlatforms ?? new List<BuildTarget>();
                EditorCompatible = editorCompatible;
                ExtraPlatformData = extraPlatformData ?? new List<PluginPlatformData>();
            }

            public string Path { get; }
            public bool AnyPlatformCompatible { get; }
            public List<BuildTarget> CompatiblePlatforms { get; }
            public List<BuildTarget> IncompatiblePlatforms { get; }
            public bool EditorCompatible { get; }
            public List<PluginPlatformData> ExtraPlatformData { get; }
        }
    }
}
