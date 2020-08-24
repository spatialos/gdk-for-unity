using System.Collections.Generic;
using Improbable.Gdk.BuildSystem.Configuration;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Tests
{
    [TestFixture]
    public class LocalBuildContextTests : BuildContextTestBase
    {
        [TestCase(ClientType, 1, LocalTarget)]
        [TestCase(ServerType, 1, LocalTarget)]
        [TestCase(MobileClientType, 2, AndroidTarget, IOSTarget)]
        [TestCase(MobileClientType, 1, IOSTarget)]
        public void GetBuildContexts_for_valid_worker_type_with_valid_filter_returns_expected_number_of_local_contexts(string workerType, int expectedContexts, params BuildTarget[] buildTargetFilter)
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

        [TestCase(ClientType, LinuxTarget)]
        [TestCase(ServerType, LinuxTarget)]
        [TestCase(MobileClientType, LinuxTarget)]
        public void GetBuildContexts_for_valid_worker_type_with_invalid_filter_returns_zero_local_contexts(string workerType, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextFilter.Local(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }

        [TestCase(ClientType, LocalTarget)]
        [TestCase(ServerType, LocalTarget)]
        [TestCase(MobileClientType, AndroidTarget, IOSTarget)]
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

        [TestCase(InvalidWorkerType, LocalTarget)]
        public void GetBuildContexts_for_unknown_worker_type_with_valid_filter_returns_zero_local_contexts(string workerType, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextFilter.Local(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }

        [TestCase(InvalidWorkerType, LinuxTarget)]
        public void GetBuildContexts_for_unknown_worker_type_with_invalid_filter_returns_zero_local_contexts(string workerType, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextFilter.Local(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(ValidBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }

        [TestCase(InvalidWorkerType)]
        public void GetBuildContexts_for_unknown_worker_type_without_filter_returns_zero_local_contexts(string workerType)
        {
            var buildContextFilter = BuildContextFilter.Local(workerType);

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(EmptyBuildConfig, buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }
    }
}
