using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    // TODO: Should this be contravarient?
    /// <summary>
    ///    Represents an object which can initialize a connection flow of a certain type.
    /// </summary>
    /// <typeparam name="TConnectionFlow">The type of connection flow that this object can initialize.</typeparam>
    public interface IConnectionFlowInitializer<TConnectionFlow> where TConnectionFlow : IConnectionFlow
    {
        /// <summary>
        ///     Initializes the flow. Seeds the flow implementation with the data required to successfully connect.
        /// </summary>
        /// <param name="flow">The flow object to initialize.</param>
        void Initialize(TConnectionFlow flow);
    }

    /// <summary>
    ///     Represents an object which can initialize the <see cref="ReceptionistFlow"/>, <see cref="LocatorFlow"/>,
    ///     and <see cref="AlphaLocatorFlow"/> connection flows from the command line.
    /// </summary>
    public class CommandLineConnectionFlowInitializer :
        IConnectionFlowInitializer<ReceptionistFlow>,
        IConnectionFlowInitializer<LocatorFlow>,
        IConnectionFlowInitializer<AlphaLocatorFlow>
    {
        private readonly CommandLineArgs commandLineArgs;

        public CommandLineConnectionFlowInitializer()
        {
            commandLineArgs = CommandLineArgs.FromCommandLine();
        }

        /// <summary>
        ///     Gets the connection service to use based on command line arguments.
        /// </summary>
        /// <returns>The connection service to use.</returns>
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
