using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Worker;
using Improbable.Worker.Core;
using Improbable.Worker.Query;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.Commands
{
    public static partial class WorldCommands
    {
        public static class EntityQuery
        {
            /// <summary>
            ///     An object that is a EntityQuery command request.
            /// </summary>
            public struct Request : ICommandRequest
            {
                public Improbable.Worker.Query.EntityQuery EntityQuery;
                public uint? TimeoutMillis;
                public object Context;

                /// <summary>
                ///     Method to create an EntityQuery command request payload.
                /// </summary>
                /// <param name="entityQuery">The EntityQuery object defining the constraints and query type.</param>
                /// <param name="timeoutMillis">
                ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
                /// </param>
                /// <param name="context">
                ///    (Optional) A context object that will be returned with the command response.
                /// </param>
                /// <returns></returns>
                public Request(Improbable.Worker.Query.EntityQuery entityQuery,
                    uint? timeoutMillis = null, object context = null)
                {
                    EntityQuery = entityQuery;
                    TimeoutMillis = timeoutMillis;
                    Context = context;
                }
            }

            /// <summary>
            ///     An object that is the response of an EntityQuery command from the SpatialOS runtime.
            /// </summary>
            public readonly struct ReceivedResponse : IReceivedCommandResponse
            {
                public readonly Entity SendingEntity;

                /// <summary>
                ///     The status code of the command response. If equal to <see cref="StatusCode"/>.Success then
                ///     the command succeeded.
                /// </summary>
                public readonly StatusCode StatusCode;

                /// <summary>
                ///     The failure message of the command. Will only be non-null if the command failed.
                /// </summary>
                public readonly string Message;

                /// <summary>
                ///     A dictionary that represents the results of a <see cref="SnapshotResultType"/> entity query.
                ///     This is null for <see cref="CountResultType"/> entity queries.
                /// </summary>
                public readonly Dictionary<EntityId, EntityQuerySnapshot> Result;

                /// <summary>
                ///     The number of entities that matched the entity query constraints.
                /// </summary>
                public readonly int ResultCount;

                /// <summary>
                ///     The request payload that was originally sent with this command.
                /// </summary>
                public readonly Request RequestPayload;

                /// <summary>
                ///     The context object that was provided when sending the command.
                /// </summary>
                public readonly object Context;

                /// <summary>
                ///     The unique request ID of this command. Will match the request ID in the corresponding request.
                /// </summary>
                public readonly long RequestId;

                internal ReceivedResponse(EntityQueryResponseOp op, Entity sendingEntity, Request req, long requestId,
                    World world)
                {
                    SendingEntity = sendingEntity;
                    StatusCode = op.StatusCode;
                    Message = op.Message;
                    ResultCount = op.ResultCount;
                    RequestPayload = req;
                    Context = req.Context;
                    RequestId = requestId;

                    if (op.Result == null)
                    {
                        Result = null;
                        return;
                    }

                    Result = new Dictionary<EntityId, EntityQuerySnapshot>();
                    foreach (var entityIdToEntity in op.Result)
                    {
                        Result.Add(entityIdToEntity.Key, new EntityQuerySnapshot(entityIdToEntity.Value, world));
                    }
                }
            }

            /// <summary>
            ///     ECS component is for sending EntityQuery command requests to the SpatialOS runtime.
            /// </summary>
            public struct CommandSender : IComponentData
            {
                internal uint Handle;

                /// <summary>
                ///     The list of pending EntityQuery command requests.
                ///     To send a command request, add an element to this list.
                /// </summary>
                public List<Request> RequestsToSend
                {
                    get => RequestsProvider.Get(Handle);
                    set => RequestsProvider.Set(Handle, value);
                }
            }

            /// <summary>
            ///     ECS component contains a list of CreateEntity command responses received this frame.
            /// </summary>
            public struct CommandResponses : IComponentData
            {
                internal uint Handle;

                /// <summary>
                ///     The list of EntityQuery command responses.
                /// </summary>
                public List<ReceivedResponse> Responses
                {
                    get => ResponsesProvider.Get(Handle);
                    set => ResponsesProvider.Set(Handle, value);
                }
            }

            internal static class RequestsProvider
            {
                private static readonly Dictionary<long, List<Request>> Storage = new Dictionary<long, List<Request>>();
                private static readonly Dictionary<uint, World> WorldMapping = new Dictionary<uint, World>();

                private static uint nextHandle = 0;

                public static uint Allocate(World world)
                {
                    var handle = GetNextHandle();
                    Storage.Add(handle, default(List<Request>));
                    WorldMapping.Add(handle, world);

                    return handle;
                }

                public static List<Request> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"RequestsProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<Request> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"RequestsProvider does not contain handle {handle}");
                    }

                    Storage[handle] = value;
                }

                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }

                public static void CleanDataInWorld(World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();

                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }

                private static uint GetNextHandle()
                {
                    nextHandle++;

                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }

                    return nextHandle;
                }
            }

            internal static class ResponsesProvider
            {
                private static readonly Dictionary<uint, List<ReceivedResponse>> Storage =
                    new Dictionary<uint, List<ReceivedResponse>>();

                private static readonly Dictionary<uint, World> WorldMapping = new Dictionary<uint, World>();

                private static uint nextHandle = 0;

                public static uint Allocate(World world)
                {
                    var handle = GetNextHandle();
                    Storage.Add(handle, default(List<ReceivedResponse>));
                    WorldMapping.Add(handle, world);

                    return handle;
                }

                public static List<ReceivedResponse> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"ResponsesProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<ReceivedResponse> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"ResponsesProvider does not contain handle {handle}");
                    }

                    Storage[handle] = value;
                }

                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }

                public static void CleanDataInWorld(World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();

                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }

                private static uint GetNextHandle()
                {
                    nextHandle++;

                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }

                    return nextHandle;
                }
            }

            public class CommandComponentManager : ICommandComponentManager
            {
                public void PopulateCommandComponents(CommandSystem commandSystem, EntityManager entityManager,
                    WorkerSystem workerSystem, World world)
                {
                    var receivedResponses = commandSystem.GetResponses<ReceivedResponse>();
                    // todo Not efficient if it keeps jumping all over entities but don't care right now
                    foreach (var response in receivedResponses)
                    {
                        if (response.SendingEntity == Unity.Entities.Entity.Null ||
                            !entityManager.Exists(response.SendingEntity))
                        {
                            continue;
                        }

                        List<ReceivedResponse> responses;
                        if (entityManager.HasComponent<CommandResponses>(
                            response.SendingEntity))
                        {
                            responses = entityManager
                                .GetComponentData<CommandResponses>(response.SendingEntity).Responses;
                        }
                        else
                        {
                            var data = new CommandResponses
                            {
                                Handle = ResponsesProvider.Allocate(world)
                            };
                            data.Responses = new List<ReceivedResponse>();
                            responses = data.Responses;
                            entityManager.AddComponentData(response.SendingEntity, data);
                        }

                        responses.Add(response);
                    }
                }
            }


            public class CommandManager : IWorldCommandManager<Request, ReceivedResponse>
            {
                private WorkerSystem workerSystem;
                private EntityManager entityManager;

                // todo only entity query stuff needs a world - see if there is some other way of doing that
                private World world;

                private List<(Request Request, long Id)> requestsToSend = new List<(Request Request, long Id)>();

                private List<ReceivedResponse> responsesReceived =
                    new List<ReceivedResponse>();

                private Dictionary<long, Request> sentInternalRequestIdToRequest =
                    new Dictionary<long, Request>();

                private Dictionary<long, Unity.Entities.Entity> sentInternalRequestIdToEntity =
                    new Dictionary<long, Unity.Entities.Entity>();

                private Dictionary<long, long> sentWorkerRequestIdToInternalRequestId = new Dictionary<long, long>();

                public Type GetRequestType()
                {
                    return typeof(Request);
                }

                public Type GetReceivedRequestType()
                {
                    return null;
                }

                public Type GetResponseType()
                {
                    return null;
                }

                public Type GetReceivedResponseType()
                {
                    return typeof(ReceivedResponse);
                }

                public void SendAll()
                {
                    var connection = workerSystem.Connection;

                    foreach (var (request, id) in requestsToSend)
                    {
                        var requestId = connection.SendEntityQueryRequest(request.EntityQuery,
                            request.TimeoutMillis);

                        sentWorkerRequestIdToInternalRequestId[requestId.Id] = id;
                    }

                    requestsToSend.Clear();
                    responsesReceived.Clear();
                }

                public void Init(World world)
                {
                    this.world = world;
                    workerSystem = world.GetExistingManager<WorkerSystem>();

                    if (workerSystem == null)
                    {
                        throw new ArgumentException("World instance is not running a valid SpatialOS worker");
                    }

                    var dispatcher = world.GetExistingManager<SpatialOSReceiveSystem>().Dispatcher;

                    dispatcher.OnEntityQueryResponse(AddResponse);
                }

                public long SendCommand(Request request, Unity.Entities.Entity entity)
                {
                    var id = global::Improbable.Gdk.Core.CommandRequestIdGenerator.GetNext();
                    requestsToSend.Add((request, id));
                    sentInternalRequestIdToEntity.Add(id, entity);
                    sentInternalRequestIdToRequest[id] = request;
                    return id;
                }

                public List<(Request Request, long Id)> GetRequestsToSend()
                {
                    return requestsToSend;
                }

                public List<ReceivedResponse> GetResponsesReceived()
                {
                    return responsesReceived;
                }

                public List<ReceivedResponse> GetResponsesReceivedForEntity(Unity.Entities.Entity entity)
                {
                    // todo don't actually use this - decide if this function is needed or not and if so index things properly
                    return responsesReceived.Where(response =>
                    {
                        if (!sentInternalRequestIdToEntity.TryGetValue(response.RequestId, out var entityForRequest))
                        {
                            return false;
                        }

                        return entityForRequest == entity;
                    }).ToList();
                    return null;
                }

                public bool TryGetResponseReceivedForRequestId(long requestId,
                    out ReceivedResponse response)
                {
                    foreach (var r in responsesReceived)
                    {
                        if (r.RequestId == requestId)
                        {
                            response = r;
                            return true;
                        }
                    }

                    response = default(ReceivedResponse);
                    return false;
                }

                private void AddResponse(EntityQueryResponseOp op)
                {
                    var internalRequestId = sentWorkerRequestIdToInternalRequestId[op.RequestId.Id];
                    sentWorkerRequestIdToInternalRequestId.Remove(op.RequestId.Id);

                    var sendingEntity = sentInternalRequestIdToEntity[internalRequestId];
                    sentInternalRequestIdToEntity.Remove(internalRequestId);

                    var request = sentInternalRequestIdToRequest[internalRequestId];
                    sentInternalRequestIdToRequest.Remove(internalRequestId);

                    var response = new ReceivedResponse(
                        op,
                        sendingEntity,
                        request,
                        internalRequestId, world);

                    responsesReceived.Add(response);
                }
            }
        }
    }
}
