using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    public class MessagesToSend
    {
        private readonly Dictionary<uint, IComponentDiffStorage> componentIdToComponentStorage =
            new Dictionary<uint, IComponentDiffStorage>();

        private readonly Dictionary<Type, IComponentDiffStorage> typeToComponentStorage =
            new Dictionary<Type, IComponentDiffStorage>();

        private readonly List<IComponentDiffStorage> componentStorageList = new List<IComponentDiffStorage>();

        private readonly Dictionary<uint, Dictionary<uint, ICommandSendStorage>> componentIdToCommandIdToStorage =
            new Dictionary<uint, Dictionary<uint, ICommandSendStorage>>();

        private readonly Dictionary<Type, ICommandSendStorage> typeToCommandStorage =
            new Dictionary<Type, ICommandSendStorage>();

        private readonly List<ICommandSendStorage> commandStorageList = new List<ICommandSendStorage>();

        //private readonly WorldCommandStorage worldCommandStorage = new WorldCommandStorage();

        private readonly MessageList<EntityComponent> authorityLossAcks =
            new MessageList<EntityComponent>();

        public MessagesToSend()
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

                        typeToComponentStorage.Add(instance.GetUpdateType(), instance);
                        foreach (var eventType in instance.GetEventTypes())
                        {
                            typeToComponentStorage.Add(eventType, instance);
                        }
                    }

                    if (typeof(ICommandSendStorage).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (ICommandSendStorage) Activator.CreateInstance(type);

                        commandStorageList.Add(instance);
                        if (!componentIdToCommandIdToStorage.TryGetValue(instance.GetComponentId(),
                            out var commandIdToStorage))
                        {
                            commandIdToStorage = new Dictionary<uint, ICommandSendStorage>();
                            componentIdToCommandIdToStorage.Add(instance.GetComponentId(), commandIdToStorage);
                        }

                        commandIdToStorage.Add(instance.GetCommandId(), instance);
                        typeToCommandStorage.Add(instance.GetRequestType(), instance);
                        typeToCommandStorage.Add(instance.GetResponseType(), instance);
                    }
                }
            }
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

            //worldCommandStorage.Clear();
            authorityLossAcks.Clear();
        }

        public void AcknowledgeAuthorityLoss(long entityId, uint componentId)
        {
            authorityLossAcks.Add(new EntityComponent(entityId, componentId));
        }

        public void AddComponentUpdate<T>(T update, long entityId)
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

        public void AddCommandRequest<T>(T request, Unity.Entities.Entity sendingEntity, long requestId) where T : ICommandRequest
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

        // public void AddCreateEntityRequest(CreateEntityResponseOp response)
        // {
        //     worldCommandStorage.AddResponse(response);
        // }
        //
        // public void AddDeleteEntityRequest(DeleteEntityResponseOp response)
        // {
        //     worldCommandStorage.AddResponse(response);
        // }
        //
        // public void AddReserveEntityIdsRequest(ReserveEntityIdsResponseOp response)
        // {
        //     worldCommandStorage.AddResponse(response);
        // }
        //
        // public void AddEntityQueryRequest(EntityQueryResponseOp response)
        // {
        //     worldCommandStorage.AddResponse(response);
        // }

        internal MessageList<EntityComponent> GetAuthorityLossAcknowledgements()
        {
            return authorityLossAcks;
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

        internal ICommandSendStorage GetCommandSendStorage(uint componentId, uint commandId)
        {
            if (!componentIdToCommandIdToStorage.TryGetValue(componentId, out var commandIdToStorage))
            {
                throw new ArgumentException($"Can not find command send storage. Unknown component ID {componentId}");
            }

            if (!commandIdToStorage.TryGetValue(commandId, out var storage))
            {
                throw new ArgumentException($"Can not find command send storage. Unknown command ID {commandId}");
            }

            return storage;
        }

        // internal WorldCommandStorage GetWorldCommandStorage()
        // {
        //     return worldCommandStorage;
        // }
    }
}
