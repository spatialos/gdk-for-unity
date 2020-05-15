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
        private readonly ComponentUpdateSystem componentUpdateSystem;
        private readonly EntityManager entityManager;
        private Dictionary<EntityId, HashSet<Subscription<TWriter>>> entityIdToWriterSubscriptions;

        private readonly HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private readonly HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        private static readonly uint ComponentId = ComponentDatabase.GetComponentId<TComponent>();
        private static readonly ComponentType ComponentType = ComponentDatabase.Metaclasses[ComponentId].Data;
        private static readonly ComponentType ComponentAuthType = ComponentDatabase.Metaclasses[ComponentId].Authority;

        protected WriterSubscriptionManager(World world) : base(world)
        {
            entityManager = World.EntityManager;
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            RegisterComponentCallbacks();
        }

        protected abstract TWriter CreateWriter(Entity entity, EntityId entityId);

        private void RegisterComponentCallbacks()
        {
            var constraintCallbackSystem = World.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterAuthorityCallback(ComponentId, authorityChange =>
            {
                if (authorityChange.Authority == Authority.Authoritative)
                {
                    if (!entitiesNotMatchingRequirements.Contains(authorityChange.EntityId))
                    {
                        return;
                    }

                    var entity = WorkerSystem.GetEntity(authorityChange.EntityId);

                    foreach (var subscription in entityIdToWriterSubscriptions[authorityChange.EntityId])
                    {
                        subscription.SetAvailable(CreateWriter(entity, authorityChange.EntityId));
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

        public override Subscription<TWriter> Subscribe(EntityId entityId)
        {
            if (entityIdToWriterSubscriptions == null)
            {
                entityIdToWriterSubscriptions = new Dictionary<EntityId, HashSet<Subscription<TWriter>>>();
            }

            var subscription = new Subscription<TWriter>(this, entityId);

            if (!entityIdToWriterSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<TWriter>>();
                entityIdToWriterSubscriptions.Add(entityId, subscriptions);
            }

            if (WorkerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent(entity, ComponentType)
                && entityManager.HasComponent(entity, ComponentAuthType))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(CreateWriter(entity, entityId));
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
            var sub = (Subscription<TWriter>) subscription;
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
            var sub = (Subscription<TWriter>) subscription;
            if (sub.HasValue)
            {
                var reader = sub.Value;
                reader.IsValid = false;
                reader.RemoveAllCallbacks();
            }
        }
    }
}
