using System;
using System.Collections.Generic;
using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Forwards logEvents and exceptions to the SpatialOS Console and logs locally.
    /// </summary>
    public class ForwardingDispatcher : ILogDispatcher, IDisposable
    {
        private Connection connection;

        private readonly LogLevel minimumLogLevel;

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

        private void LogCallback(string message, string stackTrace, LogType type)
        {
            // This is required to avoid duplicate forwarding caused by HandleLog also logging to console
            if (type == LogType.Exception)
            {
                connection.SendLogMessage(LogLevel.Error, connection.GetWorkerId(), $"{message}\n{stackTrace}");
            }
        }

        public void SetConnection(Connection connection)
        {
            this.connection = connection;
        }

        public void HandleLog(LogType type, LogEvent logEvent)
        {
            Debug.unityLogger.Log(type, logEvent);
            LogLevel logLevel = LogTypeMapping[type];

            if (connection == null || logLevel < minimumLogLevel)
            {
                return;
            }

            var message = logEvent.ToString();
            var entityId = LoggingUtils.ExtractEntityId(logEvent.Data);
            connection.SendLogMessage(logLevel, LoggingUtils.ExtractLoggerName(logEvent.Data), message, entityId);
        }

        public void Dispose()
        {
            Application.logMessageReceived -= LogCallback;
        }
    }
}
