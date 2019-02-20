using System;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface ICommandSendStorage
    {
        uint GetComponentId();
        uint GetCommandId();

        Type GetRequestType();
        Type GetResponseType();

        void Clear();
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
}
