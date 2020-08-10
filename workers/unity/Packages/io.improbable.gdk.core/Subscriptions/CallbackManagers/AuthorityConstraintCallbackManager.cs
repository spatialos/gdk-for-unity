using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    internal class AuthorityConstraintCallbackManager : ICallbackManager
    {
        private readonly CallbackCollection<AuthorityChangeReceived> callbackCollection = new CallbackCollection<AuthorityChangeReceived>();
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
            for (var i = 0; i < changes.Count; ++i)
            {
                if (changes[i].Authority == Authority.Authoritative)
                {
                    callbackCollection.InvokeAll(changes[i]);
                }
                else if (changes[i].Authority == Authority.NotAuthoritative)
                {
                    callbackCollection.InvokeAllReverse(changes[i]);
                }
            }
        }

        public ulong RegisterCallback(Action<AuthorityChangeReceived> callback)
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
