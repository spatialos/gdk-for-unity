using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Improbable.Gdk.Tools
{
    internal enum PluginType
    {
        Core,
        Sdk,
    }

    internal enum CPUType
    {
        ARMv7,
        Arm64,
        X86_64,
        X86,
        Unused,
    }

    internal class PluginCompatibilitySetting
    {
        public readonly string CPU;
        public readonly string PluginPath;
        public readonly PluginType PluginType;
        public readonly BuildTarget CompatiblePlatform;
        public readonly List<BuildTarget> IncompatiblePlatforms;
        public readonly bool CompatibleWithAnyPlatform;
        public readonly bool CompatibleWithEditor;

        private static readonly Dictionary<BuildTarget, string> BuildTargetToFolder =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneOSX, "OSX" },
                { BuildTarget.StandaloneLinux64, "Linux" },
                { BuildTarget.StandaloneWindows64, "Windows" },
                { BuildTarget.StandaloneWindows, "Windows" },
                { BuildTarget.Android, "Android" },
                { BuildTarget.iOS, "iOS" },
            };

        private static readonly Dictionary<CPUType, string> CPUToFolder =
            new Dictionary<CPUType, string>
            {
                { CPUType.ARMv7, "armv7" },
                { CPUType.Arm64, "arm64" },
                { CPUType.X86, "x86" },
                { CPUType.X86_64, "x86_64" },
            };

        public PluginCompatibilitySetting(
            PluginType pluginType,
            BuildTarget compatiblePlatform,
            CPUType cpuType = CPUType.Unused,
            bool compatibleWithEditor = true)
        {
            
            PluginType = pluginType;
            PluginPath = Path.Combine(DownloadCoreSdk.ImprobablePluginsPath, pluginType.ToString(), BuildTargetToFolder[compatiblePlatform]);
            CompatibleWithAnyPlatform = false;
            CompatibleWithEditor = compatibleWithEditor;
            CompatiblePlatform = compatiblePlatform;
            if (CPUType.Unused != cpuType)
            {
                PluginPath = Path.Combine(PluginPath, CPUToFolder[cpuType]);
                CPU = CPUToFolder[cpuType];
            }
            IncompatiblePlatforms = new List<BuildTarget>();
        }

        public PluginCompatibilitySetting(
            PluginType pluginType,
            List<BuildTarget> incompatiblePlatforms,
            bool compatibleWithEditor = true)
        {
            PluginType = pluginType;
            PluginPath = Path.Combine(DownloadCoreSdk.ImprobablePluginsPath, pluginType.ToString(), "Common");
            CompatibleWithAnyPlatform = true;
            IncompatiblePlatforms = incompatiblePlatforms;
            CompatibleWithEditor = compatibleWithEditor;
        }
    }
}
