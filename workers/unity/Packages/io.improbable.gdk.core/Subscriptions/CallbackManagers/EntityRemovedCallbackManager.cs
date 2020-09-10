using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class EntityRemovedCallbackManager : ICallbackManager
    {
        private readonly CallbackCollection<EntityId> callbackCollection = new CallbackCollection<EntityId>();
        private readonly EntitySystem entitySystem;

        private ulong nextCallbackId = 1;

        public EntityRemovedCallbackManager(World world)
        {
            entitySystem = world.GetExistingSystem<EntitySystem>();
        }

        public void InvokeCallbacks()
        {
            foreach (var entityId in entitySystem.EntitiesRemoved)
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
