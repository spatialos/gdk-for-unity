// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Entity = Unity.Entities.Entity;

namespace Improbable.DependentSchema
{
    [AutoRegisterSubscriptionManager]
    public class DependentDataComponentCommandSenderSubscriptionManager : CommandSenderSubscriptionManagerBase<DependentDataComponentCommandSender>
    {
        public DependentDataComponentCommandSenderSubscriptionManager(World world) : base(world)
        {
        }

        protected override DependentDataComponentCommandSender CreateSender(Entity entity, World world)
        {
            return new DependentDataComponentCommandSender(entity, world);
        }
    }

    [AutoRegisterSubscriptionManager]
    public class DependentDataComponentCommandReceiverSubscriptionManager : CommandReceiverSubscriptionManagerBase<DependentDataComponentCommandReceiver>
    {
        public DependentDataComponentCommandReceiverSubscriptionManager(World world) : base(world, DependentDataComponent.ComponentId)
        {
        }

        protected override DependentDataComponentCommandReceiver CreateReceiver(World world, Entity entity, EntityId entityId)
        {
            return new DependentDataComponentCommandReceiver(world, entity, entityId);
        }
    }

    public class DependentDataComponentCommandSender : ICommandSender
    {
        private readonly Entity entity;
        private readonly CommandSystem commandSender;
        private readonly CommandCallbackSystem callbackSystem;
        private int callbackEpoch;

        public bool IsValid { get; set; }

        internal DependentDataComponentCommandSender(Entity entity, World world)
        {
            this.entity = entity;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            // todo check that this exists
            commandSender = world.GetExistingSystem<CommandSystem>();

            IsValid = true;
        }

        public void SendBarCommandCommand(EntityId targetEntityId, global::Improbable.TestSchema.SomeType request, Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedResponse> callback = null)
        {
            var commandRequest = new DependentDataComponent.BarCommand.Request(targetEntityId, request);
            SendBarCommandCommand(commandRequest, callback);
        }

        public void SendBarCommandCommand(global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Request request, Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedResponse> wrappedCallback = response =>
                {
                    if (!this.IsValid || validCallbackEpoch != this.callbackEpoch)
                    {
                        return;
                    }

                    callback(response);
                };
                callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
            }
        }

        public void RemoveAllCallbacks()
        {
            ++callbackEpoch;
        }
    }

    public class DependentDataComponentCommandReceiver : ICommandReceiver
    {
        private readonly EntityId entityId;
        private readonly CommandCallbackSystem callbackSystem;
        private readonly CommandSystem commandSystem;

        public bool IsValid { get; set; }

        private Dictionary<Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedRequest>, ulong> barCommandCallbackToCallbackKey;

        public event Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedRequest> OnBarCommandRequestReceived
        {
            add
            {
                if (barCommandCallbackToCallbackKey == null)
                {
                    barCommandCallbackToCallbackKey = new Dictionary<Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                barCommandCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!barCommandCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                barCommandCallbackToCallbackKey.Remove(value);
            }
        }

        internal DependentDataComponentCommandReceiver(World world, Entity entity, EntityId entityId)
        {
            this.entityId = entityId;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            commandSystem = world.GetExistingSystem<CommandSystem>();
            // should check the system actually exists

            IsValid = true;
        }

        public void SendBarCommandResponse(global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendBarCommandResponse(long requestId, global::Improbable.TestSchema.SomeType response)
        {
            commandSystem.SendResponse(new global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Response(requestId, response));
        }

        public void SendBarCommandFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Response(requestId, failureMessage));
        }

        public void RemoveAllCallbacks()
        {
            if (barCommandCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in barCommandCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                barCommandCallbackToCallbackKey.Clear();
            }
        }
    }
}
