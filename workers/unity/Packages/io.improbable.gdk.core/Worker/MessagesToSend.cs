using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class MessagesToSend
    {
        private static class MessagesToSendMetadata
        {
            internal static IEnumerable<(uint componentId, Type diffStorageType)> ComponentTypes => ComponentDatabase.Metaclasses
                .Select(pair => (pair.Key, pair.Value.DiffStorage));

            internal static IEnumerable<(uint componentId, IEnumerable<Type> storageTypes)> CommandSendStorageTypes => ComponentDatabase.Metaclasses
                .Select(componentCommands => (componentCommands.Key, componentCommands.Value.Commands.Select(m => m.SendStorage)));
        }

        private readonly Dictionary<uint, IComponentDiffStorage> componentIdToComponentStorage =
            new Dictionary<uint, IComponentDiffStorage>();

        private readonly Dictionary<Type, IComponentDiffStorage> typeToComponentStorage =
            new Dictionary<Type, IComponentDiffStorage>();

        private readonly List<IComponentDiffStorage> componentStorageList = new List<IComponentDiffStorage>();

        private readonly Dictionary<(uint componentId, uint commandId), IComponentCommandSendStorage> componentCommandToStorage =
            new Dictionary<(uint componentId, uint commandId), IComponentCommandSendStorage>();

        private readonly Dictionary<Type, ICommandSendStorage> typeToCommandStorage =
            new Dictionary<Type, ICommandSendStorage>();

        private readonly List<ICommandSendStorage> commandStorageList = new List<ICommandSendStorage>();

        private readonly WorldCommandsToSendStorage worldCommandStorage = new WorldCommandsToSendStorage();

        private readonly MessageList<LogMessageToSend> logsToSend = new MessageList<LogMessageToSend>();

        private readonly List<Metrics> metricsToSend = new List<Metrics>();

        public MessagesToSend()
        {
            foreach (var (componentId, diffStorageType) in MessagesToSendMetadata.ComponentTypes)
            {
                var instance = (IComponentDiffStorage) Activator.CreateInstance(diffStorageType);

                componentStorageList.Add(instance);
                componentIdToComponentStorage.Add(componentId, instance);

                typeToComponentStorage.Add(instance.GetUpdateType(), instance);
                foreach (var eventType in instance.GetEventTypes())
                {
                    typeToComponentStorage.Add(eventType, instance);
                }
            }

            foreach (var (componentId, storageTypes) in MessagesToSendMetadata.CommandSendStorageTypes)
            {
                foreach (var sendStorageType in storageTypes)
                {
                    var instance = (IComponentCommandSendStorage) Activator.CreateInstance(sendStorageType);

                    commandStorageList.Add(instance);
                    componentCommandToStorage.Add((componentId, instance.CommandId), instance);
                    typeToCommandStorage.Add(instance.RequestType, instance);
                    typeToCommandStorage.Add(instance.ResponseType, instance);
                }
            }

            commandStorageList.Add(worldCommandStorage);
            typeToCommandStorage.Add(typeof(WorldCommands.CreateEntity.Request), worldCommandStorage);
            typeToCommandStorage.Add(typeof(WorldCommands.DeleteEntity.Request), worldCommandStorage);
            typeToCommandStorage.Add(typeof(WorldCommands.ReserveEntityIds.Request), worldCommandStorage);
            typeToCommandStorage.Add(typeof(WorldCommands.EntityQuery.Request), worldCommandStorage);
        }

        public void Clear()
        {
            foreach (var storage in componentStorageList)
            {
                storage.Clear();
            }

            foreach (var storage in commandStorageList)
            {
                storage.Clear();
            }

            logsToSend.Clear();
            metricsToSend.Clear();
        }

        public void AddComponentUpdate<T>(in T update, long entityId)
            where T : ISpatialComponentUpdate
        {
            var storage = GetComponentDiffStorage(typeof(T));

            // Update ID isn't needed so we set it to 0
            ((IDiffUpdateStorage<T>) storage).AddUpdate(new ComponentUpdateReceived<T>(update, new EntityId(entityId),
                0));
        }

        public void AddEvent<T>(T ev, long entityId) where T : IEvent
        {
            var storage = GetComponentDiffStorage(typeof(T));

            // Update ID isn't needed so we set it to 0
            ((IDiffEventStorage<T>) storage).AddEvent(new ComponentEventReceived<T>(ev, new EntityId(entityId),
                0));
        }

        public void AddCommandRequest<T>(T request, Unity.Entities.Entity sendingEntity, CommandRequestId requestId) where T : ICommandRequest
        {
            var storage = (ICommandRequestSendStorage<T>) GetCommandSendStorage(typeof(T));
            storage.AddRequest(request, sendingEntity, requestId);
        }

        public void AddCommandResponse<T>(T response)
            where T : ICommandResponse
        {
            var storage = (ICommandResponseSendStorage<T>) GetCommandSendStorage(typeof(T));
            storage.AddResponse(response);
        }

        public void AddLogMessage(in LogMessageToSend log)
        {
            logsToSend.Add(in log);
        }

        public void AddMetrics(Metrics metrics)
        {
            metricsToSend.Add(metrics);
        }

        internal MessageList<LogMessageToSend> GetLogMessages()
        {
            return logsToSend;
        }

        internal List<Metrics> GetMetrics()
        {
            return metricsToSend;
        }

        internal IComponentDiffStorage GetComponentDiffStorage(uint componentId)
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException($"Can not find component diff storage. Unknown component ID {componentId}");
            }

            return storage;
        }

        internal IComponentDiffStorage GetComponentDiffStorage(Type type)
        {
            if (!typeToComponentStorage.TryGetValue(type, out var storage))
            {
                throw new ArgumentException($"Can not find component diff storage. Unknown type {type.FullName}");
            }

            return storage;
        }

        internal ICommandSendStorage GetCommandSendStorage(Type type)
        {
            if (!typeToCommandStorage.TryGetValue(type, out var storage))
            {
                throw new ArgumentException($"Can not find command send storage. Unknown command type {type.FullName}");
            }

            return storage;
        }

        internal IComponentCommandSendStorage GetCommandSendStorage(uint componentId, uint commandId)
        {
            if (!componentCommandToStorage.TryGetValue((componentId, commandId), out var commandSendStorage))
            {
                throw new ArgumentException($"Can not find command send storage. Could not find Command ID {commandId} for Component ID {componentId}");
            }

            return commandSendStorage;
        }

        internal WorldCommandsToSendStorage GetWorldCommandStorage()
        {
            return worldCommandStorage;
        }
    }
}
