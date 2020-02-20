// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.TestSchema
{
    [AutoRegisterSubscriptionManager]
    public class ComponentUsingNestedTypeSameNameReaderSubscriptionManager : SubscriptionManager<ComponentUsingNestedTypeSameNameReader>
    {
        private readonly EntityManager entityManager;

        private Dictionary<EntityId, HashSet<Subscription<ComponentUsingNestedTypeSameNameReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public ComponentUsingNestedTypeSameNameReaderSubscriptionManager(World world) : base(world)
        {
            entityManager = world.EntityManager;

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(ComponentUsingNestedTypeSameName.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new ComponentUsingNestedTypeSameNameReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(ComponentUsingNestedTypeSameName.ComponentId, entityId =>
            {
                if (!entitiesMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    ResetValue(subscription);
                    subscription.SetUnavailable();
                }

                entitiesNotMatchingRequirements.Add(entityId);
                entitiesMatchingRequirements.Remove(entityId);
            });
        }

        public override Subscription<ComponentUsingNestedTypeSameNameReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ComponentUsingNestedTypeSameNameReader>>>();
            }

            var subscription = new Subscription<ComponentUsingNestedTypeSameNameReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ComponentUsingNestedTypeSameNameReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<ComponentUsingNestedTypeSameName.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new ComponentUsingNestedTypeSameNameReader(world, entity, entityId));
            }
            else
            {
                entitiesNotMatchingRequirements.Add(entityId);
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<ComponentUsingNestedTypeSameNameReader>) subscription);
            ResetValue(sub);

            var subscriptions = entityIdToReaderSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToReaderSubscriptions.Remove(sub.EntityId);
                entitiesMatchingRequirements.Remove(sub.EntityId);
                entitiesNotMatchingRequirements.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = ((Subscription<ComponentUsingNestedTypeSameNameReader>) subscription);
            if (sub.HasValue)
            {
                var reader = sub.Value;
                reader.IsValid = false;
                reader.RemoveAllCallbacks();
            }
        }

        private void OnComponentAdded(EntityId entityId)
        {
        }

        private void OnComponentRemoved(EntityId entityId)
        {
        }
    }

    [AutoRegisterSubscriptionManager]
    public class ComponentUsingNestedTypeSameNameWriterSubscriptionManager : SubscriptionManager<ComponentUsingNestedTypeSameNameWriter>
    {
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<ComponentUsingNestedTypeSameNameWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public ComponentUsingNestedTypeSameNameWriterSubscriptionManager(World world) : base(world)
        {
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(ComponentUsingNestedTypeSameName.ComponentId, authorityChange =>
            {
                if (authorityChange.Authority == Authority.Authoritative)
                {
                    if (!entitiesNotMatchingRequirements.Contains(authorityChange.EntityId))
                    {
                        return;
                    }

                    workerSystem.TryGetEntity(authorityChange.EntityId, out var entity);

                    foreach (var subscription in entityIdToWriterSubscriptions[authorityChange.EntityId])
                    {
                        subscription.SetAvailable(new ComponentUsingNestedTypeSameNameWriter(world, entity, authorityChange.EntityId));
                    }

                    entitiesMatchingRequirements.Add(authorityChange.EntityId);
                    entitiesNotMatchingRequirements.Remove(authorityChange.EntityId);
                }
                else if (authorityChange.Authority == Authority.NotAuthoritative)
                {
                    if (!entitiesMatchingRequirements.Contains(authorityChange.EntityId))
                    {
                        return;
                    }

                    workerSystem.TryGetEntity(authorityChange.EntityId, out var entity);

                    foreach (var subscription in entityIdToWriterSubscriptions[authorityChange.EntityId])
                    {
                        ResetValue(subscription);
                        subscription.SetUnavailable();
                    }

                    entitiesNotMatchingRequirements.Add(authorityChange.EntityId);
                    entitiesMatchingRequirements.Remove(authorityChange.EntityId);
                }
            });
        }

        public override Subscription<ComponentUsingNestedTypeSameNameWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ComponentUsingNestedTypeSameNameWriter>>>();
            }

            var subscription = new Subscription<ComponentUsingNestedTypeSameNameWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ComponentUsingNestedTypeSameNameWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(ComponentUsingNestedTypeSameName.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, ComponentUsingNestedTypeSameName.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new ComponentUsingNestedTypeSameNameWriter(world, entity, entityId));
            }
            else
            {
                entitiesNotMatchingRequirements.Add(entityId);
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<ComponentUsingNestedTypeSameNameWriter>) subscription);
            ResetValue(sub);

            var subscriptions = entityIdToWriterSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToWriterSubscriptions.Remove(sub.EntityId);
                entitiesMatchingRequirements.Remove(sub.EntityId);
                entitiesNotMatchingRequirements.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = ((Subscription<ComponentUsingNestedTypeSameNameWriter>) subscription);
            if (sub.HasValue)
            {
                var reader = sub.Value;
                reader.IsValid = false;
                reader.RemoveAllCallbacks();
            }
        }
    }

    public class ComponentUsingNestedTypeSameNameReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public ComponentUsingNestedTypeSameName.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot read component data when Reader is not valid.");
                }

                return EntityManager.GetComponentData<ComponentUsingNestedTypeSameName.Component>(Entity);
            }
        }

        public Authority Authority
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot read authority when Reader is not valid");
                }

                return ComponentUpdateSystem.GetAuthority(EntityId, ComponentUsingNestedTypeSameName.ComponentId);
            }
        }

        private Dictionary<Action<Authority>, ulong> authorityCallbackToCallbackKey;
        public event Action<Authority> OnAuthorityUpdate
        {
            add
            {
                if (authorityCallbackToCallbackKey == null)
                {
                    authorityCallbackToCallbackKey = new Dictionary<Action<Authority>, ulong>();
                }

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, ComponentUsingNestedTypeSameName.ComponentId, value);
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

        private Dictionary<Action<ComponentUsingNestedTypeSameName.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<ComponentUsingNestedTypeSameName.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<ComponentUsingNestedTypeSameName.Update>, ulong>();
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

        private Dictionary<Action<int>, ulong> nestedFieldUpdateCallbackToCallbackKey;
        public event Action<int> OnNestedFieldUpdate
        {
            add
            {
                if (nestedFieldUpdateCallbackToCallbackKey == null)
                {
                    nestedFieldUpdateCallbackToCallbackKey = new Dictionary<Action<int>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ComponentUsingNestedTypeSameName.Update>(EntityId, update =>
                {
                    if (update.NestedField.HasValue)
                    {
                        value(update.NestedField.Value);
                    }
                });
                nestedFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!nestedFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                nestedFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName>, ulong> other0FieldUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName> OnOther0FieldUpdate
        {
            add
            {
                if (other0FieldUpdateCallbackToCallbackKey == null)
                {
                    other0FieldUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ComponentUsingNestedTypeSameName.Update>(EntityId, update =>
                {
                    if (update.Other0Field.HasValue)
                    {
                        value(update.Other0Field.Value);
                    }
                });
                other0FieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!other0FieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                other0FieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName>, ulong> other1FieldUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName> OnOther1FieldUpdate
        {
            add
            {
                if (other1FieldUpdateCallbackToCallbackKey == null)
                {
                    other1FieldUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ComponentUsingNestedTypeSameName.Update>(EntityId, update =>
                {
                    if (update.Other1Field.HasValue)
                    {
                        value(update.Other1Field.Value);
                    }
                });
                other1FieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!other1FieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                other1FieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        internal ComponentUsingNestedTypeSameNameReader(World world, Entity entity, EntityId entityId)
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

            if (nestedFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in nestedFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                nestedFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (other0FieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in other0FieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                other0FieldUpdateCallbackToCallbackKey.Clear();
            }

            if (other1FieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in other1FieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                other1FieldUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class ComponentUsingNestedTypeSameNameWriter : ComponentUsingNestedTypeSameNameReader
    {
        internal ComponentUsingNestedTypeSameNameWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(ComponentUsingNestedTypeSameName.Update update)
        {
            var component = EntityManager.GetComponentData<ComponentUsingNestedTypeSameName.Component>(Entity);

            if (update.NestedField.HasValue)
            {
                component.NestedField = update.NestedField.Value;
            }

            if (update.Other0Field.HasValue)
            {
                component.Other0Field = update.Other0Field.Value;
            }

            if (update.Other1Field.HasValue)
            {
                component.Other1Field = update.Other1Field.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, ComponentUsingNestedTypeSameName.ComponentId);
        }
    }
}
