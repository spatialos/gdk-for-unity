using UnityEngine;

namespace Improbable.Gdk.Core
{
    public interface ILogger
    {
        void Log(LogType type, LogEvent logEvent);
    }
}
