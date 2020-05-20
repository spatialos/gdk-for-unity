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

        private readonly Dictionary<(uint componentId, uint commandId), ICommandMetaDataStorage> componentCommandToStorage =
            new Dictionary<(uint componentId, uint commandId), ICommandMetaDataStorage>();

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
                componentCommandToStorage.Add((componentId, instance.CommandId), instance);
            }

            componentCommandToStorage.Add((0, 0), new WorldCommandMetaDataStorage());
        }

        public void RemoveRequest(uint componentId, uint commandId, long internalRequestId)
        {
            var commandMetaDataStorage = GetCommandDiffStorage(componentId, commandId);
            commandMetaDataStorage.RemoveMetaData(internalRequestId);
            internalRequestIds.Remove(internalRequestId);
        }

        public void AddRequest<T>(uint componentId, uint commandId, in CommandContext<T> context)
        {
            var commandPayloadStorage = (ICommandPayloadStorage<T>) GetCommandDiffStorage(componentId, commandId);
            commandPayloadStorage.AddRequest(in context);
        }

        public void AddInternalRequestId(uint componentId, uint commandId, long requestId, long internalRequestId)
        {
            internalRequestIds.Add(internalRequestId);
            var commandMetaDataStorage = GetCommandDiffStorage(componentId, commandId);
            commandMetaDataStorage.SetInternalRequestId(internalRequestId, requestId);
        }

        public CommandContext<T> GetContext<T>(uint componentId, uint commandId, long internalRequestId)
        {
            var commandPayloadStorage = (ICommandPayloadStorage<T>) GetCommandDiffStorage(componentId, commandId);
            return commandPayloadStorage.GetPayload(internalRequestId);
        }

        private ICommandMetaDataStorage GetCommandDiffStorage(uint componentId, uint commandId)
        {
            if (!componentCommandToStorage.TryGetValue((componentId, commandId), out var storage))
            {
                throw new ArgumentException($"Can not find command meta data. Unknown component:command ID {componentId}:{commandId}");
            }

            return storage;
        }
    }
}
