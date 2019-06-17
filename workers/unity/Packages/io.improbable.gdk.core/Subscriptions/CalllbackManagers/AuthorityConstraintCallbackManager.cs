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
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private ulong nextCallbackId = 1;

        public AuthorityConstraintCallbackManager(uint componentId, World world)
        {
            this.componentId = componentId;
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
        }

        public void InvokeCallbacks()
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

        public void InvokeLossImminentCallbacks()
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
