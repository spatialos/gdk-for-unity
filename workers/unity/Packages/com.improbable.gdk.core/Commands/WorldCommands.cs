using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using Entity = Improbable.Worker.Core.Entity;

namespace Improbable.Gdk.Core.Commands
{
    public static class WorldCommands
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
            public struct Request
            {
                public Entity Entity;
                public EntityId? EntityId;
                public uint? TimeoutMillis;
            }

            public struct ReceivedResponse
            {
                public CreateEntityResponseOp Op { get; }
                public Request RawRequest { get; }
                public object Context { get; }

                internal ReceivedResponse(CreateEntityResponseOp op, Request req, object context)
                {
                    Op = op;
                    RawRequest = req;
                    Context = context;
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
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<Request> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
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
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<ReceivedResponse> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
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
                public Dictionary<uint, CommandRequestStore<Request>> CommandRequestsInFlight = new Dictionary<uint, CommandRequestStore<Request>>();
            }
        }

        public static class DeleteEntity
        {
            public struct Request
            {
                public EntityId EntityId;
                public uint? TimeoutMillis;
            }

            public struct ReceivedResponse
            {
                public DeleteEntityResponseOp Op { get; }
                public Request RawRequest { get; }
                public object Context { get; }

                internal ReceivedResponse(DeleteEntityResponseOp op, Request req, object context)
                {
                    Op = op;
                    RawRequest = req;
                    Context = context;
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
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<Request> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
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
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<ReceivedResponse> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
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
                public Dictionary<uint, CommandRequestStore<Request>> CommandRequestsInFlight = new Dictionary<uint, CommandRequestStore<Request>>();
            }
        }

        public static class ReserveEntityIds
        {
            public struct Request
            {
                public uint NumberOfEntityIds;
                public uint? TimeoutMillis;
            }

            public struct ReceivedResponse
            {
                public ReserveEntityIdsResponseOp Op { get; }
                public Request RawRequest { get; }
                public object Context { get; }

                internal ReceivedResponse(ReserveEntityIdsResponseOp op, Request req, object context)
                {
                    Op = op;
                    RawRequest = req;
                    Context = context;
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
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<Request> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
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
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<ReceivedResponse> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
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
                public Dictionary<uint, CommandRequestStore<Request>> CommandRequestsInFlight = new Dictionary<uint, CommandRequestStore<Request>>();
            }
        }

        public static class EntityQuery
        {
            public struct Request
            {
                public Improbable.Worker.Query.EntityQuery EntityQuery;
                public uint? TimeoutMillis;
            }

            public struct ReceivedResponse
            {
                public EntityQueryResponseOp Op { get; }
                public Request RawRequest { get; }
                public object Context { get; }

                internal ReceivedResponse(EntityQueryResponseOp op, Request req, object context)
                {
                    Op = op;
                    RawRequest = req;
                    Context = context;
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
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<Request> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
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
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, List<ReceivedResponse> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
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
                public Dictionary<uint, CommandRequestStore<Request>> CommandRequestsInFlight = new Dictionary<uint, CommandRequestStore<Request>>();
            }
        }
    }
}
