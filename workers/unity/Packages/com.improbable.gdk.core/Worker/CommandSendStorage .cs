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

    internal readonly struct CommandRequestWithMetaData<T>
    {
        public readonly T Request;
        public readonly Entity SendingEntity;
        public readonly long RequestId;

        public CommandRequestWithMetaData(T request, Entity sendingEntity, long requestId)
        {
            Request = request;
            SendingEntity = sendingEntity;
            RequestId = requestId;
        }
    }
}
