using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Subscriptions
{
    public class CommandCallbackManager<T> : ICommandCallbackManager where T : IReceivedCommandRequest
    {
        private readonly IndexedCallbacks<T> callbacks = new IndexedCallbacks<T>();

        private ulong nextCallbackId = 1;

        public void InvokeCallbacks(CommandSystem commandSystem)
        {
            var requests = commandSystem.GetRequests<T>();
            foreach (var request in requests)
            {
                callbacks.InvokeAll(request.GetTargetEntityId().Id, request);
            }
        }

        public ulong RegisterCallback(EntityId entityId, Action<T> callback)
        {
            callbacks.Add(entityId.Id, nextCallbackId, callback);
            return nextCallbackId++;
        }

        public bool UnregisterCallback(ulong callbackKey)
        {
            return callbacks.Remove(callbackKey);
        }
    }
}
