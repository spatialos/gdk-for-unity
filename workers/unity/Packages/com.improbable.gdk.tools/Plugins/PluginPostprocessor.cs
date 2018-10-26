using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    internal class PluginPostprocessor : AssetPostprocessor
    {
        private static readonly List<PluginDirectoryCompatibility> PluginsCompatibilityList = new List<PluginDirectoryCompatibility>
        {
            new PluginDirectoryCompatibility(
                PluginType.Core,
                BuildTarget.StandaloneOSX
                ),
            new PluginDirectoryCompatibility(
                PluginType.Core,
                BuildTarget.StandaloneLinux64
                ),
            new PluginDirectoryCompatibility(
                PluginType.Core,
                BuildTarget.StandaloneWindows64,
                CPUType.X86_64
                ),
            new PluginDirectoryCompatibility(
                PluginType.Core,
                BuildTarget.StandaloneWindows,
                CPUType.X86
                ),
            new PluginDirectoryCompatibility(
                PluginType.Core,
                BuildTarget.Android,
                CPUType.Arm64
                ),
            new PluginDirectoryCompatibility(
                PluginType.Core,
                BuildTarget.Android,
                CPUType.ARMv7
                ),
            new PluginDirectoryCompatibility(
                PluginType.Core,
                BuildTarget.Android,
                CPUType.X86
                ),
            new PluginDirectoryCompatibility(
                PluginType.Core,
                BuildTarget.iOS
                ),
            new PluginDirectoryCompatibility(
                PluginType.Sdk,
                new List<BuildTarget> { BuildTarget.iOS }
                ),
            new PluginDirectoryCompatibility(
                PluginType.Sdk,
                BuildTarget.iOS,
                CPUType.Unused,
                false
                ),
        };

        internal static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (importedAssets.Any(DownloadCoreSdk.IsImprobablePlugin))
            {
                SetPluginsCompatibility();
            }
        }

        /// <summary>
        ///     Sets plugin platform compatibility based on directory structure
        /// </summary>
        internal static void SetPluginsCompatibility()
        {
            AssetDatabase.StartAssetEditing();
            foreach (var pluginDirectoryCompatibility in PluginsCompatibilityList)
            {
                if (!Directory.Exists(pluginDirectoryCompatibility.PluginPath))
                {
                    continue;
                }

                var pluginGuids = AssetDatabase.FindAssets(string.Empty, new[] { pluginDirectoryCompatibility.PluginPath });
                foreach (var pluginGuid in pluginGuids)
                {
                    var pluginImporter = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(pluginGuid)) as PluginImporter;
                    if (pluginImporter == null)
                    {
                        continue;
                    }

                    var needsReimport = false;
                    if (pluginDirectoryCompatibility.CompatibleWithAnyPlatform != pluginImporter.GetCompatibleWithAnyPlatform())
                    {
                        pluginImporter.SetCompatibleWithAnyPlatform(pluginDirectoryCompatibility.CompatibleWithAnyPlatform);
                        needsReimport = true;
                    }

                    if (pluginDirectoryCompatibility.CompatibleWithEditor != pluginImporter.GetCompatibleWithEditor())
                    {
                        pluginImporter.SetCompatibleWithEditor(pluginDirectoryCompatibility.CompatibleWithEditor);
                        needsReimport = true;
                    }

                    if (pluginDirectoryCompatibility.CompatiblePlatform != 0 && !pluginImporter.GetCompatibleWithPlatform(pluginDirectoryCompatibility.CompatiblePlatform))
                    {
                        pluginImporter.SetCompatibleWithPlatform(pluginDirectoryCompatibility.CompatiblePlatform, true);
                        if (!string.IsNullOrEmpty(pluginDirectoryCompatibility.CPU))
                        {
                            pluginImporter.SetPlatformData(pluginDirectoryCompatibility.CompatiblePlatform, "CPU", pluginDirectoryCompatibility.CPU);
                        }
                        needsReimport = true;
                    }

                    foreach (var incompatiblePlatform in pluginDirectoryCompatibility.IncompatiblePlatforms)
                    {
                        if (!pluginImporter.GetExcludeFromAnyPlatform(incompatiblePlatform))
                        {
                            pluginImporter.SetExcludeFromAnyPlatform(incompatiblePlatform, true);
                            needsReimport = true;
                        }
                    }

                    if (needsReimport)
                    {
                        pluginImporter.SaveAndReimport();
                    }
                }
            }

            AssetDatabase.StopAssetEditing();
        }
    }
}
