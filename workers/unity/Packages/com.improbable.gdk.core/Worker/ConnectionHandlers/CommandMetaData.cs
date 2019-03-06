using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public struct CommandContext<T>
    {
        public Entity SendingEntity;
        public T Request;
        public object Context;

        public CommandContext(Entity sendingEntity, T request, object context)
        {
            SendingEntity = sendingEntity;
            Request = request;
            Context = context;
        }
    }

    public class CommandMetaData
    {
        private readonly Dictionary<uint, Dictionary<uint, ICommandMetaDataStorage>> componentIdToCommandIdToStorage =
            new Dictionary<uint, Dictionary<uint, ICommandMetaDataStorage>>();

        private List<CommandIds> requestsToRemove = new List<CommandIds>();

        public CommandMetaData()
        {
            var types = ReflectionUtility.GetNonAbstractTypes(typeof(ICommandMetaDataStorage));
            foreach (var type in types)
            {
                var instance = (ICommandMetaDataStorage) Activator.CreateInstance(type);

                if (!componentIdToCommandIdToStorage.TryGetValue(instance.GetComponentId(),
                    out var commandIdToStorage))
                {
                    commandIdToStorage = new Dictionary<uint, ICommandMetaDataStorage>();
                    componentIdToCommandIdToStorage.Add(instance.GetComponentId(), commandIdToStorage);
                }

                commandIdToStorage.Add(instance.GetCommandId(), instance);
            }
        }

        public void MarkIdForRemoval(uint componentId, uint commandId, uint internalRequestId)
        {
            requestsToRemove.Add(new CommandIds(componentId, commandId, internalRequestId));
        }

        public void FlushRemovedIds()
        {
            foreach (var commandIds in requestsToRemove)
            {
                var s = GetCommandDiffStorage(commandIds.ComponentId, commandIds.CommandId);
                s.RemoveMetaData(commandIds.InternalRequestId);
            }

            requestsToRemove.Clear();
        }

        public void AddRequest<T>(uint componentId, uint commandId, long requestId, CommandContext<T> context)
        {
            var s = (ICommandPayloadStorage<T>) GetCommandDiffStorage(componentId, commandId);
            s.AddRequest(context, requestId);
        }

        public void AddInternalRequestId(uint componentId, uint commandId, long requestId, uint internalRequestId)
        {
            var s = GetCommandDiffStorage(componentId, commandId);
            s.AddRequestId(internalRequestId, requestId);
        }

        public long GetRequestId(uint componentId, uint commandId, uint internalRequestId)
        {
            var s = GetCommandDiffStorage(componentId, commandId);
            return s.GetRequestId(internalRequestId);
        }

        public CommandContext<T> GetContext<T>(uint componentId, uint commandId, long requestId)
        {
            var s = (ICommandPayloadStorage<T>) GetCommandDiffStorage(componentId, commandId);
            return s.GetPayload(requestId);
        }

        private ICommandMetaDataStorage GetCommandDiffStorage(uint componentId, uint commandId)
        {
            if (!componentIdToCommandIdToStorage.TryGetValue(componentId, out var commandIdToStorage))
            {
                throw new ArgumentException($"Can not find command meta data. Unknown component ID {componentId}");
            }

            if (!commandIdToStorage.TryGetValue(commandId, out var storage))
            {
                throw new ArgumentException($"Can not find command meta data storage. Unknown command ID {commandId}");
            }

            return storage;
        }

        private readonly struct CommandIds
        {
            public readonly uint ComponentId;
            public readonly uint CommandId;
            public readonly uint InternalRequestId;

            public CommandIds(uint componentId, uint commandId, uint internalRequestId)
            {
                ComponentId = componentId;
                CommandId = commandId;
                InternalRequestId = internalRequestId;
            }
        }
    }
}
