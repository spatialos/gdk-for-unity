using UnityEngine;

namespace Improbable.Gdk.Core
{
    public interface ILogDispatcher
    {
        void HandleLog(LogType type, LogEvent logEvent);
    }
}
