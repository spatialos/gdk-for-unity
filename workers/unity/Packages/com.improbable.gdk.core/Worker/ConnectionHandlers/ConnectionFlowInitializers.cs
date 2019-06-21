using System;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    // TODO: Should this be contravarient?
    public interface IConnectionFlowInitializer<TConnectionFlow> where TConnectionFlow : IConnectionFlow
    {
        void Initialize(TConnectionFlow flow);
    }

    public class CommandLineConnectionFlowInitializer :
        IConnectionFlowInitializer<ReceptionistFlow>,
        IConnectionFlowInitializer<LocatorFlow>,
        IConnectionFlowInitializer<AlphaLocatorFlow>
    {
        private CommandLineArgs commandLineArgs;

        public CommandLineConnectionFlowInitializer()
        {
            commandLineArgs = CommandLineArgs.FromCommandLine();
        }

        public ConnectionService GetConnectionService()
        {
            if (commandLineArgs.Contains(RuntimeConfigNames.SteamDeploymentTag) ||
                commandLineArgs.Contains(RuntimeConfigNames.SteamTicket))
            {
                return ConnectionService.Locator;
            }

            if (commandLineArgs.Contains(RuntimeConfigNames.LoginToken))
            {
                return commandLineArgs.Contains(RuntimeConfigNames.PlayerIdentityToken)
                    ? ConnectionService.AlphaLocator
                    : ConnectionService.Locator;
            }

            return ConnectionService.Receptionist;
        }

        public void Initialize(ReceptionistFlow receptionist)
        {
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.ReceptionistHost, ref receptionist.ReceptionistHost);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.ReceptionistPort, ref receptionist.ReceptionistPort);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.WorkerId, ref receptionist.WorkerId);
        }

        public void Initialize(LocatorFlow locator)
        {
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.LocatorHost, ref locator.LocatorHost);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.ProjectName,
                ref locator.LocatorParameters.ProjectName);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.LoginToken,
                ref locator.LocatorParameters.LoginToken.Token);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.SteamDeploymentTag,
                ref locator.LocatorParameters.Steam.DeploymentTag);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.SteamTicket,
                ref locator.LocatorParameters.Steam.Ticket);


            if (!string.IsNullOrEmpty(locator.LocatorParameters.LoginToken.Token))
            {
                locator.LocatorParameters.CredentialsType = LocatorCredentialsType.LoginToken;
            }
            else if (!string.IsNullOrEmpty(locator.LocatorParameters.Steam.Ticket) && !string.IsNullOrEmpty(locator.LocatorParameters.Steam.DeploymentTag))
            {
                locator.LocatorParameters.CredentialsType = LocatorCredentialsType.Steam;
            }
        }

        public void Initialize(AlphaLocatorFlow alphaLocator)
        {
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.LocatorHost, ref alphaLocator.LocatorHost);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.PlayerIdentityToken, ref alphaLocator.LocatorParameters.PlayerIdentity.PlayerIdentityToken);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.LoginToken, ref alphaLocator.LocatorParameters.PlayerIdentity.LoginToken);

            if (!string.IsNullOrEmpty(alphaLocator.LocatorParameters.PlayerIdentity.PlayerIdentityToken)
                && !string.IsNullOrEmpty(alphaLocator.LocatorParameters.PlayerIdentity.LoginToken))
            {
                alphaLocator.UseDevAuthFlow = false;
            }
        }
    }
}
