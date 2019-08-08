using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Connection
{
    [TestFixture]
    public class CommandLineConnectionParametersInitializerTests
    {
        [Test]
        public void Initialize_should_set_network_protocol()
        {
            var args = new Dictionary<string, string>
            {
                { RuntimeConfigNames.LinkProtocol, "RakNet" }
            };

            var connParams = new ConnectionParameters();

            new CommandLineConnectionParameterInitializer(args).Initialize(connParams);

            Assert.AreEqual(NetworkConnectionType.RakNet, connParams.Network.ConnectionType);
        }

        [Test]
        public void Initialize_should_throw_with_invalid_protocol()
        {
            var args = new Dictionary<string, string>
            {
                { RuntimeConfigNames.LinkProtocol, "ranket" }
            };

            var connParams = new ConnectionParameters();
            var initializer = new CommandLineConnectionParameterInitializer(args);

            Assert.Throws<FormatException>(() => initializer.Initialize(connParams));
        }
    }
}
