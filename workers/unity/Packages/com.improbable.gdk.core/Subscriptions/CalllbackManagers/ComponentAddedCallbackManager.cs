using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentAddedCallbackManager : ICallbackManager
    {
        private readonly Callbacks<EntityId> callbacks = new Callbacks<EntityId>();
        private readonly uint componentId;
        private readonly EntityManager entityManager;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private ulong nextCallbackId = 1;

        public ComponentAddedCallbackManager(uint componentId, World world)
        {
            this.componentId = componentId;
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
        }

        public void InvokeCallbacks()
        {
            var entities = componentUpdateSystem.GetComponentsAdded(componentId);
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
