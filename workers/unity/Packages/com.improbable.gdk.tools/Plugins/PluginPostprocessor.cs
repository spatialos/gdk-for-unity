using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    internal class PluginPostprocessor : AssetPostprocessor
    {
        private static readonly List<PluginCompatibilitySetting> PluginCompatibilitySettings = new List<PluginCompatibilitySetting>
        {
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.StandaloneOSX
                ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.StandaloneLinux64
                ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.StandaloneWindows64,
                CPUType.X86_64
                ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.StandaloneWindows,
                CPUType.X86
                ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.Android,
                CPUType.Arm64
                ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.Android,
                CPUType.ARMv7
                ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.Android,
                CPUType.X86
                ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.iOS
                ),
            new PluginCompatibilitySetting(
                PluginType.Sdk,
                new List<BuildTarget> { BuildTarget.iOS }
                ),
            new PluginCompatibilitySetting(
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
            foreach (var pluginCompatibilitySetting in PluginCompatibilitySettings)
            {
                if (!Directory.Exists(pluginCompatibilitySetting.PluginPath))
                {
                    continue;
                }

                var pluginGuids = AssetDatabase.FindAssets(string.Empty, new[] { pluginCompatibilitySetting.PluginPath });
                foreach (var pluginGuid in pluginGuids)
                {
                    var pluginImporter = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(pluginGuid)) as PluginImporter;
                    if (pluginImporter == null)
                    {
                        continue;
                    }

                    var needsReimport = false;
                    if (pluginCompatibilitySetting.CompatibleWithAnyPlatform != pluginImporter.GetCompatibleWithAnyPlatform())
                    {
                        pluginImporter.SetCompatibleWithAnyPlatform(pluginCompatibilitySetting.CompatibleWithAnyPlatform);
                        needsReimport = true;
                    }

                    if (pluginCompatibilitySetting.CompatibleWithEditor != pluginImporter.GetCompatibleWithEditor())
                    {
                        pluginImporter.SetCompatibleWithEditor(pluginCompatibilitySetting.CompatibleWithEditor);
                        needsReimport = true;
                    }

                    if (pluginCompatibilitySetting.CompatiblePlatform != 0 && !pluginImporter.GetCompatibleWithPlatform(pluginCompatibilitySetting.CompatiblePlatform))
                    {
                        pluginImporter.SetCompatibleWithPlatform(pluginCompatibilitySetting.CompatiblePlatform, true);
                        if (!string.IsNullOrEmpty(pluginCompatibilitySetting.CPU))
                        {
                            pluginImporter.SetPlatformData(pluginCompatibilitySetting.CompatiblePlatform, "CPU", pluginCompatibilitySetting.CPU);
                        }
                        needsReimport = true;
                    }

                    foreach (var incompatiblePlatform in pluginCompatibilitySetting.IncompatiblePlatforms)
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
