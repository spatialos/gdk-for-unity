using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentUpdateCallbackManager<T> : ICallbackManager where T : struct, ISpatialComponentUpdate
    {
        private readonly EntityCallbacks<T> callbacks = new EntityCallbacks<T>();
        private readonly ComponentUpdateSystem componentUpdateSystem;

        public ComponentUpdateCallbackManager(World world)
        {
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
        }

        public void InvokeCallbacks()
        {
            var updates = componentUpdateSystem.GetComponentUpdatesReceived<T>();
            for (var i = 0; i < updates.Count; ++i)
            {
                ref readonly var update = ref updates[i];
                callbacks.InvokeAll(update.EntityId, in update.Update);
            }
        }

        public ulong RegisterCallback(EntityId entityId, Action<T> callback)
        {
            return callbacks.Add(entityId, callback);
        }

        public bool UnregisterCallback(ulong callbackKey)
        {
            return callbacks.Remove(callbackKey);
        }
    }
}
