using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Improbable.Gdk.Core.NewWorld
{
    // Maybe use??
    // public static class WorkerConnector
    // {
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // void DoStuff(), create some sort of asset to make decisions?;
    // }
    public abstract class WorkerConnectorV2 : MonoBehaviour, IDisposable
    {
        public SpatialOSWorker Worker;

        public SpatialOSWorld World;

        public int maxConnectionAttempts = 3;

        private readonly List<Action<Worker>> workerConnectedCallbacks = new List<Action<Worker>>();

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

        protected void OnApplicationQuit()
        {
            Dispose();
        }

        protected void OnDestroy()
        {
            Dispose();
        }

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

                Worker = await ConnectWithRetries(builder, maxConnectionAttempts, logger, builder.WorkerType, transform.position);

                Worker.OnDisconnect += OnDisconnected;

                if (!Application.isPlaying)
                {
                    Dispose();
                    throw new ConnectionFailedException("Editor application stopped",
                        ConnectionErrorReason.EditorApplicationStopped);
                }

                HandleWorkerConnectionEstablished();

                // Update PlayerLoop
                PlayerLoopUtils.ResolveSystemGroups(World);
                ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World, PlayerLoop.GetCurrentPlayerLoop());
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
            World = new SpatialOSWorld(Worker);
        }

        private static async Task<SpatialOSWorker> ConnectWithRetries(IConnectionHandlerBuilder connectionHandlerBuilder, int maxAttempts,
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
                        void CancelTask()
                        {
                            // ReSharper disable once AccessToDisposedClosure
                            tokenSource.Cancel();
                        }

                        Application.quitting += CancelTask;

                        var worker = await SpatialOSWorker.CreateSpatialOSWorkerAsync(connectionHandlerBuilder, workerType, logger, origin, tokenSource.Token);

                        Application.quitting -= CancelTask;
                        return worker;
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
            World?.Dispose();
            Worker = null;
            World = null;
        }

        private void RemoveFromPlayerLoop()
        {
            if (World != null)
            {
                // Remove root systems from the disposing world from the PlayerLoop
                // This only affects the loop next frame
                PlayerLoopUtils.RemoveFromPlayerLoop(World);
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
