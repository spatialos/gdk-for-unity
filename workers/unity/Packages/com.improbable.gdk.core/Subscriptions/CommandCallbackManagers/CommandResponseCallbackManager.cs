using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Subscriptions.CommandCallbackManagers
{
    public class CommandResponseCallbackManager<T> : ICommandResponseCallbackManager where T : IReceivedCommandResponse
    {
        private readonly IndexedCallbacks<WorldCommands.CreateEntity.ReceivedResponse> callbacks =
            new IndexedCallbacks<WorldCommands.CreateEntity.ReceivedResponse>();

        private ulong nextCallbackId = 1;

        public void InvokeCallbacks(CommandSystem commandSystem)
        {
            var responses = commandSystem.GetResponses<T>();
            foreach (var response in responses)
            {
                // todo should then remove all callbacks that have this request ID as they will never be called again
                callbacks.InvokeAll(response.RequestId, response);
            }
        }

        public ulong RegisterCallback(long requestId, Action<WorldCommands.CreateEntity.ReceivedResponse> callback)
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
