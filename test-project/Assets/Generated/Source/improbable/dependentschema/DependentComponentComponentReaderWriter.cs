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

namespace Improbable.DependentSchema
{
    [AutoRegisterSubscriptionManager]
    public class DependentComponentReaderSubscriptionManager : SubscriptionManager<DependentComponentReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<DependentComponentReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DependentComponentReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.EntityManager;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(DependentComponent.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new DependentComponentReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(DependentComponent.ComponentId, entityId =>
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

        public override Subscription<DependentComponentReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DependentComponentReader>>>();
            }

            var subscription = new Subscription<DependentComponentReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DependentComponentReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<DependentComponent.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DependentComponentReader(world, entity, entityId));
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
            var sub = ((Subscription<DependentComponentReader>) subscription);
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
            var sub = ((Subscription<DependentComponentReader>) subscription);
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
    public class DependentComponentWriterSubscriptionManager : SubscriptionManager<DependentComponentWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<DependentComponentWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DependentComponentWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(DependentComponent.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new DependentComponentWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<DependentComponentWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DependentComponentWriter>>>();
            }

            var subscription = new Subscription<DependentComponentWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DependentComponentWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(DependentComponent.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, DependentComponent.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DependentComponentWriter(world, entity, entityId));
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
            var sub = ((Subscription<DependentComponentWriter>) subscription);
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
            var sub = ((Subscription<DependentComponentWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class DependentComponentReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public DependentComponent.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<DependentComponent.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, DependentComponent.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, DependentComponent.ComponentId, value);
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

        private Dictionary<Action<DependentComponent.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<DependentComponent.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<DependentComponent.Update>, ulong>();
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

        private Dictionary<Action<global::Improbable.TestSchema.ExhaustiveRepeatedData>, ulong> aUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.TestSchema.ExhaustiveRepeatedData> OnAUpdate
        {
            add
            {
                if (aUpdateCallbackToCallbackKey == null)
                {
                    aUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.ExhaustiveRepeatedData>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
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

        private Dictionary<Action<global::Improbable.TestSchema.SomeEnum>, ulong> bUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.TestSchema.SomeEnum> OnBUpdate
        {
            add
            {
                if (bUpdateCallbackToCallbackKey == null)
                {
                    bUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.SomeEnum>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
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

        private Dictionary<Action<global::Improbable.TestSchema.SomeEnum?>, ulong> cUpdateCallbackToCallbackKey;
        public event Action<global::Improbable.TestSchema.SomeEnum?> OnCUpdate
        {
            add
            {
                if (cUpdateCallbackToCallbackKey == null)
                {
                    cUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.SomeEnum?>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
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

        private Dictionary<Action<global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>>, ulong> dUpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>> OnDUpdate
        {
            add
            {
                if (dUpdateCallbackToCallbackKey == null)
                {
                    dUpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
                {
                    if (update.D.HasValue)
                    {
                        value(update.D.Value);
                    }
                });
                dUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!dUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                dUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>>, ulong> eUpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>> OnEUpdate
        {
            add
            {
                if (eUpdateCallbackToCallbackKey == null)
                {
                    eUpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
                {
                    if (update.E.HasValue)
                    {
                        value(update.E.Value);
                    }
                });
                eUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!eUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                eUpdateCallbackToCallbackKey.Remove(value);
            }
        }


        internal DependentComponentReader(World world, Entity entity, EntityId entityId)
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

            if (dUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in dUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                dUpdateCallbackToCallbackKey.Clear();
            }

            if (eUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in eUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                eUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class DependentComponentWriter : DependentComponentReader
    {
        internal DependentComponentWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(DependentComponent.Update update)
        {
            var component = EntityManager.GetComponentData<DependentComponent.Component>(Entity);

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

            if (update.D.HasValue)
            {
                component.D = update.D.Value;
            }

            if (update.E.HasValue)
            {
                component.E = update.E.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }


        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, DependentComponent.ComponentId);
        }
    }
}
