using System;
using System.Collections.Generic;
using System.Linq;
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
        private static List<(uint componentId, Type command)> storageTypes;

        private readonly HashSet<long> internalRequestIds = new HashSet<long>();

        private readonly Dictionary<uint, Dictionary<uint, ICommandMetaDataStorage>> componentIdToCommandIdToStorage =
            new Dictionary<uint, Dictionary<uint, ICommandMetaDataStorage>>();

        private List<CommandIds> requestsToRemove = new List<CommandIds>();

        public CommandMetaData()
        {
            if (storageTypes == null)
            {
                storageTypes = ComponentDatabase.Metaclasses
                    .SelectMany(type => type.Value.Commands
                        .Select(c => (componentId: type.Value.ComponentId, command: c.MetaDataStorage)))
                    .ToList();
            }

            foreach (var (componentId, type) in storageTypes)
            {
                var instance = (ICommandMetaDataStorage) Activator.CreateInstance(type);

                if (!componentIdToCommandIdToStorage.TryGetValue(componentId,
                    out var commandIdToStorage))
                {
                    commandIdToStorage = new Dictionary<uint, ICommandMetaDataStorage>();
                    componentIdToCommandIdToStorage.Add(componentId, commandIdToStorage);
                }

                commandIdToStorage.Add(instance.CommandId, instance);
            }

            componentIdToCommandIdToStorage.Add(0, new Dictionary<uint, ICommandMetaDataStorage>
            {
                { 0, new WorldCommandMetaDataStorage() }
            });
        }

        public void MarkIdForRemoval(uint componentId, uint commandId, long internalRequestId)
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

        public void AddInternalRequestId(uint componentId, uint commandId, long requestId, long internalRequestId)
        {
            internalRequestIds.Add(internalRequestId);
            var s = GetCommandDiffStorage(componentId, commandId);
            s.SetInternalRequestId(internalRequestId, requestId);
        }

        public CommandContext<T> GetContext<T>(uint componentId, uint commandId, long internalRequestId)
        {
            var s = (ICommandPayloadStorage<T>) GetCommandDiffStorage(componentId, commandId);
            return s.GetPayload(internalRequestId);
        }

        internal bool ContainsCommandMetaData(long internalRequestId)
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
            public readonly long InternalRequestId;

            public CommandIds(uint componentId, uint commandId, long internalRequestId)
            {
                ComponentId = componentId;
                CommandId = commandId;
                InternalRequestId = internalRequestId;
            }
        }
    }
}
