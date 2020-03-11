using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentRemovedCallbackManager : ICallbackManager
    {
        private readonly CallbackCollection<EntityId> callbackCollection = new CallbackCollection<EntityId>();
        private readonly uint componentId;
        private readonly EntityManager entityManager;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private ulong nextCallbackId = 1;

        public ComponentRemovedCallbackManager(uint componentId, World world)
        {
            this.componentId = componentId;
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
        }

        public void InvokeCallbacks()
        {
            // todo like entity stuff this should also be temporarily removed components
            var entities = componentUpdateSystem.GetComponentsRemoved(componentId);
            foreach (var entityId in entities)
            {
                callbackCollection.InvokeAllReverse(entityId);
            }
        }

        public ulong RegisterCallback(Action<EntityId> callback)
        {
            callbackCollection.Add(nextCallbackId, callback);
            return nextCallbackId++;
        }

        public bool UnregisterCallback(ulong callbackKey)
        {
            return callbackCollection.Remove(callbackKey);
        }
    }
}
