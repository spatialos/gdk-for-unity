using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class ViewDiff
    {
        public bool Disconnected { get; private set; }

        public string DisconnectMessage;

        private readonly HashSet<EntityId> entitiesAdded = new HashSet<EntityId>();
        private readonly HashSet<EntityId> entitiesRemoved = new HashSet<EntityId>();

        private readonly List<LogMessageOp> logs = new List<LogMessageOp>();
        private readonly List<MetricsOp> metrics = new List<MetricsOp>();
        private readonly List<FlagUpdateOp> flags = new List<FlagUpdateOp>();

        private readonly Dictionary<uint, IComponentDiffStorage> componentIdToComponentStorage =
            new Dictionary<uint, IComponentDiffStorage>();

        private readonly List<IComponentDiffStorage> componentStorageList = new List<IComponentDiffStorage>();

        private readonly Dictionary<uint, Dictionary<uint, ICommandDiffStorage>> componentIdToCommandIdToStorage =
            new Dictionary<uint, Dictionary<uint, ICommandDiffStorage>>();

        private readonly List<ICommandDiffStorage> commandStorageList = new List<ICommandDiffStorage>();

        private readonly WorldCommandStorage worldCommandStorage = new WorldCommandStorage();

        public ViewDiff()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IComponentDiffStorage).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (IComponentDiffStorage) Activator.CreateInstance(type);

                        componentStorageList.Add(instance);
                        componentIdToComponentStorage.Add(instance.GetComponentId(), instance);
                    }

                    if (typeof(ICommandDiffStorage).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (ICommandDiffStorage) Activator.CreateInstance(type);

                        commandStorageList.Add(instance);
                        if (!componentIdToCommandIdToStorage.TryGetValue(instance.GetComponentId(),
                            out var commandIdToStorage))
                        {
                            commandIdToStorage = new Dictionary<uint, ICommandDiffStorage>();
                            componentIdToCommandIdToStorage.Add(instance.GetComponentId(), commandIdToStorage);
                        }

                        commandIdToStorage.Add(instance.GetCommandId(), instance);
                    }
                }
            }
        }

        public void Clean()
        {
            foreach (var storage in componentStorageList)
            {
                storage.Clear();
            }

            foreach (var storage in commandStorageList)
            {
                storage.Clear();
            }

            worldCommandStorage.Clear();
            entitiesAdded.Clear();
            entitiesRemoved.Clear();
            logs.Clear();
            metrics.Clear();
            flags.Clear();
        }

        public void AddEntity(long entityId)
        {
            if (!entitiesRemoved.Remove(new EntityId(entityId)))
            {
                entitiesAdded.Add(new EntityId(entityId));
            }
        }

        public void RemoveEntity(long entityId)
        {
            if (!entitiesAdded.Remove(new EntityId(entityId)))
            {
                entitiesRemoved.Add(new EntityId(entityId));
            }
        }

        public void AddComponent<T>(T component, long entityId, uint componentId) where T : ISpatialComponentUpdate
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException("Unknown component ID");
            }

            ((IDiffComponentAddedStorage<T>) storage).AddEntityComponent(entityId, component);
        }

        public void RemoveComponent(long entityId, uint componentId)
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException("Unknown component ID");
            }

            storage.RemoveEntityComponent(entityId);
        }

        public void SetAuthority(long entityId, uint componentId, Authority authority)
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var authorityStorage))
            {
                throw new ArgumentException("Unknown component");
            }

            ((IDiffAuthorityStorage) authorityStorage).AddAuthorityChange(
                new AuthorityChangeReceived(authority, new EntityId(entityId)));

            // Remove received command requests if authority has been lost
            if (authority == Authority.NotAuthoritative)
            {
                if (componentIdToCommandIdToStorage.TryGetValue(componentId, out var commandIdToStorage))
                {
                    foreach (var storage in commandIdToStorage)
                    {
                        storage.Value.RemoveEntityComponent(entityId);
                    }
                }
            }
        }

        // public void MarkAuthorityTemporarilyLost(long entityId, uint componentId)
        // {
        //     if (!componentIdToComponentStorage.TryGetValue(componentId, out var authorityStorage))
        //     {
        //         throw new ArgumentException("Unknown component");
        //     }
        //
        //     ((IDiffAuthorityStorage) authorityStorage).AddAuthorityChange(
        //         new AuthorityChangeReceived(authority, new EntityId(entityId)));
        //
        //     authorityLostTemporarily.Add(new EntityComponent(entityId, componentId));
        // }

        public void AddComponentUpdate<T>(T update, long entityId, uint componentId, uint updateId)
            where T : ISpatialComponentUpdate
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException("Unknown component");
            }

            ((IDiffUpdateStorage<T>) storage).AddUpdate(new ComponentUpdateReceived<T>(update, new EntityId(entityId),
                updateId));
        }

        public void AddEvent<T>(T ev, long entityId, uint componentId, uint updateId) where T : IEvent
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException("Unknown component");
            }

            ((IDiffEventStorage<T>) storage).AddEvent(new ComponentEventReceived<T>(ev, new EntityId(entityId),
                updateId));
        }

        public void AddCommandRequest<T>(T request, uint componentId, uint commandId) where T : IReceivedCommandRequest
        {
            var storage = componentIdToCommandIdToStorage[componentId][commandId];

            ((IDiffCommandRequestStorage<T>) storage).AddRequest(request);
        }

        public void AddCommandResponse<T>(T response, uint componentId, uint commandId) where T : IRawReceivedCommandResponse
        {
            var storage = componentIdToCommandIdToStorage[componentId][commandId];

            ((IDiffCommandResponseStorage<T>) storage).AddResponse(response);
        }

        public void AddCreateEntityResponse(CreateEntityResponseOp response)
        {
            worldCommandStorage.AddResponse(response);
        }

        public void AddDeleteEntityResponse(DeleteEntityResponseOp response)
        {
            worldCommandStorage.AddResponse(response);
        }

        public void AddReserveEntityIdsResponse(ReserveEntityIdsResponseOp response)
        {
            worldCommandStorage.AddResponse(response);
        }

        public void AddEntityQueryResponse(EntityQueryResponseOp response)
        {
            worldCommandStorage.AddResponse(response);
        }

        public void SetFlag(string name, string value)
        {
        }

        public void SetMetrics(Metrics metrics)
        {
        }

        public void SetLogs(string message, LogLevel logLevel)
        {
        }

        public void Disconnect(string message)
        {
            Disconnected = true;
            DisconnectMessage = message;
        }

        internal IComponentDiffStorage GetComponentDiffStorage(uint componentId)
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException("Can not find component diff storage. Unknown component ID");
            }

            return storage;
        }

        internal ICommandDiffStorage GetCommandDiffStorage(uint componentId, uint commandId)
        {
            if (!componentIdToCommandIdToStorage.TryGetValue(componentId, out var commandIdToStorage))
            {
                throw new ArgumentException("Can not find command diff storage. Unknown component ID");
            }

            if (!commandIdToStorage.TryGetValue(commandId, out var storage))
            {
                throw new ArgumentException("Can not find command diff storage. Unknown command ID");
            }

            return storage;
        }

        internal WorldCommandStorage GetWorldCommandStorage()
        {
            return worldCommandStorage;
        }

        internal HashSet<EntityId> GetEntitiesAdded()
        {
            return entitiesAdded;
        }

        internal HashSet<EntityId> GetEntitiesRemoved()
        {
            return entitiesRemoved;
        }

        internal readonly struct EntityComponent : IEquatable<EntityComponent>
        {
            public readonly long EntityId;
            public readonly uint ComponentId;

            public EntityComponent(long entityId, uint componentId)
            {
                ComponentId = componentId;
                EntityId = entityId;
            }

            public bool Equals(EntityComponent other)
            {
                return EntityId == other.EntityId && ComponentId == other.ComponentId;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                return obj is EntityComponent other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (EntityId.GetHashCode() * 397) ^ (int) ComponentId;
                }
            }
        }
    }
}
