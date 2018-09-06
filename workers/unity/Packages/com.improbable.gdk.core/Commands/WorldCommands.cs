using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using Entity = Improbable.Worker.Core.Entity;

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
            ///     Please do not use the default constructor. Use CreateRequest instead.
            ///     Using CreateRequest will ensure a correctly formed structure.
            /// </summary>
            public struct Request
            {
                public Entity Entity;
                public EntityId? EntityId;
                public uint? TimeoutMillis;
                public Object Context;
                public long RequestId;
            }

            public static Request CreateRequest(Entity entity, EntityId? entityId = null, uint? timeoutMillis = null, Object context = null)
            {
                return new Request
                {
                    Entity = entity,
                    EntityId = entityId,
                    TimeoutMillis = timeoutMillis,
                    Context = context,
                    RequestId = CommandRequestIdGenerator.GetNext(),
                };
            }

            public struct ReceivedResponse
            {
                public CreateEntityResponseOp Op { get; }
                public Request RequestPayload { get; }
                public object Context { get; }
                public long RequestId { get; }

                internal ReceivedResponse(CreateEntityResponseOp op, Request req, object context, long requestId)
                {
                    Op = op;
                    RequestPayload = req;
                    Context = context;
                    RequestId = requestId;
                }
            }

            public struct CommandSender : IComponentData
            {
                internal uint Handle;

                public List<Request> RequestsToSend
                {
                    get => RequestsProvider.Get(Handle);
                    set => RequestsProvider.Set(Handle, value);
                }
            }

            public struct CommandResponses : IComponentData
            {
                internal uint Handle;

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
            ///     Please do not use the default constructor. Use CreateRequest instead.
            ///     Using CreateRequest will ensure a correctly formed structure.
            /// </summary>
            public struct Request
            {
                public EntityId EntityId;
                public uint? TimeoutMillis;
                public Object Context;
                public long RequestId;
            }

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

            public struct ReceivedResponse
            {
                public DeleteEntityResponseOp Op { get; }
                public Request RequestPayload { get; }
                public object Context { get; }
                public long RequestId { get; }

                internal ReceivedResponse(DeleteEntityResponseOp op, Request req, object context, long requestId)
                {
                    Op = op;
                    RequestPayload = req;
                    Context = context;
                    RequestId = requestId;
                }
            }

            public struct CommandSender : IComponentData
            {
                internal uint Handle;

                public List<Request> RequestsToSend
                {
                    get => RequestsProvider.Get(Handle);
                    set => RequestsProvider.Set(Handle, value);
                }
            }

            public struct CommandResponses : IComponentData
            {
                internal uint Handle;

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
            ///     Please do not use the default constructor. Use CreateRequest instead.
            ///     Using CreateRequest will ensure a correctly formed structure.
            /// </summary>
            public struct Request
            {
                public uint NumberOfEntityIds;
                public uint? TimeoutMillis;
                public Object Context;
                public long RequestId;
            }

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

            public struct ReceivedResponse
            {
                public ReserveEntityIdsResponseOp Op { get; }
                public Request RequestPayload { get; }
                public object Context { get; }
                public long RequestId { get; }

                internal ReceivedResponse(ReserveEntityIdsResponseOp op, Request req, object context, long requestId)
                {
                    Op = op;
                    RequestPayload = req;
                    Context = context;
                    RequestId = requestId;
                }
            }

            public struct CommandSender : IComponentData
            {
                internal uint Handle;

                public List<Request> RequestsToSend
                {
                    get => RequestsProvider.Get(Handle);
                    set => RequestsProvider.Set(Handle, value);
                }
            }

            public struct CommandResponses : IComponentData
            {
                internal uint Handle;

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
            ///     Please do not use the default constructor. Use CreateRequest instead.
            ///     Using CreateRequest will ensure a correctly formed structure.
            /// </summary>
            public struct Request
            {
                public Improbable.Worker.Query.EntityQuery EntityQuery;
                public uint? TimeoutMillis;
                public Object Context;
                public long RequestId;
            }

            public static Request CreateRequest(Improbable.Worker.Query.EntityQuery entityQuery, uint? timeoutMillis = null, Object context = null)
            {
                return new Request
                {
                    EntityQuery = entityQuery,
                    TimeoutMillis = timeoutMillis,
                    Context = context,
                    RequestId = CommandRequestIdGenerator.GetNext(),
                };
            }

            public struct ReceivedResponse
            {
                public EntityQueryResponseOp Op { get; }
                public Request RequestPayload { get; }
                public object Context { get; }
                public long RequestId { get; }

                internal ReceivedResponse(EntityQueryResponseOp op, Request req, object context, long requestId)
                {
                    Op = op;
                    RequestPayload = req;
                    Context = context;
                    RequestId = requestId;
                }
            }

            public struct CommandSender : IComponentData
            {
                internal uint Handle;

                public List<Request> RequestsToSend
                {
                    get => RequestsProvider.Get(Handle);
                    set => RequestsProvider.Set(Handle, value);
                }
            }

            public struct CommandResponses : IComponentData
            {
                internal uint Handle;

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
