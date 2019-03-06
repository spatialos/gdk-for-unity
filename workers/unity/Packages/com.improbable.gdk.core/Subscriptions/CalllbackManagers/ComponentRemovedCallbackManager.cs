using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentRemovedCallbackManager : ICallbackManager
    {
        private readonly Callbacks<EntityId> callbacks = new Callbacks<EntityId>();
        private readonly uint componentId;
        private readonly EntityManager entityManager;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private ulong nextCallbackId = 1;

        public ComponentRemovedCallbackManager(uint componentId, World world)
        {
            this.componentId = componentId;
            componentUpdateSystem = world.GetExistingManager<ComponentUpdateSystem>();
        }

        public void InvokeCallbacks()
        {
            // todo like entity stuff this should also be temporarily removed components
            var entities = componentUpdateSystem.GetComponentsRemoved(componentId);
            for (int i = 0; i < entities.Count; ++i)
            {
                callbacks.InvokeAllReverse(entities[i]);
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
