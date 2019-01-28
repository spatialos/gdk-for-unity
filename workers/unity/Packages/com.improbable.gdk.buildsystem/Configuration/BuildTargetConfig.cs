using System;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [Serializable]
    public struct BuildTargetConfig
    {
        public BuildOptions Options;

        public BuildTarget Target;

        public bool Enabled;

        [NonSerialized] public readonly string Label;

        public BuildTargetConfig SetEnabled(bool enabled)
        {
            return new BuildTargetConfig(Target, Options, enabled);
        }

        /// <summary>
        ///     Initialize a new build target for a specific platform.
        /// </summary>
        public BuildTargetConfig(BuildTarget target, BuildOptions options, bool enabled)
        {
            Enabled = enabled;
            Target = target;
            Options = options;
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                    Label = "Windows (x86)";
                    break;
                case BuildTarget.StandaloneWindows64:
                    Label = "Windows (x86_64)";
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
