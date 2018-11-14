using System;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Logs to the Unity Console.
    /// </summary>
    /// <remarks>
    ///    Forwards logs to UnityEngine.Debug.unityLogger.
    /// </remarks>
    public class LoggingDispatcher : ILogDispatcher
    {
        public Connection Connection { get; set; }
        public string WorkerType { get; set; }

        /// <summary>
        ///     Log locally to the console.
        /// </summary>
        /// <param name="type">The type of the log.</param>
        /// <param name="logEvent">A LogEvent instance.</param>
        public void HandleLog(LogType type, LogEvent logEvent)
        {
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
        }

        public void Dispose()
        {
        }
    }
}
