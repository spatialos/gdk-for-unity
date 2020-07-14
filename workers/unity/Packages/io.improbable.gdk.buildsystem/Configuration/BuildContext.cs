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
        public iOSSdkVersion? IOSSdkVersion;

        public static List<BuildContext> GetBuildContexts(IEnumerable<string> wantedWorkerTypes,
            BuildEnvironment buildEnvironment, ScriptingImplementation? scriptImplementation = null,
            ICollection<BuildTarget> buildTargetFilter = null, iOSSdkVersion? iosSdkVersion = null)
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

                // Filter out any deprecated targets
                var supportedTargets = targetConfigs
                    .Where(c => !c.Deprecated)
                    .ToList();

                // Which build targets are not supported by current install?
                var missingTargets = supportedTargets
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

                    targetConfigs.RemoveAll(t => missingTargets.Contains(t));
                }

                result.AddRange(supportedTargets.Select(targetConfig => new BuildContext
                {
                    WorkerType = workerType,
                    BuildEnvironment = buildEnvironment,
                    ScriptingImplementation = scriptImplementation ?? targetConfig.ScriptingImplementation,
                    BuildTargetConfig = targetConfig,
                    IOSSdkVersion = (targetConfig.Target == BuildTarget.iOS) ? iosSdkVersion : null
                }));
            }

            return result;
        }
    }
}
