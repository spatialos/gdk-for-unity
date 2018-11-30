using System;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [Serializable]
    public class WorkerBuildConfiguration
    {
        public string WorkerType;
        public SceneAsset[] ScenesForWorker = { };

        public BuildEnvironmentConfig LocalBuildConfig = new BuildEnvironmentConfig(t => BuildOptions.None, t => false);
        public BuildEnvironmentConfig CloudBuildConfig = new BuildEnvironmentConfig(t => BuildOptions.None, t => false);

        public BuildEnvironmentConfig GetEnvironmentConfig(BuildEnvironment targetEnvironment)
        {
            switch (targetEnvironment)
            {
                case BuildEnvironment.Local:
                    return LocalBuildConfig;
                case BuildEnvironment.Cloud:
                    return CloudBuildConfig;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetEnvironment), targetEnvironment, null);
            }
        }
    }
}
