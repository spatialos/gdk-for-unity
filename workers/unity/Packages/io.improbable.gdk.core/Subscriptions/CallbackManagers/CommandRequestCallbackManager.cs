using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public class CommandRequestCallbackManager<T> : ICallbackManager where T : struct, IReceivedCommandRequest
    {
        private readonly EntityCallbacks<T> callbacks = new EntityCallbacks<T>();
        private readonly CommandSystem commandSystem;

        public CommandRequestCallbackManager(CommandSystem commandSystem)
        {
            this.commandSystem = commandSystem;
        }

        public void InvokeCallbacks()
        {
            var requests = commandSystem.GetRequests<T>();
            for (var i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                callbacks.InvokeAll(request.EntityId, request);
            }
        }

        public ulong RegisterCallback(EntityId entityId, Action<T> callback)
        {
            return callbacks.Add(entityId, callback);
        }

        public bool UnregisterCallback(ulong callbackKey)
        {
            return callbacks.Remove(callbackKey);
        }
    }
}
