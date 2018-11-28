using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    // meant to be an example of abstractions that can be made on top of stuff rather than core core stuff
    // for example a more efficient callback thing might have functors that can be reused to remove allocation
    // could also use this to create tasks out of commands rather than callbacks
    [DisableAutoCreation]
    public class CommandCallbackSystem : ComponentSystem
    {
        private readonly GuardedRequestCallbackManagerSet<Type, ICommandCallbackManager> callbackManagers =
            new GuardedRequestCallbackManagerSet<Type, ICommandCallbackManager>();

        private readonly Dictionary<ulong, (ulong, ICommandCallbackManager)> keyToInternalKeyAndManager =
            new Dictionary<ulong, (ulong, ICommandCallbackManager)>();

        private ulong callbacksRegistered = 1;

        public ulong RegisterCommandRequestCallback<T>(EntityId entityId, Action<T> callback)
            where T : IReceivedCommandRequest
        {
            if (!callbackManagers.TryGetManager(typeof(T), out var manager))
            {
                manager = new CommandCallbackManager<T>();
                callbackManagers.AddCallbackManager(typeof(T), manager);
            }

            var key = ((CommandCallbackManager<T>) manager).RegisterCallback(entityId, callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        public ulong RegisterCommandResponseCallback<T>(long requestId, Action<T> callback)
            where T : IReceivedCommandResponse
        {
            if (!callbackManagers.TryGetManager(typeof(T), out var manager))
            {
                manager = new ResponseCallbackManager<T>();
                callbackManagers.AddCallbackManager(typeof(T), manager);
            }

            var key = ((ResponseCallbackManager<T>) manager).RegisterCallback(requestId, callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        public bool UnregisterCommandRequestCallback(ulong callbackKey)
        {
            if (!keyToInternalKeyAndManager.TryGetValue(callbackKey, out var keyAndManager))
            {
                return false;
            }

            return keyAndManager.Item2.UnregisterCallback(callbackKey);
        }

        internal void InvokeCallbacks(CommandSystem commandSystem)
        {
            // todo could split these out to ensure requests are done before responses
            callbackManagers.InvokeCallbacks(commandSystem);
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}
