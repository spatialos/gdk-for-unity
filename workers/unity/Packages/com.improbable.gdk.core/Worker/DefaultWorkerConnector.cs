using System;
using Improbable.Worker.CInterop;
using Alpha = Improbable.Worker.CInterop.Alpha;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public abstract class DefaultWorkerConnector : WorkerConnector
    {
        /// <summary>
        ///     Denotes whether to connect using an external IP address.
        /// </summary>
        public bool UseExternalIp;

        protected override ConnectionService GetConnectionService()
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);

            if (commandLineArgs.ContainsKey(RuntimeConfigNames.SteamDeploymentTag) ||
                commandLineArgs.ContainsKey(RuntimeConfigNames.SteamTicket))
            {
                return ConnectionService.Locator;
            }

            if (commandLineArgs.ContainsKey(RuntimeConfigNames.LoginToken))
            {
                return commandLineArgs.ContainsKey(RuntimeConfigNames.PlayerIdentityToken)
                    ? ConnectionService.AlphaLocator
                    : ConnectionService.Locator;
            }

            return ConnectionService.Receptionist;
        }

        protected override ConnectionParameters GetConnectionParameters(string workerType, ConnectionService service)
        {
            // UseExternalIp needs to be true when using the locator
            var useExternalIp = service == ConnectionService.Locator
                || service == ConnectionService.AlphaLocator
                || UseExternalIp;

            if (Application.isEditor)
            {
                return new ConnectionParameters
                {
                    WorkerType = workerType,
                    Network =
                    {
                        ConnectionType = RuntimeConfigDefaults.LinkProtocol,
                        UseExternalIp = useExternalIp,
                    },
                    EnableProtocolLoggingAtStartup = false,
                    DefaultComponentVtable = new ComponentVtable(),
                };
            }

            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            var linkProtocol = CommandLineUtility.GetCommandLineValue(
                commandLineArgs, RuntimeConfigNames.LinkProtocol, RuntimeConfigDefaults.LinkProtocol);

            return new ConnectionParameters
            {
                WorkerType = workerType,
                Network =
                {
                    ConnectionType = linkProtocol,
                    UseExternalIp = useExternalIp,
                },
                EnableProtocolLoggingAtStartup = false,
                DefaultComponentVtable = new ComponentVtable()
            };
        }

        protected override ReceptionistConfig GetReceptionistConfig(string workerType)
        {
            if (Application.isEditor)
            {
                return new ReceptionistConfig
                {
                    ReceptionistHost = RuntimeConfigDefaults.ReceptionistHost,
                    ReceptionistPort = RuntimeConfigDefaults.ReceptionistPort,
                    WorkerId = CreateNewWorkerId(workerType)
                };
            }

            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            return new ReceptionistConfig
            {
                ReceptionistHost = CommandLineUtility.GetCommandLineValue(
                    commandLineArgs, RuntimeConfigNames.ReceptionistHost, RuntimeConfigDefaults.ReceptionistHost),
                ReceptionistPort = CommandLineUtility.GetCommandLineValue(
                    commandLineArgs, RuntimeConfigNames.ReceptionistPort, RuntimeConfigDefaults.ReceptionistPort),
                WorkerId = CommandLineUtility.GetCommandLineValue(
                    commandLineArgs, RuntimeConfigNames.WorkerId, CreateNewWorkerId(workerType)),
            };
        }

        protected override LocatorConfig GetLocatorConfig()
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);

            var projectName = CommandLineUtility.GetCommandLineValue(
                commandLineArgs, RuntimeConfigNames.ProjectName, string.Empty);

            if (string.IsNullOrEmpty(projectName))
            {
                throw new ConnectionFailedException("Project name is not set. Can't connect via the Locator.", ConnectionErrorReason.InvalidConfig);
            }

            var loginToken = CommandLineUtility.GetCommandLineValue(
                commandLineArgs, RuntimeConfigNames.LoginToken, string.Empty);
            var steamDeploymentTag = CommandLineUtility.GetCommandLineValue(
                commandLineArgs, RuntimeConfigNames.SteamDeploymentTag, string.Empty);
            var steamTicket = CommandLineUtility.GetCommandLineValue(
                commandLineArgs, RuntimeConfigNames.SteamTicket, string.Empty);


            LocatorCredentialsType credentialType;
            if (!string.IsNullOrEmpty(loginToken))
            {
                credentialType = LocatorCredentialsType.LoginToken;
            }
            else if (!string.IsNullOrEmpty(steamDeploymentTag) && !string.IsNullOrEmpty(steamTicket))
            {
                credentialType = LocatorCredentialsType.Steam;
            }
            else
            {
                throw new ConnectionFailedException("Neither steam credentials nor login token is set. Can't connect via the Locator.", ConnectionErrorReason.InvalidConfig);
            }

            return new LocatorConfig
            {
                DeploymentListCallback = SelectDeploymentName,
                LocatorHost = CommandLineUtility.GetCommandLineValue(
                    commandLineArgs, RuntimeConfigNames.LocatorHost, RuntimeConfigDefaults.LocatorHost),
                LocatorParameters = new LocatorParameters
                {
                    CredentialsType = credentialType,
                    LoginToken = new LoginTokenCredentials
                    {
                        Token = loginToken,
                    },
                    Steam = new SteamCredentials
                    {
                        DeploymentTag = steamDeploymentTag,
                        Ticket = steamTicket,
                    },
                    ProjectName = projectName,
                }
            };
        }

        protected override AlphaLocatorConfig GetAlphaLocatorConfig(string workerType)
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            return new AlphaLocatorConfig
            {
                LocatorHost = CommandLineUtility.GetCommandLineValue(
                    commandLineArgs, RuntimeConfigNames.LocatorHost, RuntimeConfigDefaults.LocatorHost),
                LocatorParameters = new Alpha.LocatorParameters
                {
                    PlayerIdentity = new Alpha.PlayerIdentityCredentials
                    {
                        PlayerIdentityToken = CommandLineUtility.GetCommandLineValue(
                            commandLineArgs, RuntimeConfigNames.PlayerIdentityToken, string.Empty),
                        LoginToken = CommandLineUtility.GetCommandLineValue(
                            commandLineArgs, RuntimeConfigNames.LoginToken, string.Empty)
                    },
                },
            };
        }
    }
}
