using Improbable.Gdk.BuildSystem.Configuration;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Tests
{
    internal static class TestUtils
    {
        internal static BuildConfig AddWorker(this BuildConfig buildConfig, string workerType, params BuildTargetConfig[] targetConfigs)
        {
            var localConfigs = WorkerBuildData.GetCurrentBuildTargetConfig();
            var cloudConfigs = targetConfigs;

            var workerConfig = new WorkerBuildConfiguration
            {
                WorkerType = workerType,
                LocalBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.LocalBuildTargets, localConfigs),
                CloudBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.AllBuildTargets, cloudConfigs)
            };

            buildConfig.WorkerBuildConfigurations.Add(workerConfig);
            return buildConfig;
        }

        internal static BuildTargetConfig CreateTarget(
            BuildTarget target,
            bool development = false,
            bool headless = false,
            bool enabled = true,
            bool required = false,
            ScriptingImplementation scriptingImplementation = ScriptingImplementation.Mono2x)
        {
            var buildOptions = BuildOptions.None;

            if (development)
            {
                buildOptions |= BuildOptions.Development;
            }

            if (headless)
            {
                buildOptions |= BuildOptions.EnableHeadlessMode;
            }

            return new BuildTargetConfig(target, buildOptions, enabled, required, scriptingImplementation: scriptingImplementation);
        }
    }
}
