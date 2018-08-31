using System;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public class RequirableBase : IInjectable, IDisposable
    {
        private const string AccessedDisposedRequirable =
            "Tried to access a requirable SpatialOS MonoBehaviour API object (e.g. Reader, Writer, CommandRequestSender, CommandRequestHandler etc.) that is already disposed.";

        private readonly ILogDispatcher logDispatcher;
        private bool isDisposed;

        public RequirableBase(ILogDispatcher logDispatcher)
        {
            this.logDispatcher = logDispatcher;
        }

        protected bool LogErrorIfDisposed()
        {
            if (isDisposed)
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent(AccessedDisposedRequirable)
                    .WithField(LoggingUtils.LoggerName, GetType()));
            }

            return isDisposed;
        }

        public void Dispose()
        {
            Debug.LogWarning("Disposing this " + this);
            isDisposed = true;
        }
    }
}
