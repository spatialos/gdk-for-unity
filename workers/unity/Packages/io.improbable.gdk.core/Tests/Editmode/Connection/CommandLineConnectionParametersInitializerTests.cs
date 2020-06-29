using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Connection
{
    [TestFixture]
    public class CommandLineConnectionParametersInitializerTests
    {
#pragma warning disable 612
        [TestCase("RakNet", NetworkConnectionType.RakNet)]
        [TestCase("Tcp", NetworkConnectionType.Tcp)]
        [TestCase("Kcp", NetworkConnectionType.Kcp)]
#pragma warning restore 612
        [TestCase("ModularKcp", NetworkConnectionType.ModularKcp)]
        public void Initialize_should_set_network_protocol(string protocolStr, NetworkConnectionType connectionType)
        {
            var args = new Dictionary<string, string>
            {
                { RuntimeConfigNames.LinkProtocol, protocolStr }
            };

            var connParams = new ConnectionParameters();

            new CommandLineConnectionParameterInitializer(args).Initialize(connParams);

            Assert.AreEqual(connectionType, connParams.Network.ConnectionType);
        }

        [TestCase("ranket")]
        [TestCase("raknet")]
        public void Initialize_should_throw_with_invalid_protocol(string protocolStr)
        {
            var args = new Dictionary<string, string>
            {
                { RuntimeConfigNames.LinkProtocol, protocolStr }
            };

            var connParams = new ConnectionParameters();
            var initializer = new CommandLineConnectionParameterInitializer(args);

            Assert.Throws<FormatException>(() => initializer.Initialize(connParams));
        }

        [Test]
        public void Initialize_with_UseExternalIp_true_when_flag_is_set()
        {
            var args = new Dictionary<string, string>
            {
                { RuntimeConfigNames.UseExternalIp, string.Empty }
            };

            var connParams = new ConnectionParameters();
            new CommandLineConnectionParameterInitializer(args).Initialize(connParams);

            Assert.AreEqual(true, connParams.Network.UseExternalIp);
        }

        [Test]
        public void Initialize_with_UseExternalIp_false_when_flag_is_unset()
        {
            var args = new Dictionary<string, string>();

            var connParams = new ConnectionParameters();
            new CommandLineConnectionParameterInitializer(args).Initialize(connParams);

            Assert.AreEqual(false, connParams.Network.UseExternalIp);
        }
    }
}
