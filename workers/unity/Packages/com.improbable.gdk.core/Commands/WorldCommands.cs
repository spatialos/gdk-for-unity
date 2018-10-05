using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Worker;
using Improbable.Worker.Core;
using Improbable.Worker.Query;
using Unity.Entities;
using UnityEngine;
using Entity = Improbable.Worker.Core.Entity;
using Object = System.Object;

namespace Improbable.Gdk.Core.Commands
{
    public static partial class WorldCommands
    {
        internal static void AddWorldCommandRequesters(World world, EntityManager manager, Unity.Entities.Entity entity)
        {
            var createEntitySender = new CreateEntity.CommandSender
            {
                Handle = CreateEntity.RequestsProvider.Allocate(world)
            };

            createEntitySender.RequestsToSend = new List<CreateEntity.Request>();
            manager.AddComponentData(entity, createEntitySender);

            var deleteEntitySender = new DeleteEntity.CommandSender
            {
                Handle = DeleteEntity.RequestsProvider.Allocate(world)
            };

            deleteEntitySender.RequestsToSend = new List<DeleteEntity.Request>();
            manager.AddComponentData(entity, deleteEntitySender);

            var reserveEntityIdsSender = new ReserveEntityIds.CommandSender
            {
                Handle = ReserveEntityIds.RequestsProvider.Allocate(world)
            };

            reserveEntityIdsSender.RequestsToSend = new List<ReserveEntityIds.Request>();
            manager.AddComponentData(entity, reserveEntityIdsSender);

            var entityQuerySender = new EntityQuery.CommandSender
            {
                Handle = EntityQuery.RequestsProvider.Allocate(world)
            };

            entityQuerySender.RequestsToSend = new List<EntityQuery.Request>();
            manager.AddComponentData(entity, entityQuerySender);
        }

        internal static void RemoveWorldCommandRequesters(EntityManager manager, Unity.Entities.Entity entity)
        {
            manager.RemoveComponent<CreateEntity.CommandSender>(entity);
            manager.RemoveComponent<DeleteEntity.CommandSender>(entity);
            manager.RemoveComponent<ReserveEntityIds.CommandSender>(entity);
            manager.RemoveComponent<EntityQuery.CommandSender>(entity);
        }

        internal static void DeallocateWorldCommandRequesters(EntityManager manager, Unity.Entities.Entity entity)
        {
            var createEntityData = manager.GetComponentData<CreateEntity.CommandSender>(entity);
            CreateEntity.RequestsProvider.Free(createEntityData.Handle);

            var deleteEntityData = manager.GetComponentData<DeleteEntity.CommandSender>(entity);
            DeleteEntity.RequestsProvider.Free(deleteEntityData.Handle);

            var reserveEntityIdsData = manager.GetComponentData<ReserveEntityIds.CommandSender>(entity);
            ReserveEntityIds.RequestsProvider.Free(reserveEntityIdsData.Handle);

            var entityQueryData = manager.GetComponentData<EntityQuery.CommandSender>(entity);
            EntityQuery.RequestsProvider.Free(entityQueryData.Handle);
        }

        public static class CreateEntity
        {
            /// <summary>
            ///     An object that is a CreateEntity command request.
            /// </summary>
            /// <remarks>
            ///     Do not use the default constructor. Use <see cref="CreateEntity.CreateRequest"/> instead.
            ///     Using <see cref="CreateEntity.CreateRequest"/> will ensure a correctly formed structure.
            /// </remarks>
            public struct Request
            {
                public Entity Entity;
                public EntityId? EntityId;
                public uint? TimeoutMillis;
                public Object Context;
                public long RequestId;
            }

            /// <summary>
            ///     Method to create a CreateEntity command request payload.
            /// </summary>
            /// <param name="template">
            ///     The EntityTemplate object that defines the SpatialOS components on the to-be-created entity.
            /// </param>
            /// <param name="entityId">
            ///     (Optional) The EntityId that the to-be-created entity should take.
            ///     This should only be provided if received as the result of a ReserveEntityIds command.
            /// </param>
            /// <param name="timeoutMillis">
            ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
            /// </param>
            /// <param name="context">
            ///    (Optional) A context object that will be returned with the command response.
            /// </param>
            /// <returns>The CreateEntity command request payload.</returns>
            public static Request CreateRequest(EntityTemplate template, EntityId? entityId = null, uint? timeoutMillis = null, Object context = null)
            {
                return new Request
                {
                    Entity = template.GetEntity(),
                    EntityId = entityId,
                    TimeoutMillis = timeoutMillis,
                    Context = context,
                    RequestId = CommandRequestIdGenerator.GetNext(),
                };
            }

            /// <summary>
            ///     An object that is the response of a CreateEntity command from the SpatialOS runtime.
            /// </summary>
            public struct ReceivedResponse
            {
                /// <summary>
                ///     The status code of the command response. If equal to <see cref="StatusCode"/>.Success then
                ///     the command succeeded.
                /// </summary>
                public StatusCode StatusCode { get; }

                /// <summary>
                ///     The failure message of the command. Will only be non-null if the command failed.
                /// </summary>
                public string Message { get; }

                /// <summary>
                ///     The Entity ID of the created entity. Will only be non-null if the command succeeded.
                /// </summary>
                public EntityId? EntityId { get; }

                /// <summary>
                ///     The request payload that was originally sent with this command.
                /// </summary>
                public Request RequestPayload { get; }

                /// <summary>
                ///     The context object that was provided when sending the command.
                /// </summary>
                public object Context { get; }

                /// <summary>
                ///     The unique request ID of this command. Will match the request ID in the corresponding request.
                /// </summary>
                public long RequestId { get; }

                internal ReceivedResponse(CreateEntityResponseOp op, Request req, object context, long requestId)
                {
                    StatusCode = op.StatusCode;
                    Message = op.Message;
                    EntityId = op.EntityId;
                    RequestPayload = req;
                    Context = context;
                    RequestId = requestId;
                }
            }

            /// <summary>
            ///     ECS component is for sending CreateEntity command requests to the SpatialOS runtime.
            /// </summary>
            public struct CommandSender : IComponentData
            {
                internal uint Handle;

                /// <summary>
                ///     The list of pending CreateEntity command requests.
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
                ///     The list of CreateEntity command responses.
                /// </summary>
                public List<ReceivedResponse> Responses
                {
                    get => ResponsesProvider.Get(Handle);
                    set => ResponsesProvider.Set(Handle, value);
                }
            }

            internal static class RequestsProvider
            {
                private static readonly Dictionary<uint, List<Request>> Storage = new Dictionary<uint, List<Request>>();
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
                private static readonly Dictionary<uint, List<ReceivedResponse>> Storage = new Dictionary<uint, List<ReceivedResponse>>();
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

            internal class Storage : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<Request>> CommandRequestsInFlight = new Dictionary<long, CommandRequestStore<Request>>();
            }
        }

        public static class DeleteEntity
        {
            /// <summary>
            ///     An object that is a DeleteEntity command request.
            /// </summary>
            /// <remarks>
            ///     Do not use the default constructor. Use <see cref="DeleteEntity.CreateRequest"/> instead.
            ///     Using <see cref="DeleteEntity.CreateRequest"/> will ensure a correctly formed structure.
            /// </remarks>
            public struct Request
            {
                public EntityId EntityId;
                public uint? TimeoutMillis;
                public Object Context;
                public long RequestId;
            }

            /// <summary>
            ///     Method to create a DeleteEntity command request payload.
            /// </summary>
            /// <param name="entityId"> The entity ID that is to be deleted.</param>
            /// <param name="timeoutMillis">
            ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
            /// </param>
            /// <param name="context">
            ///    (Optional) A context object that will be returned with the command response.
            /// </param>
            /// <returns>The DeleteEntity command request payload.</returns>
            public static Request CreateRequest(EntityId entityId, uint? timeoutMillis = null, Object context = null)
            {
                return new Request
                {
                    EntityId = entityId,
                    TimeoutMillis = timeoutMillis,
                    Context = context,
                    RequestId = CommandRequestIdGenerator.GetNext(),
                };
            }

            /// <summary>
            ///     An object that is the response of a DeleteEntity command from the SpatialOS runtime.
            /// </summary>
            public struct ReceivedResponse
            {
                /// <summary>
                ///     The status code of the command response. If equal to <see cref="StatusCode"/>.Success then
                ///     the command succeeded.
                /// </summary>
                public StatusCode StatusCode { get; }

                /// <summary>
                ///     The failure message of the command. Will only be non-null if the command failed.
                /// </summary>
                public string Message { get; }

                /// <summary>
                ///     The Entity ID that was the target of the DeleteEntity command.
                /// </summary>
                public EntityId EntityId { get; }

                /// <summary>
                ///     The request payload that was originally sent with this command.
                /// </summary>
                public Request RequestPayload { get; }

                /// <summary>
                ///     The context object that was provided when sending the command.
                /// </summary>
                public object Context { get; }

                /// <summary>
                ///     The unique request ID of this command. Will match the request ID in the corresponding request.
                /// </summary>
                public long RequestId { get; }

                internal ReceivedResponse(DeleteEntityResponseOp op, Request req, object context, long requestId)
                {
                    StatusCode = op.StatusCode;
                    Message = op.Message;
                    EntityId = op.EntityId;
                    RequestPayload = req;
                    Context = context;
                    RequestId = requestId;
                }
            }

            /// <summary>
            ///     ECS component is for sending DeleteEntity command requests to the SpatialOS runtime.
            /// </summary>
            public struct CommandSender : IComponentData
            {
                internal uint Handle;

                /// <summary>
                ///     The list of pending DeleteEntity command requests.
                ///     To send a command request, add an element to this list.
                /// </summary>
                public List<Request> RequestsToSend
                {
                    get => RequestsProvider.Get(Handle);
                    set => RequestsProvider.Set(Handle, value);
                }
            }

            /// <summary>
            ///     ECS component contains a list of DeleteEntity command responses received this frame.
            /// </summary>
            public struct CommandResponses : IComponentData
            {
                internal uint Handle;

                /// <summary>
                ///     The list of DeleteEntity command responses.
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
                private static readonly Dictionary<uint, List<ReceivedResponse>> Storage = new Dictionary<uint, List<ReceivedResponse>>();
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

            internal class Storage : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<Request>> CommandRequestsInFlight = new Dictionary<long, CommandRequestStore<Request>>();
            }
        }

        public static class ReserveEntityIds
        {
            /// <summary>
            ///     An object that is a ReserveEntityIds command request.
            /// </summary>
            /// <remarks>
            ///     Do not use the default constructor. Use <see cref="ReserveEntityIds.CreateRequest"/> instead.
            ///     Using <see cref="ReserveEntityIds.CreateRequest"/> will ensure a correctly formed structure.
            /// </remarks>
            public struct Request
            {
                public uint NumberOfEntityIds;
                public uint? TimeoutMillis;
                public Object Context;
                public long RequestId;
            }

            /// <summary>
            ///     Method used to create a ReserveEntityIds command request payload.
            /// </summary>
            /// <param name="numberOfEntityIds">The number of entity IDs to reserve.</param>
            /// <param name="timeoutMillis">
            ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
            /// </param>
            /// <param name="context">
            ///    (Optional) A context object that will be returned with the command response.
            /// </param>
            /// <returns>The ReserveEntityIds command request payload.</returns>
            public static Request CreateRequest(uint numberOfEntityIds, uint? timeoutMillis = null, Object context = null)
            {
                return new Request
                {
                    NumberOfEntityIds = numberOfEntityIds,
                    TimeoutMillis = timeoutMillis,
                    Context = context,
                    RequestId = CommandRequestIdGenerator.GetNext(),
                };
            }

            /// <summary>
            ///     An object that is the response of a ReserveEntityIds command from the SpatialOS runtime.
            /// </summary>
            public struct ReceivedResponse
            {
                /// <summary>
                ///     The status code of the command response. If equal to <see cref="StatusCode"/>.Success then
                ///     the command succeeded.
                /// </summary>
                public StatusCode StatusCode { get; }

                /// <summary>
                ///     The failure message of the command. Will only be non-null if the command failed.
                /// </summary>
                public string Message { get; }

                /// <summary>
                ///     The first entity ID in the range that was reserved. Will only be non-null if the command
                ///     succeeded.
                /// </summary>
                public EntityId? FirstEntityId { get; }

                /// <summary>
                ///     The number of entity IDs that were reserved.
                /// </summary>
                public int NumberOfEntityIds { get; }

                /// <summary>
                ///     The request payload that was originally sent with this command.
                /// </summary>
                public Request RequestPayload { get; }

                /// <summary>
                ///     The context object that was provided when sending the command.
                /// </summary>
                public object Context { get; }

                /// <summary>
                ///     The unique request ID of this command. Will match the request ID in the corresponding request.
                /// </summary>
                public long RequestId { get; }

                internal ReceivedResponse(ReserveEntityIdsResponseOp op, Request req, object context, long requestId)
                {
                    StatusCode = op.StatusCode;
                    Message = op.Message;
                    FirstEntityId = op.FirstEntityId;
                    NumberOfEntityIds = op.NumberOfEntityIds;
                    RequestPayload = req;
                    Context = context;
                    RequestId = requestId;
                }
            }

            /// <summary>
            ///     ECS component is for sending ReserveEntityIds command requests to the SpatialOS runtime.
            /// </summary>
            public struct CommandSender : IComponentData
            {
                internal uint Handle;

                /// <summary>
                ///     The list of pending ReserveEntityIds command requests.
                ///     To send a command request, add an element to this list.
                /// </summary>
                public List<Request> RequestsToSend
                {
                    get => RequestsProvider.Get(Handle);
                    set => RequestsProvider.Set(Handle, value);
                }
            }

            /// <summary>
            ///     ECS component contains a list of ReserveEntityIds command responses received this frame.
            /// </summary>
            public struct CommandResponses : IComponentData
            {
                internal uint Handle;

                /// <summary>
                ///     The list of ReserveEntityIds command responses.
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
                private static readonly Dictionary<uint, List<ReceivedResponse>> Storage = new Dictionary<uint, List<ReceivedResponse>>();
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

            internal class Storage : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<Request>> CommandRequestsInFlight = new Dictionary<long, CommandRequestStore<Request>>();
            }
        }

        public static class EntityQuery
        {
            /// <summary>
            ///     An object that is a EntityQuery command request.
            /// </summary>
            /// <remarks>
            ///     Do not use the default constructor. Use <see cref="EntityQuery.CreateRequest"/> instead.
            ///     Using <see cref="EntityQuery.CreateRequest"/> will ensure a correctly formed structure.
            /// </remarks>
            public struct Request
            {
                public Improbable.Worker.Query.EntityQuery EntityQuery;
                public uint? TimeoutMillis;
                public Object Context;
                public long RequestId;
            }

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
            public static Request CreateRequest(Improbable.Worker.Query.EntityQuery entityQuery, uint? timeoutMillis = null, Object context = null)
            {
                if (entityQuery.ResultType is SnapshotResultType)
                {
                    Debug.LogWarning("Cannot safely access component data from entity query - this is a known issue. To protect its integrity, the worker will drop the request before sending.");
                }

                return new Request
                {
                    EntityQuery = entityQuery,
                    TimeoutMillis = timeoutMillis,
                    Context = context,
                    RequestId = CommandRequestIdGenerator.GetNext(),
                };
            }

            /// <summary>
            ///     An object that is the response of an EntityQuery command from the SpatialOS runtime.
            /// </summary>
            public struct ReceivedResponse
            {
                /// <summary>
                ///     The status code of the command response. If equal to <see cref="StatusCode"/>.Success then
                ///     the command succeeded.
                /// </summary>
                public StatusCode StatusCode { get; }

                /// <summary>
                ///     The failure message of the command. Will only be non-null if the command failed.
                /// </summary>
                public string Message { get; }

                /// <summary>
                ///     A dictionary that represents the results of the entity query.
                /// </summary>
                /// <remarks>
                ///     Warning: Accessing underlying component data on the Entity object will cause crashes.
                ///     This dictionary should not be populated until this bug is fixed.
                /// </remarks>
                public Dictionary<EntityId, Entity> Result { get; }

                /// <summary>
                ///     The number of entities that matched the entity query constraints.
                /// </summary>
                public int ResultCount { get; }

                /// <summary>
                ///     The request payload that was originally sent with this command.
                /// </summary>
                public Request RequestPayload { get; }

                /// <summary>
                ///     The context object that was provided when sending the command.
                /// </summary>
                public object Context { get; }

                /// <summary>
                ///     The unique request ID of this command. Will match the request ID in the corresponding request.
                /// </summary>
                public long RequestId { get; }

                internal ReceivedResponse(EntityQueryResponseOp op, Request req, object context, long requestId)
                {
                    StatusCode = op.StatusCode;
                    Message = op.Message;
                    Result = op.Result;
                    ResultCount = op.ResultCount;
                    RequestPayload = req;
                    Context = context;
                    RequestId = requestId;
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
                private static readonly Dictionary<uint, List<ReceivedResponse>> Storage = new Dictionary<uint, List<ReceivedResponse>>();
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

            internal class Storage : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<Request>> CommandRequestsInFlight = new Dictionary<long, CommandRequestStore<Request>>();
            }
        }
    }
}
