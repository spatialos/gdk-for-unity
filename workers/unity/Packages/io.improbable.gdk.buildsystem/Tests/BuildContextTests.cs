using System;
using System.Collections.Generic;
using Improbable.Gdk.BuildSystem.Configuration;
using NUnit.Framework;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Tests
{
    [TestFixture]
    public class BuildContextTests
    {
        [Test]
        public void GetBuildContexts_for_unconfigured_worker_type_does_not_throw_for_local_build()
        {
            var buildContextFilter = BuildContextFilter.Local("DummyWorkerType");

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(buildContextFilter);
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
                buildContexts = BuildContext.GetBuildContexts(buildContextFilter);
            });

            Assert.AreEqual(0, buildContexts.Count);
        }

#if UNITY_EDITOR_WIN
        [TestCase("UnityClient", 1, BuildTarget.StandaloneWindows64)]
        [TestCase("UnityGameLogic", 1, BuildTarget.StandaloneWindows64)]
        [TestCase("DummyWorkerType", 0, BuildTarget.StandaloneWindows64)]
#elif UNITY_EDITOR_OSX
        [TestCase("UnityClient", 1, BuildTarget.StandaloneOSX)]
        [TestCase("UnityGameLogic", 1, BuildTarget.StandaloneOSX)]
        [TestCase("DummyWorkerType", 0, BuildTarget.StandaloneOSX)]
#endif
        [TestCase("UnityClient", 0, BuildTarget.StandaloneLinux64)]
        [TestCase("UnityGameLogic", 0, BuildTarget.StandaloneLinux64)]
        [TestCase("DummyWorkerType", 0, BuildTarget.StandaloneLinux64)]
        public void GetBuildContexts_for_worker_type_with_filter_returns_expected_number_of_local_contexts(string workerType, int expectedContexts, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextFilter.Local(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(buildContextFilter);
            });

            Assert.AreEqual(expectedContexts, buildContexts.Count);
        }

        [TestCase("UnityClient", 2, BuildTarget.StandaloneWindows64, BuildTarget.StandaloneOSX)]
        [TestCase("UnityClient", 1, BuildTarget.StandaloneWindows64, BuildTarget.StandaloneLinux64)]
        [TestCase("UnityClient", 0, BuildTarget.StandaloneLinux64)]
        [TestCase("UnityGameLogic", 1, BuildTarget.StandaloneLinux64)]
        [TestCase("UnityGameLogic", 0, BuildTarget.StandaloneWindows64)]
        [TestCase("DummyWorkerType", 0, BuildTarget.StandaloneWindows64, BuildTarget.StandaloneLinux64)]
        [TestCase("DummyWorkerType", 0, BuildTarget.StandaloneLinux64)]
        public void GetBuildContexts_for_worker_type_with_filter_returns_expected_number_of_cloud_contexts(string workerType, int expectedContexts, params BuildTarget[] buildTargetFilter)
        {
            var buildContextFilter = BuildContextFilter.Cloud(workerType);
            buildContextFilter.BuildTargetFilter = buildTargetFilter;

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(buildContextFilter);
            });

            Assert.AreEqual(expectedContexts, buildContexts.Count);
        }

#if UNITY_EDITOR_WIN
        [TestCase("UnityClient", BuildTarget.StandaloneWindows64)]
        [TestCase("UnityGameLogic", BuildTarget.StandaloneWindows64)]
#elif UNITY_EDITOR_OSX
        [TestCase("UnityClient", BuildTarget.StandaloneOSX)]
        [TestCase("UnityGameLogic", BuildTarget.StandaloneOSX)]
#endif
        public void GetBuildContexts_for_valid_worker_type_without_filter_returns_all_expected_local_contexts(string workerType, params BuildTarget[] expectedTargets)
        {
            var buildContextFilter = BuildContextFilter.Local(workerType);

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(buildContextFilter);
            });

            Assert.AreEqual(expectedTargets.Length, buildContexts.Count);

            for (var i = 0; i < buildContexts.Count; i++)
            {
                Assert.AreEqual(workerType, buildContexts[i].WorkerType);
                Assert.AreEqual(expectedTargets[i], buildContexts[i].BuildTargetConfig.Target);
            }
        }

        [TestCase("UnityClient", BuildTarget.StandaloneWindows64, BuildTarget.StandaloneOSX)]
        [TestCase("UnityGameLogic", BuildTarget.StandaloneLinux64)]
        public void GetBuildContexts_for_valid_worker_type_without_filter_returns_all_expected_cloud_contexts(string workerType, params BuildTarget[] expectedTargets)
        {
            var buildContextFilter = BuildContextFilter.Cloud(workerType);

            List<BuildContext> buildContexts = null;
            Assert.DoesNotThrow(() =>
            {
                buildContexts = BuildContext.GetBuildContexts(buildContextFilter);
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
