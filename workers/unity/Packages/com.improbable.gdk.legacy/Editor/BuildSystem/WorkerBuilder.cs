using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Legacy.BuildSystem.Configuration;
using Improbable.Gdk.Legacy.BuildSystem.Util;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Improbable.Gdk.Legacy.BuildSystem
{
    public static class WorkerBuilder
    {
        private static readonly string PlayerBuildDirectory =
            Path.GetFullPath(PathUtil.Combine(Directory.GetCurrentDirectory(), EditorPaths.AssetDatabaseDirectory,
                "worker"));

        /// <summary>
        ///     Build method that is invoked by commandline
        /// </summary>
        public static void Build()
        {
            try
            {
                var commandLine = Environment.GetCommandLineArgs();

                Debug.LogFormat("Want to build with args: {0}", String.Join(", ", commandLine));

                var buildTargetArg =
                    CommandLineUtility.GetCommandLineValue(commandLine, "buildTarget", "local");

                if (string.IsNullOrEmpty(buildTargetArg))
                {
                    // The default above does not get filled when -t parameter is not passed
                    buildTargetArg = BuildEnvironment.Local.ToString();
                    Debug.LogWarningFormat("Using default build target value: \"{0}\".", buildTargetArg);
                }

                BuildEnvironment buildEnvironment;

                switch (buildTargetArg.ToLower())
                {
                    case "cloud":
                        buildEnvironment = BuildEnvironment.Cloud;
                        break;
                    case "local":
                        buildEnvironment = BuildEnvironment.Local;
                        break;
                    default:
                        throw new BuildFailedException("Unknown build target value: " + buildTargetArg);
                }

                var workerTypesArg =
                    CommandLineUtility.GetCommandLineValue(commandLine, BuildConfigNames.BuildWorkerTypes,
                        "UnityClient,UnityGameLogic");

                var wantedWorkerPlatforms = GetWorkerPlatforms(workerTypesArg);

                SpatialCommands.GenerateBuildConfiguration();

                foreach (var workerPlatform in wantedWorkerPlatforms)
                {
                    BuildWorkerForEnvironment(workerPlatform, buildEnvironment);
                }
            }
            catch (Exception e)
            {
                // Log the exception so it appears in the command line, and rethrow as a BuildFailedException so the build fails.
                Debug.LogException(e);

                if (e is BuildFailedException)
                {
                    throw;
                }

                throw new BuildFailedException(e);
            }
        }

        internal static WorkerPlatform[] GetWorkerPlatforms(string workerTypesArg)
        {
            WorkerPlatform[] wantedWorkerPlatforms;

            try
            {
                wantedWorkerPlatforms = workerTypesArg.Split(',')
                    .Select(workerType => new WorkerPlatform(workerType.Trim()))
                    .Distinct()
                    .ToArray();

                Array.Sort(wantedWorkerPlatforms);
            }
            catch (ArgumentException innerException)
            {
                throw new ArgumentException(string.Format("Invalid argument: +{0} {1}\n{2}",
                    BuildConfigNames.BuildWorkerTypes, workerTypesArg,
                    innerException.Message));
            }

            return wantedWorkerPlatforms;
        }

        public static void BuildWorkerForEnvironment(WorkerPlatform workerPlatform, BuildEnvironment targetEnvironment)
        {
            var spatialOSBuildConfiguration = GetBuildConfiguration();
            var environmentConfig =
                spatialOSBuildConfiguration.GetEnvironmentConfigForWorker(workerPlatform, targetEnvironment);
            var buildPlatforms = environmentConfig.BuildPlatforms;
            var buildOptions = environmentConfig.BuildOptions;

            PathUtil.EnsureDirectoryExists(PlayerBuildDirectory);

            foreach (var unityBuildTarget in GetUnityBuildTargets(buildPlatforms))
            {
                BuildWorkerForTarget(workerPlatform, unityBuildTarget, buildOptions, targetEnvironment);
            }
        }

        internal const string IncompatibleWindowsPlatformsErrorMessage =
            "Please choose only one of Windows32 or Windows64 as a build platform.";

        internal static IEnumerable<BuildTarget> GetUnityBuildTargets(SpatialBuildPlatforms actualPlatforms)
        {
            var result = new List<BuildTarget>();

            if ((actualPlatforms & SpatialBuildPlatforms.Current) != 0)
            {
                actualPlatforms |= GetCurrentBuildPlatform();
            }

            if ((actualPlatforms & SpatialBuildPlatforms.Linux) != 0)
            {
                result.Add(BuildTarget.StandaloneLinux64);
            }

            if ((actualPlatforms & SpatialBuildPlatforms.OSX) != 0)
            {
                result.Add(BuildTarget.StandaloneOSX);
            }

            if ((actualPlatforms & SpatialBuildPlatforms.Windows32) != 0)
            {
                if ((actualPlatforms & SpatialBuildPlatforms.Windows64) != 0)
                {
                    throw new Exception(IncompatibleWindowsPlatformsErrorMessage);
                }

                result.Add(BuildTarget.StandaloneWindows);
            }

            if ((actualPlatforms & SpatialBuildPlatforms.Windows64) != 0)
            {
                result.Add(BuildTarget.StandaloneWindows64);
            }

            return result.ToArray();
        }

        private static void BuildWorkerForTarget(WorkerPlatform workerPlatform, BuildTarget buildTarget,
            BuildOptions buildOptions, BuildEnvironment targetEnvironment)
        {
            var spatialOSBuildConfiguration = GetBuildConfiguration();

            Debug.LogFormat("Building \"{0}\" for worker platform: \"{1}\", environment: \"{2}\"", buildTarget,
                workerPlatform, targetEnvironment);

            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);

            try
            {
                var workerBuildData = new WorkerBuildData(workerPlatform, buildTarget);
                var scenes = spatialOSBuildConfiguration.GetScenePathsForWorker(workerPlatform);

                var typeSymbol = $"IMPROBABLE_WORKERTYPE_{workerBuildData.WorkerPlatformName.ToUpper()}";
                var workerSymbols = symbols.Split(';')
                    .Concat(new[] { typeSymbol })
                    .Distinct()
                    .Aggregate((current, next) => current + ";" + next);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, workerSymbols);

                var buildPlayerOptions = new BuildPlayerOptions
                {
                    options = buildOptions,
                    target = buildTarget,
                    scenes = scenes,
                    locationPathName = workerBuildData.BuildScratchDirectory
                };

                BuildPipeline.BuildPlayer(buildPlayerOptions);

                var zipPath = Path.GetFullPath(Path.Combine(PlayerBuildDirectory, workerBuildData.PackageName));

                var basePath = PathUtil.Combine(BuildPaths.BuildScratchDirectory, workerBuildData.PackageName);

                SpatialCommands.Zip(zipPath, basePath,
                    targetEnvironment == BuildEnvironment.Local
                        ? PlayerCompression.Disabled
                        : PlayerCompression.Enabled);
            }
            finally
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
            }
        }

        internal const string BuildConfigurationMissingErrorMessage =
            "No objects of type SpatialOSBuildConfiguration found in the project.\nPlease create one using Assets/Create/" +
            SpatialOSBuildConfiguration.CreateMenuPath + ".";

        /// <returns>An instance of SpatialOSBuildConfiguration if one exists.</returns>
        /// <exception cref="Exception">If no assets exist of type SpatialOSBuildConfiguration</exception>
        private static SpatialOSBuildConfiguration GetBuildConfiguration()
        {
            var spatialOSBuildConfiguration = SpatialOSBuildConfiguration.GetInstance();

            if (spatialOSBuildConfiguration == null)
            {
                throw new Exception(BuildConfigurationMissingErrorMessage);
            }

            return spatialOSBuildConfiguration;
        }

        internal static SpatialBuildPlatforms GetCurrentBuildPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    return SpatialBuildPlatforms.Windows64;
                case RuntimePlatform.OSXEditor:
                    return SpatialBuildPlatforms.OSX;
                case RuntimePlatform.LinuxEditor:
                    return SpatialBuildPlatforms.Linux;
                default:
                    throw new Exception($"Unsupported platform detected: {Application.platform}");
            }
        }

        public static void Clean()
        {
            FileUtil.DeleteFileOrDirectory(PlayerBuildDirectory);
            FileUtil.DeleteFileOrDirectory(BuildPaths.BuildScratchDirectory);
        }
    }
}
