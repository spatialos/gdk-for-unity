using System;
using System.Collections.Generic;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [Serializable]
    public class WorkerBuildConfiguration
    {
        public string WorkerType;
        public List<SceneAsset> ScenesForWorker = new List<SceneAsset>();

        public BuildEnvironmentConfig LocalBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.LocalBuildTargets, WorkerBuildData.GetCurrentBuildTargetConfig());
        public BuildEnvironmentConfig CloudBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.AllBuildTargets, WorkerBuildData.GetCurrentBuildTargetConfig());

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
