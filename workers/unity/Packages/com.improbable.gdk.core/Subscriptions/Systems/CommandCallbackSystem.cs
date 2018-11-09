using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    // todo change this to be like the command system but for callbacks
    // meant to be an example of abstractions that can be made on top of stuff rather than core core stuff
    // for example a more efficient callback thing might have functors that can be reused to remove allocation
    // could also use this to create tasks out of commands rather than callbacks
    // command senders and receivers should then just register things with this system rather than the stuff they are currently doing
    // although what they are currently doing is wrong anyway; they should be given the stuff in the constructor rather than the silly back and forth
    // they should just be an interface onto some other system, not the systems themselves
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class CommandCallbackSystem : ComponentSystem
    {
        private readonly Dictionary<Type, ICommandRequestCallbackManager> typeToRequestCallbackManager =
            new Dictionary<Type, ICommandRequestCallbackManager>();

        private readonly List<ICommandRequestCallbackManager> requestCallbackManagers = new List<ICommandRequestCallbackManager>();

        private readonly Dictionary<Type, ICommandResponseCallbackManager> typeToResponseCallbackManager =
            new Dictionary<Type, ICommandResponseCallbackManager>();

        private readonly List<ICommandResponseCallbackManager> responseCallbackManagers = new List<ICommandResponseCallbackManager>();

        public ICommandRequestCallbackManager GetCommandRequestCallbackManager(Type type)
        {
            return typeToRequestCallbackManager[type];
        }

        public ICommandResponseCallbackManager GetCommandResponseCallbackManager(Type type)
        {
            return typeToResponseCallbackManager[type];
        }

        public void RegisterCommandRequestCallbackManager(Type type, ICommandRequestCallbackManager manager)
        {
            typeToRequestCallbackManager.Add(type, manager);
            requestCallbackManagers.Add(manager);
        }

        public void RegisterCommandResponseCallbackManager(Type type, ICommandResponseCallbackManager manager)
        {
            typeToResponseCallbackManager.Add(type, manager);
            responseCallbackManagers.Add(manager);
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
