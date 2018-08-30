using System;
using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public abstract class CommandRequestWrapper<
        TRequest,
        TResponsePayload,
        TComponentData
    > where TComponentData : struct, IComponentData
    {
        private bool responseSent = false;
        private readonly EntityManager entityManager;
        private readonly Entity entity;
        public TRequest Request { get; }

        internal CommandRequestWrapper(EntityManager entityManager, Entity entity, TRequest request)
        {
            this.entity = entity;
            this.entityManager = entityManager;
            Request = request;
        }

        private TComponentData GetComponentData()
        {
            return entityManager.GetComponentData<TComponentData>(entity);
        }

        public void SendResponse(TResponsePayload response)
        {
            if (responseSent)
            {
                throw new CommandResponseSendFailure("This command request was already responded to.");
            }

            SendResponseInternal(GetComponentData(), response);

            responseSent = true;
        }

        public void SendResponseFailure(string message)
        {
            if (responseSent)
            {
                throw new CommandResponseSendFailure("This command request was already responded to.");
            }

            SendResponseFailureInternal(GetComponentData(), message);

            responseSent = true;
        }

        protected abstract void SendResponseInternal(TComponentData componentData, TResponsePayload response);
        protected abstract void SendResponseFailureInternal(TComponentData componentData, string message);
    }

    public class CommandResponseSendFailure : Exception
    {
        public CommandResponseSendFailure(string message) : base(message)
        {
        }
    }
}
