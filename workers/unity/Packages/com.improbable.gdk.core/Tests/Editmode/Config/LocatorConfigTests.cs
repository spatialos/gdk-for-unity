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
            var config = GetDefaultWorkingConfigWithLoginToken();
            Assert.IsTrue(config.UseExternalIp);
        }

        [Test]
        public void Validate_should_return_true_when_LocatorConfig_is_set_to_default_value()
        {
            var config = GetDefaultWorkingConfigWithLoginToken();
            Assert.IsTrue(config.Validate(out var errorMessage));
        }

        [Test]
        public void Validate_should_return_true_when_LocatorHost_is_set_to_arbritary_string()
        {
            var config = GetDefaultWorkingConfigWithLoginToken();
            config.LocatorHost = "test";
            Assert.IsTrue(config.Validate(out var errorMessage));
        }

        [Test]
        public void Validate_should_return_false_when_LocatorHost_is_empty()
        {
            var config = GetDefaultWorkingConfigWithLoginToken();
            config.LocatorHost = "";
            Assert.IsFalse(config.Validate(out var errorMessage));
        }

        [Test]
        public void Validate_should_return_false_when_ProjectName_is_empty()
        {
            var config = GetDefaultWorkingConfigWithLoginToken();
            config.LocatorParameters.ProjectName = "";
            Assert.IsFalse(config.Validate(out var errorMessage));
        }

        [Test]
        public void Validate_should_return_false_when_LoginToken_is_empty_for_login_token_flow()
        {
            var config = GetDefaultWorkingConfigWithLoginToken();
            config.LocatorParameters.CredentialsType = LocatorCredentialsType.LoginToken;
            config.LocatorParameters.LoginToken.Token = "";
            Assert.IsFalse(config.Validate(out var errorMessage));
        }

        [Test]
        public void Validate_should_return_false_when_SteamDeploymentTag_empty_for_steam_flow()
        {
            var config = GetDefaultWorkingConfigWithLoginToken();
            config.LocatorParameters.CredentialsType = LocatorCredentialsType.Steam;
            config.LocatorParameters.Steam.DeploymentTag = "";
            config.LocatorParameters.Steam.Ticket = "something";
            Assert.IsFalse(config.Validate(out var errorMessage));
        }

        [Test]
        public void Validate_should_return_false_when_steam_ticket_empty_for_steam_flow()
        {
            var config = GetDefaultWorkingConfigWithLoginToken();
            config.LocatorParameters.CredentialsType = LocatorCredentialsType.Steam;
            config.LocatorParameters.Steam.DeploymentTag = "something";
            config.LocatorParameters.Steam.Ticket = "";
            Assert.IsFalse(config.Validate(out var errorMessage));
        }

        [Test]
        public void SetProjectName_should_update_ProjectName_property()
        {
            var config = GetDefaultWorkingConfigWithLoginToken();
            const string projectName = "myproject";
            config.SetProjectName(projectName);

            Assert.AreEqual(projectName, config.LocatorParameters.ProjectName);
        }

        [Test]
        public void SetLoginToken_should_update_LoginToken_and_CredentialsType_properties()
        {
            var config = new LocatorConfig();
            const string loginToken = "mylogintoken";
            config.SetLoginToken(loginToken);

            Assert.AreEqual(loginToken, config.LocatorParameters.LoginToken.Token);
            Assert.AreEqual(LocatorCredentialsType.LoginToken, config.LocatorParameters.CredentialsType);
        }

        [Test]
        public void SetSteamCredentials_should_update_LoginToken_and_CredentialsType_properties()
        {
            var config = new LocatorConfig();
            const string steamDeploymentTag = "mysteamdeploymenttag";
            const string steamTicket = "mysteamTicket";
            config.SetSteamCredentials(steamDeploymentTag, steamTicket);

            Assert.AreEqual(steamDeploymentTag, config.LocatorParameters.Steam.DeploymentTag);
            Assert.AreEqual(steamTicket, config.LocatorParameters.Steam.Ticket);
            Assert.AreEqual(LocatorCredentialsType.Steam, config.LocatorParameters.CredentialsType);
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
        public void CreateConnectionConfigFromCommandLine_should_provide_default_values()
        {
            var parsedArgs = new Dictionary<string, string>();
            var config = LocatorConfig.CreateConnectionConfigFromCommandLine(parsedArgs);

            Assert.AreEqual(RuntimeConfigDefaults.LocatorHost, config.LocatorHost);
            Assert.AreEqual(RuntimeConfigDefaults.LinkProtocol, config.LinkProtocol);
        }

        private static LocatorConfig GetDefaultWorkingConfigWithLoginToken()
        {
            var locatorConfig = new LocatorConfig();
            locatorConfig.SetProjectName("defaultProjectName");
            locatorConfig.SetLoginToken("defaultLoginToken");
            return locatorConfig;
        }
    }
}
