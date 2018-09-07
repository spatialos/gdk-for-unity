using System;
using Improbable.Worker.Core;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Logs logEvents to console.
    /// </summary>
    public class LoggingDispatcher : ILogDispatcher
    {
        public Worker Worker { get; set; }

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
