using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.BuildSystem.Configuration;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem
{
    public static class WorkerBuilder
    {
        private static BuildContext? currentContext;

        public static BuildContext CurrentContext
        {
            get
            {
                if (!currentContext.HasValue)
                {
                    throw new InvalidOperationException(
                        "CurrentContext is only valid while building a SpatialOS worker.");
                }

                return currentContext.Value;
            }
        }

        /// <summary>
        ///     Build method that is invoked by commandline
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static void Build()
        {
            try
            {
                var args = CommandLineArgs.FromCommandLine();

                // Parse command line arguments
                var buildTargetFilter = CommandlineParser.GetBuildTargetFilter(args);
                var wantedWorkerTypes = CommandlineParser.GetWorkerTypesToBuild(args);
                var scriptImplementation = CommandlineParser.GetScriptingImplementation(args);
                var buildEnvironment = CommandlineParser.GetBuildEnvironment(args);

                // Create BuildContext for each worker
                var buildContexts = GetBuildContexts(wantedWorkerTypes, buildEnvironment, scriptImplementation,
                    buildTargetFilter);

                if (buildContexts.Count == 0)
                {
                    throw new BuildFailedException(
                        $"Attempted a build with no valid targets!");
                }

                var buildsSucceeded = BuildWorkers(buildContexts);
                if (!buildsSucceeded)
                {
                    throw new BuildFailedException(
                        "Not all builds were completed successfully. See the log for more information.");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                if (e is BuildFailedException)
                {
                    throw;
                }

                throw new BuildFailedException(e);
            }
        }

        private static List<BuildContext> GetBuildContexts(IEnumerable<string> wantedWorkerTypes,
            BuildEnvironment buildEnvironment, ScriptingImplementation? scriptImplementation = null,
            ICollection<BuildTarget> buildTargetFilter = null)
        {
            var spatialOSBuildConfiguration = BuildConfig.GetInstance();
            var result = new List<BuildContext>();

            foreach (var workerType in wantedWorkerTypes)
            {
                var environmentConfig =
                    spatialOSBuildConfiguration.GetEnvironmentConfigForWorker(workerType, buildEnvironment);

                // Filter targets for CI
                var targetConfigs = environmentConfig.BuildTargets
                    .Where(t => t.Enabled && (buildTargetFilter?.Contains(t.Target) ?? true))
                    .ToList();

                // Which build targets are not supported by current install?
                var missingTargets = targetConfigs
                    .Where(c => !WorkerBuildData.BuildTargetsThatCanBeBuilt.ContainsKey(c.Target))
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
                    ScriptingImplementation = scriptImplementation ?? ScriptingImplementation.Mono2x,
                    BuildTargetConfig = targetConfig
                }));
            }

            return result;
        }

        internal static void MenuBuild(BuildEnvironment environment, params string[] workerTypes)
        {
            // Delaying build by a frame to ensure the editor has re-rendered the UI to avoid odd glitches.
            EditorApplication.delayCall += () =>
            {
                try
                {
                    // TODO READ FROM SETTINGS
                    var buildContexts = GetBuildContexts(workerTypes, environment, null, null);
                    BuildWorkers(buildContexts);
                }
                catch (Exception)
                {
                    EditorUtility.DisplayDialog("Build Failed",
                        "Build failed. Please see the Unity Console Window for information.",
                        "OK");

                    throw;
                }
            };
        }

        private static bool BuildWorkers(IReadOnlyList<BuildContext> buildContexts)
        {
            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            var activeBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(activeBuildTarget);

            if (BuildConfig.GetInstance() == null)
            {
                const string errorMessage =
                    "Could not find an instance of the SpatialOS Build Configuration.\n\nPlease create one via Assets > Create > SpatialOS > SpatialOS Build Configuration.\n\nIf you already have an instance of the SpatialOS Build Configuration in your project, please open it in the Unity Inspector to force the asset to load and retry the build.";

                if (Application.isEditor)
                {
                    EditorApplication.delayCall += () =>
                    {
                        EditorUtility.DisplayDialog("Could not find SpatialOS Build Configuration",
                            errorMessage,
                            "OK");
                    };
                }

                Debug.LogError(errorMessage);
                return false;
            }

            if (!Directory.Exists(EditorPaths.PlayerBuildDirectory))
            {
                Directory.CreateDirectory(EditorPaths.PlayerBuildDirectory);
            }

            try
            {
                LocalLaunch.BuildConfig();

                var workerResults = new Dictionary<BuildContext, bool>();
                foreach (var buildContext in buildContexts)
                {
                    var result = BuildWorkerWithContext(buildContext);
                    workerResults[buildContext] = result;
                }

                var missingWorkerTypes = string.Join(" ",
                    workerResults.Keys.Where(k => !workerResults[k]).Select(k => k.WorkerType));
                var completedWorkerTypes = string.Join(" ",
                    workerResults.Keys.Where(k => workerResults[k]).Select(k => k.WorkerType));

                if (missingWorkerTypes.Length > 0)
                {
                    Debug.LogWarning(
                        $"Completed build for {buildContexts[0].BuildEnvironment} target.\n"
                        + $"Completed builds for: {completedWorkerTypes}\n"
                        + $"Skipped builds for: {missingWorkerTypes}. See above for more information.");
                    return false;
                }
                else
                {
                    Debug.Log($"Completed build for {buildContexts[0].BuildEnvironment} target.");
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new BuildFailedException(e);
            }
            finally
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(activeBuildTargetGroup, activeBuildTarget);
            }
        }

        public static void Clean()
        {
            // Delete all but the schema directory where the schema descriptor is placed.
            if (Directory.Exists(EditorPaths.SpatialAssemblyDirectory))
            {
                var children = new DirectoryInfo(EditorPaths.SpatialAssemblyDirectory).GetDirectories();

                foreach (var child in children)
                {
                    if (child.Name != "schema")
                    {
                        Directory.Delete(child.FullName, true);
                    }
                }
            }

            if (Directory.Exists(Common.BuildScratchDirectory))
            {
                Directory.Delete(Common.BuildScratchDirectory, true);
            }
        }

        private static bool BuildWorkerWithContext(
            BuildContext buildContext)
        {
            var spatialOSBuildConfiguration = BuildConfig.GetInstance();

            Debug.Log(
                $"Building \"{buildContext.WorkerType}\" for platform: \"{buildContext.BuildTargetConfig.Target}\", environment: \"{buildContext.BuildEnvironment}\"");

            var workerBuildData = new WorkerBuildData(buildContext.WorkerType, buildContext.BuildTargetConfig.Target);
            var scenes = spatialOSBuildConfiguration.GetScenePathsForWorker(buildContext.WorkerType);

            var buildPlayerOptions = new BuildPlayerOptions
            {
                options = buildContext.BuildTargetConfig.Options,
                target = buildContext.BuildTargetConfig.Target,
                scenes = scenes,
                locationPathName = workerBuildData.BuildScratchDirectory
            };

            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildContext.BuildTargetConfig.Target);
            var activeScriptingBackend = PlayerSettings.GetScriptingBackend(buildTargetGroup);
            if (activeScriptingBackend != buildContext.ScriptingImplementation)
            {
                PlayerSettings.SetScriptingBackend(buildTargetGroup, buildContext.ScriptingImplementation);
            }

            try
            {
                currentContext = buildContext;
                var result = BuildPipeline.BuildPlayer(buildPlayerOptions);
                if (result.summary.result != BuildResult.Succeeded)
                {
                    if (buildContext.BuildTargetConfig.Target == BuildTarget.Android &&
                        string.IsNullOrEmpty(EditorPrefs.GetString("AndroidSdkRoot")))
                    {
                        Debug.LogWarning($"Unable to build worker {buildContext.WorkerType} for platform Android. " +
                            $"Ensure you have the Android SDK set inside the Unity Editor Preferences.");
                        return false;
                    }

                    throw new BuildFailedException($"Build failed for {buildContext.WorkerType}");
                }
            }
            catch (BuildFailedException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new BuildFailedException(e);
            }
            finally
            {
                currentContext = null;
                PlayerSettings.SetScriptingBackend(buildTargetGroup, activeScriptingBackend);
            }

            if (buildContext.BuildTargetConfig.Target == BuildTarget.StandaloneOSX)
            {
                // Unity executable name has changed on MacOS
                // This is a temp work around our Launcher trying to run the wrong file.
                // See [UTY-2294]
                CreateLaunchJson(workerBuildData.PackageName, Application.productName);
            }

            // Package up standalone builds
            if (buildContext.BuildTargetConfig.Target != BuildTarget.Android &&
                buildContext.BuildTargetConfig.Target != BuildTarget.iOS)
            {
                Package(workerBuildData.PackageName, useCompression: buildContext.BuildEnvironment == BuildEnvironment.Cloud);
            }

            return true;
        }

        private static void CreateLaunchJson(string packageName, string productName)
        {
            var basePath = Path.Combine(Common.BuildScratchDirectory, packageName);
            using (var jsonWriter = File.CreateText(Path.Combine(basePath, "launcher_client_config.json")))
            {
                var json = @"{
  ""command"": " + $"\"./{packageName}.app/Contents/MacOS/{productName}\"," + @"
  ""arguments"": [
    ""+projectName"", ""${IMPROBABLE_PROJECT_NAME}"",
    ""+deploymentName"", ""${IMPROBABLE_DEPLOYMENT_NAME}"",
    ""+loginToken"", ""${IMPROBABLE_LOGIN_TOKEN}"",
    ""+playerIdentityToken"", ""${IMPROBABLE_PLAYER_IDENTITY_TOKEN}"",
    ""+locatorHost"", ""${IMPROBABLE_LOCATOR_HOSTNAME}""
  ]
}";
                jsonWriter.Write(json);
            }
        }

        private static void Package(string packageName, bool useCompression)
        {
            var zipPath = Path.Combine(EditorPaths.PlayerBuildDirectory, packageName);
            var basePath = Path.Combine(Common.BuildScratchDirectory, packageName);

            using (new ShowProgressBarScope($"Package {basePath}"))
            {
                RedirectedProcess.Command(Common.SpatialBinary)
                    .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                    .WithArgs("file", "zip", $"--output=\"{Path.GetFullPath(zipPath)}\"",
                        $"--basePath=\"{Path.GetFullPath(basePath)}\"", "\"**\"",
                        $"--compression={useCompression}")
                    .Run();
            }
        }
    }
}
