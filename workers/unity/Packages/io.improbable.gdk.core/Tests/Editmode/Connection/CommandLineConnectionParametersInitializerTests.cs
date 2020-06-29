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
        [TestCase("")]
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

        [TestCase("true")]
        [TestCase("false")]
        public void Initialize_should_set_UseExternalIp_when_defined(string useExternalIp)
        {
            var args = new Dictionary<string, string>
            {
                { RuntimeConfigNames.UseExternalIp, useExternalIp }
            };

            var connParams = new ConnectionParameters();
            new CommandLineConnectionParameterInitializer(args).Initialize(connParams);

            Assert.AreEqual(bool.Parse(useExternalIp), connParams.Network.UseExternalIp);
        }

        [TestCase("")]
        [TestCase("Ffalse")]
        [TestCase("Ture")]
        [TestCase(null)]
        public void Initialize_should_throw_with_invalid_externalIp_argument(string useExternalIp)
        {
            var args = new Dictionary<string, string>
            {
                { RuntimeConfigNames.UseExternalIp, useExternalIp }
            };

            var connParams = new ConnectionParameters();
            var initializer = new CommandLineConnectionParameterInitializer(args);
            if (useExternalIp == null)
            {
                Assert.Throws<ArgumentNullException>(() => initializer.Initialize(connParams));
            }
            else
            {
                Assert.Throws<FormatException>(() => initializer.Initialize(connParams));
            }
        }
    }
}
