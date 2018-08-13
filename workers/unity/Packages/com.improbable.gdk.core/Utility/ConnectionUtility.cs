using System.Collections.Generic;
using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public static class ConnectionUtility
    {
        public static Connection ConnectToSpatial(ReceptionistConfig config, string workerType, string workerId)
        {
            config.Validate();

            Debug.Log("Attempting connection to SpatialOS...");

            var parameters = CreateConnectionParameters(config, workerType);
            using (var connectionFuture = Connection
                .ConnectAsync(config.ReceptionistHost, config.ReceptionistPort,
                    workerId, parameters))
            {
                return TryToConnect(connectionFuture);
            }
        }

        public static Connection LocatorConnectToSpatial(LocatorConfig config, string workerType)
        {
            config.Validate();

            using (var locator = new Locator(config.LocatorHost, config.LocatorParameters))
            {
                Debug.Log("Attempting to retrieve deployment name...");
                var deploymentName = GetDeploymentName(locator);

                Debug.Log("Successfully obtained deployment name!");

                Debug.Log("Attempting connection to SpatialOS with Locator...");
                var parameters = CreateConnectionParameters(config, workerType);
                using (var connectionFuture = locator
                    .ConnectAsync(deploymentName, parameters, status => true))
                {
                    return TryToConnect(connectionFuture);
                }
            }
        }

        public static ConnectionConfig CreateConnectionConfigFromCommandLine(Dictionary<string, string> parsedArgs)
        {
            if (parsedArgs.ContainsKey(RuntimeConfigNames.LoginToken))
            {
                return LocatorConfig.CreateConnectionConfigFromCommandLine(parsedArgs);
            }

            return ReceptionistConfig.CreateConnectionConfigFromCommandLine(parsedArgs);
        }

        private static ConnectionParameters CreateConnectionParameters(ConnectionConfig config, string workerType)
        {
            var connectionParameters = new ConnectionParameters
            {
                WorkerType = workerType,
                Network =
                {
                    ConnectionType = config.LinkProtocol,
                    UseExternalIp = config.UseExternalIp
                },
                EnableProtocolLoggingAtStartup = config.EnableProtocolLoggingAtStartup
            };
            return connectionParameters;
        }

        private static Connection TryToConnect(Future<Connection> connectionFuture)
        {
            var connection = connectionFuture.Get(RuntimeConfigDefaults.ConnectionTimeout);

            if (!connection.HasValue || !connection.Value.IsConnected)
            {
                throw new ConnectionFailedException("Failed to connect to SpatialOS.",
                    ConnectionErrorReason.CannotEstablishConnection);
            }

            Debug.Log("Successfully connected to SpatialOS!");

            return connection.Value;
        }

        private static string GetDeploymentName(Locator locator)
        {
            using (var deploymentsFuture = locator.GetDeploymentListAsync())
            {
                var deployments = deploymentsFuture.Get(RuntimeConfigDefaults.ConnectionTimeout);

                if (!deployments.HasValue)
                {
                    throw new ConnectionFailedException("Failed to retrieve deployment.",
                        ConnectionErrorReason.DeploymentNotFound);
                }

                if (deployments.Value.Error != null)
                {
                    throw new ConnectionFailedException(
                        $"Failed to obtain deployment name with error: {deployments.Value.Error}.",
                        ConnectionErrorReason.DeploymentNotFound);
                }

                if (deployments.Value.Deployments.Count == 0)
                {
                    throw new ConnectionFailedException("Received an empty list of deployments.",
                        ConnectionErrorReason.DeploymentNotFound);
                }

                return deployments.Value.Deployments[0].DeploymentName;
            }
        }

        public static void Disconnect(Connection connection)
        {
            Debug.Log("Disposing of connection");
            connection.Dispose();
        }
    }
}
