using System;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public abstract class CommandSenderBase : IRequireable
    {
        private readonly Entity entity;
        private readonly CommandSystem commandSystem;
        private readonly CommandCallbackSystem callbackSystem;
        private int callbackEpoch;

        public bool IsValid { get; set; }

        protected CommandSenderBase(Entity entity, World world)
        {
            this.entity = entity;
            commandSystem = world.GetOrCreateSystem<CommandSystem>();
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            IsValid = true;
        }

        protected void SendCommand<TRequest, TResponse>(TRequest request, Action<TResponse> callback = null)
            where TRequest : struct, ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            var validCallbackEpoch = callbackEpoch;
            var requestId = commandSystem.SendCommand(request, entity);

            if (callback != null)
            {
                Action<TResponse> wrappedCallback = response =>
                {
                    if (!IsValid || validCallbackEpoch != callbackEpoch)
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
}
