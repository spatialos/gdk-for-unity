using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation
{
    public static class GameObjectDelegates
    {
        public delegate void AuthorityChanged(Authority newAuthority);

        public delegate void ComponentUpdated<TComponentUpdate>(TComponentUpdate updateData)
            where TComponentUpdate : ISpatialComponentUpdate;

        /// <summary>
        /// Helper method to dispatch arbitrary payloads to callbacks while forwarding exceptions to the log dispatcher.
        /// </summary>
        /// <param name="payload">The payload value.</param>
        /// <param name="callbacks">The callbacks.</param>
        /// <param name="logDispatcher">The logging dispatcher that will be receiving the exceptions.</param>
        /// <typeparam name="TPayload">The payload type.</typeparam>
        public static void DispatchWithErrorHandling<TPayload>(
            TPayload payload,
            List<Action<TPayload>> callbacks,
            ILogDispatcher logDispatcher)
        {
            foreach (var callback in callbacks)
            {
                try
                {
                    callback(payload);
                }
                catch (Exception e)
                {
                    // Log the exception but do not rethrow it, as other delegates should still get called
                    logDispatcher.HandleLog(LogType.Exception, new LogEvent("Caught exception in a MonoBehaviour").WithException(e));
                }
            }
        }
    }
}
