using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Legacy.BuildSystem.Configuration
{
    [CreateAssetMenu(fileName = "SpatialOS Build Configuration", menuName = CreateMenuPath)]
    public class SpatialOSBuildConfiguration : SingletonScriptableObject<SpatialOSBuildConfiguration>
    {
        internal const string CreateMenuPath = "SpatialOS/Build Configuration";

        [SerializeField] private bool isInitialised;

        [SerializeField] public List<WorkerBuildConfiguration> WorkerBuildConfigurations;

        public override void OnEnable()
        {
            base.OnEnable();

            if (!isInitialised)
            {
                ResetToDefault();
            }

            if (IsAnAsset())
            {
                UpdateEditorScenesForBuild();
            }
        }

        private void ResetToDefault()
        {
            // Build default settings
            var client = new WorkerBuildConfiguration()
            {
                WorkerPlatform = WorkerPlatform.UnityClient,
                ScenesForWorker = AssetDatabase.FindAssets("t:Scene")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Where(path => path.Contains(WorkerPlatform.UnityClient.ToString()))
                    .Select(AssetDatabase.LoadAssetAtPath<SceneAsset>).ToArray(),
                LocalBuildConfig = new BuildEnvironmentConfig()
                {
                    BuildPlatforms = SpatialBuildPlatforms.Current,
                    BuildOptions = BuildOptions.Development
                },
                CloudBuildConfig = new BuildEnvironmentConfig()
                {
                    BuildPlatforms = SpatialBuildPlatforms.Current
                }
            };

            var worker = new WorkerBuildConfiguration()
            {
                WorkerPlatform = WorkerPlatform.UnityGameLogic,
                ScenesForWorker = AssetDatabase.FindAssets("t:Scene")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Where(path => path.Contains(WorkerPlatform.UnityGameLogic.ToString()))
                    .Select(AssetDatabase.LoadAssetAtPath<SceneAsset>).ToArray(),
                LocalBuildConfig = new BuildEnvironmentConfig()
                {
                    BuildPlatforms = SpatialBuildPlatforms.Current,
                    BuildOptions = BuildOptions.EnableHeadlessMode
                },
                CloudBuildConfig = new BuildEnvironmentConfig()
                {
                    BuildPlatforms = SpatialBuildPlatforms.Linux,
                    BuildOptions = BuildOptions.EnableHeadlessMode
                }
            };

            WorkerBuildConfigurations = new List<WorkerBuildConfiguration>
            {
                client,
                worker
            };

            isInitialised = true;
        }

        private void OnValidate()
        {
            if (!isInitialised)
            {
                ResetToDefault();
            }

            if (IsAnAsset())
            {
                UpdateEditorScenesForBuild();
            }
        }

        private SceneAsset[] GetScenesForWorker(WorkerPlatform workerPlatform)
        {
            WorkerBuildConfiguration configurationForWorker = null;

            if (WorkerBuildConfigurations != null)
            {
                configurationForWorker =
                    WorkerBuildConfigurations.FirstOrDefault(config => config.WorkerPlatform == workerPlatform);
            }

            return configurationForWorker == null
                ? new SceneAsset[0]
                : configurationForWorker.ScenesForWorker;
        }

        internal void UpdateEditorScenesForBuild()
        {
            EditorApplication.delayCall += () =>
            {
                EditorBuildSettings.scenes =
                    WorkerBuildConfigurations.SelectMany(x => GetScenesForWorker(x.WorkerPlatform))
                        .Select(AssetDatabase.GetAssetPath)
                        .Distinct()
                        .Select(scenePath => new EditorBuildSettingsScene(scenePath, true))
                        .ToArray();
            };
        }

        public BuildEnvironmentConfig GetEnvironmentConfigForWorker(WorkerPlatform platform,
            BuildEnvironment targetEnvironment)
        {
            var config = WorkerBuildConfigurations.FirstOrDefault(x => x.WorkerPlatform == platform);
            if (config == null)
            {
                throw new ArgumentException("Unknown WorkerPlatform " + platform);
            }

            return config.GetEnvironmentConfig(targetEnvironment);
        }

        public string[] GetScenePathsForWorker(WorkerPlatform workerType)
        {
            return GetScenesForWorker(workerType)
                .Where(sceneAsset => sceneAsset != null)
                .Select(AssetDatabase.GetAssetPath)
                .ToArray();
        }
    }
}
