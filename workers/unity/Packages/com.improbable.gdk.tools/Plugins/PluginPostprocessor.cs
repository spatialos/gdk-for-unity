using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

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
                CpuType.X86_64
            ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.StandaloneWindows,
                CpuType.X86,
                compatibleWithEditor: false
            ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.Android,
                CpuType.Arm64
            ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.Android,
                CpuType.ARMv7
            ),
            new PluginCompatibilitySetting(
                PluginType.Core,
                BuildTarget.Android,
                CpuType.X86
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
                CpuType.Agnostic,
                compatibleWithEditor: false
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
                    needsReimport |= pluginCompatibilitySetting.UpdateCompatibleWithAnyPlatform(pluginImporter);
                    needsReimport |= pluginCompatibilitySetting.UpdateCompatibleWithEditor(pluginImporter);
                    needsReimport |= pluginCompatibilitySetting.UpdateCompatibleWithPlatform(pluginImporter);
                    needsReimport |= pluginCompatibilitySetting.UpdateIncompatibleWithPlatforms(pluginImporter);

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
