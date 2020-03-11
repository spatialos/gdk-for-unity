using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public class CommandResponseCallbackManager<T> : ICallbackManager where T : struct, IReceivedCommandResponse
    {
        private readonly Dictionary<long, Action<T>> callbacks = new Dictionary<long, Action<T>>();
        private readonly CommandSystem commandSystem;

        public CommandResponseCallbackManager(World world)
        {
            commandSystem = world.GetExistingSystem<CommandSystem>();
        }

        public void InvokeCallbacks()
        {
            var responses = commandSystem.GetResponses<T>();
            for (var i = 0; i < responses.Count; ++i)
            {
                ref readonly var response = ref responses[i];
                if (callbacks.TryGetValue(response.RequestId, out var callback))
                {
                    callbacks.Remove(response.RequestId);
                    callback(response);
                }
            }
        }

        public void RegisterCallback(long requestId, Action<T> callback)
        {
            callbacks.Add(requestId, callback);
        }

        public bool UnregisterCallback(ulong requestId)
        {
            return callbacks.Remove((long) requestId);
        }
    }
}
