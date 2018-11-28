using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentAddedCallbackManager : IComponentCallbackManager
    {
        // todo int is a placeholder until I make a callbacks copy that takes no param
        private readonly IndexedCallbacks<int> callbacks = new IndexedCallbacks<int>();
        private readonly uint componentId;
        private readonly EntityManager entityManager;

        private ulong nextCallbackId = 1;

        public ComponentAddedCallbackManager(uint componentId)
        {
            this.componentId = componentId;
        }

        public void InvokeCallbacks(ComponentUpdateSystem updateSystem)
        {
            var components = updateSystem.GetComponentsAdded(componentId);
            for (int i = 0; i < components.Count; ++i)
            {
                callbacks.InvokeAll(components[i].Id, (int) componentId);
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
