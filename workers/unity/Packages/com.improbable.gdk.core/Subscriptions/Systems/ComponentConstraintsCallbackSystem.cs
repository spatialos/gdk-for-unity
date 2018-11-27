using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    [UpdateBefore(typeof(CommandCallbackSystem))]
    public class ComponentConstraintsCallbackSystem : ComponentSystem
    {
        private readonly GuardedComponentCallbackManagerSet<uint, ComponentAddedCallbackManager> componentAdded =
            new GuardedComponentCallbackManagerSet<uint, ComponentAddedCallbackManager>();

        private readonly GuardedComponentCallbackManagerSet<uint, ComponentRemovedCallbackManager> componentRemoved =
            new GuardedComponentCallbackManagerSet<uint, ComponentRemovedCallbackManager>();

        private readonly GuardedComponentCallbackManagerSet<uint, ComponentAuthorityCallbackManager> authority =
            new GuardedComponentCallbackManagerSet<uint, ComponentAuthorityCallbackManager>();

        private readonly Dictionary<ulong, (ulong, IComponentCallbackManager)> keyToInternalKeyAndManager =
            new Dictionary<ulong, (ulong, IComponentCallbackManager)>();

        private ulong callbacksRegistered = 1;

        public ulong RegisterComponentRemovedCallback(EntityId entityId, uint componentId, Action<int> callback)
        {
            if (!componentRemoved.TryGetManager(componentId, out var manager))
            {
                manager = new ComponentRemovedCallbackManager(componentId);
                componentRemoved.AddCallbackManager(componentId, manager);
            }

            var key = manager.RegisterCallback(entityId, callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        public ulong RegisterComponentAddedCallback(EntityId entityId, uint componentId, Action<int> callback)
        {
            if (!componentAdded.TryGetManager(componentId, out var manager))
            {
                manager = new ComponentAddedCallbackManager(componentId);
                componentAdded.AddCallbackManager(componentId, manager);
            }

            var key = manager.RegisterCallback(entityId, callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        public ulong RegisterAuthorityCallback(EntityId entityId, uint componentId, Action<Authority> callback)
        {
            if (!authority.TryGetManager(componentId, out var manager))
            {
                manager = new ComponentAuthorityCallbackManager(componentId);
                authority.AddCallbackManager(componentId, manager);
            }

            var key = manager.RegisterCallback(entityId, callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        public bool UnregisterCallback(ulong callbackKey)
        {
            if (!keyToInternalKeyAndManager.TryGetValue(callbackKey, out var keyAndManager))
            {
                return false;
            }

            return keyAndManager.Item2.UnregisterCallback(callbackKey);
        }

        [Inject] private ComponentUpdateSystem componentSystem;

        protected override void OnUpdate()
        {
            componentRemoved.InvokeCallbacks(componentSystem);
            componentAdded.InvokeCallbacks(componentSystem);
            authority.InvokeCallbacks(componentSystem);
        }
    }
}
