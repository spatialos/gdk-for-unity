using System;
using System.Collections.Generic;
using Improbable.Worker;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     An implementation of <see cref="ConnectionConfig"/> which is used when connecting to the SpatialOS runtime
    ///     through the locator.
    /// </summary>
    /// <remarks>
    ///     This is most often used when
    ///     <a href="https://docs.improbable.io/reference/latest/shared/operate/launcher#the-launcher">connecting via
    ///     the Launcher</a>.
    /// </remarks>
    public class LocatorConfig : ConnectionConfig
    {
        /// <summary>
        ///     The host for connecting to the SpatialOS runtime via the Locator.
        /// </summary>
        /// <remarks>
        ///    Default is "locator.improbable.io".
        /// </remarks>
        public string LocatorHost = RuntimeConfigDefaults.LocatorHost;

        /// <summary>
        ///     Parameters used to connect to SpatialOS through the Locator.
        /// </summary>
        public readonly LocatorParameters LocatorParameters = new LocatorParameters();

        public LocatorConfig()
        {
            UseExternalIp = true;
        }

        /// <summary>
        ///     Checks that the LocatorConfig instance is valid. This does not guarantee a successful connection.
        /// </summary>
        public override void Validate()
        {
            ValidateString(LocatorHost, RuntimeConfigNames.LocatorHost);
            ValidateString(LocatorParameters.ProjectName, RuntimeConfigNames.ProjectName);
            ValidateCondition(
                LocatorParameters.CredentialsType == LocatorCredentialsType.LoginToken ||
                LocatorParameters.CredentialsType == LocatorCredentialsType.Steam,
                "LocatorConfig has invalid CredentialsType.");

            if (LocatorParameters.CredentialsType == LocatorCredentialsType.LoginToken)
            {
                ValidateString(LocatorParameters.LoginToken.Token, RuntimeConfigNames.LoginToken);
                ValidateCondition(
                    string.IsNullOrEmpty(LocatorParameters.Steam.DeploymentTag) &&
                    string.IsNullOrEmpty(LocatorParameters.Steam.Ticket),
                    "SpatialOS login token and Steam credentials may not be set at the same time.");
            }

            if (LocatorParameters.CredentialsType == LocatorCredentialsType.Steam)
            {
                ValidateString(LocatorParameters.Steam.DeploymentTag, RuntimeConfigNames.DeploymentTag);
                ValidateString(LocatorParameters.Steam.Ticket, RuntimeConfigNames.SteamTicket);
                ValidateCondition(
                    string.IsNullOrEmpty(LocatorParameters.LoginToken.Token),
                    "SpatialOS login token and Steam credentials may not be set at the same time.");
            }
        }

        /// <summary>
        ///     Sets the project name for the Locator to connect to.
        /// </summary>
        /// <param name="projectName">The name of the project.</param>
        public void SetProjectName(string projectName)
        {
            LocatorParameters.ProjectName = projectName;
        }

        /// <summary>
        ///     Sets the LocatorCredentialsType to LoginToken and sets the login token for the Locator to use.
        /// </summary>
        /// <remarks>
        ///     SpatialOS login token and Steam credentials may not be set at the same time when connecting to a deployment.
        /// </remarks>
        /// <param name="loginToken">The login token.</param>
        /// <exception cref="ConnectionFailedException">
        ///     Thrown if this method is called after Steam credentials were set before.
        /// </exception>
        public void SetLoginToken(string loginToken)
        {
            if (LocatorParameters.CredentialsType == LocatorCredentialsType.Steam)
            {
                throw new ConnectionFailedException(
                    "SpatialOS login token and Steam credentials may not be set at the same time when connecting to a deployment.",
                    ConnectionErrorReason.InvalidConfig);
            }

            LocatorParameters.CredentialsType = LocatorCredentialsType.LoginToken;
            LocatorParameters.LoginToken.Token = loginToken;
        }

        /// <summary>
        ///     Sets the LocatorCredentialsType to Steam and sets the deploymentTag and steam ticket for the Locator to use.
        /// </summary>
        /// <remarks>
        ///     SpatialOS login token and Steam credentials may not be set at the same time when connecting to a deployment.
        /// </remarks>
        /// <param name="deploymentTag">The deployment tag.</param>
        /// <param name="steamTicket">The steam ticket.</param>
        /// <exception cref="ConnectionFailedException">
        ///     Thrown if this method is called after a SpatialOS login token was set before.
        /// </exception>
        public void SetSteamCredentials(string deploymentTag, string steamTicket)
        {
            if (LocatorParameters.CredentialsType == LocatorCredentialsType.LoginToken)
            {
                throw new ConnectionFailedException(
                    "SpatialOS login token and Steam credentials may not be set at the same time when connecting to a deployment.",
                    ConnectionErrorReason.InvalidConfig);
            }

            LocatorParameters.CredentialsType = LocatorCredentialsType.Steam;
            LocatorParameters.Steam.DeploymentTag = deploymentTag;
            LocatorParameters.Steam.Ticket = steamTicket;
        }

        /// <summary>
        ///     Creates a <see cref="LocatorConfig"/> instance from a set of command line arguments.
        /// </summary>
        /// <param name="parsedArgs">A dictionary of command line argument to command line value.</param>
        /// <returns>A <see cref="LocatorConfig"/> instance.</returns>
        public static LocatorConfig CreateConnectionConfigFromCommandLine(Dictionary<string, string> parsedArgs)
        {
            var config = new LocatorConfig();
            var projectName = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.ProjectName, string.Empty);
            config.SetProjectName(projectName);

            var loginToken = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.LoginToken, string.Empty);
            if (!string.IsNullOrEmpty(loginToken))
            {
                config.SetLoginToken(loginToken);
            }

            var deploymentTag = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.DeploymentTag, string.Empty);
            var steamTicket = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.SteamTicket, string.Empty);
            if (!string.IsNullOrEmpty(deploymentTag) && !string.IsNullOrEmpty(steamTicket))
            {
                config.SetSteamCredentials(deploymentTag, steamTicket);
            }

            config.LocatorHost = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.LocatorHost, RuntimeConfigDefaults.LocatorHost);
            config.LinkProtocol = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.LinkProtocol, RuntimeConfigDefaults.LinkProtocol);
            config.WorkerId = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.WorkerId, string.Empty);
            config.WorkerType = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.WorkerType, string.Empty);
            return config;
        }
    }
}
