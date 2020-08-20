using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    public struct BuildContext
    {
        public string WorkerType;
        public BuildTargetConfig BuildTargetConfig;
        public BuildEnvironment BuildEnvironment;
        public ScriptingImplementation ScriptingImplementation;
        public iOSSdkVersion? IOSSdkVersion;

        public static List<BuildContext> GetBuildContexts(BuildContextFilter contextFilter)
        {
            var spatialOsBuildConfiguration = BuildConfig.GetInstance();
            var result = new List<BuildContext>();

            foreach (var workerType in contextFilter.WantedWorkerTypes)
            {
                var environmentConfig =
                    spatialOsBuildConfiguration.GetEnvironmentConfigForWorker(workerType, contextFilter.BuildEnvironment);

                var supportedTargets = environmentConfig.GetSupportedTargets(workerType, contextFilter);

                result.AddRange(supportedTargets.Select(targetConfig => new BuildContext
                {
                    WorkerType = workerType,
                    BuildEnvironment = contextFilter.BuildEnvironment,
                    ScriptingImplementation = contextFilter.ScriptImplementation ?? targetConfig.ScriptingImplementation,
                    BuildTargetConfig = targetConfig,
                    IOSSdkVersion = (targetConfig.Target == BuildTarget.iOS) ? contextFilter.IOSSdkVersion : null
                }));
            }

            return result;
        }
    }
}
