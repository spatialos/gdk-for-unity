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

namespace Improbable.Gdk.Tests
{
    [AutoRegisterSubscriptionManager]
    public class ExhaustiveMapKeyReaderSubscriptionManager : SubscriptionManager<ExhaustiveMapKeyReader>
    {
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<ExhaustiveMapKeyReader>>> entityIdToReaderSubscriptions;

        private Dictionary<EntityId, (ulong Added, ulong Removed)> entityIdToCallbackKey =
            new Dictionary<EntityId, (ulong Added, ulong Removed)>();

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public ExhaustiveMapKeyReaderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.EntityManager;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(ExhaustiveMapKey.ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(new ExhaustiveMapKeyReader(world, entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(ExhaustiveMapKey.ComponentId, entityId =>
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

        public override Subscription<ExhaustiveMapKeyReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ExhaustiveMapKeyReader>>>();
            }

            var subscription = new Subscription<ExhaustiveMapKeyReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ExhaustiveMapKeyReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<ExhaustiveMapKey.Component>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new ExhaustiveMapKeyReader(world, entity, entityId));
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
            var sub = ((Subscription<ExhaustiveMapKeyReader>) subscription);
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
            var sub = ((Subscription<ExhaustiveMapKeyReader>) subscription);
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
    public class ExhaustiveMapKeyWriterSubscriptionManager : SubscriptionManager<ExhaustiveMapKeyWriter>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<ExhaustiveMapKeyWriter>>> entityIdToWriterSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public ExhaustiveMapKeyWriterSubscriptionManager(World world)
        {
            this.world = world;

            // todo Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(ExhaustiveMapKey.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new ExhaustiveMapKeyWriter(world, entity, authorityChange.EntityId));
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

        public override Subscription<ExhaustiveMapKeyWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ExhaustiveMapKeyWriter>>>();
            }

            var subscription = new Subscription<ExhaustiveMapKeyWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ExhaustiveMapKeyWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(ExhaustiveMapKey.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, ExhaustiveMapKey.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new ExhaustiveMapKeyWriter(world, entity, entityId));
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
            var sub = ((Subscription<ExhaustiveMapKeyWriter>) subscription);
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
            var sub = ((Subscription<ExhaustiveMapKeyWriter>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class ExhaustiveMapKeyReader
    {
        public bool IsValid;

        protected readonly ComponentUpdateSystem ComponentUpdateSystem;
        protected readonly ComponentCallbackSystem CallbackSystem;
        protected readonly EntityManager EntityManager;
        protected readonly Entity Entity;
        protected readonly EntityId EntityId;

        public ExhaustiveMapKey.Component Data
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Oh noes!");
                }

                return EntityManager.GetComponentData<ExhaustiveMapKey.Component>(Entity);
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

                return ComponentUpdateSystem.GetAuthority(EntityId, ExhaustiveMapKey.ComponentId);
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

                var key = CallbackSystem.RegisterAuthorityCallback(EntityId, ExhaustiveMapKey.ComponentId, value);
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

        private Dictionary<Action<ExhaustiveMapKey.Update>, ulong> updateCallbackToCallbackKey;
        public event Action<ExhaustiveMapKey.Update> OnUpdate
        {
            add
            {
                if (updateCallbackToCallbackKey == null)
                {
                    updateCallbackToCallbackKey = new Dictionary<Action<ExhaustiveMapKey.Update>, ulong>();
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

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<bool,string>>, ulong> field1UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<bool,string>> OnField1Update
        {
            add
            {
                if (field1UpdateCallbackToCallbackKey == null)
                {
                    field1UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<bool,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
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

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<float,string>>, ulong> field2UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<float,string>> OnField2Update
        {
            add
            {
                if (field2UpdateCallbackToCallbackKey == null)
                {
                    field2UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<float,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
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

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<byte[],string>>, ulong> field3UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<byte[],string>> OnField3Update
        {
            add
            {
                if (field3UpdateCallbackToCallbackKey == null)
                {
                    field3UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<byte[],string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
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

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<int,string>>, ulong> field4UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<int,string>> OnField4Update
        {
            add
            {
                if (field4UpdateCallbackToCallbackKey == null)
                {
                    field4UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<int,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
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

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<long,string>>, ulong> field5UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<long,string>> OnField5Update
        {
            add
            {
                if (field5UpdateCallbackToCallbackKey == null)
                {
                    field5UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<long,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
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

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<double,string>>, ulong> field6UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<double,string>> OnField6Update
        {
            add
            {
                if (field6UpdateCallbackToCallbackKey == null)
                {
                    field6UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<double,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field6.HasValue)
                    {
                        value(update.Field6.Value);
                    }
                });
                field6UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field6UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field6UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<string,string>>, ulong> field7UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<string,string>> OnField7Update
        {
            add
            {
                if (field7UpdateCallbackToCallbackKey == null)
                {
                    field7UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<string,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field7.HasValue)
                    {
                        value(update.Field7.Value);
                    }
                });
                field7UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field7UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field7UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<uint,string>>, ulong> field8UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<uint,string>> OnField8Update
        {
            add
            {
                if (field8UpdateCallbackToCallbackKey == null)
                {
                    field8UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<uint,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field8.HasValue)
                    {
                        value(update.Field8.Value);
                    }
                });
                field8UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field8UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field8UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<ulong,string>>, ulong> field9UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<ulong,string>> OnField9Update
        {
            add
            {
                if (field9UpdateCallbackToCallbackKey == null)
                {
                    field9UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<ulong,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field9.HasValue)
                    {
                        value(update.Field9.Value);
                    }
                });
                field9UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field9UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field9UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<int,string>>, ulong> field10UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<int,string>> OnField10Update
        {
            add
            {
                if (field10UpdateCallbackToCallbackKey == null)
                {
                    field10UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<int,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field10.HasValue)
                    {
                        value(update.Field10.Value);
                    }
                });
                field10UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field10UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field10UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<long,string>>, ulong> field11UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<long,string>> OnField11Update
        {
            add
            {
                if (field11UpdateCallbackToCallbackKey == null)
                {
                    field11UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<long,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field11.HasValue)
                    {
                        value(update.Field11.Value);
                    }
                });
                field11UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field11UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field11UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<uint,string>>, ulong> field12UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<uint,string>> OnField12Update
        {
            add
            {
                if (field12UpdateCallbackToCallbackKey == null)
                {
                    field12UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<uint,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field12.HasValue)
                    {
                        value(update.Field12.Value);
                    }
                });
                field12UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field12UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field12UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<ulong,string>>, ulong> field13UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<ulong,string>> OnField13Update
        {
            add
            {
                if (field13UpdateCallbackToCallbackKey == null)
                {
                    field13UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<ulong,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field13.HasValue)
                    {
                        value(update.Field13.Value);
                    }
                });
                field13UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field13UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field13UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<int,string>>, ulong> field14UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<int,string>> OnField14Update
        {
            add
            {
                if (field14UpdateCallbackToCallbackKey == null)
                {
                    field14UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<int,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field14.HasValue)
                    {
                        value(update.Field14.Value);
                    }
                });
                field14UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field14UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field14UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<long,string>>, ulong> field15UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<long,string>> OnField15Update
        {
            add
            {
                if (field15UpdateCallbackToCallbackKey == null)
                {
                    field15UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<long,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field15.HasValue)
                    {
                        value(update.Field15.Value);
                    }
                });
                field15UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field15UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field15UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>>, ulong> field16UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>> OnField16Update
        {
            add
            {
                if (field16UpdateCallbackToCallbackKey == null)
                {
                    field16UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field16.HasValue)
                    {
                        value(update.Field16.Value);
                    }
                });
                field16UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field16UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field16UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>>, ulong> field17UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>> OnField17Update
        {
            add
            {
                if (field17UpdateCallbackToCallbackKey == null)
                {
                    field17UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field17.HasValue)
                    {
                        value(update.Field17.Value);
                    }
                });
                field17UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field17UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field17UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>>, ulong> field18UpdateCallbackToCallbackKey;
        public event Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>> OnField18Update
        {
            add
            {
                if (field18UpdateCallbackToCallbackKey == null)
                {
                    field18UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveMapKey.Update>(EntityId, update =>
                {
                    if (update.Field18.HasValue)
                    {
                        value(update.Field18.Value);
                    }
                });
                field18UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field18UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field18UpdateCallbackToCallbackKey.Remove(value);
            }
        }


        internal ExhaustiveMapKeyReader(World world, Entity entity, EntityId entityId)
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

            if (field6UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field6UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field6UpdateCallbackToCallbackKey.Clear();
            }

            if (field7UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field7UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field7UpdateCallbackToCallbackKey.Clear();
            }

            if (field8UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field8UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field8UpdateCallbackToCallbackKey.Clear();
            }

            if (field9UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field9UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field9UpdateCallbackToCallbackKey.Clear();
            }

            if (field10UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field10UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field10UpdateCallbackToCallbackKey.Clear();
            }

            if (field11UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field11UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field11UpdateCallbackToCallbackKey.Clear();
            }

            if (field12UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field12UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field12UpdateCallbackToCallbackKey.Clear();
            }

            if (field13UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field13UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field13UpdateCallbackToCallbackKey.Clear();
            }

            if (field14UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field14UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field14UpdateCallbackToCallbackKey.Clear();
            }

            if (field15UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field15UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field15UpdateCallbackToCallbackKey.Clear();
            }

            if (field16UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field16UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field16UpdateCallbackToCallbackKey.Clear();
            }

            if (field17UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field17UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field17UpdateCallbackToCallbackKey.Clear();
            }

            if (field18UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field18UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field18UpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class ExhaustiveMapKeyWriter : ExhaustiveMapKeyReader
    {
        internal ExhaustiveMapKeyWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(ExhaustiveMapKey.Update update)
        {
            var component = EntityManager.GetComponentData<ExhaustiveMapKey.Component>(Entity);

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

            if (update.Field6.HasValue)
            {
                component.Field6 = update.Field6.Value;
            }

            if (update.Field7.HasValue)
            {
                component.Field7 = update.Field7.Value;
            }

            if (update.Field8.HasValue)
            {
                component.Field8 = update.Field8.Value;
            }

            if (update.Field9.HasValue)
            {
                component.Field9 = update.Field9.Value;
            }

            if (update.Field10.HasValue)
            {
                component.Field10 = update.Field10.Value;
            }

            if (update.Field11.HasValue)
            {
                component.Field11 = update.Field11.Value;
            }

            if (update.Field12.HasValue)
            {
                component.Field12 = update.Field12.Value;
            }

            if (update.Field13.HasValue)
            {
                component.Field13 = update.Field13.Value;
            }

            if (update.Field14.HasValue)
            {
                component.Field14 = update.Field14.Value;
            }

            if (update.Field15.HasValue)
            {
                component.Field15 = update.Field15.Value;
            }

            if (update.Field16.HasValue)
            {
                component.Field16 = update.Field16.Value;
            }

            if (update.Field17.HasValue)
            {
                component.Field17 = update.Field17.Value;
            }

            if (update.Field18.HasValue)
            {
                component.Field18 = update.Field18.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }


        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, ExhaustiveMapKey.ComponentId);
        }
    }
}
