using System;
using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandSendStorage
    {
        void Clear();
    }

    public interface IComponentCommandSendStorage : ICommandSendStorage
    {
        uint ComponentId { get; }
        uint CommandId { get; }

        Type RequestType { get; }
        Type ResponseType { get; }
    }

    public interface ICommandRequestSendStorage<T> : ICommandSendStorage
        where T : ICommandRequest
    {
        void AddRequest(T request, Entity sendingEntity, CommandRequestId requestId);
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
        public readonly CommandRequestId RequestId;

        public CommandRequestWithMetaData(T request, Entity sendingEntity, CommandRequestId requestId)
        {
            Request = request;
            SendingEntity = sendingEntity;
            RequestId = requestId;
        }
    }
}
