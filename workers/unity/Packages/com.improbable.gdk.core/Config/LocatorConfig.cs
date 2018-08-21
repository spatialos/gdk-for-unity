using System.Collections.Generic;
using Improbable.Worker;

namespace Improbable.Gdk.Core
{
    public class LocatorConfig : ConnectionConfig
    {
        public string LocatorHost = RuntimeConfigDefaults.LocatorHost;
        public LocatorParameters LocatorParameters = new LocatorParameters();

        public LocatorConfig()
        {
            UseExternalIp = true;
        }

        public override void Validate()
        {
            ValidateConfig(LocatorHost, RuntimeConfigNames.LocatorHost);
            if (LocatorParameters.CredentialsType.Equals(LocatorCredentialsType.LoginToken))
            {
                ValidateConfig(LocatorParameters.LoginToken.Token, RuntimeConfigNames.LoginToken);
                ValidateConfig(LocatorParameters.ProjectName, RuntimeConfigNames.ProjectName);
            }
        }

        public void SetProjectName(string projectName)
        {
            LocatorParameters.ProjectName = projectName;
        }

        public void SetLoginToken(string loginToken)
        {
            LocatorParameters.CredentialsType = LocatorCredentialsType.LoginToken;
            LocatorParameters.LoginToken = new LoginTokenCredentials
            {
                Token = loginToken
            };
        }

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
