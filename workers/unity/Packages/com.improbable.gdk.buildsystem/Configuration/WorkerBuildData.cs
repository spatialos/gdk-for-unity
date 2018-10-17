using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    public class WorkerBuildData
    {
        public readonly string WorkerType;

        public string PackageName => $"{WorkerType}@{BuildTargetName}";

        public string BuildScratchDirectory =>
            Path.Combine(EditorPaths.BuildScratchDirectory, PackageName, ExecutableName);

        private string BuildTargetName => BuildTargetNames[buildTarget];
        private string ExecutableName => PackageName + BuildPlatformExtensions[buildTarget];

        private readonly BuildTarget buildTarget;

        private static readonly Dictionary<BuildTarget, string> BuildTargetNames =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneWindows, "Windows" },
                { BuildTarget.StandaloneWindows64, "Windows" },
                { BuildTarget.StandaloneLinux64, "Linux" },
                { BuildTarget.StandaloneOSX, "Mac" },
                { BuildTarget.Android, "Android" },
                { BuildTarget.iOS, "iOS" }
            };

        private static readonly Dictionary<BuildTarget, string> BuildPlatformExtensions =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneWindows, ".exe" },
                { BuildTarget.StandaloneWindows64, ".exe" },
                { BuildTarget.StandaloneLinux64, "" },
                { BuildTarget.StandaloneOSX, "" },
                { BuildTarget.Android, ".apk" },
                { BuildTarget.iOS, "" }
            };

        public static readonly Dictionary<BuildTarget, string> BuildTargetSupportDirectoryNames =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneWindows, "WindowsStandaloneSupport" },
                { BuildTarget.StandaloneWindows64, "WindowsStandaloneSupport" },
                { BuildTarget.StandaloneLinux64, "LinuxStandaloneSupport" },
                { BuildTarget.StandaloneOSX, "MacStandaloneSupport" },
                { BuildTarget.Android, "AndroidPlayer" },
                { BuildTarget.iOS, "iOSSupport" }
            };

        public WorkerBuildData(string workerType, BuildTarget buildTarget)
        {
            if (!BuildTargetNames.ContainsKey(buildTarget))
            {
                throw new ArgumentException("Unsupported BuildPlatform " + buildTarget);
            }

            WorkerType = workerType;
            this.buildTarget = buildTarget;
        }
    }
}
