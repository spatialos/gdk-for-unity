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
    public class ExhaustiveEntityReaderSubscriptionManager : SubscriptionManager<ExhaustiveEntityReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<ExhaustiveEntityReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public ExhaustiveEntityReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.EntityManager;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(ExhaustiveEntity.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new ExhaustiveEntityReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(ExhaustiveEntity.ComponentId, entityId =>
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

        public override Subscription<ExhaustiveEntityReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ExhaustiveEntityReader>>>();
            }

            var subscription = new Subscription<ExhaustiveEntityReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ExhaustiveEntityReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<ExhaustiveEntity.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new ExhaustiveEntityReader(world, entity, entityId));
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
            var sub = ((Subscription<ExhaustiveEntityReader>) subscription);
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
            var sub = ((Subscription<ExhaustiveEntityReader>) subscription);
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
    public class ExhaustiveEntityWriterSubscriptionManager : SubscriptionManager<ExhaustiveEntityWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<ExhaustiveEntityWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public ExhaustiveEntityWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(ExhaustiveEntity.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new ExhaustiveEntityWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<ExhaustiveEntityWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ExhaustiveEntityWriter>>>();
            }

            var subscription = new Subscription<ExhaustiveEntityWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ExhaustiveEntityWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(ExhaustiveEntity.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, ExhaustiveEntity.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new ExhaustiveEntityWriter(world, entity, entityId));
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
            var sub = ((Subscription<ExhaustiveEntityWriter>) subscription);
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
            var sub = ((Subscription<ExhaustiveEntityWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class ExhaustiveEntityReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public ExhaustiveEntity.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<ExhaustiveEntity.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, ExhaustiveEntity.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, ExhaustiveEntity.ComponentId, value);
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

        private Dictionary<Action<ExhaustiveEntity.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<ExhaustiveEntity.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<ExhaustiveEntity.Update>, ulong>();
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

        private Dictionary<Action<global::Improbable.Gdk.Core.EntitySnapshot>, ulong> field1UpdateCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.Core.EntitySnapshot> OnField1Update
        {
            add
            {
                if (field1UpdateCallbackToCallbackKey == null)
                {
                    field1UpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Core.EntitySnapshot>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field1.HasValue)
                    {
                        value(update.Field1.Value);
                    }
                });
                field1UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field1UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field1UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::Improbable.Gdk.Core.EntitySnapshot?>, ulong> field2UpdateCallbackToCallbackKey;
        public event Action<global::Improbable.Gdk.Core.EntitySnapshot?> OnField2Update
        {
            add
            {
                if (field2UpdateCallbackToCallbackKey == null)
                {
                    field2UpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Core.EntitySnapshot?>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field2.HasValue)
                    {
                        value(update.Field2.Value);
                    }
                });
                field2UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field2UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field2UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>, ulong> field3UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>> OnField3Update
        {
            add
            {
                if (field3UpdateCallbackToCallbackKey == null)
                {
                    field3UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field3.HasValue)
                    {
                        value(update.Field3.Value);
                    }
                });
                field3UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field3UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field3UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>>, ulong> field4UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>> OnField4Update
        {
            add
            {
                if (field4UpdateCallbackToCallbackKey == null)
                {
                    field4UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field4.HasValue)
                    {
                        value(update.Field4.Value);
                    }
                });
                field4UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field4UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field4UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>>, ulong> field5UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>> OnField5Update
        {
            add
            {
                if (field5UpdateCallbackToCallbackKey == null)
                {
                    field5UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field5.HasValue)
                    {
                        value(update.Field5.Value);
                    }
                });
                field5UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field5UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field5UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        internal ExhaustiveEntityReader(World world, Entity entity, EntityId entityId)
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

            if (field1UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field1UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field1UpdateCallbackToCallbackKey.Clear();
            }

            if (field2UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field2UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field2UpdateCallbackToCallbackKey.Clear();
            }

            if (field3UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field3UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field3UpdateCallbackToCallbackKey.Clear();
            }

            if (field4UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field4UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field4UpdateCallbackToCallbackKey.Clear();
            }

            if (field5UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field5UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field5UpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class ExhaustiveEntityWriter : ExhaustiveEntityReader
    {
        internal ExhaustiveEntityWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(ExhaustiveEntity.Update update)
        {
            var component = EntityManager.GetComponentData<ExhaustiveEntity.Component>(Entity);

            if (update.Field1.HasValue)
            {
                component.Field1 = update.Field1.Value;
            }

            if (update.Field2.HasValue)
            {
                component.Field2 = update.Field2.Value;
            }

            if (update.Field3.HasValue)
            {
                component.Field3 = update.Field3.Value;
            }

            if (update.Field4.HasValue)
            {
                component.Field4 = update.Field4.Value;
            }

            if (update.Field5.HasValue)
            {
                component.Field5 = update.Field5.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, ExhaustiveEntity.ComponentId);
        }
    }
}
