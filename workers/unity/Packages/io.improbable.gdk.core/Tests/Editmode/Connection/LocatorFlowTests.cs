using System;
using Improbable.Gdk.TestUtils.Editor;
using Improbable.Gdk.Tools;
using Improbable.Worker.CInterop;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Connection
{
    [TestFixture]
    public class LocatorTests
    {
        private SpatialdManager spatiald;
        private Improbable.Worker.CInterop.Connection connection;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            if (!DevAuthTokenUtils.TryGenerate())
            {
                throw new ArgumentException("Could not generate a dev auth token.");
            }

            spatiald = SpatialdManager.Start().Result;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            spatiald?.Dispose();
        }

        [TearDown]
        public void TearDown()
        {
            connection?.Dispose();
            connection = null;
        }

        [Test]
        public void LocatorFlow_dev_auth_fails_if_invalid_dev_auth_token()
        {
            var flow = GetLocatorFlow();
            flow.DevAuthToken = "notvalid";
            var aggregateException = Assert.Throws<AggregateException>(() => connection = flow.CreateAsync(GetConnectionParameters()).Result);
            Assert.IsInstanceOf<AuthenticationFailedException>(aggregateException.InnerExceptions[0]);
        }

        [Test]
        public void LocatorFlow_dev_auth_fails_if_no_deployment_running()
        {
            var flow = GetLocatorFlow();
            flow.DevAuthToken = DevAuthTokenUtils.DevAuthToken;
            var aggregateException = Assert.Throws<AggregateException>(() => connection = flow.CreateAsync(GetConnectionParameters()).Result);
            Assert.IsInstanceOf<AuthenticationFailedException>(aggregateException.InnerExceptions[0]);
        }

        [Test]
        public void LocatorFlow_dev_auth_fails_if_depl_isnt_tagged()
        {
            using (spatiald.StartLocalDeployment("test", "default_launch.json").Result)
            {
                var flow = GetLocatorFlow();
                flow.DevAuthToken = DevAuthTokenUtils.DevAuthToken;

                var aggregateException = Assert.Throws<AggregateException>(() => connection = flow.CreateAsync(GetConnectionParameters()).Result);
                Assert.IsInstanceOf<AuthenticationFailedException>(aggregateException.InnerExceptions[0]);
            }
        }

        [Test]
        public void LocatorFlow_dev_auth_works_with_tagged_depl()
        {
            using (var depl = spatiald.StartLocalDeployment("test", "default_launch.json").Result)
            {
                depl.AddDevLoginTag().Wait();
                var flow = GetLocatorFlow();
                flow.DevAuthToken = DevAuthTokenUtils.DevAuthToken;

                Assert.DoesNotThrow(() => connection = flow.CreateAsync(GetConnectionParameters()).Result);
                Assert.AreEqual(ConnectionStatusCode.Success, connection.GetConnectionStatusCode());
            }
        }

        private static LocatorFlow GetLocatorFlow()
        {
            return new LocatorFlow
            {
                LocatorHost = "localhost",
                LocatorPort = 9876,
                UseInsecureConnection = true
            };
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
