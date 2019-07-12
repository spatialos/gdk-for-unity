using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentUpdateCallbackManager<T> : ICallbackManager where T : ISpatialComponentUpdate
    {
        private readonly IndexedCallbacks<T> callbacks = new IndexedCallbacks<T>();
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private ulong nextCallbackId = 1;

        public ComponentUpdateCallbackManager(World world)
        {
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
        }

        public void InvokeCallbacks()
        {
            var updates = componentUpdateSystem.GetComponentUpdatesReceived<T>();
            for (int i = 0; i < updates.Count; ++i)
            {
                ref readonly var update = ref updates[i];
                callbacks.InvokeAll(update.EntityId.Id, update.Update);
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
