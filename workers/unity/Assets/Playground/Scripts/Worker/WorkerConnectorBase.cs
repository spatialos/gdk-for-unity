using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public abstract class WorkerConnectorBase : MonoBehaviour, IDisposable
    {
        private delegate Task<Worker> ConnectionDelegate();

        public GameObject LevelPrefab;
        public int MaxConnectionAttempts = 3;
        public bool UseExternalIp = false;

        public Worker Worker;

        protected WorkerConnectorBase[] RequiredWorkerConnection;

        private GameObject levelInstance;

        private bool hasFinishedConnectionAttempt = false;
        private static readonly object FinishedConnectionAttemptLock = new object();
        private Task connectionAttemptFinishedTask;

        private void Awake()
        {
            connectionAttemptFinishedTask = Task.Run(() =>
            {
                lock (FinishedConnectionAttemptLock)
                {
                    while (!hasFinishedConnectionAttempt)
                    {
                        Monitor.Wait(FinishedConnectionAttemptLock);
                    }
                }
            });
        }

        // Important run in this step as otherwise it can interfere with the the domain unloading logic
        private void OnApplicationQuit()
        {
            Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public async Task Connect(string workerType, ILogDispatcher logger)
        {
            // Check that other workers have finished trying to connect before this one starts
            // This prevents races on the workers starting and races on when we start ticking systems
            if (RequiredWorkerConnection != null)
            {
                Task[] requiredWorkers = RequiredWorkerConnection
                    .Select(connection => connection.connectionAttemptFinishedTask).ToArray();
                await Task.WhenAll(requiredWorkers);
            }

            try
            {
                var origin = transform.position;
                if (ShouldUseLocator())
                {
                    var config = GetLocatorConfig(workerType);
                    ConnectionDelegate connectionDelegate = async () =>
                        await Worker.CreateWorkerAsync(config, SelectDeploymentName, logger, origin)
                            .ConfigureAwait(false);
                    var worker =
                        await ConnectWithRetries(connectionDelegate, MaxConnectionAttempts, logger, workerType);
                    InitializeWorker(worker);
                }
                else
                {
                    var config = GetReceptionistConfig(workerType);
                    ConnectionDelegate connectionDelegate = async () =>
                        await Worker.CreateWorkerAsync(config, logger, origin).ConfigureAwait(false);
                    var worker =
                        await ConnectWithRetries(connectionDelegate, MaxConnectionAttempts, logger, workerType);
                    InitializeWorker(worker);
                }
            }
            catch (Exception e)
            {
                logger.HandleLog(LogType.Error, new LogEvent("Failed to create worker")
                    .WithException(e)
                    .WithField("WorkerType", workerType)
                    .WithField("Message", e.Message));
                Dispose();
            }

            lock (FinishedConnectionAttemptLock)
            {
                hasFinishedConnectionAttempt = true;
                Monitor.PulseAll(FinishedConnectionAttemptLock);
            }
        }

        protected virtual void AddWorkerSystems()
        {
        }

        protected virtual string SelectDeploymentName(DeploymentList deployments)
        {
            return deployments.Deployments[0].DeploymentName;
        }

        private static async Task<Worker> ConnectWithRetries(ConnectionDelegate connectionDelegate, int attempts,
            ILogDispatcher logger, string workerType)
        {
            while (attempts > 0)
            {
                try
                {
                    var worker = await connectionDelegate();
                    return worker;
                }
                catch (ConnectionFailedException e)
                {
                    logger.HandleLog(LogType.Error,
                        new LogEvent($"Failed attempt to create worker")
                            .WithField("WorkerType", workerType)
                            .WithField("Message", e.Message));
                    attempts--;
                }
            }

            throw new ConnectionFailedException(
                $"Exceeded maximum connection attempts ({attempts})",
                ConnectionErrorReason.ExceededMaximumRetries);
        }

        private bool ShouldUseLocator()
        {
            if (Application.isEditor)
            {
                var commandLineArguments = Environment.GetCommandLineArgs();
                var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
                return commandLineArgs.ContainsKey(RuntimeConfigNames.LoginToken);
            }

            return false;
        }

        private ReceptionistConfig GetReceptionistConfig(string workerType)
        {
            ReceptionistConfig config;
            if (Application.isEditor)
            {
                config = new ReceptionistConfig
                {
                    WorkerType = workerType,
                    WorkerId = $"{workerType}-{Guid.NewGuid()}",
                    UseExternalIp = UseExternalIp
                };
            }
            else
            {
                var commandLineArguments = Environment.GetCommandLineArgs();
                var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
                config = ReceptionistConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
                config.UseExternalIp = UseExternalIp;
                config.WorkerId = $"{workerType}-{Guid.NewGuid()}";
            }

            return config;
        }

        private LocatorConfig GetLocatorConfig(string workerType)
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            var config = LocatorConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
            config.WorkerType = workerType;
            config.WorkerId = $"{workerType}-{Guid.NewGuid()}";
            return config;
        }

        private void InitializeWorker(Worker worker)
        {
            Worker = worker;
            AddWorkerSystems();
            InstantiateLevel();
            Worker.OnDisconnect += OnDisconnected;
            World.Active = World.Active ?? Worker.World;

            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.AllWorlds.ToArray());
        }

        private void InstantiateLevel()
        {
            levelInstance = Instantiate(LevelPrefab, transform);
            levelInstance.transform.SetParent(null);
        }

        private void OnDisconnected(string reason)
        {
            Worker.LogDispatcher.HandleLog(LogType.Log, new LogEvent($"Worker disconnected")
                .WithField("WorkerId", Worker.WorkerId)
                .WithField("Reason", reason));
            StartCoroutine(DeferredDisposeWorker());
        }

        private IEnumerator DeferredDisposeWorker()
        {
            yield return null;
            Dispose();
        }

        public void Dispose()
        {
            Worker?.Dispose();
            Worker = null;
            // Check needed for the case that play mode is exited before the connection can complete
            if (Application.isPlaying)
            {
                Destroy(levelInstance);
                Destroy(this);
            }
        }
    }
}
