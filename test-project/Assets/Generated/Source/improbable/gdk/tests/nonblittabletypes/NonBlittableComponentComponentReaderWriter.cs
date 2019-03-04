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

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    [AutoRegisterSubscriptionManager]
    public class NonBlittableComponentReaderSubscriptionManager : SubscriptionManager<NonBlittableComponentReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<NonBlittableComponentReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public NonBlittableComponentReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.GetOrCreateManager<EntityManager>();

            // todo Check that these are there
            workerSystem = world.GetExistingManager<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingManager<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(NonBlittableComponent.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new NonBlittableComponentReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(NonBlittableComponent.ComponentId, entityId =>
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

        public override Subscription<NonBlittableComponentReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<NonBlittableComponentReader>>>();
            }

            var subscription = new Subscription<NonBlittableComponentReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<NonBlittableComponentReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<NonBlittableComponent.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new NonBlittableComponentReader(world, entity, entityId));
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
            var sub = ((Subscription<NonBlittableComponentReader>) subscription);
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
            var sub = ((Subscription<NonBlittableComponentReader>) subscription);
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
    public class NonBlittableComponentWriterSubscriptionManager : SubscriptionManager<NonBlittableComponentWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<NonBlittableComponentWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public NonBlittableComponentWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingManager<WorkerSystem>();
            componentUpdateSystem = world.GetExistingManager<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingManager<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(NonBlittableComponent.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new NonBlittableComponentWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<NonBlittableComponentWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<NonBlittableComponentWriter>>>();
            }

            var subscription = new Subscription<NonBlittableComponentWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<NonBlittableComponentWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(NonBlittableComponent.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, NonBlittableComponent.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new NonBlittableComponentWriter(world, entity, entityId));
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
            var sub = ((Subscription<NonBlittableComponentWriter>) subscription);
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
            var sub = ((Subscription<NonBlittableComponentWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class NonBlittableComponentReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public NonBlittableComponent.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<NonBlittableComponent.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, NonBlittableComponent.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, NonBlittableComponent.ComponentId, value);
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

        private Dictionary<Action<NonBlittableComponent.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<NonBlittableComponent.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<NonBlittableComponent.Update>, ulong>();
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

                var key = CallbackSystem.RegisterComponentUpdateCallback<NonBlittableComponent.Update>(EntityId, update =>
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

                var key = CallbackSystem.RegisterComponentUpdateCallback<NonBlittableComponent.Update>(EntityId, update =>
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

                var key = CallbackSystem.RegisterComponentUpdateCallback<NonBlittableComponent.Update>(EntityId, update =>
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

                var key = CallbackSystem.RegisterComponentUpdateCallback<NonBlittableComponent.Update>(EntityId, update =>
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

                var key = CallbackSystem.RegisterComponentUpdateCallback<NonBlittableComponent.Update>(EntityId, update =>
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

        private Dictionary<Action<string>, ulong> stringFieldUpdateCallbackToCallbackKey;
        public event Action<string> OnStringFieldUpdate
        {
            add
            {
                if (stringFieldUpdateCallbackToCallbackKey == null)
                {
                    stringFieldUpdateCallbackToCallbackKey = new Dictionary<Action<string>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<NonBlittableComponent.Update>(EntityId, update =>
                {
                    if (update.StringField.HasValue)
                    {
                        value(update.StringField.Value);
                    }
                });
                stringFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!stringFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                stringFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<int?>, ulong> optionalFieldUpdateCallbackToCallbackKey;
        public event Action<int?> OnOptionalFieldUpdate
        {
            add
            {
                if (optionalFieldUpdateCallbackToCallbackKey == null)
                {
                    optionalFieldUpdateCallbackToCallbackKey = new Dictionary<Action<int?>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<NonBlittableComponent.Update>(EntityId, update =>
                {
                    if (update.OptionalField.HasValue)
                    {
                        value(update.OptionalField.Value);
                    }
                });
                optionalFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!optionalFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                optionalFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.List<int>>, ulong> listFieldUpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.List<int>> OnListFieldUpdate
        {
            add
            {
                if (listFieldUpdateCallbackToCallbackKey == null)
                {
                    listFieldUpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.List<int>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<NonBlittableComponent.Update>(EntityId, update =>
                {
                    if (update.ListField.HasValue)
                    {
                        value(update.ListField.Value);
                    }
                });
                listFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!listFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                listFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<int,string>>, ulong> mapFieldUpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<int,string>> OnMapFieldUpdate
        {
            add
            {
                if (mapFieldUpdateCallbackToCallbackKey == null)
                {
                    mapFieldUpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<int,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<NonBlittableComponent.Update>(EntityId, update =>
                {
                    if (update.MapField.HasValue)
                    {
                        value(update.MapField.Value);
                    }
                });
                mapFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!mapFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                mapFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }


        private Dictionary<Action<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload>, ulong> firstEventEventCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload> OnFirstEventEvent
        {
            add
            {
                if (firstEventEventCallbackToCallbackKey == null)
                {
                    firstEventEventCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentEventCallback<NonBlittableComponent.FirstEvent.Event>(EntityId, ev => value(ev.Payload));
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

        private Dictionary<Action<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload>, ulong> secondEventEventCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload> OnSecondEventEvent
        {
            add
            {
                if (secondEventEventCallbackToCallbackKey == null)
                {
                    secondEventEventCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentEventCallback<NonBlittableComponent.SecondEvent.Event>(EntityId, ev => value(ev.Payload));
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

        internal NonBlittableComponentReader(World world, Entity entity, EntityId entityId)
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

            if (stringFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in stringFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                stringFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (optionalFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in optionalFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                optionalFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (listFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in listFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                listFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (mapFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in mapFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                mapFieldUpdateCallbackToCallbackKey.Clear();
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

    public class NonBlittableComponentWriter : NonBlittableComponentReader
    {
        internal NonBlittableComponentWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(NonBlittableComponent.Update update)
        {
            var component = EntityManager.GetComponentData<NonBlittableComponent.Component>(Entity);

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

            if (update.StringField.HasValue)
            {
                component.StringField = update.StringField.Value;
            }

            if (update.OptionalField.HasValue)
            {
                component.OptionalField = update.OptionalField.Value;
            }

            if (update.ListField.HasValue)
            {
                component.ListField = update.ListField.Value;
            }

            if (update.MapField.HasValue)
            {
                component.MapField = update.MapField.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void SendFirstEventEvent(global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload firstEvent)
        {
            var eventToSend = new NonBlittableComponent.FirstEvent.Event(firstEvent);
            ComponentUpdateSystem.SendEvent(eventToSend, EntityId);
        }
        public void SendSecondEventEvent(global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload secondEvent)
        {
            var eventToSend = new NonBlittableComponent.SecondEvent.Event(secondEvent);
            ComponentUpdateSystem.SendEvent(eventToSend, EntityId);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, NonBlittableComponent.ComponentId);
        }
    }
}
