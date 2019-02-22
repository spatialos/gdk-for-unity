using System;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface ICommandSendStorage
    {
        void Clear();
    }

    public interface IComponentCommandSendStorage : ICommandSendStorage
    {
        uint GetComponentId();
        uint GetCommandId();

        Type GetRequestType();
        Type GetResponseType();
    }

    public interface ICommandRequestSendStorage<T> : ICommandSendStorage
        where T : ICommandRequest
    {
        void AddRequest(T request, Entity sendingEntity, long requestId);
    }

    public interface ICommandResponseSendStorage<T> : ICommandSendStorage
        where T : ICommandResponse
    {
        void AddResponse(T response);
    }

    internal struct CommandRequestWithMetaData<T>
    {
        public T Request;
        public Entity SendingEntity;
        public long RequestId;

        public CommandRequestWithMetaData(T request, Entity sendingEntity, long requestId)
        {
            Request = request;
            SendingEntity = sendingEntity;
            RequestId = requestId;
        }
    }
}
