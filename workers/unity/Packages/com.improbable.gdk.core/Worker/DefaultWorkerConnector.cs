using System;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public abstract class DefaultWorkerConnector : WorkerConnector
    {
        /// <summary>
        ///     Denotes whether to connect using an external IP address.
        /// </summary>
        public bool UseExternalIp;

        /// <summary>
        ///     Determines whether to connect via the locator.
        /// </summary>
        /// <returns>True, if should connect via the Locator, false otherwise.</returns>
        protected override bool ShouldUseLocator()
        {
            if (Application.isEditor)
            {
                return false;
            }

            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            var shouldUseLocator = commandLineArgs.ContainsKey(RuntimeConfigNames.LoginToken) ||
                commandLineArgs.ContainsKey(RuntimeConfigNames.SteamDeploymentTag) ||
                commandLineArgs.ContainsKey(RuntimeConfigNames.SteamTicket);
            return shouldUseLocator;
        }

        protected override ReceptionistConfig GetReceptionistConfig(string workerType)
        {
            ReceptionistConfig config;

            if (Application.isEditor)
            {
                config = new ReceptionistConfig
                {
                    WorkerType = workerType,
                    WorkerId = CreateNewWorkerId(workerType),
                    UseExternalIp = UseExternalIp
                };
            }
            else
            {
                var commandLineArguments = Environment.GetCommandLineArgs();
                var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
                config = ReceptionistConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
                config.WorkerType = workerType;
                config.UseExternalIp = UseExternalIp;
                if (!commandLineArgs.ContainsKey(RuntimeConfigNames.WorkerId))
                {
                    config.WorkerId = CreateNewWorkerId(workerType);
                }
            }

            return config;
        }

        protected override LocatorConfig GetLocatorConfig(string workerType)
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            var config = LocatorConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
            config.WorkerType = workerType;
            config.WorkerId = CreateNewWorkerId(workerType);
            return config;
        }
    }
}
