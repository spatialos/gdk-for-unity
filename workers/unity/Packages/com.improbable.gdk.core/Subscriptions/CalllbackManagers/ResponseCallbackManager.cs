using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public class ResponseCallbackManager<T> : ICallbackManager where T : struct, IReceivedCommandResponse
    {
        private readonly SingleUseIndexCallbacks<T> callbacks = new SingleUseIndexCallbacks<T>();
        private readonly CommandSystem commandSystem;

        private ulong nextCallbackId = 1;

        public ResponseCallbackManager(World world)
        {
            commandSystem = world.GetExistingManager<CommandSystem>();
        }

        public void InvokeCallbacks()
        {
            var responses = commandSystem.GetResponses<T>();
            for (int i = 0; i < responses.Count; ++i)
            {
                callbacks.InvokeAll(responses[i].GetRequestId(), responses[i]);
                callbacks.RemoveAllCallbacksForIndex(responses[i].GetRequestId());
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
