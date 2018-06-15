using System.Collections.Generic;
using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public static class ConnectionUtility
    {
        public static Connection ConnectToSpatial(ReceptionistConfig config, string workerType, string workerId)
        {
            try
            {
                config.Validate();
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError($"Config validation failed with: {e.Message}");
                return null;
            }

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
            try
            {
                config.Validate();
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError($"Config validation failed with: {e.Message}");
                return null;
            }

            using (var locator = new Locator(config.LocatorHost, config.LocatorParameters))
            {
                Debug.Log("Attempting to retrieve deployment name...");
                var deploymentName = GetDeploymentName(locator);
                if (deploymentName == null)
                {
                    Debug.Log("Failed to retrieve deployment name.");
                    return null;
                }

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
            else
            {
                return ReceptionistConfig.CreateConnectionConfigFromCommandLine(parsedArgs);
            }
        }

        private static ConnectionParameters CreateConnectionParameters(ConnectionConfig config, string workerType)
        {
            var connectionParameters = new ConnectionParameters
            {
                WorkerType = workerType,
                Network =
                {
                    ConnectionType = config.LinkProtocol,
                    UseExternalIp = config.UseExternalIp,
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
                Debug.LogError("Failed to connect to SpatialOS.");
                return null;
            }

            Debug.Log("Successfully connected to SpatialOS!");

            return connection.Value;
        }

        private static string GetDeploymentName(Locator locator)
        {
            using (var deploymentsFuture = locator.GetDeploymentListAsync())
            {
                var deployments = deploymentsFuture.Get(RuntimeConfigDefaults.ConnectionTimeout);
                return deployments.HasValue ? deployments.Value.Deployments[0].DeploymentName : null;
            }
        }

        public static void Disconnect(Connection connection)
        {
            Debug.Log("Disposing of connection");
            connection.Dispose();
        }
    }
}
