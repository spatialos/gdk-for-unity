using System.Collections.Generic;
using Improbable.Worker;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class LocatorConfigTests
    {
        [Test]
        public void UseExternalIp_should_be_true_by_default()
        {
            var config = GetDefaultWorkingConfig();
            Assert.IsTrue(config.UseExternalIp);
        }

        [Test]
        public void Validate_should_return_true_when_LocatorConfig_is_set_to_default_value()
        {
            var config = GetDefaultWorkingConfig();
            Assert.DoesNotThrow(() => config.Validate());
        }

        [Test]
        public void Validate_should_return_true_when_LocatorHost_is_set_to_arbritary_string()
        {
            var config = GetDefaultWorkingConfig();
            config.LocatorHost = "test";
            Assert.DoesNotThrow(() => config.Validate());
        }

        [Test]
        public void Validate_should_return_false_when_LocatorHost_is_empty()
        {
            var config = GetDefaultWorkingConfig();
            config.LocatorHost = "";

            var exception = Assert.Throws<ConnectionFailedException>(() => config.Validate());
            Assert.IsTrue(exception.Message.Contains("locatorHost"));
        }

        [Test]
        public void Validate_should_return_false_when_ProjectName_is_empty()
        {
            var config = GetDefaultWorkingConfig();
            config.LocatorParameters.CredentialsType = LocatorCredentialsType.LoginToken;
            config.LocatorParameters.LoginToken.Token = "some token who cares";
            config.LocatorParameters.ProjectName = "";

            var exception = Assert.Throws<ConnectionFailedException>(() => config.Validate());
            Assert.IsTrue(exception.Message.Contains("projectName"));
        }

        [Test]
        public void Validate_should_return_false_when_LoginToken_is_empty()
        {
            var config = GetDefaultWorkingConfig();
            config.LocatorParameters.CredentialsType = LocatorCredentialsType.LoginToken;
            config.LocatorParameters.LoginToken.Token = "";
            config.LocatorParameters.ProjectName = "some project who cares";

            var exception = Assert.Throws<ConnectionFailedException>(() => config.Validate());
            Assert.IsTrue(exception.Message.Contains("loginToken"));
        }

        [Test]
        public void SetProjectName_should_update_ProjectName_property()
        {
            var config = GetDefaultWorkingConfig();
            const string projectName = "myproject";
            config.SetProjectName(projectName);

            Assert.AreEqual(projectName, config.LocatorParameters.ProjectName);
        }

        [Test]
        public void SetLoginToken_should_update_LoginToken_and_CredentialsType_properties()
        {
            var config = GetDefaultWorkingConfig();
            const string loginToken = "mylogintoken";
            config.SetLoginToken(loginToken);

            Assert.AreEqual(loginToken, config.LocatorParameters.LoginToken.Token);
            Assert.AreEqual(LocatorCredentialsType.LoginToken, config.LocatorParameters.CredentialsType);
        }

        [Test]
        public void CreateConnectionConfigFromCommandLine_should_parse_correctly()
        {
            const string loginToken = "myToken";
            const string projectName = "myproject";
            const string host = "myhost";
            var networkType = NetworkConnectionType.Tcp.ToString();

            var parsedArgs = new Dictionary<string, string>
            {
                { RuntimeConfigNames.LoginToken, loginToken },
                { RuntimeConfigNames.LocatorHost, host },
                { RuntimeConfigNames.ProjectName, projectName },
                { RuntimeConfigNames.LinkProtocol, networkType }
            };

            var config = LocatorConfig.CreateConnectionConfigFromCommandLine(parsedArgs);

            Assert.AreEqual(LocatorCredentialsType.LoginToken, config.LocatorParameters.CredentialsType);
            Assert.AreEqual(loginToken, config.LocatorParameters.LoginToken.Token);
            Assert.AreEqual(host, config.LocatorHost);
            Assert.AreEqual(projectName, config.LocatorParameters.ProjectName);
            Assert.AreEqual(networkType, config.LinkProtocol.ToString());
        }

        [Test]
        public void CreateConnectionConfigFromCommandLine_should_provide_defaults_for_missing_values()
        {
            var parsedArgs = new Dictionary<string, string>();
            var config = LocatorConfig.CreateConnectionConfigFromCommandLine(parsedArgs);

            Assert.AreEqual(LocatorCredentialsType.LoginToken, config.LocatorParameters.CredentialsType);
            Assert.AreEqual(string.Empty, config.LocatorParameters.LoginToken.Token);
            Assert.AreEqual(RuntimeConfigDefaults.LocatorHost, config.LocatorHost);
            Assert.AreEqual(string.Empty, config.LocatorParameters.ProjectName);
            Assert.AreEqual(RuntimeConfigDefaults.LinkProtocol, config.LinkProtocol);
        }

        private static LocatorConfig GetDefaultWorkingConfig()
        {
            return new LocatorConfig();
        }
    }
}
