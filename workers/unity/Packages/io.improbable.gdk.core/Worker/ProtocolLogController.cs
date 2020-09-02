using System;
using System.IO;
using System.Linq;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class ProtocolLogController : IDisposable
    {
        private const string ProtocolLoggingFlag = "protocol_logging_enabled";
        private WorkerSystem workerSystem;
        private WorkerFlagCallbackSystem callbackSystem;
        private ulong callbackKey;
        private bool enabled;
        private readonly string workerId;
        public LogsinkParameters LogsinkParameters { get; }

        public ProtocolLogController(string workerId)
        {
            this.workerId = workerId;
            var path = Path.GetDirectoryName(Application.consoleLogPath);
            if (string.IsNullOrEmpty(path))
            {
                throw new InvalidOperationException("Current platform does not support log files");
            }

            LogsinkParameters = new LogsinkParameters
            {
                FilterParameters =
                {
                    CustomFilter = LogFilter
                },
                RotatingLogFileParameters =
                {
                    LogPrefix = Path.Combine(path, $"protocol_{this.workerId}_")
                }
            };
        }

        public void OnWorkerConnected(World world)
        {
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            callbackSystem = world.GetExistingSystem<WorkerFlagCallbackSystem>();
            callbackKey = callbackSystem.RegisterWorkerFlagChangeCallback(OnWorkerFlagChanged);
        }

        private void OnWorkerFlagChanged((string, string) pair)
        {
            var (key, value) = pair;
            if (key != ProtocolLoggingFlag)
            {
                return;
            }

            enabled = !string.IsNullOrWhiteSpace(value) && value.Split(',').Contains(workerId);
            if (enabled)
            {
                workerSystem.EnableLogging();
            }
            else
            {
                workerSystem.DisableLogging();
            }
        }

        private bool LogFilter(LogCategory category, LogLevel level)
        {
            return enabled && (category == LogCategory.NetworkStatus || category.HasFlag(LogCategory.NetworkTraffic)) && level >= LogLevel.Info;
        }

        public void Dispose()
        {
            callbackSystem.UnregisterWorkerFlagChangeCallback(callbackKey);
        }
    }
}
