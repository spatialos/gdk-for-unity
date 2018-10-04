using System;
using System.Threading.Tasks;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

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
        ///    Unique for a given SpatialOS deployment.
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
        ///     Asynchronously connects and creates a worker via the Receptionist.
        /// </summary>
        /// <param name="config">The Receptionist connection configuration.</param>
        /// <param name="logger">The logger for this worker.</param>
        /// <param name="origin">The origin of this worker in local Unity space.</param>
        /// <returns>A task that returns a Worker when finished.</returns>
        /// <exception cref="ConnectionFailedException">
        ///     Thrown if the worker fails to connect.
        /// </exception>
        public static async Task<Worker> CreateWorkerAsync(ReceptionistConfig config, ILogDispatcher logger,
            Vector3 origin)
        {
            var connectionParams = config.CreateConnectionParameters();
            using (var connectionFuture = Connection.ConnectAsync(config.ReceptionistHost, config.ReceptionistPort,
                config.WorkerId, connectionParams))
            {
                var connection = await Task.Run(() => connectionFuture.Get());
                if (!connection.IsConnected)
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

                var worker = new Worker(config.WorkerType, connection, logger, origin);
                logger.HandleLog(LogType.Log, new LogEvent("Successfully created a worker")
                    .WithField("WorkerId", worker.WorkerId));
                return worker;
            }
        }

        /// <summary>
        ///     Asynchronously connects and creates a worker via the Locator.
        /// </summary>
        /// <param name="config">The Locator connection configuration.</param>
        /// <param name="logger">The logger for this worker.</param>
        /// <param name="origin">The origin of this worker in local Unity space.</param>
        /// <returns>A task that returns a Worker when finished.</returns>
        /// <exception cref="ConnectionFailedException">
        ///     Thrown if the worker fails to connect.
        /// </exception>
        public static async Task<Worker> CreateWorkerAsync(LocatorConfig config,
            Func<DeploymentList, string> deploymentListCallback, ILogDispatcher logger, Vector3 origin)
        {
            using (var locator = new Locator(config.LocatorHost, config.LocatorParameters))
            {
                var deploymentList = await GetDeploymentList(locator);

                var deploymentName = deploymentListCallback(deploymentList);
                if (String.IsNullOrEmpty(deploymentName))
                {
                    throw new ConnectionFailedException("No deployment name chosen",
                        ConnectionErrorReason.DeploymentNotFound);
                }

                var connectionParams = config.CreateConnectionParameters();
                using (var connectionFuture = locator.ConnectAsync(deploymentName, connectionParams, (_) => true))
                {
                    var connection = await Task.Run(() => connectionFuture.Get());
                    if (!connection.IsConnected)
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

                    var worker = new Worker(config.WorkerType, connection, logger, origin);
                    logger.HandleLog(LogType.Log, new LogEvent("Successfully created a worker")
                        .WithField("WorkerId", worker.WorkerId));
                    return worker;
                }
            }
        }

        private static async Task<DeploymentList> GetDeploymentList(Locator locator)
        {
            using (var deploymentsFuture = locator.GetDeploymentListAsync())
            {
                var deploymentList = await Task.Run(() => deploymentsFuture.Get()).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(deploymentList.Error))
                {
                    throw new ConnectionFailedException(deploymentList.Error, ConnectionErrorReason.DeploymentNotFound);
                }

                return deploymentList;
            }
        }

        private static string GetConnectionFailureReason(Connection connection)
        {
            var connectionFailureReason = "No reason found for connection failure";
            var dispatcher = new Dispatcher();
            dispatcher.OnDisconnect(op => connectionFailureReason = op.Reason);
            dispatcher.Process(connection.GetOpList(0));

            return connectionFailureReason;
        }

        private void AddCoreSystems()
        {
            World.CreateManager<WorkerSystem>(Connection, LogDispatcher, WorkerType, Origin);
            World.GetOrCreateManager<SpatialOSSendSystem>();
            World.GetOrCreateManager<SpatialOSReceiveSystem>();
            World.GetOrCreateManager<CleanReactiveComponentsSystem>();
            World.GetOrCreateManager<WorldCommandsCleanSystem>();
            World.GetOrCreateManager<WorldCommandsSendSystem>();
            World.GetOrCreateManager<CommandRequestTrackerSystem>();
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
