using System;
using System.Linq;
using System.Net;
using NUnit.Framework;

namespace Improbable.Gdk.TestUtils.Editor.Tests
{
    [TestFixture]
    public class SpatialDeploymentManagerTests
    {
        [Test]
        public void Start_throws_an_exception_if_deployment_config_does_not_exist()
        {
            var task = SpatialDeploymentManager.Start("does_not_exist.json", "snapshots/default.snapshot");
            Assert.IsTrue(task.IsFaulted);
            Assert.IsInstanceOf<ArgumentException>(task.Exception.InnerExceptions.First());
        }

        [Test]
        public void Start_throws_an_exception_if_snapshot_does_not_exist()
        {
            var task = SpatialDeploymentManager.Start("default_launch.json", "does_not_exist.snapshot");
            Assert.IsTrue(task.IsFaulted);
            Assert.IsInstanceOf<ArgumentException>(task.Exception.InnerExceptions.First());
        }

        [Test]
        public void Start_correctly_starts_a_spatial_deployment()
        {
            using (SpatialDeploymentManager.Start("default_launch.json", "snapshots/default.snapshot").Result)
            {
                var request = WebRequest.Create("http://localhost:21000/inspector");
                Assert.DoesNotThrow(() => request.GetResponse());
            }
        }

        [Test]
        public void Deployment_is_dead_after_Dispose()
        {
            using (SpatialDeploymentManager.Start("default_launch.json", "snapshots/default.snapshot").Result)
            {
            }

            var request = WebRequest.Create("http://localhost:21000/inspector");
            Assert.Throws<WebException>(() => request.GetResponse());
        }

        [Test]
        public void Start_throws_if_deployment_fails_to_start()
        {
            using (SpatialDeploymentManager.Start("default_launch.json", "snapshots/default.snapshot").Result)
            {
                var task = SpatialDeploymentManager.Start("default_launch.json", "snapshots/default.snapshot");
                Assert.Throws<AggregateException>(() => task.Wait());
            }
        }
    }
}
