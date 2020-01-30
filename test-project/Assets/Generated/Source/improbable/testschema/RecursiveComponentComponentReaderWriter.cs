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
    public class RecursiveComponentReaderSubscriptionManager : SubscriptionManager<RecursiveComponentReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<RecursiveComponentReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public RecursiveComponentReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.EntityManager;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(RecursiveComponent.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new RecursiveComponentReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(RecursiveComponent.ComponentId, entityId =>
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

        public override Subscription<RecursiveComponentReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<RecursiveComponentReader>>>();
            }

            var subscription = new Subscription<RecursiveComponentReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<RecursiveComponentReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<RecursiveComponent.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new RecursiveComponentReader(world, entity, entityId));
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
            var sub = ((Subscription<RecursiveComponentReader>) subscription);
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
            var sub = ((Subscription<RecursiveComponentReader>) subscription);
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
    public class RecursiveComponentWriterSubscriptionManager : SubscriptionManager<RecursiveComponentWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<RecursiveComponentWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public RecursiveComponentWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(RecursiveComponent.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new RecursiveComponentWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<RecursiveComponentWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<RecursiveComponentWriter>>>();
            }

            var subscription = new Subscription<RecursiveComponentWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<RecursiveComponentWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(RecursiveComponent.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, RecursiveComponent.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new RecursiveComponentWriter(world, entity, entityId));
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
            var sub = ((Subscription<RecursiveComponentWriter>) subscription);
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
            var sub = ((Subscription<RecursiveComponentWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class RecursiveComponentReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public RecursiveComponent.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<RecursiveComponent.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, RecursiveComponent.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, RecursiveComponent.ComponentId, value);
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

        private Dictionary<Action<RecursiveComponent.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<RecursiveComponent.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<RecursiveComponent.Update>, ulong>();
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

        private Dictionary<Action<global::Improbable.TestSchema.TypeA>, ulong> aUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.TestSchema.TypeA> OnAUpdate
        {
            add
            {
                if (aUpdateCallbackToCallbackKey == null)
                {
                    aUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.TypeA>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<RecursiveComponent.Update>(EntityId, update =>
                {
                    if (update.A.HasValue)
                    {
                        value(update.A.Value);
                    }
                });
                aUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!aUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                aUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.TestSchema.TypeB>, ulong> bUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.TestSchema.TypeB> OnBUpdate
        {
            add
            {
                if (bUpdateCallbackToCallbackKey == null)
                {
                    bUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.TypeB>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<RecursiveComponent.Update>(EntityId, update =>
                {
                    if (update.B.HasValue)
                    {
                        value(update.B.Value);
                    }
                });
                bUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!bUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                bUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.TestSchema.TypeC>, ulong> cUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.TestSchema.TypeC> OnCUpdate
        {
            add
            {
                if (cUpdateCallbackToCallbackKey == null)
                {
                    cUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.TypeC>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<RecursiveComponent.Update>(EntityId, update =>
                {
                    if (update.C.HasValue)
                    {
                        value(update.C.Value);
                    }
                });
                cUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!cUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                cUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        internal RecursiveComponentReader(World world, Entity entity, EntityId entityId)
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

            if (aUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in aUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                aUpdateCallbackToCallbackKey.Clear();
            }

            if (bUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in bUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                bUpdateCallbackToCallbackKey.Clear();
            }

            if (cUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in cUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                cUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class RecursiveComponentWriter : RecursiveComponentReader
    {
        internal RecursiveComponentWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(RecursiveComponent.Update update)
        {
            var component = EntityManager.GetComponentData<RecursiveComponent.Component>(Entity);

            if (update.A.HasValue)
            {
                component.A = update.A.Value;
            }

            if (update.B.HasValue)
            {
                component.B = update.B.Value;
            }

            if (update.C.HasValue)
            {
                component.C = update.C.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, RecursiveComponent.ComponentId);
        }
    }
}
