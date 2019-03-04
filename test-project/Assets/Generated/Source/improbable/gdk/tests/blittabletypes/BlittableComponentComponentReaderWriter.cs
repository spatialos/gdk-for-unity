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

namespace Improbable.Gdk.Tests.BlittableTypes
{
    [AutoRegisterSubscriptionManager]
    public class BlittableComponentReaderSubscriptionManager : SubscriptionManager<BlittableComponentReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<BlittableComponentReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public BlittableComponentReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.GetOrCreateManager<EntityManager>();

            // todo Check that these are there
            workerSystem = world.GetExistingManager<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingManager<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(BlittableComponent.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new BlittableComponentReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(BlittableComponent.ComponentId, entityId =>
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

        public override Subscription<BlittableComponentReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<BlittableComponentReader>>>();
            }

            var subscription = new Subscription<BlittableComponentReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<BlittableComponentReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<BlittableComponent.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new BlittableComponentReader(world, entity, entityId));
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
            var sub = ((Subscription<BlittableComponentReader>) subscription);
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
            var sub = ((Subscription<BlittableComponentReader>) subscription);
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
    public class BlittableComponentWriterSubscriptionManager : SubscriptionManager<BlittableComponentWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<BlittableComponentWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public BlittableComponentWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingManager<WorkerSystem>();
            componentUpdateSystem = world.GetExistingManager<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingManager<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(BlittableComponent.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new BlittableComponentWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<BlittableComponentWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<BlittableComponentWriter>>>();
            }

            var subscription = new Subscription<BlittableComponentWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<BlittableComponentWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(BlittableComponent.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, BlittableComponent.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new BlittableComponentWriter(world, entity, entityId));
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
            var sub = ((Subscription<BlittableComponentWriter>) subscription);
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
            var sub = ((Subscription<BlittableComponentWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class BlittableComponentReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public BlittableComponent.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<BlittableComponent.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, BlittableComponent.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, BlittableComponent.ComponentId, value);
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

        private Dictionary<Action<BlittableComponent.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<BlittableComponent.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<BlittableComponent.Update>, ulong>();
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

        private Dictionary<Action<BlittableBool>, ulong> boolFieldUpdateCallbackToCallbackKey;
        public event Action<BlittableBool> OnBoolFieldUpdate
        {
            add
            {
                if (boolFieldUpdateCallbackToCallbackKey == null)
                {
                    boolFieldUpdateCallbackToCallbackKey = new Dictionary<Action<BlittableBool>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<BlittableComponent.Update>(EntityId, update =>
                {
                    if (update.BoolField.HasValue)
                    {
                        value(update.BoolField.Value);
                    }
                });
                boolFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!boolFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                boolFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<int>, ulong> intFieldUpdateCallbackToCallbackKey;
        public event Action<int> OnIntFieldUpdate
        {
            add
            {
                if (intFieldUpdateCallbackToCallbackKey == null)
                {
                    intFieldUpdateCallbackToCallbackKey = new Dictionary<Action<int>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<BlittableComponent.Update>(EntityId, update =>
                {
                    if (update.IntField.HasValue)
                    {
                        value(update.IntField.Value);
                    }
                });
                intFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!intFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                intFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<long>, ulong> longFieldUpdateCallbackToCallbackKey;
        public event Action<long> OnLongFieldUpdate
        {
            add
            {
                if (longFieldUpdateCallbackToCallbackKey == null)
                {
                    longFieldUpdateCallbackToCallbackKey = new Dictionary<Action<long>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<BlittableComponent.Update>(EntityId, update =>
                {
                    if (update.LongField.HasValue)
                    {
                        value(update.LongField.Value);
                    }
                });
                longFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!longFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                longFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<float>, ulong> floatFieldUpdateCallbackToCallbackKey;
        public event Action<float> OnFloatFieldUpdate
        {
            add
            {
                if (floatFieldUpdateCallbackToCallbackKey == null)
                {
                    floatFieldUpdateCallbackToCallbackKey = new Dictionary<Action<float>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<BlittableComponent.Update>(EntityId, update =>
                {
                    if (update.FloatField.HasValue)
                    {
                        value(update.FloatField.Value);
                    }
                });
                floatFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!floatFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                floatFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<double>, ulong> doubleFieldUpdateCallbackToCallbackKey;
        public event Action<double> OnDoubleFieldUpdate
        {
            add
            {
                if (doubleFieldUpdateCallbackToCallbackKey == null)
                {
                    doubleFieldUpdateCallbackToCallbackKey = new Dictionary<Action<double>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<BlittableComponent.Update>(EntityId, update =>
                {
                    if (update.DoubleField.HasValue)
                    {
                        value(update.DoubleField.Value);
                    }
                });
                doubleFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!doubleFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                doubleFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }


        private Dictionary<Action<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>, ulong> firstEventEventCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> OnFirstEventEvent
        {
            add
            {
                if (firstEventEventCallbackToCallbackKey == null)
                {
                    firstEventEventCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentEventCallback<BlittableComponent.FirstEvent.Event>(EntityId, ev => value(ev.Payload));
                firstEventEventCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!firstEventEventCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                firstEventEventCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>, ulong> secondEventEventCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> OnSecondEventEvent
        {
            add
            {
                if (secondEventEventCallbackToCallbackKey == null)
                {
                    secondEventEventCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentEventCallback<BlittableComponent.SecondEvent.Event>(EntityId, ev => value(ev.Payload));
                secondEventEventCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!secondEventEventCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                secondEventEventCallbackToCallbackKey.Remove(value);
            }
        }

        internal BlittableComponentReader(World world, Entity entity, EntityId entityId)
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


            if (boolFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in boolFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                boolFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (intFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in intFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                intFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (longFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in longFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                longFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (floatFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in floatFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                floatFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (doubleFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in doubleFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                doubleFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (firstEventEventCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in firstEventEventCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                firstEventEventCallbackToCallbackKey.Clear();
            }

            if (secondEventEventCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in secondEventEventCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                secondEventEventCallbackToCallbackKey.Clear();
            }
        }
    }

    public class BlittableComponentWriter : BlittableComponentReader
    {
        internal BlittableComponentWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(BlittableComponent.Update update)
        {
            var component = EntityManager.GetComponentData<BlittableComponent.Component>(Entity);

            if (update.BoolField.HasValue)
            {
                component.BoolField = update.BoolField.Value;
            }

            if (update.IntField.HasValue)
            {
                component.IntField = update.IntField.Value;
            }

            if (update.LongField.HasValue)
            {
                component.LongField = update.LongField.Value;
            }

            if (update.FloatField.HasValue)
            {
                component.FloatField = update.FloatField.Value;
            }

            if (update.DoubleField.HasValue)
            {
                component.DoubleField = update.DoubleField.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void SendFirstEventEvent(global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload firstEvent)
        {
            var eventToSend = new BlittableComponent.FirstEvent.Event(firstEvent);
            ComponentUpdateSystem.SendEvent(eventToSend, EntityId);
        }
        public void SendSecondEventEvent(global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload secondEvent)
        {
            var eventToSend = new BlittableComponent.SecondEvent.Event(secondEvent);
            ComponentUpdateSystem.SendEvent(eventToSend, EntityId);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, BlittableComponent.ComponentId);
        }
    }
}
