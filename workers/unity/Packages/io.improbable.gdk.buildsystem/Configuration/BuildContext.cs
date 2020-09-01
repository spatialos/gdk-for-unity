using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    public struct BuildContext
    {
        public string WorkerType;
        public BuildTargetConfig BuildTargetConfig;
        public BuildEnvironment BuildEnvironment;
        public ScriptingImplementation ScriptingImplementation;
        public iOSSdkVersion? IOSSdkVersion;

        public static List<BuildContext> GetBuildContexts(BuildConfig buildConfig, BuildContextSettings contextSettings)
        {
            var result = new List<BuildContext>();

            foreach (var workerType in contextSettings.WantedWorkerTypes)
            {
                var environmentConfig =
                    buildConfig.GetEnvironmentConfigForWorker(workerType, contextSettings.BuildEnvironment);

                if (environmentConfig == null)
                {
                    continue;
                }

                IEnumerable<BuildTargetConfig> supportedTargets;
                try
                {
                    supportedTargets = environmentConfig.GetSupportedTargets(contextSettings);
                }
                catch (BuildFailedException exception)
                {
                    throw new BuildFailedException($"Build failed for {workerType}. {exception.Message}");
                }

                result.AddRange(supportedTargets.Select(targetConfig => new BuildContext
                {
                    WorkerType = workerType,
                    BuildEnvironment = contextSettings.BuildEnvironment,
                    ScriptingImplementation = contextSettings.ScriptImplementation ?? targetConfig.ScriptingImplementation,
                    BuildTargetConfig = targetConfig,
                    IOSSdkVersion = (targetConfig.Target == BuildTarget.iOS) ? contextSettings.IOSSdkVersion : null
                }));
            }

            return result;
        }
    }
}
