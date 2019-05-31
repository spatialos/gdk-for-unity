using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;
using Improbable.Gdk.Subscriptions;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    [AutoRegisterSubscriptionManager]
    public class WorldCommandSenderSubscriptionManager : SubscriptionManager<WorldCommandSender>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<WorldCommandSender>>>
            entityIdToSenderSubscriptions =
                new Dictionary<EntityId, HashSet<Subscription<WorldCommandSender>>>();

        public WorldCommandSenderSubscriptionManager(World world)
        {
            this.world = world;

            // Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            var constraintsSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintsSystem.RegisterEntityAddedCallback(entityId =>
            {
                if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);
                foreach (var subscription in subscriptions)
                {
                    subscription.SetAvailable(new WorldCommandSender(entity, world));
                }
            });

            constraintsSystem.RegisterEntityAddedCallback(entityId =>
            {
                if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
                {
                    return;
                }

                foreach (var subscription in subscriptions)
                {
                    ResetValue(subscription);
                    subscription.SetUnavailable();
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
            }

            if (workerSystem.TryGetEntity(entityId, out var entity))
            {
                subscription.SetAvailable(new WorldCommandSender(entity, world));
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<WorldCommandSender>) subscription);
            if (sub.HasValue)
            {
                sub.Value.IsValid = false;
            }

            var subscriptions = entityIdToSenderSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToSenderSubscriptions.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
        }
    }

    public class WorldCommandSender
    {
        public bool IsValid;

        private readonly Entity entity;
        private readonly CommandSystem commandSystem;
        private readonly CommandCallbackSystem callbackSystem;

        public WorldCommandSender(Entity entity, World world)
        {
            this.entity = entity;
            IsValid = true;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            // todo check if this exists probably put getting this in a static function that does it in one place
            commandSystem = world.GetExistingSystem<CommandSystem>();
        }

        public void SendCreateEntityCommand(WorldCommands.CreateEntity.Request request,
            Action<WorldCommands.CreateEntity.ReceivedResponse> callback = null)
        {
            var requestId = commandSystem.SendCommand(request, entity);
            if (callback == null)
            {
                return;
            }

            Action<WorldCommands.CreateEntity.ReceivedResponse> wrappedCallback = response =>
            {
                if (IsValid)
                {
                    callback(response);
                }
            };
            callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
        }

        public void SendDeleteEntityCommand(WorldCommands.DeleteEntity.Request request,
            Action<WorldCommands.DeleteEntity.ReceivedResponse> callback = null)
        {
            var requestId = commandSystem.SendCommand(request, entity);
            if (callback == null)
            {
                return;
            }

            Action<WorldCommands.DeleteEntity.ReceivedResponse> wrappedCallback = response =>
            {
                if (IsValid)
                {
                    callback(response);
                }
            };
            callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
        }

        public void SendReserveEntityIdsCommand(WorldCommands.ReserveEntityIds.Request request,
            Action<WorldCommands.ReserveEntityIds.ReceivedResponse> callback = null)
        {
            var requestId = commandSystem.SendCommand(request, entity);
            if (callback == null)
            {
                return;
            }

            Action<WorldCommands.ReserveEntityIds.ReceivedResponse> wrappedCallback = response =>
            {
                if (IsValid)
                {
                    callback(response);
                }
            };
            callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
        }

        public void SendEntityQueryCommand(WorldCommands.EntityQuery.Request request,
            Action<WorldCommands.EntityQuery.ReceivedResponse> callback = null)
        {
            var requestId = commandSystem.SendCommand(request, entity);
            if (callback == null)
            {
                return;
            }

            Action<WorldCommands.EntityQuery.ReceivedResponse> wrappedCallback = response =>
            {
                if (IsValid)
                {
                    callback(response);
                }
            };
            callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
        }
    }
}
