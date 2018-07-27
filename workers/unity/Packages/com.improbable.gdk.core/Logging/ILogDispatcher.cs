using UnityEngine;

namespace Improbable.Gdk.Core
{
    public interface ILogDispatcher
    {
        ILogger GetLogger(string loggerName);
        void HandleLog(LogType type, LogEvent logEvent);
    }
}
