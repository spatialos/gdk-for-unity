using System.Threading.Tasks;
using Improbable.Worker.CInterop;
using AlphaLocator = Improbable.Worker.CInterop.Alpha.Locator;

namespace Improbable.Gdk.Core
{
    public static class ConnectionUtils
    {
        /// <summary>
        ///     Connects to the SpatialOS Runtime via the Receptionist service and creates a <see cref="Connection" /> object
        ///     asynchronously.
        /// </summary>
        /// <param name="config">
        ///     The <see cref="ReceptionistConfig" /> object stores the configuration needed to connect via the
        ///     Receptionist Service.
        /// </param>
        /// <param name="connectionParameters">The <see cref="ConnectionParameters" /> storing </param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> to run this method asynchronously and retrieve the created
        ///     <see cref="Connection" /> object upon connecting successfully.
        /// </returns>
        public static async Task<Connection> CreateConnectionAsync(
            ReceptionistConfig config,
            ConnectionParameters connectionParameters)
        {
            using (var connectionFuture =
                Connection.ConnectAsync(config.ReceptionistHost, config.ReceptionistPort, config.WorkerId,
                    connectionParameters))
            {
                return await TryToConnectAsync(connectionFuture).ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Connects to the SpatialOS Runtime via the Locator service and creates a <see cref="Connection" /> object
        ///     asynchronously.
        /// </summary>
        /// <param name="parameters">
        ///     The <see cref="LocatorConfig" /> object stores the configuration needed to connect via the
        ///     Locator Service.
        /// </param>
        /// <param name="connectionParameters">The <see cref="ConnectionParameters" /> storing </param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> to run this method asynchronously and retrieve the created
        ///     <see cref="Connection" /> object upon connecting successfully.
        /// </returns>
        public static async Task<Connection> CreateConnectionAsync(
            LocatorConfig parameters,
            ConnectionParameters connectionParameters)
        {
            using (var locator = new Locator(parameters.LocatorHost, parameters.LocatorParameters))
            {
                var deploymentList = await GetDeploymentList(locator);

                var deploymentName = parameters.DeploymentListCallback(deploymentList);
                if (string.IsNullOrEmpty(deploymentName))
                {
                    throw new ConnectionFailedException("No deployment name chosen",
                        ConnectionErrorReason.DeploymentNotFound);
                }

                using (var connectionFuture = locator.ConnectAsync(deploymentName, connectionParameters, (_) => true))
                {
                    return await TryToConnectAsync(connectionFuture).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        ///     Connects to the SpatialOS Runtime via the Alpha Locator service and creates a <see cref="Connection" /> object
        ///     asynchronously.
        /// </summary>
        /// <param name="parameters">
        ///     The <see cref="AlphaLocatorConfig" /> object stores the configuration needed to connect via the
        ///     Locator Service.
        /// </param>
        /// <param name="connectionParameters">The <see cref="ConnectionParameters" /> storing </param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> to run this method asynchronously and retrieve the created
        ///     <see cref="Connection" /> object upon connecting successfully.
        /// </returns>
        public static async Task<Connection> CreateConnectionAsync(
            AlphaLocatorConfig parameters,
            ConnectionParameters connectionParameters)
        {
            using (var locator = new AlphaLocator(parameters.LocatorHost, parameters.LocatorParameters))
            {
                using (var connectionFuture = locator.ConnectAsync(connectionParameters))
                {
                    return await TryToConnectAsync(connectionFuture);
                }
            }
        }

        private static async Task<Connection> TryToConnectAsync(Future<Connection> connectionFuture)
        {
            var connection = await Task.Run(() => connectionFuture.Get()).ConfigureAwait(false);
            if (connection.GetConnectionStatusCode() != ConnectionStatusCode.Success)
            {
                throw new ConnectionFailedException(GetConnectionFailureReason(connection),
                    ConnectionErrorReason.CannotEstablishConnection);
            }

            return connection;
        }


        private static async Task<DeploymentList> GetDeploymentList(Locator locator)
        {
            using (var deploymentsFuture = locator.GetDeploymentListAsync())
            {
                var deploymentList = await Task.Run(() => deploymentsFuture.Get()).ConfigureAwait(false);
                // Guard against null refs. This shouldn't be triggered.
                if (!deploymentList.HasValue)
                {
                    throw new ConnectionFailedException("Deployment list future returned null.",
                        ConnectionErrorReason.DeploymentNotFound);
                }

                if (deploymentList.Value.Error != null)
                {
                    throw new ConnectionFailedException(deploymentList.Value.Error,
                        ConnectionErrorReason.DeploymentNotFound);
                }

                return deploymentList.Value;
            }
        }

        private static string GetConnectionFailureReason(Connection connection)
        {
            return connection.GetConnectionStatusCodeDetailString() ?? "No reason found for connection failure";
        }
    }
}
