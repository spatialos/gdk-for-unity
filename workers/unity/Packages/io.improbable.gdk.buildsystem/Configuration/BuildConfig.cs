using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [CreateAssetMenu(fileName = "SpatialOS Build Configuration", menuName = BuildConfigEditor.BuildConfigurationMenu)]
    public class BuildConfig : SingletonScriptableObject<BuildConfig>
    {
        [SerializeField] internal List<WorkerBuildConfiguration> WorkerBuildConfigurations =
            new List<WorkerBuildConfiguration>();

        [SerializeField] private bool isInitialised;

        internal BuildEnvironmentConfig GetEnvironmentConfigForWorker(string workerType, BuildEnvironment environment)
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
                        new BuildEnvironmentConfig(WorkerBuildData.LocalBuildTargets,
                            WorkerBuildData.GetCurrentBuildTargetConfig()),
                    CloudBuildConfig = new BuildEnvironmentConfig(
                        WorkerBuildData.AllBuildTargets,
                        new BuildTargetConfig(BuildTarget.StandaloneOSX)
                        {
                            Options = BuildOptions.Development,
                            Enabled = true,
                            Required = false
                        },
                        new BuildTargetConfig(BuildTarget.StandaloneWindows64)
                        {
                            Options = BuildOptions.Development,
                            Enabled = true,
                            Required = false
                        })
                },
                new WorkerBuildConfiguration
                {
                    WorkerType = "UnityGameLogic",
                    LocalBuildConfig =
                        new BuildEnvironmentConfig(WorkerBuildData.LocalBuildTargets,
                            WorkerBuildData.GetCurrentBuildTargetConfig()),
                    CloudBuildConfig =
                        new BuildEnvironmentConfig(
                            WorkerBuildData.AllBuildTargets, WorkerBuildData.GetLinuxBuildTargetConfig())
                },
            };
            isInitialised = true;
        }

        private List<SceneAsset> GetScenesForWorker(string workerType)
        {
            var configurationForWorker = WorkerBuildConfigurations.FirstOrDefault(x => x.WorkerType == workerType);
            return configurationForWorker == null
                ? new List<SceneAsset>()
                : configurationForWorker.ScenesForWorker;
        }
    }
}
