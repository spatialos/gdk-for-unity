using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    internal class WorkerBuildData
    {
        public readonly string WorkerType;

        public string PackageName => $"{WorkerType}@{BuildTargetName}";

        public string BuildScratchDirectory =>
            Path.Combine(EditorPaths.BuildScratchDirectory, PackageName, ExecutableName);

        private string BuildTargetName => BuildTargetNames[buildTarget];
        private string ExecutableName => PackageName + BuildPlatformExtensions[buildTarget];

        private readonly BuildTarget buildTarget;

        internal static readonly IReadOnlyList<BuildTarget> AllBuildTargets = new List<BuildTarget>
        {
            BuildTarget.StandaloneWindows,
            BuildTarget.StandaloneWindows64,
            BuildTarget.StandaloneLinux64,
            BuildTarget.StandaloneOSX,
            BuildTarget.Android,
            BuildTarget.iOS,
        };

        internal static readonly IReadOnlyList<BuildTarget> LocalBuildTargets = new List<BuildTarget>
        {
            CurrentBuildPlatform,
            BuildTarget.Android,
            BuildTarget.iOS,
        };

        internal static readonly IReadOnlyDictionary<BuildTarget, BuildOptions> BuildTargetDefaultOptions =
            new Dictionary<BuildTarget, BuildOptions>
            {
                { BuildTarget.StandaloneWindows, BuildOptions.None },
                { BuildTarget.StandaloneWindows64, BuildOptions.None },
                { BuildTarget.StandaloneLinux64, BuildOptions.EnableHeadlessMode },
                { BuildTarget.StandaloneOSX, BuildOptions.None },
                { BuildTarget.Android, BuildOptions.None },
                { BuildTarget.iOS, BuildOptions.None }
            };

        private static readonly IReadOnlyDictionary<BuildTarget, string> BuildTargetNames =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneWindows, "Windows" },
                { BuildTarget.StandaloneWindows64, "Windows" },
                { BuildTarget.StandaloneLinux64, "Linux" },
                { BuildTarget.StandaloneOSX, "Mac" },
                { BuildTarget.Android, "Android" },
                { BuildTarget.iOS, "iOS" }
            };

        private static readonly IReadOnlyDictionary<BuildTarget, string> BuildPlatformExtensions =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneWindows, ".exe" },
                { BuildTarget.StandaloneWindows64, ".exe" },
                { BuildTarget.StandaloneLinux64, string.Empty },
                { BuildTarget.StandaloneOSX, string.Empty },
                { BuildTarget.Android, ".apk" },
                { BuildTarget.iOS, string.Empty }
            };

        internal static readonly IReadOnlyDictionary<BuildTarget, string> BuildTargetSupportDirectoryNames =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneWindows, "WindowsStandaloneSupport" },
                { BuildTarget.StandaloneWindows64, "WindowsStandaloneSupport" },
                { BuildTarget.StandaloneLinux64, "LinuxStandaloneSupport" },
                { BuildTarget.StandaloneOSX, "MacStandaloneSupport" },
                { BuildTarget.Android, "AndroidPlayer" },
                { BuildTarget.iOS, "iOSSupport" }
            };

        internal static IReadOnlyDictionary<BuildTarget, bool> BuildTargetsThatCanBeBuilt
        {
            get
            {
                return buildTargetsThatCanBeBuilt ?? (buildTargetsThatCanBeBuilt =
                    AllBuildTargets.ToDictionary(k => k, BuildSupportChecker.CanBuildTarget));
            }
        }

        internal static BuildTarget CurrentBuildPlatform
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        return BuildTarget.StandaloneWindows64;
                    case RuntimePlatform.OSXEditor:
                        return BuildTarget.StandaloneOSX;
                    case RuntimePlatform.LinuxEditor:
                        return BuildTarget.StandaloneLinux;
                    default:
                        return BuildTarget.NoTarget;
                }
            }
        }

        internal WorkerBuildData(string workerType, BuildTarget buildTarget)
        {
            if (!BuildTargetNames.ContainsKey(buildTarget))
            {
                throw new ArgumentException($"Unsupported BuildPlatform {buildTarget}");
            }

            WorkerType = workerType;
            this.buildTarget = buildTarget;
        }

        internal static BuildTargetConfig GetCurrentBuildTargetConfig()
        {
            return new BuildTargetConfig(CurrentBuildPlatform, BuildTargetDefaultOptions[CurrentBuildPlatform], enabled: true, required: false);
        }

        internal static BuildTargetConfig GetLinuxBuildTargetConfig()
        {
            return new BuildTargetConfig(BuildTarget.StandaloneLinux64, BuildTargetDefaultOptions[BuildTarget.StandaloneLinux64], enabled: true, required: false);
        }

        private static IReadOnlyDictionary<BuildTarget, bool> buildTargetsThatCanBeBuilt;
    }
}
