using System.Collections.Generic;
using Improbable.Worker;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class ReceptionistConfigTests
    {
        [Test]
        public void UseExternalIp_should_be_false_by_default()
        {
            var config = GetDefaultWorkingConfig();
            Assert.IsFalse(config.UseExternalIp);
        }


        [Test]
        public void Validate_should_return_true_when_ReceptionistConfig_is_set_to_default_value()
        {
            var config = GetDefaultWorkingConfig();
            Assert.DoesNotThrow(() => config.Validate());
        }

        [Test]
        public void Validate_should_return_true_when_ReceptionistHost_is_set_to_arbritary_string()
        {
            var config = GetDefaultWorkingConfig();
            config.ReceptionistHost = "test";
            Assert.DoesNotThrow(() => config.Validate());
        }

        [Test]
        public void Validate_should_return_false_when_receptionist_host_is_empty()
        {
            var config = GetDefaultWorkingConfig();
            config.ReceptionistHost = "";

            var exception = Assert.Throws<ConnectionFailedException>(() => config.Validate());
            Assert.IsTrue(exception.Message.Contains("receptionistHost"));
        }

        [Test]
        public void CreateConnectionConfigFromCommandLine_should_parse_correctly()
        {
            const string host = "myhost";
            const short port = 10;
            var networkType = NetworkConnectionType.Tcp.ToString();

            var parsedArgs = new Dictionary<string, string>
            {
                { RuntimeConfigNames.ReceptionistHost, host },
                { RuntimeConfigNames.ReceptionistPort, port.ToString() },
                { RuntimeConfigNames.LinkProtocol, networkType }
            };

            var config = ReceptionistConfig.CreateConnectionConfigFromCommandLine(parsedArgs);

            Assert.AreEqual(host, config.ReceptionistHost);
            Assert.AreEqual(port, config.ReceptionistPort);
            Assert.AreEqual(networkType, config.LinkProtocol.ToString());
        }

        [Test]
        public void CreateConnectionConfigFromCommandLine_should_provide_defaults_for_missing_values()
        {
            var parsedArgs = new Dictionary<string, string>();
            var config = ReceptionistConfig.CreateConnectionConfigFromCommandLine(parsedArgs);

            Assert.AreEqual(RuntimeConfigDefaults.ReceptionistHost, config.ReceptionistHost);
            Assert.AreEqual(RuntimeConfigDefaults.ReceptionistPort, config.ReceptionistPort);
            Assert.AreEqual(RuntimeConfigDefaults.LinkProtocol, config.LinkProtocol);
        }

        private static ReceptionistConfig GetDefaultWorkingConfig()
        {
            return new ReceptionistConfig();
        }
    }
}
