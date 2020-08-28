using System.Collections.Generic;
using Improbable.Gdk.BuildSystem.Configuration;
using NUnit.Framework;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Tests
{
    [TestFixture]
    public class CloudBuildContextTests : BuildContextTestBase
    {
        [TestCase(ClientType, 2, WindowsTarget, MacTarget)]
        [TestCase(ClientType, 1, WindowsTarget, LinuxTarget)]
        [TestCase(ServerType, 1, LinuxTarget)]
        [TestCase(MobileClientType, 2, AndroidTarget, IOSTarget)]
        [TestCase(MobileClientType, 1, IOSTarget)]
        public void GetBuildContexts_for_valid_worker_type_with_valid_filter_returns_expected_number_of_cloud_contexts(string workerType, int expectedContexts, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextSettings.Cloud(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(expectedContexts, buildContexts.Count);
        }

        [TestCase(ClientType, LinuxTarget)]
        [TestCase(ServerType, WindowsTarget)]
        [TestCase(MobileClientType, LinuxTarget)]
        public void GetBuildContexts_for_valid_worker_type_with_invalid_filter_returns_zero_cloud_contexts(string workerType, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextSettings.Cloud(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }

        [TestCase(ClientType, WindowsTarget, MacTarget)]
        [TestCase(ServerType, LinuxTarget)]
        [TestCase(MobileClientType, AndroidTarget, IOSTarget)]
        public void GetBuildContexts_for_valid_worker_type_without_filter_returns_all_expected_cloud_contexts(string workerType, params BuildTarget[] expectedTargets)
        {
            var buildContextFilter = BuildContextSettings.Cloud(workerType);

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

        [TestCase(InvalidWorkerType, WindowsTarget, MacTarget, LinuxTarget)]
        public void GetBuildContexts_for_unknown_worker_type_with_valid_filter_returns_zero_cloud_contexts(string workerType, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextSettings.Cloud(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }

        [TestCase(InvalidWorkerType, LinuxTarget)]
        public void GetBuildContexts_for_unknown_worker_type_with_invalid_filter_returns_zero_cloud_contexts(string workerType, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextSettings.Cloud(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }

        [TestCase(InvalidWorkerType)]
        public void GetBuildContexts_for_unknown_worker_type_without_filter_returns_zero_cloud_contexts(string workerType)
        {
            var buildContextFilter = BuildContextSettings.Cloud(workerType);

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(EmptyBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }
    }
}
