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
    public class Worker : IDisposable
    {
        public readonly Vector3 Origin;
        public readonly string WorkerType;
        public readonly string WorkerId;

        public ILogDispatcher LogDispatcher;

        public Connection Connection { get; private set; }
        public World World { get; private set; }

        private readonly WorkerDisconnectCallbackSystem disconnectCallbackSystem;

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

                var worker = new Worker(config.WorkerType, connection, logger, origin);
                logger.HandleLog(LogType.Log, new LogEvent("Successfully created a worker")
                    .WithField("WorkerId", worker.WorkerId));
                return worker;
            }
        }

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
