using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class AuthorityConstraintCallbackManager : IAuthorityCallbackManager
    {
        private readonly Callbacks<AuthorityChangeReceived> callbacks = new Callbacks<AuthorityChangeReceived>();
        private readonly uint componentId;

        private ulong nextCallbackId = 1;

        public AuthorityConstraintCallbackManager(uint componentId)
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
                    callbacks.InvokeAll(changes[i]);
                }
                else if (changes[i].Authority == Authority.NotAuthoritative)
                {
                    callbacks.InvokeAllReverse(changes[i]);
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
                    callbacks.InvokeAllReverse(changes[i]);
                }
            }
        }

        public ulong RegisterCallback(Action<AuthorityChangeReceived> callback)
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
