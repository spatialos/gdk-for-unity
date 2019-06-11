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
        private readonly GuardedCallbackManagerSet<Type, ICallbackManager> componentCallbackManagers =
            new GuardedCallbackManagerSet<Type, ICallbackManager>();

        private readonly GuardedAuthorityCallbackManagerSet<uint, ComponentAuthorityCallbackManager> authorityCallbackManagers =
            new GuardedAuthorityCallbackManagerSet<uint, ComponentAuthorityCallbackManager>();

        private readonly Dictionary<ulong, (ulong, ICallbackManager)> keyToInternalKeyAndManager =
            new Dictionary<ulong, (ulong, ICallbackManager)>();

        private ulong callbacksRegistered = 1;

        public ulong RegisterComponentUpdateCallback<T>(EntityId entityId, Action<T> callback)
            where T : ISpatialComponentUpdate
        {
            if (!componentCallbackManagers.TryGetManager(typeof(T), out var manager))
            {
                manager = new ComponentUpdateCallbackManager<T>(World);
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
                manager = new ComponentEventCallbackManager<T>(World);
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
                manager = new ComponentAuthorityCallbackManager(componentId, World);
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

            return keyAndManager.Item2.UnregisterCallback(keyAndManager.Item1);
        }

        internal void InvokeNoLossImminent()
        {
            // todo might want to split updates and events out to ensure updates are called first
            componentCallbackManagers.InvokeCallbacks();
            authorityCallbackManagers.InvokeCallbacks();
        }

        internal void InvokeLossImminent()
        {
            authorityCallbackManagers.InvokeLossImminentCallbacks();
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}
