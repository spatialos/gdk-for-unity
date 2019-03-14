using System;
using System.Threading.Tasks;
using Improbable.Gdk.ReactiveComponents;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;
using Improbable.Gdk.Subscriptions;
using AlphaLocator = Improbable.Worker.CInterop.Alpha.Locator;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents a SpatialOS worker.
    /// </summary>
    public class Worker : IDisposable
    {
        /// <summary>
        ///     The origin of the worker in global Unity space.
        /// </summary>
        public readonly Vector3 Origin;

        /// <summary>
        ///     The type of the worker.
        /// </summary>
        public readonly string WorkerType;

        /// <summary>
        ///     The worker ID.
        /// </summary>
        /// <remarks>
        ///     Unique for a given SpatialOS deployment.
        /// </remarks>
        public readonly string WorkerId;

        /// <summary>
        ///     The logger for this worker.
        /// </summary>
        public ILogDispatcher LogDispatcher;

        /// <summary>
        ///     The connection to the SpatialOS runtime.
        /// </summary>
        public Connection Connection { get; private set; }

        /// <summary>
        ///     The ECS world associated with this worker.
        /// </summary>
        public World World { get; private set; }

        private readonly WorkerDisconnectCallbackSystem disconnectCallbackSystem;

        /// <summary>
        ///     An event that triggers when the worker is disconnected.
        /// </summary>
        public event Action<string> OnDisconnect
        {
            add => disconnectCallbackSystem.OnDisconnected += value;
            remove => disconnectCallbackSystem.OnDisconnected -= value;
        }

        private Worker(string workerType, Connection connection, ILogDispatcher logDispatcher, Vector3 origin)
        {
            Origin = origin;
            WorkerType = workerType;
            WorkerId = connection.GetWorkerId();

            Connection = connection;
            LogDispatcher = logDispatcher;
            logDispatcher.Connection = connection;
            logDispatcher.WorkerType = workerType;

            World = new World(Connection.GetWorkerId());
            AddCoreSystems();

            // This isn't a core system, this is for an easy disconnect event
            disconnectCallbackSystem = World.GetOrCreateManager<WorkerDisconnectCallbackSystem>();
        }

        /// <summary>
        ///     Tries to connect to the SpatialOS Runtime and creates the worker responsible for the connection upon successfully
        ///     connecting.
        /// </summary>
        /// <param name="connectionFuture">
        ///     The <see cref="Future{T}" /> of the <see cref="Connection" /> object that we use to
        ///     connect to the SpatialOS Runtime.
        /// </param>
        /// <param name="workerType">The type of the worker.</param>
        /// <param name="logger">The logger used by this worker.</param>
        /// <param name="origin">The origin of this worker in the local Unity space.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> to run this method asyncally and retrieve the created <see cref="Worker" />
        ///     object upon connecting successfully.
        /// </returns>
        private static async Task<Worker> TryToConnectAsync(Future<Connection> connectionFuture,
            string workerType,
            ILogDispatcher logger,
            Vector3 origin)
        {
            var connection = await Task.Run(() => connectionFuture.Get());
            if (connection.GetConnectionStatusCode() != ConnectionStatusCode.Success)
            {
                throw new ConnectionFailedException(GetConnectionFailureReason(connection),
                    ConnectionErrorReason.CannotEstablishConnection);
            }

            // A check is needed for the case that play mode is exited before the connection can complete.
            if (!Application.isPlaying)
            {
                connection.Dispose();
                throw new ConnectionFailedException("Editor application stopped",
                    ConnectionErrorReason.EditorApplicationStopped);
            }

            var worker = new Worker(workerType, connection, logger, origin);
            logger.HandleLog(LogType.Log, new LogEvent("Successfully created a worker")
                .WithField("WorkerId", worker.WorkerId));
            return worker;
        }

        /// <summary>
        ///     Connects to the SpatialOS Runtime via the Receptionist service and creates a <see cref="Worker" /> object
        ///     asynchronously.
        /// </summary>
        /// <param name="config">
        ///     The <see cref="ReceptionistConfig" /> object stores the configuration needed to connect via the
        ///     Receptionist Service.
        /// </param>
        /// <param name="connectionParameters">The <see cref="ConnectionParameters" /> storing </param>
        /// <param name="logger">The logger used by this worker.</param>
        /// <param name="origin">The origin of this worker in the local Unity space.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> to run this method asynchronously and retrieve the created
        ///     <see cref="Worker" /> object upon connecting successfully.
        /// </returns>
        public static async Task<Worker> CreateWorkerAsync(
            ReceptionistConfig config,
            ConnectionParameters connectionParameters,
            ILogDispatcher logger, Vector3 origin)
        {
            using (var connectionFuture =
                Connection.ConnectAsync(config.ReceptionistHost, config.ReceptionistPort, config.WorkerId,
                    connectionParameters))
            {
                return await TryToConnectAsync(connectionFuture, connectionParameters.WorkerType, logger, origin);
            }
        }

        /// <summary>
        ///     Connects to the SpatialOS Runtime via the Locator service and creates a <see cref="Worker" /> object
        ///     asynchronously.
        /// </summary>
        /// <param name="config">
        ///     The <see cref="LocatorConfig" /> object stores the configuration needed to connect via the
        ///     Receptionist Service.
        /// </param>
        /// <param name="connectionParameters">The <see cref="ConnectionParameters" /> storing </param>
        /// <param name="logger">The logger used by this worker.</param>
        /// <param name="origin">The origin of this worker in the local Unity space.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> to run this method asynchronously and retrieve the created
        ///     <see cref="Worker" /> object upon connecting successfully.
        /// </returns>
        public static async Task<Worker> CreateWorkerAsync(
            LocatorConfig parameters,
            ConnectionParameters connectionParameters,
            ILogDispatcher logger, Vector3 origin)
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
                    return await TryToConnectAsync(connectionFuture, connectionParameters.WorkerType, logger, origin);
                }
            }
        }

        /// <summary>
        ///     Connects to the SpatialOS Runtime via the Alpha Locator service and creates a <see cref="Worker" /> object
        ///     asynchronously.
        /// </summary>
        /// <param name="config">
        ///     The <see cref="AlphaLocatorConfig" /> object stores the configuration needed to connect via the
        ///     Receptionist Service.
        /// </param>
        /// <param name="connectionParameters">The <see cref="ConnectionParameters" /> storing </param>
        /// <param name="logger">The logger used by this worker.</param>
        /// <param name="origin">The origin of this worker in the local Unity space.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}" /> to run this method asynchronously and retrieve the created
        ///     <see cref="Worker" /> object upon connecting successfully.
        /// </returns>
        public static async Task<Worker> CreateWorkerAsync(
            AlphaLocatorConfig parameters,
            ConnectionParameters connectionParameters,
            ILogDispatcher logger, Vector3 origin)
        {
            using (var locator = new AlphaLocator(parameters.LocatorHost, parameters.LocatorParameters))
            {
                using (var connectionFuture = locator.ConnectAsync(connectionParameters))
                {
                    return await TryToConnectAsync(connectionFuture, connectionParameters.WorkerType, logger, origin);
                }
            }
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

        private void AddCoreSystems()
        {
            var connectionHandler = new SpatialOSConnectionHandler(Connection);
            World.CreateManager<WorkerSystem>(connectionHandler, Connection, LogDispatcher, WorkerType, Origin);
            World.GetOrCreateManager<CommandSystem>();
            World.GetOrCreateManager<ComponentUpdateSystem>();
            World.GetOrCreateManager<EntitySystem>();
            World.GetOrCreateManager<ComponentSendSystem>();
            World.GetOrCreateManager<SpatialOSReceiveSystem>();
            World.GetOrCreateManager<SpatialOSSendSystem>();
            World.GetOrCreateManager<EcsViewSystem>();
            World.GetOrCreateManager<CleanTemporaryComponentsSystem>();

            // Subscriptions systems
            World.GetOrCreateManager<CommandCallbackSystem>();
            World.GetOrCreateManager<ComponentConstraintsCallbackSystem>();
            World.GetOrCreateManager<ComponentCallbackSystem>();
            World.GetOrCreateManager<RequireLifecycleSystem>();
            World.GetOrCreateManager<SubscriptionSystem>();

            // Reactive components
            ReactiveComponentsHelper.AddCommonSystems(World);
        }

        public void Dispose()
        {
            World?.Dispose();
            World = null;
            LogDispatcher?.Dispose();
            LogDispatcher = null;
            Connection?.Dispose();
            Connection = null;
        }
    }
}
