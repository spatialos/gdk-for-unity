using System;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public class RequirableBase : IInjectable, IDisposable
    {
        private const string AccessedDisposedRequirable =
            "Tried to access a disposed [Require] SpatialOS MonoBehaviour API object (e.g. Reader, Writer, CommandRequestSender, CommandRequestHandler etc.). " +
            "Note that [Require] objects are already disposed at the time OnDisable() is called. " +
            "Also note that user callbacks are deregistered automatically so there is no need to manually deregister them in OnDisable().";

        private readonly ILogDispatcher logDispatcher;
        private bool isDisposed;

        public RequirableBase(ILogDispatcher logDispatcher)
        {
            this.logDispatcher = logDispatcher;
        }

        protected bool VerifyNotDisposed()
        {
            if (isDisposed)
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent(AccessedDisposedRequirable)
                    .WithField(LoggingUtils.LoggerName, GetType()));
            }

            return !isDisposed;
        }

        public void Dispose()
        {
            isDisposed = true;
        }
    }
}
