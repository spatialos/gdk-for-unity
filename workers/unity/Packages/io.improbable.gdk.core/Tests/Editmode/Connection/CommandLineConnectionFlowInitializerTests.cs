using System.Collections.Generic;
using Improbable.Worker.CInterop;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Connection
{
    [TestFixture]
    public class CommandLineConnectionFlowInitializerTests
    {
        private static Dictionary<string, string> GetBaseArgs()
        {
            return new Dictionary<string, string>
            {
                { RuntimeConfigNames.LoginToken, "login_token" },
                { RuntimeConfigNames.PlayerIdentityToken, "pit" },
                { RuntimeConfigNames.ReceptionistHost, "receptionist_host" },
                { RuntimeConfigNames.ReceptionistPort, "1337" },
                { RuntimeConfigNames.WorkerId, "my_worker_id" },
                { RuntimeConfigNames.LocatorHost, "locator_host" },
            };
        }

        [Test]
        public void GetConnectionService_returns_alpha_locator_if_pit_and_login_token_are_present()
        {
            var args = GetBaseArgs();
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
            var initializer = new CommandLineConnectionFlowInitializer(GetBaseArgs());
            var flow = new ReceptionistFlow("a_worker_id", initializer);

            Assert.AreEqual(GetBaseArgs()[RuntimeConfigNames.ReceptionistHost], flow.ReceptionistHost);
            Assert.AreEqual(1337, flow.ReceptionistPort);
            Assert.AreEqual(GetBaseArgs()[RuntimeConfigNames.WorkerId], flow.WorkerId);
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
        public void Initialize_alpha_locator_flow_correctly()
        {
            var initializer = new CommandLineConnectionFlowInitializer(GetBaseArgs());
            var flow = new LocatorFlow(initializer);

            Assert.AreEqual(GetBaseArgs()[RuntimeConfigNames.LocatorHost], flow.LocatorHost);
            Assert.AreEqual(GetBaseArgs()[RuntimeConfigNames.LoginToken], flow.LoginToken);
            Assert.AreEqual(GetBaseArgs()[RuntimeConfigNames.PlayerIdentityToken], flow.PlayerIdentityToken);
        }

        [Test]
        public void Initialize_locator_sets_dev_auth_flow_correctly()
        {
            {
                var initializer = new CommandLineConnectionFlowInitializer(GetBaseArgs());
                var flow = new LocatorFlow(initializer);
                Assert.IsFalse(flow.UseDevAuthFlow);
            }

            {
                var args = GetBaseArgs();
                args.Remove(RuntimeConfigNames.PlayerIdentityToken);
                args.Remove(RuntimeConfigNames.LoginToken);

                var initializer = new CommandLineConnectionFlowInitializer(args);
                var flow = new LocatorFlow(initializer);
                Assert.IsTrue(flow.UseDevAuthFlow);
            }
        }
    }
}
