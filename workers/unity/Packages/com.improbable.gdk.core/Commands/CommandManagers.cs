using System;
using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandManager
    {
        void SendAll();
        void Init(World world);
    }

    public interface IWorldCommandManager : ICommandManager
    {
        Type GetRequestType();
        Type GetReceivedResponseType();
    }

    public interface IComponentCommandManager : ICommandManager
    {
        Type GetRequestType();
        Type GetReceivedRequestType();
        Type GetResponseType();
        Type GetReceivedResponseType();
    }

    public interface ICommandRequestSender<TRequest> where TRequest : ICommandRequest
    {
        long SendCommand(TRequest request, Entity entity);
        List<(TRequest Request, long Id)> GetRequestsToSend();
    }

    public interface ICommandResponseSender<TResponse> where TResponse : ICommandResponse
    {
        void SendResponse(TResponse response);
        List<TResponse> GetResponsesToSend();
    }

    public interface ICommandRequestReceiver<TReceivedRequest> where TReceivedRequest : IReceivedCommandRequest
    {
        List<TReceivedRequest> GetRequestsReceived();
        List<TReceivedRequest> GetRequestsReceivedForEntityId(EntityId entity);
    }

    public interface ICommandResponseReceiver<TReceivedResponse> where TReceivedResponse : IReceivedCommandResponse
    {
        List<TReceivedResponse> GetResponsesReceived();
        List<TReceivedResponse> GetResponsesReceivedForEntity(Entity entity);
        bool TryGetResponseReceivedForRequestId(long requestId, out TReceivedResponse response);
    }

    // All interfaces needed by component command managers
    public interface IComponentCommandManager<TRequest, TResponse, TReceivedRequest, TReceivedResponse> :
        IComponentCommandManager, ICommandRequestSender<TRequest>, ICommandRequestReceiver<TReceivedRequest>,
        ICommandResponseSender<TResponse>, ICommandResponseReceiver<TReceivedResponse>
        where TRequest : ICommandRequest
        where TReceivedRequest : IReceivedCommandRequest
        where TResponse : ICommandResponse
        where TReceivedResponse : IReceivedCommandResponse
    {
    }

    // All interfaces needed by world command managers
    public interface IWorldCommandManager<TRequest, TReceivedResponse> :
        IWorldCommandManager, ICommandRequestSender<TRequest>, ICommandResponseReceiver<TReceivedResponse>
        where TRequest : ICommandRequest
        where TReceivedResponse : IReceivedCommandResponse
    {
    }
}
