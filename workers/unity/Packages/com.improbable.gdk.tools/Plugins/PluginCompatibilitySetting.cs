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

    internal enum CpuType
    {
        ARMv7,
        Arm64,
        X86_64,
        X86,
        Agnostic,
    }

    internal struct PluginCompatibilitySetting
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

        private static readonly Dictionary<CpuType, string> CpuToFolder =
            new Dictionary<CpuType, string>
            {
                { CpuType.ARMv7, "armv7" },
                { CpuType.Arm64, "arm64" },
                { CpuType.X86, "x86" },
                { CpuType.X86_64, "x86_64" },
            };

        public PluginCompatibilitySetting(
            PluginType pluginType,
            BuildTarget compatiblePlatform,
            CpuType cpuType = CpuType.Agnostic,
            bool compatibleWithEditor = true)
        {
            PluginType = pluginType;
            PluginPath = Path.Combine(DownloadCoreSdk.ImprobablePluginsPath, pluginType.ToString(), BuildTargetToFolder[compatiblePlatform]);
            CompatibleWithAnyPlatform = false;
            CompatibleWithEditor = compatibleWithEditor;
            CompatiblePlatform = compatiblePlatform;
            if (cpuType == CpuType.Agnostic)
            {
                CPU = string.Empty;
            }
            else
            {
                PluginPath = Path.Combine(PluginPath, CpuToFolder[cpuType]);
                CPU = CpuToFolder[cpuType];
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
            CompatiblePlatform = 0;
            CPU = string.Empty;
        }

        internal bool UpdateCompatibleWithAnyPlatform(PluginImporter pluginImporter)
        {
            if (CompatibleWithAnyPlatform != pluginImporter.GetCompatibleWithAnyPlatform())
            {
                pluginImporter.SetCompatibleWithAnyPlatform(CompatibleWithAnyPlatform);
                return true;
            }

            return false;
        }

        internal bool UpdateCompatibleWithEditor(PluginImporter pluginImporter)
        {

            if (CompatibleWithEditor != pluginImporter.GetCompatibleWithEditor())
            {
                pluginImporter.SetCompatibleWithEditor(CompatibleWithEditor);
                return true;
            }

            return false;
        }

        internal bool UpdateCompatibleWithPlatform(PluginImporter pluginImporter)
        {
            if (CompatiblePlatform != 0 && !pluginImporter.GetCompatibleWithPlatform(CompatiblePlatform))
            {
                pluginImporter.SetCompatibleWithPlatform(CompatiblePlatform, true);
                if (!string.IsNullOrEmpty(CPU))
                {
                    pluginImporter.SetPlatformData(CompatiblePlatform, "CPU", CPU);
                }

                return true;
            }

            return false;
        }
        internal bool UpdateIncompatibleWithPlatforms(PluginImporter pluginImporter)
        {
            var hasUpdated = false;
            foreach (var incompatiblePlatform in IncompatiblePlatforms)
            {
                if (!pluginImporter.GetExcludeFromAnyPlatform(incompatiblePlatform))
                {
                    pluginImporter.SetExcludeFromAnyPlatform(incompatiblePlatform, true);
                    hasUpdated = true;
                }
            }

            return hasUpdated;
        }
    }
}
