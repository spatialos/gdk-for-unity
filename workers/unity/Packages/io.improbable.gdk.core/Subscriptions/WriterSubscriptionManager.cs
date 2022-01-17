using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Subscriptions
{
    public abstract class WriterSubscriptionManager<TComponent, TWriter> : SubscriptionManager<TWriter>
        where TWriter : IRequireable where TComponent : ISpatialComponentData
    {
        private static readonly uint ComponentId = ComponentDatabase.ComponentType<TComponent>.ComponentId;
        private static readonly ComponentType ComponentDataType = ComponentDatabase.ComponentType<TComponent>.Metaclass.Data;
        private static readonly ComponentType ComponentAuthType = ComponentDatabase.ComponentType<TComponent>.Metaclass.Authority;

        private readonly EntityManager entityManager;

        private readonly Dictionary<EntityId, WriterRequirements> entityRequirements =
            new Dictionary<EntityId, WriterRequirements>();

        private readonly Dictionary<EntityId, HashSet<Subscription<TWriter>>> entityIdToWriterSubscriptions =
            new Dictionary<EntityId, HashSet<Subscription<TWriter>>>();


        protected WriterSubscriptionManager(World world) : base(world)
        {
            entityManager = World.EntityManager;

            RegisterComponentCallbacks();
        }

        protected abstract TWriter CreateWriter(Entity entity, EntityId entityId);

        private void RegisterComponentCallbacks()
        {
            var constraintCallbackSystem = World.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(ComponentId, authorityChange =>
            {
                if (!entityRequirements.TryGetValue(authorityChange.EntityId, out var requirements))
                {
                    return;
                }

                var wasSatisfied = requirements.IsSatisfied;
                requirements.IsAuthoritative = authorityChange.Authority == Authority.Authoritative;

                if (requirements.IsSatisfied && !wasSatisfied)
                {
                    if (!WorkerSystem.TryGetEntity(authorityChange.EntityId, out var entity))
                    {
                        return;
                    }

                    foreach (var subscription in entityIdToWriterSubscriptions[authorityChange.EntityId])
                    {
                        subscription.SetAvailable(CreateWriter(entity, authorityChange.EntityId));
                    }
                }
                else if (wasSatisfied && !requirements.IsSatisfied)
                {
                    foreach (var subscription in entityIdToWriterSubscriptions[authorityChange.EntityId])
                    {
                        ResetValue(subscription);
                        subscription.SetUnavailable();
                    }
                }

                entityRequirements[authorityChange.EntityId] = requirements;
            });

            constraintCallbackSystem.RegisterComponentAddedCallback(ComponentId, entityId =>
            {
                if (!entityRequirements.TryGetValue(entityId, out var requirements))
                {
                    return;
                }

                var wasSatisfied = requirements.IsSatisfied;
                requirements.IsComponentPresent = true;

                if (requirements.IsSatisfied && !wasSatisfied)
                {
                    if (!WorkerSystem.TryGetEntity(entityId, out var entity))
                    {
                        return;
                    }

                    foreach (var subscription in entityIdToWriterSubscriptions[entityId])
                    {
                        subscription.SetAvailable(CreateWriter(entity, entityId));
                    }
                }

                entityRequirements[entityId] = requirements;
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(ComponentId, entityId =>
            {
                if (!entityRequirements.TryGetValue(entityId, out var requirements))
                {
                    return;
                }

                var wasSatisfied = requirements.IsSatisfied;
                requirements.IsComponentPresent = false;

                if (wasSatisfied && !requirements.IsSatisfied)
                {
                    foreach (var subscription in entityIdToWriterSubscriptions[entityId])
                    {
                        ResetValue(subscription);
                        subscription.SetUnavailable();
                    }
                }

                entityRequirements[entityId] = requirements;
            });
        }

        public override Subscription<TWriter> Subscribe(EntityId entityId)
        {
            var subscription = new Subscription<TWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<TWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            var requirements = new WriterRequirements();

            if (WorkerSystem.TryGetEntity(entityId, out var entity))
            {
                if (entityManager.HasComponent(entity, ComponentAuthType))
                {
                    requirements.IsAuthoritative = true;
                }

                if (entityManager.HasComponent(entity, ComponentDataType))
                {
                    requirements.IsComponentPresent = true;
                }
            }

            if (requirements.IsSatisfied)
            {
                subscription.SetAvailable(CreateWriter(entity, entityId));
            }

            entityRequirements[entityId] = requirements;
            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = (Subscription<TWriter>) subscription;
            ResetValue(sub);

            var subscriptions = entityIdToWriterSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);

            if (subscriptions.Count == 0)
            {
                entityIdToWriterSubscriptions.Remove(sub.EntityId);
                entityRequirements.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = (Subscription<TWriter>) subscription;

            if (!sub.HasValue)
            {
                return;
            }

            var reader = sub.Value;
            reader.IsValid = false;
            reader.RemoveAllCallbacks();
        }

        private struct WriterRequirements
        {
            public bool IsComponentPresent;
            public bool IsAuthoritative;

            public bool IsSatisfied => IsAuthoritative && IsComponentPresent;
        }
    }
}
