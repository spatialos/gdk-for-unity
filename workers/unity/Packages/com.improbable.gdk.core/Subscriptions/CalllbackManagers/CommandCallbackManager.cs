using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public class CommandCallbackManager<T> : ICallbackManager where T : struct, IReceivedCommandRequest
    {
        private readonly IndexedCallbacks<T> callbacks = new IndexedCallbacks<T>();
        private readonly CommandSystem commandSystem;

        private ulong nextCallbackId = 1;

        public CommandCallbackManager(World world)
        {
            commandSystem = world.GetExistingManager<CommandSystem>();
        }

        public void InvokeCallbacks()
        {
            var requests = commandSystem.GetRequests<T>();
            for (int i = 0; i < requests.Count; ++i)
            {
                callbacks.InvokeAll(requests[i].GetTargetEntityId().Id, requests[i]);
            }
        }

        public ulong RegisterCallback(EntityId entityId, Action<T> callback)
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
