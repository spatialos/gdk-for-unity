using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Forwards logEvents and exceptions to the SpatialOS Console and logs locally.
    /// </summary>
    public class ForwardingDispatcher : ILogDispatcher
    {
        private readonly LogLevel minimumLogLevel;

        private static bool inHandleLog;

        public Worker Worker { get; set; }
        public string WorkerType { get; set; }

        private static readonly Dictionary<LogType, LogLevel> LogTypeMapping = new Dictionary<LogType, LogLevel>
        {
            { LogType.Exception, LogLevel.Error },
            { LogType.Error, LogLevel.Error },
            { LogType.Assert, LogLevel.Error },
            { LogType.Warning, LogLevel.Warn },
            { LogType.Log, LogLevel.Info }
        };

        /// <summary>
        ///     Constructor for the Forwarding Dispatcher
        /// </summary>
        /// <param name="minimumLogLevel">The minimum log level to forward logs to the SpatialOS runtime.</param>
        public ForwardingDispatcher(LogType minimumLogLevel = LogType.Warning)
        {
            this.minimumLogLevel = LogTypeMapping[minimumLogLevel];
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
                Worker?.SendLogMessage(LogLevel.Error, $"{message}\n{stackTrace}", Worker?.WorkerId, null);
            }
        }

        /// <summary>
        ///     Log locally and conditionally forward to the SpatialOS runtime.
        /// </summary>
        /// <param name="type">The type of the log.</param>
        /// <param name="logEvent">A LogEvent instance.</param>
        public void HandleLog(LogType type, LogEvent logEvent)
        {
            inHandleLog = true;
            try
            {
                if (!string.IsNullOrEmpty(WorkerType))
                {
                    logEvent.WithField(LoggingUtils.WorkerType, WorkerType);
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

                if (Worker == null || logLevel < minimumLogLevel)
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

                Worker.SendLogMessage(logLevel, message, LoggingUtils.ExtractLoggerName(logEvent.Data), entityId);
            }
            finally
            {
                inHandleLog = false;
            }
        }

        /// <summary>
        ///     Unregisters callbacks and ensures that the SpatialOS connection is no longer referenced
        /// </summary>
        public void Dispose()
        {
            Application.logMessageReceived -= LogCallback;
        }
    }
}
