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

namespace Improbable.Tests
{
    [AutoRegisterSubscriptionManager]
    public class DependencyTestChildReaderSubscriptionManager : SubscriptionManager<DependencyTestChildReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<DependencyTestChildReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DependencyTestChildReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.EntityManager;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(DependencyTestChild.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new DependencyTestChildReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(DependencyTestChild.ComponentId, entityId =>
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

        public override Subscription<DependencyTestChildReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DependencyTestChildReader>>>();
            }

            var subscription = new Subscription<DependencyTestChildReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DependencyTestChildReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<DependencyTestChild.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DependencyTestChildReader(world, entity, entityId));
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
            var sub = ((Subscription<DependencyTestChildReader>) subscription);
            if (sub.HasValue)
            {
                var reader = sub.Value;
                reader.IsValid = false;
                reader.RemoveAllCallbacks();
            }

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
            var sub = ((Subscription<DependencyTestChildReader>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
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
    public class DependencyTestChildWriterSubscriptionManager : SubscriptionManager<DependencyTestChildWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<DependencyTestChildWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DependencyTestChildWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(DependencyTestChild.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new DependencyTestChildWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<DependencyTestChildWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DependencyTestChildWriter>>>();
            }

            var subscription = new Subscription<DependencyTestChildWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DependencyTestChildWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(DependencyTestChild.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, DependencyTestChild.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DependencyTestChildWriter(world, entity, entityId));
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
            var sub = ((Subscription<DependencyTestChildWriter>) subscription);
            if (sub.HasValue)
            {
                var reader = sub.Value;
                reader.IsValid = false;
                reader.RemoveAllCallbacks();
            }

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
            var sub = ((Subscription<DependencyTestChildWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class DependencyTestChildReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public DependencyTestChild.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<DependencyTestChild.Component>(Entity);
            }
        }

        public Authority Authority
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return ComponentUpdateSystem.GetAuthority(EntityId, DependencyTestChild.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, DependencyTestChild.ComponentId, value);
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

        private Dictionary<Action<DependencyTestChild.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<DependencyTestChild.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<DependencyTestChild.Update>, ulong>();
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

        private Dictionary<Action<uint>, ulong> childUpdateCallbackToCallbackKey;
        public event Action<uint> OnChildUpdate
        {
            add
            {
                if (childUpdateCallbackToCallbackKey == null)
                {
                    childUpdateCallbackToCallbackKey = new Dictionary<Action<uint>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependencyTestChild.Update>(EntityId, update =>
                {
                    if (update.Child.HasValue)
                    {
                        value(update.Child.Value);
                    }
                });
                childUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!childUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                childUpdateCallbackToCallbackKey.Remove(value);
            }
        }


        internal DependencyTestChildReader(World world, Entity entity, EntityId entityId)
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


            if (childUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in childUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                childUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class DependencyTestChildWriter : DependencyTestChildReader
    {
        internal DependencyTestChildWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(DependencyTestChild.Update update)
        {
            var component = EntityManager.GetComponentData<DependencyTestChild.Component>(Entity);

            if (update.Child.HasValue)
            {
                component.Child = update.Child.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }


        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, DependencyTestChild.ComponentId);
        }
    }
}
