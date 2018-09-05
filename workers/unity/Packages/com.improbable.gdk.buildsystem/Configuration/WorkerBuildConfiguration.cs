using System;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [Serializable]
    public class WorkerBuildConfiguration
    {
        public WorkerPlatform WorkerPlatform;
        public SceneAsset[] ScenesForWorker;

        public BuildEnvironmentConfig LocalBuildConfig = new BuildEnvironmentConfig();
        public BuildEnvironmentConfig CloudBuildConfig = new BuildEnvironmentConfig();

        public BuildEnvironmentConfig GetEnvironmentConfig(BuildEnvironment targetEnvironment)
        {
            BuildEnvironmentConfig buildEnvironmentConfig;

            switch (targetEnvironment)
            {
                case BuildEnvironment.Local:
                    buildEnvironmentConfig = LocalBuildConfig;
                    break;
                case BuildEnvironment.Cloud:
                    buildEnvironmentConfig = CloudBuildConfig;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetEnvironment), targetEnvironment, null);
            }

            return buildEnvironmentConfig;
        }

        [NonSerialized] public bool ShowFoldout = true;
    }
}
