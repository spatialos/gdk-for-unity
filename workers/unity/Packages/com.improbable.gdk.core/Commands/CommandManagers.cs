using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandManager
    {
        void SendAll();
        void Init(World world);

        Type GetRequestType();
        Type GetReceivedRequestType();
        Type GetResponseType();
        Type GetReceivedResponseType();
    }

    public interface ICommandRequestSender<TRequest> : ICommandManager where TRequest : ICommandRequest
    {
        long SendCommand(TRequest request, Entity entity);
        List<(TRequest Request, long Id)> GetRequestsToSend();
    }

    public interface ICommandResponseSender<TResponse> : ICommandManager where TResponse : ICommandResponse
    {
        void SendResponse(TResponse response);
        List<TResponse> GetResponsesToSend();
    }

    public interface ICommandRequestReceiver<TReceivedRequest> : ICommandManager
        where TReceivedRequest : IReceivedCommandRequest
    {
        List<TReceivedRequest> GetRequestsReceived();
        List<TReceivedRequest> GetRequestsReceivedForEntityId(EntityId entity);
    }

    public interface ICommandResponseReceiver<TReceivedResponse> : ICommandManager
        where TReceivedResponse : IReceivedCommandResponse
    {
        List<TReceivedResponse> GetResponsesReceived();
        List<TReceivedResponse> GetResponsesReceivedForEntity(Entity entity);
        bool TryGetResponseReceivedForRequestId(long requestId, out TReceivedResponse response);
    }
}
