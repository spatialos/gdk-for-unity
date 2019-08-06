using System.Collections.Generic;
using Improbable.Worker.CInterop;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Connection
{
    [TestFixture]
    public class CommandLineConnectionFlowInitializerTests
    {
        private static Dictionary<string, string> BaseArgs => new Dictionary<string, string>
        {
            { RuntimeConfigNames.SteamDeploymentTag, "my_steam_deployment" },
            { RuntimeConfigNames.SteamTicket, "steam_ticket" },
            { RuntimeConfigNames.LoginToken, "login_token" },
            { RuntimeConfigNames.PlayerIdentityToken, "pit" },
            { RuntimeConfigNames.ReceptionistHost, "receptionist_host" },
            { RuntimeConfigNames.ReceptionistPort, "1337" },
            { RuntimeConfigNames.WorkerId, "my_worker_id" },
            { RuntimeConfigNames.LocatorHost, "locator_host" },
            { RuntimeConfigNames.ProjectName, "my_project" },
        };

        [Test]
        public void GetConnectionService_returns_locator_if_steam_is_set()
        {
            var service = new CommandLineConnectionFlowInitializer(BaseArgs).GetConnectionService();
            Assert.AreEqual(ConnectionService.Locator, service);
        }

        [Test]
        public void GetConnectionService_returns_alpha_locator_if_pit_and_login_token_are_present()
        {
            var args = BaseArgs;
            args.Remove(RuntimeConfigNames.SteamDeploymentTag);
            args.Remove(RuntimeConfigNames.SteamTicket);

            var service = new CommandLineConnectionFlowInitializer(args).GetConnectionService();
            Assert.AreEqual(ConnectionService.AlphaLocator, service);
        }

        [Test]
        public void GetConnectionService_returns_locator_if_only_login_token_is_present()
        {
            var args = BaseArgs;
            args.Remove(RuntimeConfigNames.SteamDeploymentTag);
            args.Remove(RuntimeConfigNames.SteamTicket);
            args.Remove(RuntimeConfigNames.PlayerIdentityToken);

            var service = new CommandLineConnectionFlowInitializer(args).GetConnectionService();
            Assert.AreEqual(ConnectionService.Locator, service);
        }

        [Test]
        public void GetConnectionService_falls_back_to_receptionist()
        {
            var service = new CommandLineConnectionFlowInitializer(new Dictionary<string, string>()).GetConnectionService();
            Assert.AreEqual(ConnectionService.Receptionist, service);
        }

        [Test]
        public void Initialize_receptionist_flow_correctly()
        {
            var initializer = new CommandLineConnectionFlowInitializer(BaseArgs);
            var flow = new ReceptionistFlow("a_worker_id", initializer);

            Assert.AreEqual(BaseArgs[RuntimeConfigNames.ReceptionistHost], flow.ReceptionistHost);
            Assert.AreEqual(1337, flow.ReceptionistPort);
            Assert.AreEqual(BaseArgs[RuntimeConfigNames.WorkerId], flow.WorkerId);
        }

        [Test]
        public void Initialize_receptionist_ignores_params_that_arent_there()
        {
            var initializer = new CommandLineConnectionFlowInitializer(new Dictionary<string, string>());
            var flow = new ReceptionistFlow("a_worker_id", initializer);

            Assert.AreEqual(RuntimeConfigDefaults.ReceptionistHost, flow.ReceptionistHost);
            Assert.AreEqual(RuntimeConfigDefaults.ReceptionistPort, flow.ReceptionistPort);
            Assert.AreEqual("a_worker_id", flow.WorkerId);
        }

        [Test]
        public void Initialize_locator_flow_correctly()
        {
            var initializer = new CommandLineConnectionFlowInitializer(BaseArgs);
            var flow = new LocatorFlow(initializer);

            Assert.AreEqual(BaseArgs[RuntimeConfigNames.LocatorHost], flow.LocatorHost);
            Assert.AreEqual(BaseArgs[RuntimeConfigNames.ProjectName], flow.LocatorParameters.ProjectName);
            Assert.AreEqual(BaseArgs[RuntimeConfigNames.LoginToken], flow.LocatorParameters.LoginToken.Token);
            Assert.AreEqual(BaseArgs[RuntimeConfigNames.SteamDeploymentTag], flow.LocatorParameters.Steam.DeploymentTag);
            Assert.AreEqual(BaseArgs[RuntimeConfigNames.SteamTicket], flow.LocatorParameters.Steam.Ticket);
        }

        [Test]
        public void Initialize_locator_flow_uses_steam_credentials_if_present()
        {
            var initializer = new CommandLineConnectionFlowInitializer(BaseArgs);
            var flow = new LocatorFlow(initializer);

            Assert.AreEqual(LocatorCredentialsType.Steam, flow.LocatorParameters.CredentialsType);
        }

        [Test]
        public void Initialize_locator_flow_uses_login_token_credentials_if_present()
        {
            var args = BaseArgs;
            args.Remove(RuntimeConfigNames.SteamDeploymentTag);
            args.Remove(RuntimeConfigNames.SteamTicket);

            var initializer = new CommandLineConnectionFlowInitializer(args);
            var flow = new LocatorFlow(initializer);

            Assert.AreEqual(LocatorCredentialsType.LoginToken, flow.LocatorParameters.CredentialsType);
        }

        [Test]
        public void Initialize_alpha_locator_flow_correctly()
        {
            var initializer = new CommandLineConnectionFlowInitializer(BaseArgs);
            var flow = new AlphaLocatorFlow(initializer);

            Assert.AreEqual(BaseArgs[RuntimeConfigNames.LocatorHost], flow.LocatorHost);
            Assert.AreEqual(BaseArgs[RuntimeConfigNames.LoginToken], flow.LocatorParameters.PlayerIdentity.LoginToken);
            Assert.AreEqual(BaseArgs[RuntimeConfigNames.PlayerIdentityToken], flow.LocatorParameters.PlayerIdentity.PlayerIdentityToken);
        }

        [Test]
        public void Initialize_alpha_locator_sets_dev_auth_flow_correctly()
        {
            {
                var initializer = new CommandLineConnectionFlowInitializer(BaseArgs);
                var flow = new AlphaLocatorFlow(initializer);
                Assert.IsFalse(flow.UseDevAuthFlow);
            }

            {
                var args = BaseArgs;
                args.Remove(RuntimeConfigNames.PlayerIdentityToken);
                args.Remove(RuntimeConfigNames.LoginToken);

                var initializer = new CommandLineConnectionFlowInitializer(args);
                var flow = new AlphaLocatorFlow(initializer);
                Assert.IsTrue(flow.UseDevAuthFlow);
            }
        }
    }
}
