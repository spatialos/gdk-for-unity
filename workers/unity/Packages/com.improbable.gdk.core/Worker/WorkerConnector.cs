using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Connect workers via Monobehaviours.
    /// </summary>
    public class WorkerConnector : MonoBehaviour, IDisposable
    {
        private delegate Task<Worker> ConnectionDelegate();

        /// <summary>
        ///     The number of connection attempts before giving up.
        /// </summary>
        public int MaxConnectionAttempts = 3;

        /// <summary>
        ///     Denotes whether to connect using an external IP address.
        /// </summary>
        public bool UseExternalIp;

        /// <summary>
        ///     Represents a SpatialOS worker.
        /// </summary>
        /// <remarks>
        ///    Only safe to access after the connection has succeeded.
        /// </remarks>
        public Worker Worker;

        private List<Action<Worker>> workerConnectedCallbacks = new List<Action<Worker>>();

        /// <summary>
        ///     An event that triggers when the worker has been fully created.
        /// </summary>
        public event Action<Worker> OnWorkerCreationFinished
        {
            add
            {
                workerConnectedCallbacks.Add(value);
                if (Worker != null)
                {
                    value.Invoke(Worker);
                }
            }
            remove { workerConnectedCallbacks.Remove(value); }
        }

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

        /// <summary>
        ///     Asynchronously connects a worker to the SpatialOS runtime.
        /// </summary>
        /// <remarks>
        ///     Uses the global position of this GameObject as the worker origin.
        ///     Uses <see cref="ShouldUseLocator"/> to determine whether to connect via the Locator.
        /// </remarks>
        /// <param name="workerType">The type of the worker to connect as</param>
        /// <param name="logger">The logger for the worker to use.</param>
        /// <returns></returns>
        public async Task Connect(string workerType, ILogDispatcher logger)
        {
            // Check that other workers have finished trying to connect before this one starts.
            // This prevents races on the workers starting and races on when we start ticking systems.
            await WorkerConnectionSemaphore.WaitAsync();
            try
            {
                // A check is needed for the case that play mode is exited before the semaphore was released.
                if (!Application.isPlaying)
                {
                    return;
                }

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

                Worker = await ConnectWithRetries(connectionDelegate, MaxConnectionAttempts, logger, workerType);

                Worker.OnDisconnect += OnDisconnected;

                HandleWorkerConnectionEstablished();

                World.Active = World.Active ?? Worker.World;
                ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.AllWorlds.ToArray());
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
                // A check is needed for the case that play mode is exited before the connection can complete.
                if (Application.isPlaying)
                {
                    HandleWorkerConnectionFailure();
                    Dispose();
                }
            }
            finally
            {
                WorkerConnectionSemaphore.Release();
            }

            foreach (var callback in workerConnectedCallbacks)
            {
                callback(Worker);
            }
        }

        /// <summary>
        ///     Determines whether to connect via the locator.
        /// </summary>
        /// <returns>True, if should connect via the Locator, false otherwise.</returns>
        protected virtual bool ShouldUseLocator()
        {
            if (Application.isEditor)
            {
                return false;
            }

            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            return commandLineArgs.ContainsKey(RuntimeConfigNames.LoginToken);
        }

        /// <summary>
        ///     Selects which deployment to connect to.
        /// </summary>
        /// <param name="deployments">The list of deployments.</param>
        /// <returns>The name of the deployment to connect to.</returns>
        protected virtual string SelectDeploymentName(DeploymentList deployments)
        {
            return null;
        }

        /// <summary>
        ///     Creates a Receptionist configuration.
        /// </summary>
        /// <remarks>
        ///     If in a standalone build, will use the command line arguments.
        /// </remarks>
        /// <remarks>
        ///    A worker ID is auto-generated if in the Unity Editor or if one is not provided over the command line.
        /// </remarks>
        /// <param name="workerType">The type of the worker to create.</param>
        /// <returns>The Receptionist connection configuration</returns>
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

        /// <summary>
        ///     Creates the Locator configuration.
        /// </summary>
        /// <remarks>
        ///     If in a standalone build, will use the command line arguments.
        /// </remarks>
        /// <param name="workerType">The type of the worker to create.</param>
        /// <returns>The Locator connection configuration</returns>
        protected virtual LocatorConfig GetLocatorConfig(string workerType)
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            var config = LocatorConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
            config.WorkerType = workerType;
            config.WorkerId = CreateNewWorkerId(workerType);
            return config;
        }

        protected virtual void HandleWorkerConnectionEstablished()
        {
        }

        protected virtual void HandleWorkerConnectionFailure()
        {
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
                    --remainingAttempts;
                    logger.HandleLog(LogType.Error,
                        new LogEvent($"Failed attempt {maxAttempts - remainingAttempts} to create worker")
                            .WithField("WorkerType", workerType)
                            .WithField("Message", e.Message));
                }
            }

            throw new ConnectionFailedException(
                $"Tried to connect {maxAttempts} times - giving up.",
                ConnectionErrorReason.ExceededMaximumRetries);
        }

        private static string CreateNewWorkerId(string workerType)
        {
            return $"{workerType}-{Guid.NewGuid()}";
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

        public virtual void Dispose()
        {
            Worker?.Dispose();
            Worker = null;
            Destroy(this);
        }
    }
}
