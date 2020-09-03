using System;
using System.IO;
using System.Linq;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class ProtocolLogController
    {
        private const string ProtocolLoggingFlag = "protocol_logging_enabled";
        private WorkerSystem workerSystem;
        private WorkerFlagCallbackSystem callbackSystem;
        public bool Enabled { get; private set; }

        public void OnWorkerConnected(World world)
        {
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            callbackSystem = world.GetExistingSystem<WorkerFlagCallbackSystem>();
            callbackSystem.RegisterWorkerFlagChangeCallback(OnWorkerFlagChanged);
        }

        private void OnWorkerFlagChanged((string, string) pair)
        {
            var (key, value) = pair;
            if (key != ProtocolLoggingFlag)
            {
                return;
            }

            Enabled = !string.IsNullOrWhiteSpace(value) && value.Split(',').Contains(workerSystem.WorkerId);
            if (Enabled)
            {
                workerSystem.EnableLogging();
            }
        }

        private bool LogFilter(LogCategory category, LogLevel level)
        {
            var isCategory = category == LogCategory.NetworkStatus || category.HasFlag(LogCategory.NetworkTraffic);
            return Enabled && isCategory && level >= LogLevel.Info;
        }

        public LogsinkParameters GetLogsinkParameters(string workerId)
        {
            var logPath = Path.GetDirectoryName(Application.consoleLogPath);
            if (logPath == null)
            {
                throw new InvalidOperationException("This platform does not support log files");
            }

            return new LogsinkParameters
            {
                FilterParameters =
                {
                    CustomFilter = LogFilter
                },
                RotatingLogFileParameters =
                {
                    LogPrefix = Path.Combine(logPath, $"protocol_{workerId}_")
                }
            };
        }
    }
}
