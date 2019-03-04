// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    [AutoRegisterSubscriptionManager]
    public class ComponentWithNoFieldsWithEventsReaderSubscriptionManager : SubscriptionManager<ComponentWithNoFieldsWithEventsReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<ComponentWithNoFieldsWithEventsReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public ComponentWithNoFieldsWithEventsReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.GetOrCreateManager<EntityManager>();

            // todo Check that these are there
            workerSystem = world.GetExistingManager<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingManager<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(ComponentWithNoFieldsWithEvents.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new ComponentWithNoFieldsWithEventsReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(ComponentWithNoFieldsWithEvents.ComponentId, entityId =>
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

        public override Subscription<ComponentWithNoFieldsWithEventsReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ComponentWithNoFieldsWithEventsReader>>>();
            }

            var subscription = new Subscription<ComponentWithNoFieldsWithEventsReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ComponentWithNoFieldsWithEventsReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<ComponentWithNoFieldsWithEvents.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new ComponentWithNoFieldsWithEventsReader(world, entity, entityId));
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
            var sub = ((Subscription<ComponentWithNoFieldsWithEventsReader>) subscription);
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
            var sub = ((Subscription<ComponentWithNoFieldsWithEventsReader>) subscription);
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
    public class ComponentWithNoFieldsWithEventsWriterSubscriptionManager : SubscriptionManager<ComponentWithNoFieldsWithEventsWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<ComponentWithNoFieldsWithEventsWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public ComponentWithNoFieldsWithEventsWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingManager<WorkerSystem>();
            componentUpdateSystem = world.GetExistingManager<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingManager<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(ComponentWithNoFieldsWithEvents.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new ComponentWithNoFieldsWithEventsWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<ComponentWithNoFieldsWithEventsWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ComponentWithNoFieldsWithEventsWriter>>>();
            }

            var subscription = new Subscription<ComponentWithNoFieldsWithEventsWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ComponentWithNoFieldsWithEventsWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(ComponentWithNoFieldsWithEvents.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, ComponentWithNoFieldsWithEvents.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new ComponentWithNoFieldsWithEventsWriter(world, entity, entityId));
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
            var sub = ((Subscription<ComponentWithNoFieldsWithEventsWriter>) subscription);
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
            var sub = ((Subscription<ComponentWithNoFieldsWithEventsWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class ComponentWithNoFieldsWithEventsReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public ComponentWithNoFieldsWithEvents.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<ComponentWithNoFieldsWithEvents.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, ComponentWithNoFieldsWithEvents.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, ComponentWithNoFieldsWithEvents.ComponentId, value);
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

        private Dictionary<Action<ComponentWithNoFieldsWithEvents.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<ComponentWithNoFieldsWithEvents.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<ComponentWithNoFieldsWithEvents.Update>, ulong>();
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


        private Dictionary<Action<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>, ulong> evtEventCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> OnEvtEvent
        {
            add
            {
                if (evtEventCallbackToCallbackKey == null)
                {
                    evtEventCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentEventCallback<ComponentWithNoFieldsWithEvents.Evt.Event>(EntityId, ev => value(ev.Payload));
                evtEventCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!evtEventCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                evtEventCallbackToCallbackKey.Remove(value);
            }
        }

        internal ComponentWithNoFieldsWithEventsReader(World world, Entity entity, EntityId entityId)
        {
            Entity = entity;
            EntityId = entityId;

            IsValid = true;

            ComponentUpdateSystem = world.GetExistingManager<ComponentUpdateSystem>();
            CallbackSystem = world.GetExistingManager<ComponentCallbackSystem>();
            EntityManager = world.GetExistingManager<EntityManager>();
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


            if (evtEventCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in evtEventCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                evtEventCallbackToCallbackKey.Clear();
            }
        }
    }

    public class ComponentWithNoFieldsWithEventsWriter : ComponentWithNoFieldsWithEventsReader
    {
        internal ComponentWithNoFieldsWithEventsWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(ComponentWithNoFieldsWithEvents.Update update)
        {
            var component = EntityManager.GetComponentData<ComponentWithNoFieldsWithEvents.Component>(Entity);

            EntityManager.SetComponentData(Entity, component);
        }

        public void SendEvtEvent(global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty evt)
        {
            var eventToSend = new ComponentWithNoFieldsWithEvents.Evt.Event(evt);
            ComponentUpdateSystem.SendEvent(eventToSend, EntityId);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, ComponentWithNoFieldsWithEvents.ComponentId);
        }
    }
}
