using System;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     The ILogDispatcher interface is used to implement different types of loggers. By default, the
    ///     ILogDispatcher supports structured logging.
    /// </summary>
    public interface ILogDispatcher : IDisposable
    {
        /// <summary>
        ///     The SpatialOS connection.
        /// </summary>
        Connection Connection { get; set; }

        /// <summary>
        ///     The worker type associated with this logger.
        /// </summary>
        string WorkerType { get; set; }

        void HandleLog(LogType type, LogEvent logEvent);
    }
}
