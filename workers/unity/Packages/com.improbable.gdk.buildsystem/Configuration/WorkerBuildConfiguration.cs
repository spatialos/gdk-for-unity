using System;
using System.Collections.Generic;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [Serializable]
    public class WorkerBuildConfiguration
    {
        internal static readonly string IncompatibleWindowsPlatformsErrorMessage =
            $"Please choose only one of {SpatialBuildPlatforms.Windows32} or {SpatialBuildPlatforms.Windows64} as a build platform.";

        public string WorkerType;
        public SceneAsset[] ScenesForWorker = { };

        public BuildEnvironmentConfig LocalBuildConfig = new BuildEnvironmentConfig();
        public BuildEnvironmentConfig CloudBuildConfig = new BuildEnvironmentConfig();

        public BuildEnvironmentConfig GetEnvironmentConfig(BuildEnvironment targetEnvironment)
        {
            switch (targetEnvironment)
            {
                case BuildEnvironment.Local:
                    return LocalBuildConfig;
                case BuildEnvironment.Cloud:
                    return CloudBuildConfig;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetEnvironment), targetEnvironment, null);
            }
        }

        public class Problem
        {
            public Problem(string message, MessageType type)
            {
                Message = message;
                Type = type;
            }

            public string Message { get; }

            public MessageType Type { get; }
        }
        
        public static List<Problem> GetConfigurationProblems(BuildEnvironment environment,
            WorkerBuildConfiguration configurationForWorker)
        {
            var problems = new List<Problem>();
            var environmentConfiguration =
                configurationForWorker.GetEnvironmentConfig(environment);

            var buildOptions = environmentConfiguration.BuildOptions;

            if ((buildOptions & BuildOptions.EnableHeadlessMode) != 0 &&
                (buildOptions & BuildOptions.Development) != 0)
            {
                problems.Add(new Problem
                (
                    "You cannot have both EnableHeadlessMode and Development build enabled.\n" +
                    "This will crash the Unity Editor during the build.",
                    MessageType.Error
                ));
            }

            if ((buildOptions & BuildOptions.EnableHeadlessMode) != 0 &&
                (environmentConfiguration.BuildPlatforms & ~SpatialBuildPlatforms.Linux) != 0)
            {
                problems.Add(new Problem(
                    "EnableHeadlessMode is only available for Linux builds.",
                    MessageType.Warning
                ));
            }

            var currentAdjustedPlatforms = environmentConfiguration.BuildPlatforms;

            if ((currentAdjustedPlatforms & SpatialBuildPlatforms.Current) != 0)
            {
                currentAdjustedPlatforms |= WorkerBuilder.GetCurrentBuildPlatform();
            }

            if ((currentAdjustedPlatforms & SpatialBuildPlatforms.Windows32) != 0 &&
                (currentAdjustedPlatforms & SpatialBuildPlatforms.Windows64) != 0)
            {
                problems.Add(new Problem(
                    IncompatibleWindowsPlatformsErrorMessage,
                    MessageType.Error
                ));
            }

            var buildTargetsMissingBuildSupport =
                BuildSupportChecker.GetBuildTargetsMissingBuildSupport(
                    WorkerBuilder.GetUnityBuildTargets(environmentConfiguration.BuildPlatforms));

            if (buildTargetsMissingBuildSupport.Length > 0)
            {                
                problems.Add(new Problem(
                        BuildSupportChecker.ConstructMissingSupportMessage(configurationForWorker.WorkerType, environment, buildTargetsMissingBuildSupport),
                        MessageType.Error
                    )
                );
            }

            return problems;
        }
    }
}
