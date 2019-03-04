using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public class CommandRequestCallbackManager<T> : ICallbackManager where T : struct, IReceivedCommandRequest
    {
        private readonly IndexedCallbacks<T> callbacks = new IndexedCallbacks<T>();
        private readonly CommandSystem commandSystem;

        private ulong nextCallbackId = 1;

        public CommandRequestCallbackManager(World world)
        {
            commandSystem = world.GetExistingManager<CommandSystem>();
        }

        public void InvokeCallbacks()
        {
            var requests = commandSystem.GetRequests<T>();
            for (int i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                callbacks.InvokeAll(request.GetEntityId().Id, request);
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
