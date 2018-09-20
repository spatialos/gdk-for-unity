using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class WorkerConnectorBase : MonoBehaviour, IDisposable
    {
        private delegate Task<Worker> ConnectionDelegate();

        public GameObject LevelPrefab;
        public int MaxConnectionAttempts = 3;
        public bool UseExternalIp;

        public Worker Worker;

        private GameObject levelInstance;

        private static readonly SemaphoreSlim WorkerConnectionSemaphore = new SemaphoreSlim(1, 1);

        // Important run in this step as otherwise it can interfere with the the domain unloading logic.
        private void OnApplicationQuit()
        {
            Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        protected async Task Connect(string workerType, ILogDispatcher logger)
        {
            // Check that other workers have finished trying to connect before this one starts
            // This prevents races on the workers starting and races on when we start ticking systems
            await WorkerConnectionSemaphore.WaitAsync();
            try
            {
                var origin = transform.position;
                ConnectionDelegate connectionDelegate;
                if (ShouldUseLocator())
                {
                    connectionDelegate = async () =>
                        await Worker
                            .CreateWorkerAsync(GetLocatorConfig(workerType), SelectDeploymentName, logger, origin)
                            .ConfigureAwait(false);
                }
                else
                {
                    connectionDelegate = async () =>
                        await Worker.CreateWorkerAsync(GetReceptionistConfig(workerType), logger, origin)
                            .ConfigureAwait(false);
                }

                var worker = await ConnectWithRetries(connectionDelegate, MaxConnectionAttempts, logger, workerType);
                InitializeWorker(worker);
            }
            catch (Exception e)
            {
                logger.HandleLog(LogType.Error, new LogEvent("Failed to create worker")
                    .WithException(e)
                    .WithField("WorkerType", workerType)
                    .WithField("Message", e.Message));
#if UNITY_EDITOR
                // Temporary warning to be replaced when we can reliably detect if a local runtime is running, or not.
                logger.HandleLog(LogType.Warning,
                    new LogEvent(
                            "Is a local runtime running? If not, you can start one from 'SpatialOS -> Local launch' or by pressing Cmd/Ctrl-L")
                        .WithField("Reason", "A worker running in the Editor failed to connect"));
#endif
                HandleWorkerConnectionFailure();
            }
            finally
            {
                WorkerConnectionSemaphore.Release();
            }
        }

        protected virtual void AddWorkerSystems()
        {
        }

        protected virtual string SelectDeploymentName(DeploymentList deployments)
        {
            return null;
        }

        protected virtual ReceptionistConfig GetReceptionistConfig(string workerType)
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

        protected virtual LocatorConfig GetLocatorConfig(string workerType)
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            var config = LocatorConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
            config.WorkerType = workerType;
            config.WorkerId = CreateNewWorkerId(workerType);
            return config;
        }

        protected virtual void HandleWorkerConnectionFailure()
        {
            Dispose();
        }

        private static async Task<Worker> ConnectWithRetries(ConnectionDelegate connectionDelegate, int maxAttempts,
            ILogDispatcher logger, string workerType)
        {
            var remainingAttempts = maxAttempts;
            while (remainingAttempts > 0)
            {
                try
                {
                    return await connectionDelegate();
                }
                catch (ConnectionFailedException e)
                {
                    logger.HandleLog(LogType.Error,
                        new LogEvent($"Failed attempt {remainingAttempts} to create worker")
                            .WithField("WorkerType", workerType)
                            .WithField("Message", e.Message));
                    remainingAttempts--;
                }
            }

            throw new ConnectionFailedException(
                $"Tried to connect {maxAttempts} times - giving up.",
                ConnectionErrorReason.ExceededMaximumRetries);
        }

        private static bool ShouldUseLocator()
        {
            if (Application.isEditor)
            {
                return false;
            }

            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            return commandLineArgs.ContainsKey(RuntimeConfigNames.LoginToken);
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
            if (LevelPrefab == null)
            {
                return;
            }

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

        private static string CreateNewWorkerId(string workerType)
        {
            return $"{workerType}-{Guid.NewGuid()}";
        }

        public void Dispose()
        {
            Worker?.Dispose();
            Worker = null;
            // A check is needed for the case that play mode is exited before the connection can complete.
            if (Application.isPlaying)
            {
                if (levelInstance != null)
                {
                    Destroy(levelInstance);
                }

                Destroy(this);
            }
        }
    }
}
