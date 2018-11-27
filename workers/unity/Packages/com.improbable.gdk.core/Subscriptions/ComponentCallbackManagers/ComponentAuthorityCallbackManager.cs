using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Subscriptions
{
    internal class ComponentAuthorityCallbackManager : IComponentCallbackManager
    {
        private readonly IndexedCallbacks<Authority> callbacks = new IndexedCallbacks<Authority>();
        private readonly uint componentId;

        private ulong nextCallbackId = 1;

        public ComponentAuthorityCallbackManager(uint componentId)
        {
            this.componentId = componentId;
        }

        public void InvokeCallbacks(ComponentUpdateSystem updateSystem)
        {
            var changes = updateSystem.GetAuthorityChangesReceived(componentId);
            for (int i = 0; i < changes.Count; ++i)
            {
                if (changes[i].Authority == Authority.Authoritative)
                {
                    callbacks.InvokeAll(changes[i].EntityId.Id, changes[i].Authority);
                }
                else
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
