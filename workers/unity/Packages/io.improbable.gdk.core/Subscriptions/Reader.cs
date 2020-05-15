using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Subscriptions
{
    public abstract class Reader<TComponent, TUpdate> : IRequireable
        where TComponent : struct, IComponentData, ISpatialComponentData
        where TUpdate : struct, ISpatialComponentUpdate
    {
        public bool IsValid { get; set; }
        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        private static readonly uint ComponentId = ComponentDatabase.GetComponentId<TComponent>();
        private static readonly ComponentType ComponentAuthType = ComponentDatabase.Metaclasses[ComponentId].Authority;

        private Dictionary<Action<Authority>, ulong> authorityCallbackToCallbackKey;
        private Dictionary<Action<TUpdate>, ulong> updateCallbackToCallbackKey;

        public TComponent Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot read component data when Reader is not valid.");
                }

                return EntityManager.GetComponentData<TComponent>(Entity);
            }
        }

        public bool Authority
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot read authority when Reader is not valid");
                }

                return EntityManager.HasComponent(Entity, ComponentAuthType);
            }
        }

        public event Action<Authority> OnAuthorityUpdate
        {
            add
            {
                if (authorityCallbackToCallbackKey == null)
                {
                    authorityCallbackToCallbackKey = new Dictionary<Action<Authority>, ulong>();
                }

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, ComponentId, value);
                authorityCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!authorityCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                authorityCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<TUpdate> OnUpdate
        {
            add
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot add OnUpdate callback when Reader is not valid.");
                }

                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<TUpdate>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback(EntityId, value);
                updateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!updateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                updateCallbackToCallbackKey.Remove(value);
            }
        }

        protected Reader(World world, Entity entity, EntityId entityId)
        {
            Entity = entity;
            EntityId = entityId;

            IsValid = true;

            ComponentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
            CallbackSystem = world.GetExistingSystem<ComponentCallbackSystem>();
            EntityManager = world.EntityManager;
        }

        public void RemoveAllCallbacks()
        {
            if (authorityCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in authorityCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                authorityCallbackToCallbackKey.Clear();
            }

            if (updateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in updateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                updateCallbackToCallbackKey.Clear();
            }

            RemoveFieldCallbacks();
            RemoveEventCallbacks();
        }

        protected virtual void RemoveFieldCallbacks()
        {
        }

        protected virtual void RemoveEventCallbacks()
        {
        }
    }
}
