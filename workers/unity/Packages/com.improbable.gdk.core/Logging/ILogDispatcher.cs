using Improbable.Worker.Core;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public interface ILogDispatcher
    {
        Connection Connection { get; set; }

        void HandleLog(LogType type, LogEvent logEvent);
    }
}
