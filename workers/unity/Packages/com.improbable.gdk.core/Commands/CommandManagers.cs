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

    public interface ICommandRequestSender<TRequest> : ICommandManager
    {
        void SendCommand(TRequest request, Entity entity);
        List<TRequest> GetRequestsToSend();
    }

    public interface ICommandResponseSender<TResponse> : ICommandManager
    {
        void SendResponse(TResponse response, long requestId);
        List<TResponse> GetResponsesToSend();
    }

    public interface ICommandRequestReceiver<TReceivedRequest> : ICommandManager
    {
        List<TReceivedRequest> GetRequestsReceived();
        List<TReceivedRequest> GetRequestsReceivedForEntityId(EntityId entity);
    }

    public interface ICommandResponseReceiver<TReceivedResponse> : ICommandManager
    {
        List<TReceivedResponse> GetResponsesReceived();
        List<TReceivedResponse> GetResponsesReceivedForEntity(Entity entity);
        bool TryGetResponseReceivedForRequestId(long requestId, out TReceivedResponse response);
    }

    // public class CommandManager<TReceivedRequest, TRequest, TReceivedResponse, TResponse> :
    //     ICommandRequestReceiver<TReceivedRequest>,
    //     ICommandRequestSender<TRequest>,
    //     ICommandResponseSender<TResponse>,
    //     ICommandResponseReceiver<TReceivedResponse>
    // {
    //     private readonly Improbable.Worker.Core.CommandParameters shortCircuitingParameters = new CommandParameters
    //     {
    //         AllowShortCircuiting = true
    //     };
    //
    //     private WorkerSystem workerSystem;
    //     private EntityManager entityManager;
    //
    //     private List<TRequest> requestsToSend = new List<TRequest>();
    //     private List<TReceivedRequest> requestsReceived = new List<TReceivedRequest>();
    //
    //     private List<TReceivedResponse> responsesReceived = new List<TReceivedResponse>();
    //     private List<TResponse> responsesToSend = new List<TResponse>();
    //
    //     private Dictionary<long, Entity> sentRequestIdToEntity = new Dictionary<long, Entity>();
    //
    //     private Dictionary<long, long> sentWorkerRequestIdToInternalRequestId = new Dictionary<long, long>();
    //
    //     public Type GetRequestType()
    //     {
    //         return typeof(TRequest);
    //     }
    //
    //     public Type GetReceivedRequestType()
    //     {
    //         return typeof(TReceivedRequest);
    //     }
    //
    //     public Type GetResponseType()
    //     {
    //         return typeof(TResponse);
    //     }
    //
    //     public Type GetReceivedResponseType()
    //     {
    //         return typeof(TReceivedResponse);
    //     }
    //
    //     public void SendAll()
    //     {
    //         requestsToSend.Clear();
    //         requestsReceived.Clear();
    //         responsesReceived.Clear();
    //         responsesToSend.Clear();
    //     }
    //
    //     public void Init(World world)
    //     {
    //         workerSystem = world.GetExistingManager<WorkerSystem>();
    //
    //         if (workerSystem == null)
    //         {
    //             throw new ArgumentException("World instance is not running a valid SpatialOS worker");
    //         }
    //
    //         var dispatcher = world.GetExistingManager<SpatialOSReceiveSystem>().Dispatcher;
    //
    //         dispatcher.OnCommandRequest(AddRequest);
    //         dispatcher.OnCommandResponse(AddResponse);
    //     }
    //
    //     public void SendCommand(TRequest request, Entity entity)
    //     {
    //         requestsToSend.Add(request);
    //         sentRequestIdToEntity.Add(request.Id, entity);
    //     }
    //
    //     public void SendResponse(TResponse response, long requestId)
    //     {
    //         responsesToSend.Add(response);
    //     }
    //
    //     public List<TRequest> GetRequestsToSend()
    //     {
    //         return requestsToSend;
    //     }
    //
    //     public List<TReceivedRequest> GetRequestsReceived()
    //     {
    //         return requestsReceived;
    //     }
    //
    //     public List<TReceivedRequest> GetRequestsReceivedForEntityId(EntityId entityId)
    //     {
    //         // todo don't actually use this - decide if this function is needed or not and if so index things properly
    //         return requestsReceived.Where(request => request.TargetId == entityId).ToList();
    //     }
    //
    //     public List<TResponse> GetResponsesToSend()
    //     {
    //         return responsesToSend;
    //     }
    //
    //     public List<TReceivedResponse> GetResponsesReceived()
    //     {
    //         return responsesReceived;
    //     }
    //
    //     public List<TReceivedResponse> GetResponsesReceivedForEntity(Entity entity)
    //     {
    //         // todo don't actually use this - decide if this function is needed or not and if so index things properly
    //         return responsesReceived.Where(response =>
    //         {
    //             if (!sentRequestIdToEntity.TryGetValue(response.RequestId, out var entityForRequest))
    //             {
    //                 return false;
    //             }
    //
    //             return entityForRequest == entity;
    //         }).ToList();
    //     }
    //
    //     public bool TryGetResponseReceivedForRequestId(long requestId, out TReceivedResponse response)
    //     {
    //         foreach (var r in responsesReceived)
    //         {
    //             if (r.RequestId == requestId)
    //             {
    //                 response = r;
    //                 return true;
    //             }
    //         }
    //
    //         response = default(TReceivedResponse);
    //         return false;
    //     }
    //
    //     private void AddRequest(CommandRequestOp op)
    //     {
    //         // deserialise the data here
    //         TReceivedRequest request;
    //
    //         requestsReceived.Add(request);
    //     }
    //
    //     private void AddResponse(CommandResponseOp op)
    //     {
    //         // deserialise the data here
    //
    //         // get the request this is assocated with
    //         // link the response to the entity
    //         // will need to link entity in the thing
    //
    //         var sentRequestId = sentWorkerRequestIdToInternalRequestId[op.RequestId.Id];
    //         sentWorkerRequestIdToInternalRequestId.Remove(op.RequestId.Id);
    //
    //         var sendingEntity = sentRequestIdToEntity[sentRequestId];
    //         sentRequestIdToEntity.Remove(sentRequestId);
    //
    //         TReceivedResponse response; // make response
    //
    //         responsesReceived.Add(response);
    //     }
    // }
}
