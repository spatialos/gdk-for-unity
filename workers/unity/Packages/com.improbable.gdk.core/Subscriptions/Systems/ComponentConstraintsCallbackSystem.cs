using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    // todo consider if these callbacks should be entity specific or not
    // todo if entity specific then registering a callback here would be called immediately if conditions are met
    [DisableAutoCreation]
    public class ComponentConstraintsCallbackSystem : ComponentSystem
    {
        private readonly GuardedComponentCallbackManagerSet<uint, ComponentAddedCallbackManager> componentAdded =
            new GuardedComponentCallbackManagerSet<uint, ComponentAddedCallbackManager>();

        private readonly GuardedComponentCallbackManagerSet<uint, ComponentRemovedCallbackManager> componentRemoved =
            new GuardedComponentCallbackManagerSet<uint, ComponentRemovedCallbackManager>();

        private readonly GuardedComponentCallbackManagerSet<uint, AuthorityConstraintCallbackManager> authority =
            new GuardedComponentCallbackManagerSet<uint, AuthorityConstraintCallbackManager>();

        private readonly Dictionary<ulong, (ulong, IComponentCallbackManager)> keyToInternalKeyAndManager =
            new Dictionary<ulong, (ulong, IComponentCallbackManager)>();

        private ulong callbacksRegistered = 1;

        internal ulong RegisterComponentRemovedCallback(uint componentId, Action<EntityId> callback)
        {
            if (!componentRemoved.TryGetManager(componentId, out var manager))
            {
                manager = new ComponentRemovedCallbackManager(componentId, World);
                componentRemoved.AddCallbackManager(componentId, manager);
            }

            var key = manager.RegisterCallback(callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        internal ulong RegisterComponentAddedCallback(uint componentId, Action<EntityId> callback)
        {
            if (!componentAdded.TryGetManager(componentId, out var manager))
            {
                manager = new ComponentAddedCallbackManager(componentId);
                componentAdded.AddCallbackManager(componentId, manager);
            }

            var key = manager.RegisterCallback(callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        internal ulong RegisterAuthorityCallback(uint componentId, Action<AuthorityChangeReceived> callback)
        {
            if (!authority.TryGetManager(componentId, out var manager))
            {
                manager = new AuthorityConstraintCallbackManager(componentId);
                authority.AddCallbackManager(componentId, manager);
            }

            var key = manager.RegisterCallback(callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        internal bool UnregisterCallback(ulong callbackKey)
        {
            if (!keyToInternalKeyAndManager.TryGetValue(callbackKey, out var keyAndManager))
            {
                return false;
            }

            return keyAndManager.Item2.UnregisterCallback(callbackKey);
        }

        internal void Invoke(ComponentUpdateSystem componentSystem)
        {
            componentRemoved.InvokeCallbacks(componentSystem);
            componentAdded.InvokeCallbacks(componentSystem);
            authority.InvokeCallbacks(componentSystem);
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
