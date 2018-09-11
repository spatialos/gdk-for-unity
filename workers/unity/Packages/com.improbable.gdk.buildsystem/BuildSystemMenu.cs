using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.BuildSystem.Configuration;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem
{
    internal static class BuildSystemMenu
    {
        private const string ParentMenu = "SpatialOS";

        [MenuItem(ParentMenu + "/Build UnityClient for local", false, 1)]
        public static void BuildLocalClient()
        {
            MenuBuild(new[] { WorkerPlatform.UnityClient }, BuildEnvironment.Local);
        }

        [MenuItem(ParentMenu + "/Build UnityGameLogic for local", false, 2)]
        public static void BuildLocalWorker()
        {
            MenuBuild(new[] { WorkerPlatform.UnityGameLogic }, BuildEnvironment.Local);
        }

        [MenuItem(ParentMenu + "/Build all workers for local", false, 3)]
        public static void BuildLocalAll()
        {
            var platforms = WorkerBuilder.GetBuildConfiguration().WorkerBuildConfigurations
                .Select((x) => x.WorkerPlatform).ToArray();
            MenuBuild(platforms, BuildEnvironment.Local);
        }

        [MenuItem(ParentMenu + "/Build UnityClient for cloud", false, 14)]
        public static void BuildCloudClient()
        {
            MenuBuild(new[] { WorkerPlatform.UnityClient }, BuildEnvironment.Cloud);
        }

        [MenuItem(ParentMenu + "/Build UnityGameLogic for cloud", false, 15)]
        public static void BuildCloudWorker()
        {
            MenuBuild(new[] { WorkerPlatform.UnityGameLogic }, BuildEnvironment.Cloud);
        }

        [MenuItem(ParentMenu + "/Build all workers for cloud", false, 16)]
        public static void BuildCloudAll()
        {
            var platforms = WorkerBuilder.GetBuildConfiguration().WorkerBuildConfigurations
                .Select((x) => x.WorkerPlatform).ToArray();
            MenuBuild(platforms, BuildEnvironment.Cloud);
        }

        [MenuItem(ParentMenu + "/Clean all workers", false, 27)]
        public static void Clean()
        {
            WorkerBuilder.Clean();
            Debug.Log("Clean completed");
        }

        private static void MenuBuild(IEnumerable<WorkerPlatform> platforms, BuildEnvironment environment)
        {
            // Delaying build by a frame to ensure the editor has re-rendered the UI to avoid odd glitches.
            EditorApplication.delayCall += () =>
            {
                LocalLaunch.BuildConfig();

                foreach (var platform in platforms)
                {
                    WorkerBuilder.BuildWorkerForEnvironment(platform, environment);
                }

                Debug.LogFormat("Completed build for {0} target", environment);
            };
        }
    }
}
