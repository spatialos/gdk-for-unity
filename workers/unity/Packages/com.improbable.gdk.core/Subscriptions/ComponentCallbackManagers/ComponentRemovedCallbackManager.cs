using System;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentRemovedCallbackManager : IComponentCallbackManager
    {
        // todo int is a placeholder until I make a callbacks copy that takes no param
        private readonly IndexedCallbacks<int> callbacks = new IndexedCallbacks<int>();
        private readonly uint componentId;

        private ulong nextCallbackId = 1;

        public ComponentRemovedCallbackManager(uint componentId)
        {
            this.componentId = componentId;
        }

        public void InvokeCallbacks(ComponentUpdateSystem updateSystem)
        {
            var components = updateSystem.GetComponentsRemoved(componentId);
            for (int i = 0; i < components.Count; ++i)
            {
                callbacks.InvokeAllReverse(components[i].Id, (int) componentId);
            }
        }

        public ulong RegisterCallback(EntityId entityId, Action<int> callback)
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
