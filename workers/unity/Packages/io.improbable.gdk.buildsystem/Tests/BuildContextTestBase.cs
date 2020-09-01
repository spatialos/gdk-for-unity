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
                localConfigs: new[] { new BuildTargetConfig(LocalTarget) },
                cloudConfigs: new[] { new BuildTargetConfig(LinuxTarget) })
            .AddWorker(ClientType,
                localConfigs: new[] { new BuildTargetConfig(LocalTarget) },
                cloudConfigs: new[] { new BuildTargetConfig(WindowsTarget), new BuildTargetConfig(MacTarget) })
            .AddWorker(MobileClientType,
                localConfigs: new[] { new BuildTargetConfig(BuildTarget.Android), new BuildTargetConfig(BuildTarget.iOS) },
                cloudConfigs: new[] { new BuildTargetConfig(BuildTarget.Android), new BuildTargetConfig(BuildTarget.iOS) });
    }
}
