using System;
using System.IO;
using Improbable.Gdk.TestUtils.Editor;
using Improbable.Gdk.Tools;
using Improbable.Worker.CInterop;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Connection
{
    [TestFixture]
    public class ReceptionistFlowTests
    {
        private SpatialDeploymentManager deploymentManager;
        private Improbable.Worker.CInterop.Connection connection;

        private static readonly string LaunchConfigPath = Path.Combine(Common.SpatialProjectRootDir, "default_launch.json");

        private static readonly string SnapshotPath =
            Path.Combine(Common.SpatialProjectRootDir, "snapshots", "default.snapshot");

        private static readonly string RuntimeVersion = GdkToolsConfiguration.GetOrCreateInstance().RuntimeVersion;

        [SetUp]
        public void Setup()
        {
            deploymentManager = SpatialDeploymentManager.Start(LaunchConfigPath, SnapshotPath, RuntimeVersion).Result;
        }

        [TearDown]
        public void TearDown()
        {
            deploymentManager?.Dispose();
            connection?.Dispose();
        }

        [Test]
        public void ReceptionistFlow_connects_to_local_deployment_by_default()
        {
            var receptionistFlow = new ReceptionistFlow("My-Worker-Id");
            Assert.DoesNotThrow(() => connection = receptionistFlow.CreateAsync(GetConnectionParameters()).Result);
            Assert.AreEqual(ConnectionStatusCode.Success, connection.GetConnectionStatusCode());
        }

        [Test]
        public void ReceptionistFlow_propagates_my_worker_id()
        {
            var receptionistFlow = new ReceptionistFlow("My-Worker-Id");
            connection = receptionistFlow.CreateAsync(GetConnectionParameters()).Result;

            Assert.AreEqual("My-Worker-Id", connection.GetWorkerId());
        }

        [Test]
        public void ReceptionistFlow_fails_to_connect_with_invalid_host()
        {
            var receptionistFlow = new ReceptionistFlow("My-Worker-Id")
            {
                ReceptionistHost = "not-localhost"
            };

            var aggregateException = Assert.Throws<AggregateException>(() =>
                connection = receptionistFlow.CreateAsync(GetConnectionParameters()).Result);
            Assert.IsInstanceOf<ConnectionFailedException>(aggregateException.InnerExceptions[0]);
        }

        [Test]
        public void ReceptionistFlow_fails_to_connection_with_invalid_port()
        {
            var receptionistFlow = new ReceptionistFlow("My-Worker-Id")
            {
                ReceptionistPort = 1337
            };

            var aggregateException = Assert.Throws<AggregateException>(() =>
                connection = receptionistFlow.CreateAsync(GetConnectionParameters()).Result);
            Assert.IsInstanceOf<ConnectionFailedException>(aggregateException.InnerExceptions[0]);
        }

        private static ConnectionParameters GetConnectionParameters()
        {
            return new ConnectionParameters
            {
                WorkerType = "UnityClient",
                DefaultComponentVtable = new ComponentVtable()
            };
        }
    }
}
