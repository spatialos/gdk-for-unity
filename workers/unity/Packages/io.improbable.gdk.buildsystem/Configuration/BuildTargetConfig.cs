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
        [NonSerialized] public BuildTarget Target;

        /// <summary>
        /// Should this target be built?
        /// </summary>
        public bool Enabled;

        /// <summary>
        /// Is this build target required?
        /// If a required target cannot be built, it will be treated as a failure.
        /// </summary>
        public bool Required;

        internal string Label
        {
            get
            {
                switch (Target)
                {
                    case BuildTarget.StandaloneWindows:
                        return "Win x86";
                    case BuildTarget.StandaloneWindows64:
                        return "Win x64";
                    case BuildTarget.StandaloneLinux64:
                        return "Linux";
                    case BuildTarget.StandaloneOSX:
                        return "MacOS";
                    case BuildTarget.iOS:
                        return "iOS";
                    case BuildTarget.Android:
                        return "Android";
                    default:
                        return "Unknown";
                }
            }
        }

        /// <summary>
        ///     Creates a new instance of a build target and its options.
        /// </summary>
        public BuildTargetConfig(BuildTarget target, BuildOptions options, bool enabled, bool required)
        {
            Enabled = enabled;
            Required = required;
            Target = target;
            Options = options;
        }
    }
}
