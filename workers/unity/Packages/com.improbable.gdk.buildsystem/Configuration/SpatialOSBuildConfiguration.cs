using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [CreateAssetMenu(fileName = "SpatialOS Build Configuration", menuName = EditorConfig.BuildConfigurationMenu)]
    public class SpatialOSBuildConfiguration : SingletonScriptableObject<SpatialOSBuildConfiguration>
    {
        [SerializeField] public List<WorkerBuildConfiguration> WorkerBuildConfigurations =
            new List<WorkerBuildConfiguration>();

        [SerializeField] private bool isInitialised;

        public BuildEnvironmentConfig GetEnvironmentConfigForWorker(string workerType, BuildEnvironment environment)
        {
            var config = WorkerBuildConfigurations.FirstOrDefault(x => x.WorkerType == workerType);
            if (config == null)
            {
                Debug.LogWarning($"The worker type {workerType} does not have a build configuration.", this);
                return null;
            }

            return config.GetEnvironmentConfig(environment);
        }

        internal string[] GetScenePathsForWorker(string workerType)
        {
            return GetScenesForWorker(workerType)
                .Where(sceneAsset => sceneAsset != null)
                .Select(AssetDatabase.GetAssetPath)
                .ToArray();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (!isInitialised)
            {
                ResetToDefault();
            }
        }

        private void ResetToDefault()
        {
            WorkerBuildConfigurations = new List<WorkerBuildConfiguration>
            {
                new WorkerBuildConfiguration
                {
                    WorkerType = "UnityClient",
                    LocalBuildConfig =
                        new BuildEnvironmentConfig(
                            target => BuildEnvironmentConfig.IsCurrentBuildTarget(target)
                                ? BuildOptions.Development
                                : BuildOptions.None,
                            BuildEnvironmentConfig.IsCurrentBuildTarget),
                    CloudBuildConfig = new BuildEnvironmentConfig(target =>
                        BuildEnvironmentConfig.IsBuildTarget(target, BuildTarget.StandaloneOSX) ||
                        BuildEnvironmentConfig.IsBuildTarget(target, BuildTarget.StandaloneWindows64)
                            ? BuildOptions.Development
                            : BuildOptions.None, target =>
                        BuildEnvironmentConfig.IsBuildTarget(target, BuildTarget.StandaloneOSX) ||
                        BuildEnvironmentConfig.IsBuildTarget(target, BuildTarget.StandaloneWindows64))
                },
                new WorkerBuildConfiguration
                {
                    WorkerType = "UnityGameLogic",
                    LocalBuildConfig =
                        new BuildEnvironmentConfig(
                            target => BuildEnvironmentConfig.IsCurrentBuildTarget(target)
                                ? BuildOptions.None
                                : BuildOptions.Development,
                            BuildEnvironmentConfig.IsCurrentBuildTarget),
                    CloudBuildConfig =
                        new BuildEnvironmentConfig(
                            target => BuildEnvironmentConfig.IsBuildTarget(target, BuildTarget.StandaloneLinux64)
                                ? BuildOptions.EnableHeadlessMode
                                : BuildOptions.Development,
                            target => BuildEnvironmentConfig.IsBuildTarget(target, BuildTarget.StandaloneLinux64))
                },
            };
            isInitialised = true;
        }

        private SceneAsset[] GetScenesForWorker(string workerType)
        {
            var configurationForWorker = WorkerBuildConfigurations.FirstOrDefault(x => x.WorkerType == workerType);
            return configurationForWorker == null
                ? new SceneAsset[0]
                : configurationForWorker.ScenesForWorker;
        }
    }
}
