using System;
using System.Collections.Generic;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    public class WorkerBuildData
    {
        public readonly string WorkerType;

        public string PackageName => $"{WorkerType}@{BuildTargetName}";
        
        public string BuildScratchDirectory =>
            PathUtils.Combine(PathUtils.BuildScratchDirectory, PackageName, ExecutableName).ToUnityPath();
        
        private string BuildTargetName => BuildTargetNames[buildTarget];
        private string ExecutableName => PackageName + BuildPlatformExtensions[buildTarget];
        
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
