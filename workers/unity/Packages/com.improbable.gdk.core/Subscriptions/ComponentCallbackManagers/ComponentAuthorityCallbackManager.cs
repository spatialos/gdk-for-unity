using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentAuthorityCallbackManager : IAuthorityCallbackManager
    {
        private readonly IndexedCallbacks<Authority> callbacks = new IndexedCallbacks<Authority>();
        private readonly uint componentId;

        private ulong nextCallbackId = 1;

        public ComponentAuthorityCallbackManager(uint componentId)
        {
            this.componentId = componentId;
        }

        public void InvokeCallbacks(ComponentUpdateSystem componentUpdateSystem)
        {
            var changes = componentUpdateSystem.GetAuthorityChangesReceived(componentId);
            for (int i = 0; i < changes.Count; ++i)
            {
                if (changes[i].Authority == Authority.Authoritative)
                {
                    callbacks.InvokeAll(changes[i].EntityId.Id, changes[i].Authority);
                }
                else if (changes[i].Authority == Authority.NotAuthoritative)
                {
                    callbacks.InvokeAllReverse(changes[i].EntityId.Id, changes[i].Authority);
                }
            }
        }

        public void InvokeLossImminentCallbacks(ComponentUpdateSystem componentUpdateSystem)
        {
            var changes = componentUpdateSystem.GetAuthorityChangesReceived(componentId);
            for (int i = 0; i < changes.Count; ++i)
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
