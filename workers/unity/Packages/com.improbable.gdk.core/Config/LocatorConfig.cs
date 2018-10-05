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
            ValidateConfig(LocatorHost, RuntimeConfigNames.LocatorHost);
            if (LocatorParameters.CredentialsType == LocatorCredentialsType.LoginToken)
            {
                ValidateConfig(LocatorParameters.LoginToken.Token, RuntimeConfigNames.LoginToken);
                ValidateConfig(LocatorParameters.ProjectName, RuntimeConfigNames.ProjectName);
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
        ///     Sets the login token for the Locator to use.
        /// </summary>
        /// <param name="loginToken">The login token.</param>
        public void SetLoginToken(string loginToken)
        {
            LocatorParameters.CredentialsType = LocatorCredentialsType.LoginToken;
            LocatorParameters.LoginToken = new LoginTokenCredentials
            {
                Token = loginToken
            };
        }

        /// <summary>
        ///     Creates a <see cref="LocatorConfig"/> instance from a set of command line arguments.
        /// </summary>
        /// <param name="parsedArgs">A dictionary of command line argument to command line value.</param>
        /// <returns>A <see cref="LocatorConfig"/> instance.</returns>
        public static LocatorConfig CreateConnectionConfigFromCommandLine(Dictionary<string, string> parsedArgs)
        {
            var config = new LocatorConfig();
            var loginToken = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.LoginToken, string.Empty);
            var projectName = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.ProjectName, string.Empty);
            config.SetLoginToken(loginToken);
            config.SetProjectName(projectName);
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
