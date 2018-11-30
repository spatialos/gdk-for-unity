using System;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [Serializable]
    public class BuildEnvironmentConfig
    {
        public SpatialBuildPlatforms BuildPlatforms = SpatialBuildPlatforms.Current;
        public BuildOptions BuildOptions = 0;
    }
}
