using Improbable.Gdk.BuildSystem.Configuration;

namespace Improbable.Gdk.BuildSystem.Tests
{
    internal static class BuildConfigUtils
    {
        internal static BuildConfig AddWorker(this BuildConfig buildConfig, string workerType, BuildTargetConfig[] localConfigs, BuildTargetConfig[] cloudConfigs)
        {
            var workerConfig = new WorkerBuildConfiguration
            {
                WorkerType = workerType,
                LocalBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.LocalBuildTargets, localConfigs),
                CloudBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.AllBuildTargets, cloudConfigs)
            };

            buildConfig.WorkerBuildConfigurations.Add(workerConfig);
            return buildConfig;
        }
    }
}
