using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class LoggingDispatcher : ILogDispatcher
    {
        public void HandleLog(LogType type, LogEvent logEvent)
        {
            switch (type)
            {
                case LogType.Error:
                    Debug.LogError(logEvent);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(logEvent);
                    break;
                case LogType.Log:
                    Debug.Log(logEvent);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(logEvent);
                    break;
                case LogType.Exception:
                    Debug.LogError(logEvent);
                    break;
            }
        }
    }
}
