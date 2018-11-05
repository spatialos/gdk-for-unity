using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    public class CommandReceiverCallbackSystem : ComponentSystem
    {
        private struct CommandCallbackDetails
        {
            public ComponentGroup ComponentGroup;
            public ICommandRequestCallbackManager CallbackManager;
        }

        private readonly Dictionary<Type, ICommandRequestCallbackManager> typeToCallbackManager =
            new Dictionary<Type, ICommandRequestCallbackManager>();

        private readonly List<CommandCallbackDetails> callbackDetails = new List<CommandCallbackDetails>();

        public ICommandRequestCallbackManager GetCommandRequestCallbackManager(Type type)
        {
            return typeToCallbackManager[type];
        }

        public void RegisterCommandRequestCallbackManager(Type type, ICommandRequestCallbackManager manager)
        {
            typeToCallbackManager.Add(type, manager);
            callbackDetails.Add(new CommandCallbackDetails
            {
                ComponentGroup = GetComponentGroup(manager.Query),
                CallbackManager = manager
            });
        }

        protected override void OnUpdate()
        {
            foreach (var commandGroup in callbackDetails)
            {
                commandGroup.CallbackManager.InvokeCallbacks(commandGroup.ComponentGroup, this);
            }
        }
    }
}
