using System;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public interface ILogDispatcher : IDisposable
    {
        Worker Worker { get; set; }

        void HandleLog(LogType type, LogEvent logEvent);
    }
}
