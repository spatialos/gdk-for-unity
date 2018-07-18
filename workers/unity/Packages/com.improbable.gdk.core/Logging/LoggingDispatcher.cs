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
            Debug.unityLogger.Log(type, logEvent);
        }
    }
}
