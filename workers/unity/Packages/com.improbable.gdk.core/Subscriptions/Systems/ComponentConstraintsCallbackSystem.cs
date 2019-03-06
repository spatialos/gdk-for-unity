using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    // todo consider if these callbacks should be entity specific or not
    // todo if entity specific then registering a callback here would be called immediately if conditions are met
    [DisableAutoCreation]
    public class ComponentConstraintsCallbackSystem : ComponentSystem
    {
        private readonly GuardedCallbackManagerSet<uint, ComponentAddedCallbackManager> componentAdded =
            new GuardedCallbackManagerSet<uint, ComponentAddedCallbackManager>();

        private readonly GuardedCallbackManagerSet<uint, ComponentRemovedCallbackManager> componentRemoved =
            new GuardedCallbackManagerSet<uint, ComponentRemovedCallbackManager>();

        private readonly GuardedCallbackManagerSet<uint, AuthorityConstraintCallbackManager> authority =
            new GuardedCallbackManagerSet<uint, AuthorityConstraintCallbackManager>();

        private EntityAddedCallbackManager entityAdded;
        private EntityRemovedCallbackManager entityRemoved;

        private readonly Dictionary<ulong, (ulong, ICallbackManager)> keyToInternalKeyAndManager =
            new Dictionary<ulong, (ulong, ICallbackManager)>();

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
                manager = new ComponentAddedCallbackManager(componentId, World);
                componentAdded.AddCallbackManager(componentId, manager);
            }

            var key = manager.RegisterCallback(callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        // todo should possibly be separate auth an non-auth ones
        internal ulong RegisterAuthorityCallback(uint componentId, Action<AuthorityChangeReceived> callback)
        {
            if (!authority.TryGetManager(componentId, out var manager))
            {
                manager = new AuthorityConstraintCallbackManager(componentId, World);
                authority.AddCallbackManager(componentId, manager);
            }

            var key = manager.RegisterCallback(callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        internal ulong RegisterEntityAddedCallback(Action<EntityId> callback)
        {
            if (entityAdded == null)
            {
                entityAdded = new EntityAddedCallbackManager(World);
            }

            var key = entityAdded.RegisterCallback(callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, entityAdded));
            return callbacksRegistered++;
        }

        internal ulong RegisterEntityRemovedCallback(Action<EntityId> callback)
        {
            if (entityRemoved == null)
            {
                entityRemoved = new EntityRemovedCallbackManager(World);
            }

            var key = entityRemoved.RegisterCallback(callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, entityRemoved));
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

        internal void Invoke()
        {
            componentRemoved.InvokeCallbacks();
            entityRemoved?.InvokeCallbacks();
            entityAdded?.InvokeCallbacks();
            componentAdded.InvokeCallbacks();
            authority.InvokeCallbacks();
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            entityAdded = new EntityAddedCallbackManager(World);
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}
