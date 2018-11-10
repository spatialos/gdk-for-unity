using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Subscriptions
{
    public class CommandResponseCallbackManager<T> : ICommandResponseCallbackManager where T : IReceivedCommandResponse
    {
        private readonly SingleUseIndexCallbacks<T> callbacks = new SingleUseIndexCallbacks<T>();

        private ulong nextCallbackId = 1;

        public void InvokeCallbacks(CommandSystem commandSystem)
        {
            var responses = commandSystem.GetResponses<T>();
            foreach (var response in responses)
            {
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
