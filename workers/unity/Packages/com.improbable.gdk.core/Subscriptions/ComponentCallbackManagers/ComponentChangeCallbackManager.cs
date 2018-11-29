using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentAddedCallbackManager : IComponentCallbackManager
    {
        private readonly Callbacks<EntityId> callbacks = new Callbacks<EntityId>();
        private readonly uint componentId;
        private readonly EntityManager entityManager;

        private ulong nextCallbackId = 1;

        public ComponentAddedCallbackManager(uint componentId)
        {
            this.componentId = componentId;
        }

        public void InvokeCallbacks(ComponentUpdateSystem updateSystem)
        {
            var entities = updateSystem.GetComponentsAdded(componentId);
            for (int i = 0; i < entities.Count; ++i)
            {
                callbacks.InvokeAll(entities[i]);
            }
        }

        public ulong RegisterCallback(Action<EntityId> callback)
        {
            callbacks.Add(nextCallbackId, callback);
            return nextCallbackId++;
        }

        public bool UnregisterCallback(ulong callbackKey)
        {
            return callbacks.Remove(callbackKey);
        }
    }
}
