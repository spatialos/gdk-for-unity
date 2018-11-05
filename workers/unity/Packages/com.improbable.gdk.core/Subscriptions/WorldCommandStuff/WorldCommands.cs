using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker;
using Improbable.Worker.Core;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    [AutoRegisterSubscriptionManager]
    public class WorldCommandSenderSubscriptionManager : SubscriptionManager<WorldCommandSender>
    {
        private readonly Dispatcher dispatcher;
        private readonly EntityManager entityManager;
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<WorldCommandSender>>>
            entityIdToSenderSubscriptions =
                new Dictionary<EntityId, HashSet<Subscription<WorldCommandSender>>>();

        public WorldCommandSenderSubscriptionManager(World world)
        {
            this.world = world;
            entityManager = world.GetOrCreateManager<EntityManager>();

            // Check that these are there
            dispatcher = world.GetExistingManager<SpatialOSReceiveSystem>().Dispatcher;
            workerSystem = world.GetExistingManager<WorkerSystem>();

            dispatcher.OnAddEntity(op =>
            {
                if (!entityIdToSenderSubscriptions.TryGetValue(op.EntityId, out var subscriptions))
                {
                    return;
                }

                foreach (var subscription in subscriptions)
                {
                    subscription.SetUnavailable();
                }
            });

            dispatcher.OnRemoveEntity(op =>
            {
                if (!entityIdToSenderSubscriptions.TryGetValue(op.EntityId, out var subscriptions))
                {
                    return;
                }

                workerSystem.TryGetEntity(op.EntityId, out var entity);
                foreach (var subscription in subscriptions)
                {
                    subscription.SetAvailable(new WorldCommandSender(entity, world));
                }
            });
        }

        public override Subscription<WorldCommandSender> Subscribe(EntityId entityId)
        {
            if (entityIdToSenderSubscriptions == null)
            {
                entityIdToSenderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<WorldCommandSender>>>();
            }

            var subscription = new Subscription<WorldCommandSender>(this, entityId);

            if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<WorldCommandSender>>();
                entityIdToSenderSubscriptions.Add(entityId, subscriptions);

                if (workerSystem.TryGetEntity(entityId, out var entity))
                {
                    subscription.SetAvailable(new WorldCommandSender(entity, world));
                }
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(EntityId entityId, ITypeErasedSubscription subscription)
        {
            var sub = ((Subscription<WorldCommandSender>) subscription);
            // var reader = sub.Value;
            // reader.IsValid = false;
            // reader.RemoveAllCallbacks();

            var subscriptions = entityIdToSenderSubscriptions[entityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToSenderSubscriptions.Remove(entityId);
            }
        }

        public override void Invalidate(EntityId entityId, ITypeErasedSubscription subscription)
        {
            var sub = ((Subscription<WorldCommandSender>) subscription);
            if (sub.HasValue)
            {
                // var reader = sub.Value;
                // reader.IsValid = false;
                // reader.RemoveAllCallbacks();
            }
        }

        public override void Restore(EntityId entityId, ITypeErasedSubscription subscription)
        {
            var sub = ((Subscription<WorldCommandSender>) subscription);
            if (sub.HasValue)
            {
                //sub.Value.IsValid = true;
            }
        }
    }

    public class WorldCommandSender
    {
        private readonly Entity entity;
        private readonly EntityManager entityManager;

        public WorldCommandSender(Entity entity, World world)
        {
            this.entity = entity;
            entityManager = world.GetOrCreateManager<EntityManager>();
        }

        public void SendCreateEntityCommand(WorldCommands.CreateEntity.Request request)
        {
            var commandSender = entityManager.GetComponentData<WorldCommands.CreateEntity.CommandSender>(entity);
            commandSender.RequestsToSend.Add(request);
        }

        public void SendDeleteEntityCommand(WorldCommands.DeleteEntity.Request request)
        {
            var commandSender = entityManager.GetComponentData<WorldCommands.DeleteEntity.CommandSender>(entity);
            commandSender.RequestsToSend.Add(request);
        }

        public void SendReserveEntityIdsCommand(WorldCommands.ReserveEntityIds.Request request)
        {
            var commandSender = entityManager.GetComponentData<WorldCommands.ReserveEntityIds.CommandSender>(entity);
            commandSender.RequestsToSend.Add(request);
        }

        public void SendEntityQueryCommand(WorldCommands.EntityQuery.Request request)
        {
            var commandSender = entityManager.GetComponentData<WorldCommands.EntityQuery.CommandSender>(entity);
            commandSender.RequestsToSend.Add(request);
        }
    }
}
