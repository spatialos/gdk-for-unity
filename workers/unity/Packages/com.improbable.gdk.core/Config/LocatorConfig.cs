using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Improbable.Worker;

namespace Improbable.Gdk.Core
{
    public class LocatorConfig : ConnectionConfig
    {
        public string LocatorHost = RuntimeConfigDefaults.LocatorHost;
        public readonly LocatorParameters LocatorParameters = new LocatorParameters();

        public LocatorConfig()
        {
            UseExternalIp = true;
        }

        public override void Validate()
        {
            ValidateConfig(LocatorHost, RuntimeConfigNames.LocatorHost);
            switch (LocatorParameters.CredentialsType)
            {
                case LocatorCredentialsType.LoginToken:
                    ValidateConfig(LocatorParameters.LoginToken.Token, RuntimeConfigNames.LoginToken);
                    ValidateConfig(LocatorParameters.ProjectName, RuntimeConfigNames.ProjectName);
                    break;
                case LocatorCredentialsType.Steam:
                    ValidateConfig(LocatorParameters.Steam.Ticket, RuntimeConfigNames.SteamToken);
                    ValidateConfig(LocatorParameters.Steam.DeploymentTag, RuntimeConfigNames.SteamDeploymentTag);
                    ValidateSteamDeploymentTag(LocatorParameters.Steam.DeploymentTag);
                    break;
            }
        }

        private void ValidateSteamDeploymentTag(string deploymentTag)
        {
            const string regex = @"^[A-Za-z0-9][A-Za-z0-9_]*$";
            var match = Regex.Match(deploymentTag, regex);

            if (!match.Success)
            {
                throw new ConnectionFailedException(
                    $"Config validation failed with: No valid {RuntimeConfigNames.SteamDeploymentTag} - must match the following regex: {regex} ",
                    ConnectionErrorReason.InvalidConfig);
            }
        }

        public void SetProjectName(string projectName)
        {
            LocatorParameters.ProjectName = projectName;
        }

        public void SetLoginToken(string loginToken)
        {
            if (loginToken.Equals(string.Empty) && LocatorParameters.CredentialsType.Equals(null))
            {
                return;
            }

            LocatorParameters.CredentialsType = LocatorCredentialsType.LoginToken;
            LocatorParameters.LoginToken = new LoginTokenCredentials
            {
                Token = loginToken
            };
        }

        public void SetSteamCredentials(string steamToken, string deploymentTag)
        {
            if (steamToken.Equals(string.Empty))
            {
                return;
            }

            LocatorParameters.CredentialsType = LocatorCredentialsType.Steam;
            LocatorParameters.Steam = new SteamCredentials
            {
                Ticket = steamToken,
                DeploymentTag = deploymentTag
            };
        }

        public static LocatorConfig CreateConnectionConfigFromCommandLine(Dictionary<string, string> parsedArgs)
        {
            var config = new LocatorConfig();
            var loginToken = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.LoginToken, string.Empty);
            var projectName = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.ProjectName, string.Empty);
            var steamDeploymentTag = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.SteamDeploymentTag, string.Empty);
            var steamToken = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.SteamToken, string.Empty);
            config.SetLoginToken(loginToken);
            config.SetSteamCredentials(steamToken, steamDeploymentTag);
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
