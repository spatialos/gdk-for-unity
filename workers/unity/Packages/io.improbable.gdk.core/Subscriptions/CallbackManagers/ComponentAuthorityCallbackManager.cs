using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentAuthorityCallbackManager : IAuthorityCallbackManager
    {
        private readonly IndexedCallbacks<Authority> callbacks = new IndexedCallbacks<Authority>();
        private readonly uint componentId;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private ulong nextCallbackId = 1;

        public ComponentAuthorityCallbackManager(uint componentId, World world)
        {
            this.componentId = componentId;
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
        }

        public void InvokeCallbacks()
        {
            var changes = componentUpdateSystem.GetAuthorityChangesReceived(componentId);
            for (var i = 0; i < changes.Count; ++i)
            {
                switch (changes[i].Authority)
                {
                    case Authority.Authoritative:
                        callbacks.InvokeAll(changes[i].EntityId.Id, changes[i].Authority);
                        break;
                    case Authority.NotAuthoritative:
                        callbacks.InvokeAllReverse(changes[i].EntityId.Id, changes[i].Authority);
                        break;
                }
            }
        }

        public void InvokeLossImminentCallbacks()
        {
            var changes = componentUpdateSystem.GetAuthorityChangesReceived(componentId);
            for (var i = 0; i < changes.Count; ++i)
            {
                if (changes[i].Authority == Authority.AuthorityLossImminent)
                {
                    callbacks.InvokeAllReverse(changes[i].EntityId.Id, changes[i].Authority);
                }
            }
        }

        public ulong RegisterCallback(EntityId entityId, Action<Authority> callback)
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
