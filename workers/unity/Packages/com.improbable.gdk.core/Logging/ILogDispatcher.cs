using System;
using Improbable.Worker.Core;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public interface ILogDispatcher : IDisposable
    {
        Connection Connection { get; set; }

        void HandleLog(LogType type, LogEvent logEvent);
    }
}
