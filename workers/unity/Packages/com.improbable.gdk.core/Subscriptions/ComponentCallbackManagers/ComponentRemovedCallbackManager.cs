using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentRemovedCallbackManager : IComponentCallbackManager
    {
        private readonly Callbacks<EntityId> callbacks = new Callbacks<EntityId>();
        private readonly uint componentId;
        private readonly World world;
        private readonly EntityManager entityManager;

        private ulong nextCallbackId = 1;

        public ComponentRemovedCallbackManager(uint componentId, World world)
        {
            this.world = world;
            this.componentId = componentId;
        }

        public void InvokeCallbacks(ComponentUpdateSystem updateSystem)
        {
            var entities = updateSystem.GetComponentsRemoved(componentId);
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
