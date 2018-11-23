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
        ///     Timeout in milliseconds for the connection to SpatialOS to be established.
        /// </summary>
        /// <remarks>
        ///     Default is 60000ms (60 seconds).
        /// </remarks>
        public ulong ConnectionTimeoutMillis = 60000;

        /// <summary>
        ///     Default timeout in milliseconds for worker commands if one is not specified when command is sent.
        /// </summary>
        /// <remarks>
        ///     Default is 5000ms (5 seconds).
        /// </remarks>
        public uint DefaultCommandTimeoutMillis = 5000;

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
                    UseExternalIp = UseExternalIp,
                    DefaultCommandTimeoutMillis = DefaultCommandTimeoutMillis,
                    ConnectionTimeoutMillis = ConnectionTimeoutMillis,
                };
            }
            else
            {
                var commandLineArguments = Environment.GetCommandLineArgs();
                var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
                config = ReceptionistConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
                config.WorkerType = workerType;
                config.UseExternalIp = UseExternalIp;
                config.DefaultCommandTimeoutMillis = DefaultCommandTimeoutMillis;
                config.ConnectionTimeoutMillis = ConnectionTimeoutMillis;
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
            config.DefaultCommandTimeoutMillis = DefaultCommandTimeoutMillis;
            config.ConnectionTimeoutMillis = ConnectionTimeoutMillis;
            return config;
        }
    }
}
