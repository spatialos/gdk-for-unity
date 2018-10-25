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

    internal class PluginDirectoryCompatibility
    {
        public readonly bool AnyPlatformCompatible;
        public readonly bool IsEditorCompatible;
        public readonly string CPU;
        public readonly string PluginPath;
        public readonly PluginType PluginType;
        public readonly BuildTarget CompatiblePlatform;
        public readonly List<BuildTarget> IncompatiblePlatforms;

        private static readonly Dictionary<BuildTarget, string> targetToFolder =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneOSX, "OSX" },
                { BuildTarget.StandaloneLinux64, "Linux" },
                { BuildTarget.StandaloneWindows64, "Windows" },
                { BuildTarget.StandaloneWindows, "Windows" },
                { BuildTarget.Android, "Android" },
                { BuildTarget.iOS, "iOS" },
            };

        private static readonly Dictionary<CPUType, string> cpuToFolder =
            new Dictionary<CPUType, string>
            {
                { CPUType.ARMv7, "armv7" },
                { CPUType.Arm64, "arm64" },
                { CPUType.X86, "x86" },
                { CPUType.X86_64, "x86_64" },
            };

        public PluginDirectoryCompatibility(
            PluginType pluginType,
            BuildTarget buildTarget,
            CPUType cpuType = CPUType.Unused,
            bool isEditorCompatible = true
            )
        {
            
            PluginType = pluginType;
            PluginPath = Path.Combine(DownloadCoreSdk.ImprobablePluginsPath, pluginType.ToString(), targetToFolder[buildTarget]);
            AnyPlatformCompatible = false;
            IsEditorCompatible = isEditorCompatible;
            CompatiblePlatform = buildTarget;
            if (CPUType.Unused != cpuType)
            {
                PluginPath = Path.Combine(PluginPath, cpuToFolder[cpuType]);
                CPU = cpuToFolder[cpuType];
            }
            IncompatiblePlatforms = new List<BuildTarget>();
        }

        public PluginDirectoryCompatibility(
            PluginType pluginType,
            List<BuildTarget> incompatiblePlatforms,
            bool isEditorCompatible = true
        )
        {
            PluginType = pluginType;
            PluginPath = Path.Combine(DownloadCoreSdk.ImprobablePluginsPath, pluginType.ToString(), "Common");
            AnyPlatformCompatible = true;
            IncompatiblePlatforms = incompatiblePlatforms;
            IsEditorCompatible = isEditorCompatible;
        }
    }
}
