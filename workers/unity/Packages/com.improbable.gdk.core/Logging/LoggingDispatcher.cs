using System;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Logs logEvents to console.
    /// </summary>
    public class LoggingDispatcher : ILogDispatcher
    {
        public void HandleLog(LogType type, LogEvent logEvent)
        {
            if (type == LogType.Exception)
            {
                // For exception types, Unity expects an exception object to be passed.
                // Otherwise, it will not be displayed.
                Exception exception = logEvent.Exception ?? new Exception(logEvent.ToString());

                Debug.unityLogger.LogException(exception, logEvent.Context);
            }
            else
            {
                Debug.unityLogger.Log(
                    logType: type,
                    message: logEvent,
                    context: logEvent.Context);
            }
        }
    }
}
