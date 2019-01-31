using System;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    /// <summary>
    /// Build options for a particular build target.
    /// </summary>
    [Serializable]
    internal struct BuildTargetConfig
    {
        /// <summary>
        /// The options to apply when the target is built.
        /// </summary>
        public BuildOptions Options;

        /// <summary>
        /// The target to build.
        /// </summary>
        public BuildTarget Target;

        /// <summary>
        /// Should this target be built?
        /// </summary>
        public bool Enabled;

        [NonSerialized] internal readonly string Label;        

        /// <summary>
        ///     Creates a new instance of a build target and its options.
        /// </summary>
        public BuildTargetConfig(BuildTarget target, BuildOptions options, bool enabled)
        {
            Enabled = enabled;
            Target = target;
            Options = options;
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                    Label = "Win x86";
                    break;
                case BuildTarget.StandaloneWindows64:
                    Label = "Win x64";
                    break;
                case BuildTarget.StandaloneLinux64:
                    Label = "Linux";
                    break;
                case BuildTarget.StandaloneOSX:
                    Label = "MacOS";
                    break;
                case BuildTarget.iOS:
                    Label = "iOS";
                    break;
                case BuildTarget.Android:
                    Label = "Android";
                    break;
                default:
                    Label = "Unknown";
                    break;
            }
        }
    }
}
