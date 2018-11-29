using System;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentUpdateCallbackManager<T> : IComponentCallbackManager where T : ISpatialComponentUpdate
    {
        private readonly IndexedCallbacks<T> callbacks = new IndexedCallbacks<T>();

        private ulong nextCallbackId = 1;

        public void InvokeCallbacks(ComponentUpdateSystem updateSystem)
        {
            var updates = updateSystem.GetComponentUpdatesReceived<T>();
            for (int i = 0; i < updates.Count; ++i)
            {
                callbacks.InvokeAll(updates[i].EntityId.Id, updates[i].Update);
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
