using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [Serializable]
    public class BuildEnvironmentConfig
    {
        public BuildTargetConfig[] BuildTargets;

        public BuildEnvironmentConfig(Func<BuildTarget?, BuildOptions> configureFunc,
            Func<BuildTarget?, bool> enableFunc)
        {
            // Start with the "Current" running platform.
            var options = new List<BuildTargetConfig> { new BuildTargetConfig(configureFunc(null), enableFunc(null)) };

            options.AddRange(
                WorkerBuildData.SupportedBuildTargets.Select(t =>
                    new BuildTargetConfig(t, configureFunc(t), enableFunc(t))));
            BuildTargets = options.ToArray();
        }

        public static bool IsCurrentBuildTarget(BuildTarget? target)
        {
            return !target.HasValue;
        }

        public static bool IsBuildTarget(BuildTarget? target, BuildTarget desired)
        {
            return target.HasValue && target.Value == desired;
        }
    }

    [Serializable]
    public class BuildTargetConfig
    {
        private readonly BuildTarget? target;
        public BuildOptions Options;
        public bool Enabled;

        public bool BuildSupportInstalled => WorkerBuildData.GetBuildTargetsThatCanBeBuilt()[Target];

        public string Label => !target.HasValue ? $"Current ({GetCurrentBuildPlatform()})" : target.ToString();

        public BuildTarget Target => target ?? GetCurrentBuildPlatform();

        /// <summary>
        /// Initialize a new build target for the currently running platform.
        /// </summary>
        public BuildTargetConfig(BuildOptions options, bool enabled = false)
        {
            Options = options;
            Enabled = enabled;
        }

        /// <summary>
        /// Initialize a new build target for a specific platform.
        /// </summary>
        public BuildTargetConfig(BuildTarget target, BuildOptions options, bool enabled = false)
        {
            this.target = target;
            Options = options;
            Enabled = enabled;
        }

        internal static BuildTarget GetCurrentBuildPlatform()
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
}
