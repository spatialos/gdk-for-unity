using System.Collections.Generic;
using Improbable.Gdk.BuildSystem.Configuration;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Tests
{
    [TestFixture]
    public class BuildContextTests
    {
        private const string ClientType = "UnityClient";
        private const string ServerType = "UnityGameLogic";
        private const string MobileClientType = "MobileClient";

        private const BuildTarget WindowsTarget = BuildTarget.StandaloneWindows64;
        private const BuildTarget MacTarget = BuildTarget.StandaloneOSX;
        private const BuildTarget LinuxTarget = BuildTarget.StandaloneLinux64;

        private const BuildTarget AndroidTarget = BuildTarget.Android;
        private const BuildTarget iOSTarget = BuildTarget.iOS;

        private static readonly BuildConfig EmptyBuildConfig = ScriptableObject.CreateInstance<BuildConfig>();

        private static readonly BuildConfig ValidBuildConfig = ScriptableObject.CreateInstance<BuildConfig>()
            .AddWorker(ClientType, new[]
            {
                TestUtils.CreateTarget(WindowsTarget), TestUtils.CreateTarget(MacTarget)
            })
            .AddWorker(ServerType, WorkerBuildData.GetLinuxBuildTargetConfig());

        [Test]
        public void GetBuildContexts_for_unconfigured_worker_type_does_not_throw_for_local_build()
        {
            var buildContextFilter = BuildContextFilter.Local("DummyWorkerType");

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(EmptyBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }

        [Test]
        public void GetBuildContexts_for_unconfigured_worker_type_does_not_throw_for_cloud_build()
        {
            var buildContextFilter = BuildContextFilter.Cloud("DummyWorkerType");

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(EmptyBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }

#if UNITY_EDITOR_WIN
        [TestCase(ClientType, 1, WindowsTarget)]
        [TestCase(ServerType, 1, WindowsTarget)]
        [TestCase("DummyWorkerType", 0, WindowsTarget)]
#elif UNITY_EDITOR_OSX
        [TestCase(ClientType, 1, MacTarget)]
        [TestCase(ServerType, 1, MacTarget)]
        [TestCase("DummyWorkerType", 0, MacTarget)]
#endif
        [TestCase(ClientType, 0, LinuxTarget)]
        [TestCase(ServerType, 0, LinuxTarget)]
        [TestCase(MobileClientType, 2, AndroidTarget, iOSTarget)]
        [TestCase(MobileClientType, 1, iOSTarget)]
        [TestCase(MobileClientType, 0, LinuxTarget)]
        [TestCase("DummyWorkerType", 0, LinuxTarget)]
        public void GetBuildContexts_for_worker_type_with_filter_returns_expected_number_of_local_contexts(string workerType, int expectedContexts, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextFilter.Local(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(expectedContexts, buildContexts.Count);
        }

        [TestCase(ClientType, 2, WindowsTarget, MacTarget)]
        [TestCase(ClientType, 1, WindowsTarget, LinuxTarget)]
        [TestCase(ClientType, 0, LinuxTarget)]
        [TestCase(ServerType, 1, LinuxTarget)]
        [TestCase(ServerType, 0, WindowsTarget)]
        [TestCase(MobileClientType, 2, AndroidTarget, iOSTarget)]
        [TestCase(MobileClientType, 1, iOSTarget)]
        [TestCase(MobileClientType, 0, LinuxTarget)]
        [TestCase("DummyWorkerType", 0, WindowsTarget, LinuxTarget)]
        [TestCase("DummyWorkerType", 0, LinuxTarget)]
        public void GetBuildContexts_for_worker_type_with_filter_returns_expected_number_of_cloud_contexts(string workerType, int expectedContexts, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextFilter.Cloud(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(expectedContexts, buildContexts.Count);
        }

#if UNITY_EDITOR_WIN
        [TestCase(ClientType, WindowsTarget)]
        [TestCase(ServerType, WindowsTarget)]
#elif UNITY_EDITOR_OSX
        [TestCase(ClientType, MacTarget)]
        [TestCase(ServerType, MacTarget)]
#endif
        [TestCase(MobileClientType, AndroidTarget, iOSTarget)]
        public void GetBuildContexts_for_valid_worker_type_without_filter_returns_all_expected_local_contexts(string workerType, params BuildTarget[] expectedTargets)
        {
            var buildContextFilter = BuildContextFilter.Local(workerType);

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(expectedTargets.Length, buildContexts.Count);

            for (var i = 0; i < buildContexts.Count; i++)
            {
                Assert.AreEqual(workerType, buildContexts[i].WorkerType);
                Assert.AreEqual(expectedTargets[i], buildContexts[i].BuildTargetConfig.Target);
            }
        }

        [TestCase(ClientType, WindowsTarget, MacTarget)]
        [TestCase(ServerType, LinuxTarget)]
        [TestCase(MobileClientType, AndroidTarget, iOSTarget)]
        public void GetBuildContexts_for_valid_worker_type_without_filter_returns_all_expected_cloud_contexts(string workerType, params BuildTarget[] expectedTargets)
        {
            var buildContextFilter = BuildContextFilter.Cloud(workerType);

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(expectedTargets.Length, buildContexts.Count);

            for (var i = 0; i < buildContexts.Count; i++)
            {
                Assert.AreEqual(workerType, buildContexts[i].WorkerType);
                Assert.AreEqual(expectedTargets[i], buildContexts[i].BuildTargetConfig.Target);
            }
        }
    }
}
