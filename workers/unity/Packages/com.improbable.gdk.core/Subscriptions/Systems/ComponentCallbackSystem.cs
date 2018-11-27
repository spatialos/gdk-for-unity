using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    [UpdateBefore(typeof(CommandCallbackSystem))]
    public class ComponentCallbackSystem : ComponentSystem
    {
        private readonly GuardedComponentCallbackManagerSet<Type, IComponentCallbackManager> componentCallbackManagers =
            new GuardedComponentCallbackManagerSet<Type, IComponentCallbackManager>();

        private readonly GuardedComponentCallbackManagerSet<uint, ComponentAuthorityCallbackManager> authorityCallbackManagers =
            new GuardedComponentCallbackManagerSet<uint, ComponentAuthorityCallbackManager>();

        private readonly Dictionary<ulong, (ulong, IComponentCallbackManager)> keyToInternalKeyAndManager =
            new Dictionary<ulong, (ulong, IComponentCallbackManager)>();

        private ulong callbacksRegistered = 1;

        public ulong RegisterComponentUpdateCallback<T>(EntityId entityId, Action<T> callback)
            where T : ISpatialComponentUpdate
        {
            if (!componentCallbackManagers.TryGetManager(typeof(T), out var manager))
            {
                manager = new ComponentUpdateCallbackManager<T>();
                componentCallbackManagers.AddCallbackManager(typeof(T), manager);
            }

            var key = ((ComponentUpdateCallbackManager<T>) manager).RegisterCallback(entityId, callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        public ulong RegisterComponentEventCallback<T>(EntityId entityId, Action<T> callback)
            where T : IEvent
        {
            if (!componentCallbackManagers.TryGetManager(typeof(T), out var manager))
            {
                manager = new ComponentEventCallbackManager<T>();
                componentCallbackManagers.AddCallbackManager(typeof(T), manager);
            }

            var key = ((ComponentEventCallbackManager<T>) manager).RegisterCallback(entityId, callback);
            keyToInternalKeyAndManager.Add(callbacksRegistered, (key, manager));
            return callbacksRegistered++;
        }

        public ulong RegisterAuthorityCallback(EntityId entityId, uint componentId, Action<Authority> callback)
        {
            if (!authorityCallbackManagers.TryGetManager(componentId, out var manager))
            {
                manager = new ComponentAuthorityCallbackManager(componentId);
                authorityCallbackManagers.AddCallbackManager(componentId, manager);
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
        [Inject] private ComponentConstraintsCallbackSystem yoyoyo;

        protected override void OnUpdate()
        {
            // might want to split updates and events out to ensure updates are called first
            componentCallbackManagers.InvokeCallbacks(componentSystem);
            authorityCallbackManagers.InvokeCallbacks(componentSystem);
        }
    }
}
