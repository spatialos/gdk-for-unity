using System;
using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Forwards logEvents and exceptions to the SpatialOS Console and logs locally.
    /// </summary>
    public class ForwardingDispatcher : ILogDispatcher
    {
        private readonly LogLevel minimumLogLevel;

        private bool inHandleLog;

        public Worker Worker { get; set; }

        private static readonly Dictionary<LogType, LogLevel> LogTypeMapping = new Dictionary<LogType, LogLevel>
        {
            { LogType.Exception, LogLevel.Error },
            { LogType.Error, LogLevel.Error },
            { LogType.Assert, LogLevel.Error },
            { LogType.Warning, LogLevel.Warn },
            { LogType.Log, LogLevel.Info }
        };

        public ForwardingDispatcher(LogLevel minimumLogLevel = LogLevel.Warn)
        {
            this.minimumLogLevel = minimumLogLevel;
            Application.logMessageReceived += LogCallback;
        }

        // This method catches exceptions coming from the engine, or third party code.
        private void LogCallback(string message, string stackTrace, LogType type)
        {
            if (inHandleLog)
            {
                // HandleLog will do its own message sending.
                return;
            }

            // This is required to avoid duplicate forwarding caused by HandleLog also logging to console
            if (type == LogType.Exception)
            {
                Worker.Connection?.SendLogMessage(LogLevel.Error, Worker.Connection.GetWorkerId(), $"{message}\n{stackTrace}");
            }
        }

        public void HandleLog(LogType type, LogEvent logEvent)
        {
            inHandleLog = true;
            try
            {
                if (!string.IsNullOrEmpty(Worker?.WorkerType))
                {
                    logEvent.WithField(LoggingUtils.WorkerType, Worker.WorkerType);
                }

                if (type == LogType.Exception)
                {
                    // For exception types, Unity expects an exception object to be passed.
                    // Otherwise, it will not be displayed.
                    var exception = logEvent.Exception ?? new Exception(logEvent.ToString());

                    Debug.unityLogger.LogException(exception, logEvent.Context);
                }
                else
                {
                    Debug.unityLogger.Log(
                        type,
                        logEvent,
                        logEvent.Context);
                }

                var logLevel = LogTypeMapping[type];

                if (Worker?.Connection == null || logLevel < minimumLogLevel)
                {
                    return;
                }

                var message = logEvent.ToString();

                if (type == LogType.Exception && logEvent.Exception != null)
                {
                    // Append the stack trace of the exception to the message.
                    message += $"\n{logEvent.Exception.StackTrace}";
                }

                var entityId = LoggingUtils.ExtractEntityId(logEvent.Data);

                Worker.Connection.SendLogMessage(logLevel, LoggingUtils.ExtractLoggerName(logEvent.Data), message, entityId);
            }
            finally
            {
                inHandleLog = false;
            }
        }

        public void Dispose()
        {
            Worker = null;
            Application.logMessageReceived -= LogCallback;
        }
    }
}
