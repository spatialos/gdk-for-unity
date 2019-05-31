using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public class CommandResponseCallbackManager<T> : ICallbackManager where T : struct, IReceivedCommandResponse
    {
        private readonly SingleUseIndexCallbacks<T> callbacks = new SingleUseIndexCallbacks<T>();
        private readonly CommandSystem commandSystem;

        private ulong nextCallbackId = 1;

        public CommandResponseCallbackManager(World world)
        {
            commandSystem = world.GetExistingSystem<CommandSystem>();
        }

        public void InvokeCallbacks()
        {
            var responses = commandSystem.GetResponses<T>();
            for (int i = 0; i < responses.Count; ++i)
            {
                ref readonly var response = ref responses[i];
                callbacks.InvokeAll(response.GetRequestId(), response);
                callbacks.RemoveAllCallbacksForIndex(response.GetRequestId());
            }
        }

        public ulong RegisterCallback(long requestId, Action<T> callback)
        {
            callbacks.Add(requestId, nextCallbackId, callback);
            return nextCallbackId++;
        }

        public bool UnregisterCallback(ulong callbackKey)
        {
            return callbacks.Remove(callbackKey);
        }
    }
}
