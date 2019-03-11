using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public class CommandMetaDataAggregate
    {
        private readonly List<CommandMetaData> metaData = new List<CommandMetaData>();

        public void MarkIdForRemoval(uint componentId, uint commandId, uint internalRequestId)
        {
            foreach (var data in metaData)
            {
                if (data.ContainsCommandMetaData(internalRequestId))
                {
                    data.MarkIdForRemoval(componentId, commandId, internalRequestId);
                }
            }
        }

        public void FlushRemovedIds()
        {
            foreach (var data in metaData)
            {
                data.FlushRemovedIds();
            }
        }

        public CommandContext<T> GetContext<T>(uint componentId, uint commandId, uint internalRequestId)
        {
            foreach (var data in metaData)
            {
                if (data.ContainsCommandMetaData(internalRequestId))
                {
                    return data.GetContext<T>(componentId, commandId, internalRequestId);
                }
            }

            throw new ArgumentException($"Can not find meta data for internal request ID {internalRequestId}");
        }

        internal void AddMetaData(CommandMetaData commandMetaData)
        {
            metaData.Add(commandMetaData);
        }

        internal void ReturnEmptyStorageToPool(ConcurrentPool<CommandMetaData> pool)
        {
            for (int i = 0; i < metaData.Count; ++i)
            {
                if (!metaData[i].IsEmpty())
                {
                    continue;
                }

                pool.Return(metaData[i]);
                metaData.RemoveAt(i);
                ReturnEmptyStorageToPool(pool);
            }
        }
    }
}
