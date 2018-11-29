using System;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentEventCallbackManager<T> : IComponentCallbackManager where T : IEvent
    {
        private readonly IndexedCallbacks<T> callbacks = new IndexedCallbacks<T>();

        private ulong nextCallbackId = 1;

        public void InvokeCallbacks(ComponentUpdateSystem updateSystem)
        {
            var updates = updateSystem.GetEventsReceived<T>();
            for (int i = 0; i < updates.Count; ++i)
            {
                callbacks.InvokeAll(updates[i].EntityId.Id, updates[i].Event);
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
