using System;
using Improbable.Gdk.BuildSystem.Configuration;
using NUnit.Framework;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Tests
{
    [TestFixture]
    public class BuildContextTests
    {
        private static readonly string[] AllWorkers = new string[]
        {
            "MobileClient",
            "UnityClient",
            "UnityGameLogic"
        };

        [Test]
        public void NoExceptionIfMissingWorkerType()
        {
            var buildContextFilter = new BuildContextFilter(new[] { "DummyWorkerType" },
                BuildEnvironment.Cloud);

            Assert.Throws<NullReferenceException>(() =>
            {
                BuildContext.GetBuildContexts(buildContextFilter);
            });
        }

        [Test]
        public void NoExceptionIfMissingWorkerType2()
        {
            var buildContextFilter = new BuildContextFilter
            {
                WantedWorkerTypes = new[] { "UnityClient" },
                BuildEnvironment = BuildEnvironment.Cloud,
                BuildTargetFilter = new[] { BuildTarget.StandaloneLinux64 }
            };

            Assert.Throws<NullReferenceException>(() =>
            {
                BuildContext.GetBuildContexts(buildContextFilter);
            });
        }
    }
}
