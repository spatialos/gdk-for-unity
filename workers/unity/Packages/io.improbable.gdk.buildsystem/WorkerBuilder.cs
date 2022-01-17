using System;
using System.Collections.Generic;
using System.IO;
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

        public static BuildContext CurrentContext =>
            currentContext ?? throw new InvalidOperationException(
                "CurrentContext is only valid while building a SpatialOS worker.");

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
                var buildContextFilter = BuildContextSettings.FromCommandLine(args);

                // Create BuildContext for each worker
                var buildConfig = BuildConfig.GetInstance();
                var buildContexts = BuildContext.GetBuildContexts(buildConfig, buildContextFilter);

                if (buildContexts.Count == 0)
                {
                    throw new BuildFailedException("Attempted a build with no valid targets!");
                }

                BuildWorkers(buildContexts);
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

        internal static void MenuBuild(BuildContextSettings buildContextSettings)
        {
            // Delaying build by a frame to ensure the editor has re-rendered the UI to avoid odd glitches.
            EditorApplication.delayCall += () =>
            {
                try
                {
                    var buildConfig = BuildConfig.GetInstance();
                    var buildContexts = BuildContext.GetBuildContexts(buildConfig, buildContextSettings);
                    BuildWorkers(buildContexts);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    EditorUtility.DisplayDialog("Build Failed",
                        "Build failed. Please see the Unity Console Window for information.",
                        "OK");

                    throw;
                }
            };
        }

        private static void BuildWorkers(IReadOnlyList<BuildContext> buildContexts)
        {
            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            var activeBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(activeBuildTarget);

            if (BuildConfig.GetInstance() == null)
            {
                const string errorMessage =
                    "Could not find an instance of the SpatialOS Build Configuration.\n\nPlease create one via Assets > Create > SpatialOS > SpatialOS Build Configuration.\n\nIf you already have an instance of the SpatialOS Build Configuration in your project, please open it in the Unity Inspector to force the asset to load and retry the build.";

                throw new BuildFailedException(errorMessage);
            }

            if (!Directory.Exists(EditorPaths.PlayerBuildDirectory))
            {
                Directory.CreateDirectory(EditorPaths.PlayerBuildDirectory);
            }

            try
            {
                LocalLaunch.BuildConfig();

                foreach (var buildContext in buildContexts)
                {
                    BuildWorkerWithContext(buildContext);
                }

                Debug.Log($"Completed build for {buildContexts[0].BuildEnvironment} target.");
            }
            catch (Exception e)
            {
                if (e is BuildFailedException)
                {
                    throw;
                }

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

        private static void BuildWorkerWithContext(
            BuildContext buildContext)
        {
            var spatialOsBuildConfiguration = BuildConfig.GetInstance();

            Debug.Log(
                $"Building \"{buildContext.WorkerType}\" for platform: \"{buildContext.BuildTargetConfig.Target}\", environment: \"{buildContext.BuildEnvironment}\"");

            var workerBuildData = new WorkerBuildData(buildContext.WorkerType, buildContext.BuildTargetConfig.Target);
            var scenes = spatialOsBuildConfiguration.GetScenePathsForWorker(buildContext.WorkerType);

            var buildPlayerOptions = new BuildPlayerOptions
            {
                options = buildContext.BuildTargetConfig.Options,
                target = buildContext.BuildTargetConfig.Target,
                scenes = scenes,
                locationPathName = workerBuildData.BuildScratchDirectory,
                extraScriptingDefines = new[] { $"WORKERTYPE_{buildContext.WorkerType}".ToUpper() }
            };

            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildContext.BuildTargetConfig.Target);
            var activeScriptingBackend = PlayerSettings.GetScriptingBackend(buildTargetGroup);
            if (activeScriptingBackend != buildContext.ScriptingImplementation)
            {
                PlayerSettings.SetScriptingBackend(buildTargetGroup, buildContext.ScriptingImplementation);
            }

            var activeIOSSdkVersion = PlayerSettings.iOS.sdkVersion;
            if (buildContext.IOSSdkVersion.HasValue && activeIOSSdkVersion != buildContext.IOSSdkVersion)
            {
                PlayerSettings.iOS.sdkVersion = buildContext.IOSSdkVersion.Value;
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
                    }

                    throw new BuildFailedException($"Build failed for {buildContext.WorkerType}");
                }
            }
            catch (Exception e)
            {
                if (e is BuildFailedException)
                {
                    throw;
                }

                throw new BuildFailedException(e);
            }
            finally
            {
                currentContext = null;
                PlayerSettings.iOS.sdkVersion = activeIOSSdkVersion;
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
                Package(workerBuildData.PackageName, buildContext.BuildEnvironment == BuildEnvironment.Cloud);
            }
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
                RedirectedProcess
                    .Spatial("file", "zip")
                    .WithArgs($"--output=\"{Path.GetFullPath(zipPath)}\"",
                        $"--basePath=\"{Path.GetFullPath(basePath)}\"", "\"**\"",
                        $"--compression={useCompression}")
                    .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                    .Run();
            }
        }
    }
}
