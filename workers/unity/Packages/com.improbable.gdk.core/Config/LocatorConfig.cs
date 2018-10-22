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

        private const string UseOfMultipleCredentialTypesError = "SpatialOS login token and Steam credentials may not be set at the same time.";

        public LocatorConfig()
        {
            UseExternalIp = true;
        }

        /// <summary>
        ///    Checks that the connection configuration is valid. This does not guarantee a successful connection.
        /// </summary>
        /// <param name="errorMessage">Reason for failing the validation in case false is returned.</param>
        /// <returns>True, if the connection configuration is valid.</returns>
        public override bool Validate(out string errorMessage)
        {
            if (string.IsNullOrEmpty(LocatorHost))
            {
                errorMessage = string.Format(MissingConfigError, RuntimeConfigNames.LocatorHost);
                return false;
            }

            if (string.IsNullOrEmpty(LocatorParameters.ProjectName))
            {
                errorMessage = string.Format(MissingConfigError, RuntimeConfigNames.ProjectName);
                return false;
            }

            if (LocatorParameters.CredentialsType != LocatorCredentialsType.LoginToken &&
                LocatorParameters.CredentialsType != LocatorCredentialsType.Steam)
            {
                errorMessage = "LocatorConfig is using an invalid CredentialsType. Valid CredentialsTypes are: LoginToken, Steam.";
                return false;
            }

            if (LocatorParameters.CredentialsType == LocatorCredentialsType.LoginToken)
            {
                if (string.IsNullOrEmpty(LocatorParameters.LoginToken.Token))
                {
                    errorMessage = string.Format(MissingConfigError, RuntimeConfigNames.LoginToken);
                    return false;
                }

                if (!string.IsNullOrEmpty(LocatorParameters.Steam.DeploymentTag) ||
                    !string.IsNullOrEmpty(LocatorParameters.Steam.Ticket))
                {
                    errorMessage = UseOfMultipleCredentialTypesError;
                    return false;
                }
            }

            if (LocatorParameters.CredentialsType == LocatorCredentialsType.Steam)
            {
                if (string.IsNullOrEmpty(LocatorParameters.Steam.DeploymentTag))
                {
                    errorMessage = string.Format(MissingConfigError, RuntimeConfigNames.SteamDeploymentTag);
                    return false;
                }

                if (string.IsNullOrEmpty(LocatorParameters.Steam.Ticket))
                {
                    errorMessage = string.Format(MissingConfigError, RuntimeConfigNames.SteamTicket);
                    return false;
                }

                if (!string.IsNullOrEmpty(LocatorParameters.LoginToken.Token))
                {
                    errorMessage = UseOfMultipleCredentialTypesError;
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
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
        ///     Sets the login token for the locator to use. Additionally, sets the CredentialsType to LocatorCredentialsType.LoginToken.
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
                    "SpatialOS login token may not be set when Steam credentials are already specified.",
                    ConnectionErrorReason.InvalidConfig);
            }

            LocatorParameters.CredentialsType = LocatorCredentialsType.LoginToken;
            LocatorParameters.LoginToken.Token = loginToken;
        }

        /// <summary>
        ///     Sets the Steam deployment tag and steam ticket for the locator to use. Additionally, sets the CredentialsType to LocatorCredentialsType.Steam.
        /// </summary>
        /// <remarks>
        ///     SpatialOS login token and Steam credentials may not be set at the same time when connecting to a deployment.
        /// </remarks>
        /// <param name="steamDeploymentTag">The deployment tag.</param>
        /// <param name="steamTicket">The steam ticket.</param>
        /// <exception cref="ConnectionFailedException">
        ///     Thrown if this method is called after a SpatialOS login token was set before.
        /// </exception>
        public void SetSteamCredentials(string steamDeploymentTag, string steamTicket)
        {
            if (LocatorParameters.CredentialsType == LocatorCredentialsType.LoginToken)
            {
                throw new ConnectionFailedException(
                    "Steam credentials may not be set when a SpatialOS login token is already specified.",
                    ConnectionErrorReason.InvalidConfig);
            }

            LocatorParameters.CredentialsType = LocatorCredentialsType.Steam;
            LocatorParameters.Steam.DeploymentTag = steamDeploymentTag;
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

            var steamDeploymentTag = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.SteamDeploymentTag, string.Empty);
            var steamTicket = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.SteamTicket, string.Empty);
            if (!string.IsNullOrEmpty(steamDeploymentTag) && !string.IsNullOrEmpty(steamTicket))
            {
                config.SetSteamCredentials(steamDeploymentTag, steamTicket);
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
