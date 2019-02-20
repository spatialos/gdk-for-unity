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

        private List<(uint ComponentId, uint CommandId, uint InternalRequestId)> requestsToRemove =
            new List<(uint ComponentId, uint CommandId, uint InternalRequestId)>();

        public CommandMetaData()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(ICommandMetaDataStorage).IsAssignableFrom(type) && !type.IsAbstract)
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
            }
        }

        public void MarkIdForRemoval(uint componentId, uint commandId, uint internalRequestId)
        {
            requestsToRemove.Add((componentId, commandId, internalRequestId));
        }

        public void FlushRemovedIds()
        {
            foreach (var (componentId, commandId, internalRequestId) in requestsToRemove)
            {
                var s = GetCommandDiffStorage(componentId, commandId);
                s.RemoveMetaData(internalRequestId);
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
    }
}
