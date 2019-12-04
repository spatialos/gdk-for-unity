using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    public struct BuildContext
    {
        public string WorkerType;
        public BuildTargetConfig BuildTargetConfig;
        public BuildEnvironment BuildEnvironment;
        public ScriptingImplementation ScriptingImplementation;

        public static List<BuildContext> GetBuildContexts(IEnumerable<string> wantedWorkerTypes,
            BuildEnvironment buildEnvironment, ScriptingImplementation? scriptImplementation = null,
            ICollection<BuildTarget> buildTargetFilter = null)
        {
            var spatialOsBuildConfiguration = BuildConfig.GetInstance();
            var result = new List<BuildContext>();

            foreach (var workerType in wantedWorkerTypes)
            {
                var environmentConfig =
                    spatialOsBuildConfiguration.GetEnvironmentConfigForWorker(workerType, buildEnvironment);

                // Filter targets for CI
                var targetConfigs = environmentConfig.BuildTargets
                    .Where(t => t.Enabled && (buildTargetFilter?.Contains(t.Target) ?? true))
                    .ToList();

                // Which build targets are not supported by current install?
                var missingTargets = targetConfigs
                    .Where(c => !BuildSupportChecker.CanBuildTarget(c.Target))
                    .ToList();

                // Error on missing required build support
                if (missingTargets.Any(c => c.Required))
                {
                    var targetNames = string.Join(", ", missingTargets
                        .Where(c => c.Required)
                        .Select(c => c.Target.ToString()));
                    throw new BuildFailedException(
                        $"Build failed for {workerType}. Cannot build for required ({targetNames}) because build support is not installed in the Unity Editor.");
                }

                // Log builds we're skipping
                if (missingTargets.Count > 0)
                {
                    var targetNames = string.Join(", ", missingTargets.Select(c => c.Target.ToString()));
                    Debug.LogWarning(
                        $"Skipping ({targetNames}) because build support is not installed in the Unity Editor and the build target is not marked as 'Required'.");
                }

                result.AddRange(targetConfigs.Select(targetConfig => new BuildContext
                {
                    WorkerType = workerType, BuildEnvironment = buildEnvironment,
                    ScriptingImplementation = scriptImplementation ??
                        PlayerSettings.GetScriptingBackend(BuildPipeline.GetBuildTargetGroup(targetConfig.Target)),
                    BuildTargetConfig = targetConfig
                }));
            }

            return result;
        }
    }
}
