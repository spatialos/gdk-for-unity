using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    // meant to be an example of abstractions that can be made on top of stuff rather than core core stuff
    // for example a more efficient callback thing might have functors that can be reused to remove allocation
    // could also use this to create tasks out of commands rather than callbacks
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class CommandCallbackSystem : ComponentSystem
    {
        private readonly Dictionary<Type, ICommandRequestCallbackManager> typeToRequestCallbackManager =
            new Dictionary<Type, ICommandRequestCallbackManager>();

        private readonly List<ICommandRequestCallbackManager> requestCallbackManagers =
            new List<ICommandRequestCallbackManager>();

        private readonly Dictionary<Type, ICommandResponseCallbackManager> typeToResponseCallbackManager =
            new Dictionary<Type, ICommandResponseCallbackManager>();

        private readonly List<ICommandResponseCallbackManager> responseCallbackManagers =
            new List<ICommandResponseCallbackManager>();

        public ulong RegisterCommandRequestCallback<T>(EntityId entityId, Action<T> callback)
            where T : IReceivedCommandRequest
        {
            if (!typeToRequestCallbackManager.TryGetValue(typeof(T), out var manager))
            {
                manager = new CommandRequestCallbackManager<T>();
                typeToRequestCallbackManager.Add(typeof(T), manager);
                requestCallbackManagers.Add(manager);
            }

            return ((CommandRequestCallbackManager<T>) manager).RegisterCallback(entityId, callback);
        }

        public ulong RegisterCommandResponseCallback<T>(long requestId, Action<T> callback)
            where T : IReceivedCommandResponse
        {
            if (!typeToResponseCallbackManager.TryGetValue(typeof(T), out var manager))
            {
                manager = new CommandResponseCallbackManager<T>();
                typeToResponseCallbackManager.Add(typeof(T), manager);
                responseCallbackManagers.Add(manager);
            }

            return ((CommandResponseCallbackManager<T>) manager).RegisterCallback(requestId, callback);
        }

        // todo caller shouldn't have to know the type
        public bool UnregisterCommandRequestCallback<T>(ulong callbackKey) where T : IReceivedCommandRequest
        {
            if (!typeToRequestCallbackManager.TryGetValue(typeof(T), out var manager))
            {
                return false;
            }

            return ((CommandRequestCallbackManager<T>) manager).UnregisterCallback(callbackKey);
        }

        [Inject] private CommandSystem commandSystem;

        protected override void OnUpdate()
        {
            foreach (var callbackManager in requestCallbackManagers)
            {
                callbackManager.InvokeCallbacks(commandSystem);
            }

            foreach (var callbackManager in responseCallbackManagers)
            {
                callbackManager.InvokeCallbacks(commandSystem);
            }
        }
    }
}
