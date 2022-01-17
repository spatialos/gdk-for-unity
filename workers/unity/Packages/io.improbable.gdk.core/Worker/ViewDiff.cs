using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.NetworkStats;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class ViewDiff
    {
        private static class ViewDiffMetadata
        {
            internal static IEnumerable<(uint componentId, Type diffStorageType)> ComponentStorageTypes => ComponentDatabase.Metaclasses
                .Select(pair => (pair.Key, pair.Value.DiffStorage));

            internal static IEnumerable<(uint Key, IEnumerable<Type> storageTypes)> CommandDiffStorageTypes => ComponentDatabase.Metaclasses
                .Select(componentCommands => (componentCommands.Key,
                    componentCommands.Value.Commands.Select(m => m.DiffStorage)));
        }

        public string DisconnectMessage;

        public bool Disconnected { get; private set; }

        public bool InCriticalSection { get; private set; }

        private readonly HashSet<EntityId> entitiesAdded = new HashSet<EntityId>();
        private readonly HashSet<EntityId> entitiesRemoved = new HashSet<EntityId>();
        private readonly HashSet<uint> componentsChanged = new HashSet<uint>();

        private readonly List<LogMessageReceived> logsReceived = new List<LogMessageReceived>();
        private readonly List<(string, string)> workerFlagsChanged = new List<(string, string)>();

        private readonly Dictionary<uint, IComponentDiffStorage> componentIdToComponentStorage =
            new Dictionary<uint, IComponentDiffStorage>();

        private readonly List<IComponentDiffStorage> componentStorageList = new List<IComponentDiffStorage>();

        private readonly Dictionary<uint, (int firstIndex, int count)> componentIdStorageRange =
            new Dictionary<uint, (int firstIndex, int count)>();

        private readonly Dictionary<(uint componentId, uint commandId), IComponentCommandDiffStorage> componentCommandToStorage =
            new Dictionary<(uint componentId, uint commandId), IComponentCommandDiffStorage>();

        private readonly Dictionary<Type, ICommandDiffStorage> typeToCommandStorage =
            new Dictionary<Type, ICommandDiffStorage>();

        private readonly List<IComponentCommandDiffStorage> commandStorageList = new List<IComponentCommandDiffStorage>();

        private readonly WorldCommandsReceivedStorage worldCommandsReceivedStorage = new WorldCommandsReceivedStorage();

        private readonly NetFrameStats netFrameStats = new NetFrameStats();

        public ViewDiff()
        {
            foreach (var (componentId, diffStorageType) in ViewDiffMetadata.ComponentStorageTypes)
            {
                var instance = (IComponentDiffStorage) Activator.CreateInstance(diffStorageType);

                componentStorageList.Add(instance);
                componentIdToComponentStorage.Add(componentId, instance);
            }

            var storageIndex = 0;
            foreach (var (componentId, storageTypes) in ViewDiffMetadata.CommandDiffStorageTypes)
            {
                var firstIndex = storageIndex;

                foreach (var diffStorageType in storageTypes)
                {
                    var instance = (IComponentCommandDiffStorage) Activator.CreateInstance(diffStorageType);

                    commandStorageList.Add(instance);
                    componentCommandToStorage.Add((componentId, instance.CommandId), instance);
                    typeToCommandStorage.Add(instance.RequestType, instance);
                    typeToCommandStorage.Add(instance.ResponseType, instance);
                    storageIndex++;
                }

                componentIdStorageRange.Add(componentId, (firstIndex, storageIndex - firstIndex));
            }

            typeToCommandStorage.Add(typeof(WorldCommands.CreateEntity.ReceivedResponse), worldCommandsReceivedStorage);
            typeToCommandStorage.Add(typeof(WorldCommands.DeleteEntity.ReceivedResponse), worldCommandsReceivedStorage);
            typeToCommandStorage.Add(typeof(WorldCommands.ReserveEntityIds.ReceivedResponse), worldCommandsReceivedStorage);
            typeToCommandStorage.Add(typeof(WorldCommands.EntityQuery.ReceivedResponse), worldCommandsReceivedStorage);
        }

        public void Clear()
        {
            foreach (var storage in componentStorageList)
            {
                if (storage.Dirty)
                {
                    storage.Clear();
                }
            }

            foreach (var storage in commandStorageList)
            {
                if (storage.Dirty)
                {
                    storage.Clear();
                }
            }

            worldCommandsReceivedStorage.Clear();

            entitiesAdded.Clear();
            entitiesRemoved.Clear();
            componentsChanged.Clear();
            logsReceived.Clear();
            workerFlagsChanged.Clear();
            InCriticalSection = false;
            Disconnected = false;
            DisconnectMessage = null;

            netFrameStats.Clear();
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

        public void AddComponent<T>(T component, long entityId, uint componentId, uint updateId) where T : ISpatialComponentUpdate
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException(
                    $"Can not add component with ID {componentId} on entity with ID {entityId}. " +
                    $"Unknown component ID");
            }

            ((IDiffComponentAddedStorage<T>) storage).AddEntityComponent(entityId, component, updateId);
            componentsChanged.Add(componentId);
        }

        public void RemoveComponent(long entityId, uint componentId)
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException(
                    $"Can not set remove component with ID {componentId} from entity with ID {entityId}. " +
                    $"Unknown component ID");
            }

            storage.RemoveEntityComponent(entityId);
            componentsChanged.Add(componentId);
        }

        internal void SetComponentAuthority(long entityId, uint componentId, Authority authority,
            uint authorityChangeId)
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var authorityStorage))
            {
                throw new ArgumentException(
                    $"Can not set authority over component with ID {componentId} for entity with ID {entityId}. " +
                    "Unknown component ID");
            }

            ((IDiffAuthorityStorage) authorityStorage).AddAuthorityChange(
                new AuthorityChangeReceived(authority, new EntityId(entityId), authorityChangeId));
            componentsChanged.Add(componentId);

            // Remove received command requests if authority has been lost
            if (authority == Authority.NotAuthoritative)
            {
                var (firstIndex, count) = componentIdStorageRange[componentId];
                for (var i = 0; i < count; i++)
                {
                    commandStorageList[firstIndex + i].RemoveRequests(entityId);
                }
            }
        }

        public void AddComponentUpdate<T>(T update, long entityId, uint componentId, uint updateId)
            where T : ISpatialComponentUpdate
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException(
                    $"Can not update component with ID {componentId} on entity with ID {entityId}. " +
                    $"Unknown component ID");
            }

            ((IDiffUpdateStorage<T>) storage).AddUpdate(new ComponentUpdateReceived<T>(update, new EntityId(entityId),
                updateId));
            componentsChanged.Add(componentId);
        }

        public void AddEvent<T>(T ev, long entityId, uint componentId, uint updateId) where T : IEvent
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException(
                    $"Can not add event from component with ID {componentId} on entity with ID {entityId}. " +
                    $"Unknown component ID");
            }

            ((IDiffEventStorage<T>) storage).AddEvent(new ComponentEventReceived<T>(ev, new EntityId(entityId),
                updateId));
            componentsChanged.Add(componentId);
        }

        public void AddCommandRequest<T>(T request, uint componentId, uint commandId) where T : struct, IReceivedCommandRequest
        {
            var storage = (IDiffCommandRequestStorage<T>) GetComponentCommandDiffStorage(componentId, commandId);

            storage.AddRequest(request);
        }

        public void AddCommandResponse<T>(T response, uint componentId, uint commandId)
            where T : struct, IReceivedCommandResponse
        {
            var storage = (IDiffCommandResponseStorage<T>) GetComponentCommandDiffStorage(componentId, commandId);

            storage.AddResponse(response);
        }

        public void AddCreateEntityResponse(WorldCommands.CreateEntity.ReceivedResponse response)
        {
            worldCommandsReceivedStorage.AddResponse(response);
        }

        public void AddDeleteEntityResponse(WorldCommands.DeleteEntity.ReceivedResponse response)
        {
            worldCommandsReceivedStorage.AddResponse(response);
        }

        public void AddReserveEntityIdsResponse(WorldCommands.ReserveEntityIds.ReceivedResponse response)
        {
            worldCommandsReceivedStorage.AddResponse(response);
        }

        public void AddEntityQueryResponse(WorldCommands.EntityQuery.ReceivedResponse response)
        {
            worldCommandsReceivedStorage.AddResponse(response);
        }

        public void AddLogMessage(string message, LogLevel level)
        {
            logsReceived.Add(new LogMessageReceived(message, level));
        }

        public void SetWorkerFlag(string flag, string value)
        {
            workerFlagsChanged.Add((flag, value));
        }

        public void Disconnect(string message)
        {
            Disconnected = true;
            DisconnectMessage = message;
        }

        public void SetCriticalSection(bool inCriticalSection)
        {
            InCriticalSection = inCriticalSection;
        }

        internal List<LogMessageReceived> GetLogsMessages()
        {
            return logsReceived;
        }

        internal List<(string, string)> GetWorkerFlagChanges()
        {
            return workerFlagsChanged;
        }

        public bool IsComponentChanged(uint componentId)
        {
            return componentsChanged.Contains(componentId);
        }

        internal IComponentDiffStorage GetComponentDiffStorage(uint componentId)
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException($"Can not find component diff storage. Unknown component ID {componentId}");
            }

            return storage;
        }

        internal IComponentDiffStorage GetComponentDiffStorageForEvent<T>()  where T : IEvent
        {
            var componentId = ComponentDatabase.ComponentEventType<T>.ComponentId;
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException($"Can not find component diff storage. Unknown event type {typeof(T).FullName}");
            }

            return storage;
        }

        internal IComponentCommandDiffStorage GetComponentCommandDiffStorage(uint componentId, uint commandId)
        {
            if (!componentCommandToStorage.TryGetValue((componentId, commandId), out var storage))
            {
                throw new ArgumentException($"Can not find CommandDiffStorage. Unknown component ID {componentId} with command ID {commandId}");
            }

            return storage;
        }

        internal ICommandDiffStorage GetCommandDiffStorageForRequest<T>() where T : IReceivedCommandRequest
        {
            if (!typeToCommandStorage.TryGetValue(typeof(T), out var storage))
            {
                throw new ArgumentException($"Can not find command diff storage. Unknown command type {typeof(T).FullName}");
            }

            return storage;
        }
        
        internal ICommandDiffStorage GetCommandDiffStorageForResponse<T>() where T : IReceivedCommandResponse
        {
            if (!typeToCommandStorage.TryGetValue(typeof(T), out var storage))
            {
                throw new ArgumentException($"Can not find command diff storage. Unknown command type {typeof(T).FullName}");
            }

            return storage;
        }

        internal HashSet<EntityId> GetEntitiesAdded()
        {
            return entitiesAdded;
        }

        internal HashSet<EntityId> GetEntitiesRemoved()
        {
            return entitiesRemoved;
        }

        internal NetFrameStats GetNetStats()
        {
            return netFrameStats;
        }
    }
}
