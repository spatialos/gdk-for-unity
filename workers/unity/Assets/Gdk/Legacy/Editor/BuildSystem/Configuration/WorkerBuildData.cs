using System;
using System.Collections.Generic;
using Improbable.Gdk.Legacy.BuildSystem.Util;
using UnityEditor;

namespace Improbable.Gdk.Legacy.BuildSystem.Configuration
{
    public class WorkerBuildData
    {
        private readonly WorkerPlatform workerPlatform;
        private readonly BuildTarget buildTarget;

        private static readonly Dictionary<BuildTarget, string> BuildTargetNames =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneWindows, "Windows" },
                { BuildTarget.StandaloneWindows64, "Windows" },
                { BuildTarget.StandaloneLinux64, "Linux" },
                { BuildTarget.StandaloneOSX, "Mac" }
            };

        private static readonly Dictionary<BuildTarget, string> BuildPlatformExtensions =
            new Dictionary<BuildTarget, string>
            {
                { BuildTarget.StandaloneWindows, ".exe" },
                { BuildTarget.StandaloneWindows64, ".exe" },
                { BuildTarget.StandaloneLinux64, "" },
                { BuildTarget.StandaloneOSX, "" }
            };

        public WorkerBuildData(WorkerPlatform workerPlatform, BuildTarget buildTarget)
        {
            if (!BuildTargetNames.ContainsKey(buildTarget))
            {
                throw new ArgumentException("Unsupported BuildPlatform " + buildTarget);
            }

            this.workerPlatform = workerPlatform;
            this.buildTarget = buildTarget;
        }

        private string BuildTargetName => BuildTargetNames[buildTarget];

        public string BuildScratchDirectory =>
            PathUtil.Combine(BuildPaths.BuildScratchDirectory, PackageName, ExecutableName).ToUnityPath();

        public string WorkerPlatformName => workerPlatform.ToString();

        private string ExecutableName => PackageName + BuildPlatformExtensions[buildTarget];

        public string PackageName => string.Format("{0}@{1}", workerPlatform, BuildTargetName);
    }
}
