using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public readonly struct CommandContext<T>
    {
        public readonly Entity SendingEntity;
        public readonly T Request;
        public readonly object Context;
        public readonly long RequestId;

        public CommandContext(Entity sendingEntity, T request, object context, long requestId)
        {
            SendingEntity = sendingEntity;
            Request = request;
            Context = context;
            RequestId = requestId;
        }
    }

    public class CommandMetaData
    {
        // Cache the types needed to instantiate a new CommandMetaData
        private static List<Type> storageTypes;

        private readonly HashSet<uint> internalRequestIds = new HashSet<uint>();

        private readonly Dictionary<uint, Dictionary<uint, ICommandMetaDataStorage>> componentIdToCommandIdToStorage =
            new Dictionary<uint, Dictionary<uint, ICommandMetaDataStorage>>();

        private List<CommandIds> requestsToRemove = new List<CommandIds>();

        public CommandMetaData()
        {
            if (storageTypes == null)
            {
                storageTypes = ReflectionUtility.GetNonAbstractTypes(typeof(ICommandMetaDataStorage));
            }

            foreach (var type in storageTypes)
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
                internalRequestIds.Remove(commandIds.InternalRequestId);
            }

            requestsToRemove.Clear();
        }

        public void AddRequest<T>(uint componentId, uint commandId, in CommandContext<T> context)
        {
            var s = (ICommandPayloadStorage<T>) GetCommandDiffStorage(componentId, commandId);
            s.AddRequest(in context);
        }

        public void AddInternalRequestId(uint componentId, uint commandId, long requestId, uint internalRequestId)
        {
            internalRequestIds.Add(internalRequestId);
            var s = GetCommandDiffStorage(componentId, commandId);
            s.SetInternalRequestId(internalRequestId, requestId);
        }

        public CommandContext<T> GetContext<T>(uint componentId, uint commandId, uint internalRequestId)
        {
            var s = (ICommandPayloadStorage<T>) GetCommandDiffStorage(componentId, commandId);
            return s.GetPayload(internalRequestId);
        }

        internal bool ContainsCommandMetaData(uint internalRequestId)
        {
            return internalRequestIds.Contains(internalRequestId);
        }

        internal bool IsEmpty()
        {
            return internalRequestIds.Count == 0;
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
