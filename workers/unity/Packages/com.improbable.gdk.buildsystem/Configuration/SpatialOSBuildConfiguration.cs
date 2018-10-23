using System;
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

        internal void UpdateEditorScenesForBuild()
        {
            EditorBuildSettings.scenes =
                WorkerBuildConfigurations.SelectMany(x => GetScenesForWorker(x.WorkerType))
                    .Select(AssetDatabase.GetAssetPath)
                    .Distinct()
                    .Select(scenePath => new EditorBuildSettingsScene(scenePath, true))
                    .ToArray();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (!isInitialised)
            {
                ResetToDefault();
            }

            if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this)))
            {
                EditorApplication.delayCall += UpdateEditorScenesForBuild;
            }
        }

        private void ResetToDefault()
        {
            WorkerBuildConfigurations = new List<WorkerBuildConfiguration>
            {
                {
                    new WorkerBuildConfiguration
                    {
                        WorkerType = "UnityClient",
                        LocalBuildConfig = new BuildEnvironmentConfig { BuildOptions = BuildOptions.Development },
                    }
                },
                {
                    new WorkerBuildConfiguration
                    {
                        WorkerType = "UnityGameLogic",
                        LocalBuildConfig =
                            new BuildEnvironmentConfig { BuildOptions = BuildOptions.EnableHeadlessMode },
                        CloudBuildConfig = new BuildEnvironmentConfig
                        {
                            BuildPlatforms = SpatialBuildPlatforms.Linux,
                            BuildOptions = BuildOptions.EnableHeadlessMode
                        }
                    }
                }
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
