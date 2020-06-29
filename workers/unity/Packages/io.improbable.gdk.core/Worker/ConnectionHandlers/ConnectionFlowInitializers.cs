using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents an object which can initialize a connection flow of a certain type.
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
    ///     Represents an object which can initialize the <see cref="ReceptionistFlow" />, <see cref="LocatorFlow" />,
    ///     and <see cref="LocatorFlow" /> connection flows from the command line.
    /// </summary>
    public class CommandLineConnectionFlowInitializer :
        IConnectionFlowInitializer<ReceptionistFlow>,
        IConnectionFlowInitializer<LocatorFlow>
    {
        private readonly CommandLineArgs commandLineArgs;

        public CommandLineConnectionFlowInitializer()
        {
            commandLineArgs = CommandLineArgs.FromCommandLine();
        }

        internal CommandLineConnectionFlowInitializer(Dictionary<string, string> commandLineArgs)
        {
            this.commandLineArgs = CommandLineArgs.From(commandLineArgs);
        }

        /// <summary>
        ///     Gets the connection service to use based on command line arguments.
        /// </summary>
        /// <returns>The connection service to use.</returns>
        public ConnectionService GetConnectionService()
        {
            if (commandLineArgs.Contains(RuntimeConfigNames.LoginToken) && commandLineArgs.Contains(RuntimeConfigNames.PlayerIdentityToken))
            {
                return ConnectionService.Locator;
            }

            return ConnectionService.Receptionist;
        }

        public void Initialize(ReceptionistFlow receptionist)
        {
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.ReceptionistHost,
                ref receptionist.ReceptionistHost);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.ReceptionistPort,
                ref receptionist.ReceptionistPort);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.WorkerId, ref receptionist.WorkerId);

            var useExternalIp = string.Empty;
            if (commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.UseExternalIp, ref useExternalIp))
            {
                receptionist.UseExternalIp = bool.Parse(useExternalIp);
            }
        }

        public void Initialize(LocatorFlow locator)
        {
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.LocatorHost, ref locator.LocatorHost);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.PlayerIdentityToken,
                ref locator.PlayerIdentityToken);
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.LoginToken, ref locator.LoginToken);

            var useExternalIp = string.Empty;
            if (commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.UseExternalIp, ref useExternalIp))
            {
                locator.UseExternalIp = bool.Parse(useExternalIp);
            }

            if (!string.IsNullOrEmpty(locator.PlayerIdentityToken)
                && !string.IsNullOrEmpty(locator.LoginToken))
            {
                locator.UseDevAuthFlow = false;
            }
        }
    }
}
