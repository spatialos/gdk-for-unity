using Improbable.Gdk.BuildSystem.Configuration;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Tests
{
    public class BuildContextTestBase
    {
        internal const string ClientType = "UnityClient";
        internal const string ServerType = "UnityGameLogic";
        internal const string MobileClientType = "MobileClient";
        internal const string InvalidWorkerType = "DummyWorkerType";

        internal const BuildTarget WindowsTarget = BuildTarget.StandaloneWindows64;
        internal const BuildTarget MacTarget = BuildTarget.StandaloneOSX;
        internal const BuildTarget LinuxTarget = BuildTarget.StandaloneLinux64;

#if UNITY_EDITOR_WIN
        internal const BuildTarget LocalTarget = WindowsTarget;
#elif UNITY_EDITOR_OSX
        internal const BuildTarget LocalTarget = MacTarget;
#endif

        internal const BuildTarget AndroidTarget = BuildTarget.Android;
        internal const BuildTarget IOSTarget = BuildTarget.iOS;

        internal static readonly BuildConfig EmptyBuildConfig = ScriptableObject.CreateInstance<BuildConfig>();

        internal static readonly BuildConfig ValidBuildConfig = ScriptableObject.CreateInstance<BuildConfig>()
            .AddWorker(ServerType,
                localConfigs: new[] { CreateTarget(LocalTarget) },
                cloudConfigs: new[] { CreateTarget(LinuxTarget) })
            .AddWorker(ClientType,
                localConfigs: new[] { CreateTarget(LocalTarget) },
                cloudConfigs: new[] { CreateTarget(WindowsTarget), CreateTarget(MacTarget) })
            .AddWorker(MobileClientType,
                localConfigs: new[] { CreateTarget(BuildTarget.Android), CreateTarget(BuildTarget.iOS) },
                cloudConfigs: new[] { CreateTarget(BuildTarget.Android), CreateTarget(BuildTarget.iOS) });

        private static BuildTargetConfig CreateTarget(
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
