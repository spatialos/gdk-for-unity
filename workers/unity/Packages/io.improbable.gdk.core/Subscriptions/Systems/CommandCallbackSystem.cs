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
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class CommandCallbackSystem : ComponentSystem
    {
        private readonly GuardedCallbackManagerSet<Type, ICallbackManager> requestCallbackManagers =
            new GuardedCallbackManagerSet<Type, ICallbackManager>();

        private readonly GuardedCallbackManagerSet<Type, ICallbackManager> responseCallbackManagers =
            new GuardedCallbackManagerSet<Type, ICallbackManager>();

        private CommandSystem commandSystem;

        public ulong RegisterCommandRequestCallback<T>(EntityId entityId, Action<T> callback)
            where T : struct, IReceivedCommandRequest
        {
            if (!requestCallbackManagers.TryGetManager(typeof(T), out var manager))
            {
                manager = new CommandRequestCallbackManager<T>(commandSystem);
                requestCallbackManagers.AddCallbackManager(typeof(T), manager);
            }

            return ((CommandRequestCallbackManager<T>) manager).RegisterCallback(entityId, callback);
        }

        public void RegisterCommandResponseCallback<T>(long requestId, Action<T> callback)
            where T : struct, IReceivedCommandResponse
        {
            if (!responseCallbackManagers.TryGetManager(typeof(T), out var manager))
            {
                manager = new CommandResponseCallbackManager<T>(World);
                responseCallbackManagers.AddCallbackManager(typeof(T), manager);
            }

            ((CommandResponseCallbackManager<T>) manager).RegisterCallback(requestId, callback);
        }

        public bool UnregisterCommandRequestCallback<T>(ulong callbackKey)
            where T : struct, IReceivedCommandRequest
        {
            if (!requestCallbackManagers.TryGetManager(typeof(T), out var manager))
            {
                return false;
            }

            return manager.UnregisterCallback(callbackKey);
        }

        internal void InvokeCallbacks()
        {
            responseCallbackManagers.InvokeEach(manager => manager.InvokeCallbacks());
            requestCallbackManagers.InvokeEach(manager => manager.InvokeCallbacks());
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false;

            commandSystem = World.GetExistingSystem<CommandSystem>();
        }

        protected override void OnUpdate()
        {
        }
    }
}
