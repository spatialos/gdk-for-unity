using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Alpha;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Connect workers via Monobehaviours.
    /// </summary>
    public abstract class WorkerConnector : MonoBehaviour, IDisposable
    {
        /// <summary>
        ///     The number of connection attempts before giving up.
        /// </summary>
        public int MaxConnectionAttempts = 3;

        /// <summary>
        ///     Represents a SpatialOS worker.
        /// </summary>
        /// <remarks>
        ///    Only safe to access after the connection has succeeded.
        /// </remarks>
        public WorkerInWorld Worker;

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
            remove => workerConnectedCallbacks.Remove(value);
        }

        private static readonly SemaphoreSlim WorkerConnectionSemaphore = new SemaphoreSlim(1, 1);

        // Important run in this step as otherwise it can interfere with the domain unloading logic.
        protected void OnApplicationQuit()
        {
            Dispose();
        }

        protected void OnDestroy()
        {
            Dispose();
        }

        /// <summary>
        ///     Asynchronously connects a worker to the SpatialOS runtime.
        /// </summary>
        /// <param name="builder">Describes how to create a <see cref="IConnectionHandler"/> for this worker.</param>
        /// <param name="logger">The logger for the worker to use.</param>
        /// <returns></returns>
        protected async Task Connect(IConnectionHandlerBuilder builder, ILogDispatcher logger)
        {
            if (builder == null)
            {
                throw new ArgumentException("Builder cannot be null.", nameof(builder));
            }

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

                Worker = await ConnectWithRetries(builder, MaxConnectionAttempts, logger, builder.WorkerType, origin);

                Worker.OnDisconnect += OnDisconnected;

                if (!Application.isPlaying)
                {
                    Dispose();
                    throw new ConnectionFailedException("Editor application stopped",
                        ConnectionErrorReason.EditorApplicationStopped);
                }

                HandleWorkerConnectionEstablished();

                // Update PlayerLoop
                PlayerLoopUtils.ResolveSystemGroups(Worker.World);
                ScriptBehaviourUpdateOrder.UpdatePlayerLoop(Worker.World, PlayerLoop.GetCurrentPlayerLoop());
            }
            catch (Exception)
            {
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
                    Dispose();
                }

                throw;
            }
            finally
            {
                WorkerConnectionSemaphore.Release();
            }

#if !UNITY_EDITOR && DEVELOPMENT_BUILD && !UNITY_ANDROID && !UNITY_IPHONE
            try
            {
                var port = GetPlayerConnectionPort();
                Worker.SendLogMessage(LogLevel.Info, $"Unity PlayerConnection port: {port}.", Worker.WorkerId, null);
            }
            catch (Exception e)
            {
                logger.HandleLog(LogType.Exception, new LogEvent("Could not find the Unity PlayerConnection port.").WithException(e));
            }
#endif

            foreach (var callback in workerConnectedCallbacks)
            {
                callback(Worker);
            }
        }

        protected virtual void HandleWorkerConnectionEstablished()
        {
        }

        private static async Task<WorkerInWorld> ConnectWithRetries(IConnectionHandlerBuilder connectionHandlerBuilder, int maxAttempts,
            ILogDispatcher logger, string workerType, Vector3 origin)
        {
            var remainingAttempts = maxAttempts;
            while (remainingAttempts > 0)
            {
                if (!Application.isPlaying)
                {
                    throw new ConnectionFailedException("Editor application stopped", ConnectionErrorReason.EditorApplicationStopped);
                }

                try
                {
                    using (var tokenSource = new CancellationTokenSource())
                    {
                        Action cancelTask = delegate { tokenSource.Cancel(); };
                        Application.quitting += cancelTask;

                        var workerInWorld = await WorkerInWorld.CreateWorkerInWorldAsync(connectionHandlerBuilder, workerType, logger, origin, tokenSource.Token);

                        Application.quitting -= cancelTask;
                        return workerInWorld;
                    }
                }
                catch (ConnectionFailedException e)
                {
                    if (e.Reason == ConnectionErrorReason.EditorApplicationStopped)
                    {
                        throw;
                    }

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

        protected static string CreateNewWorkerId(string workerType)
        {
            return $"{workerType}-{Guid.NewGuid().GetHashCode():x}";
        }

        protected ConnectionParameters CreateConnectionParameters(string workerType, IConnectionParameterInitializer initializer = null)
        {
            var @params = new ConnectionParameters
            {
                WorkerType = workerType,
                DefaultComponentVtable = new ComponentVtable(),
                Network =
                {
                    ConnectionType = NetworkConnectionType.ModularKcp,
                    ModularKcp =
                    {
                        DownstreamCompression = new CompressionParameters(),
                        UpstreamCompression = new CompressionParameters(),
                    }
                }
            };

            initializer?.Initialize(@params);

            return @params;
        }

        private void OnDisconnected(string reason)
        {
            Worker.LogDispatcher.HandleLog(LogType.Log, new LogEvent($"Worker disconnected")
                .WithField("WorkerId", Worker.WorkerId)
                .WithField("Reason", reason));
            StartCoroutine(DeferredDisposeWorker());
        }

        protected IEnumerator DeferredDisposeWorker()
        {
            // Remove the world from the loop early, to avoid errors during the delay frame
            RemoveFromPlayerLoop();
            yield return null;
            Dispose();
        }

        public virtual void Dispose()
        {
            RemoveFromPlayerLoop();
            Worker?.Dispose();
            Worker = null;
        }

        private void RemoveFromPlayerLoop()
        {
            if (Worker?.World != null)
            {
                // Remove root systems from the disposing world from the PlayerLoop
                // This only affects the loop next frame
                PlayerLoopUtils.RemoveFromPlayerLoop(Worker.World);
            }
        }

        private static ushort GetPlayerConnectionPort()
        {
            // We need to open the File as ReadWrite since this process _already_ has it open as ReadWrite.
            // Attempting to open it as Read only results in IO exceptions due to permissions. Go figure.
            using (var stream = new FileStream(Application.consoleLogPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (var readStream = new StreamReader(stream))
            {
                var logContents = readStream.ReadToEnd();
                return ExtractPlayerConnectionPort(logContents);
            }
        }

        internal static ushort ExtractPlayerConnectionPort(string fileContents)
        {
            const string portRegex =
                "PlayerConnection initialized network socket : 0\\.0\\.0\\.0 ([0-9]+)";

            var regex = new Regex(portRegex, RegexOptions.Compiled);

            if (!regex.IsMatch(fileContents))
            {
                throw new Exception("Could not find PlayerConnection port in logfile");
            }

            var port = ushort.Parse(regex.Match(fileContents).Groups[1].Value);

            if (port == 0)
            {
                throw new Exception("PlayerConnection port cannot be 0.");
            }

            return port;
        }
    }
}
