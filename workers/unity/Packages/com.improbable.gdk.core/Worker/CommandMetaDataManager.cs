using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class CommandMetaDataManager
    {
        private readonly CommandMetaDataAggregate aggregate = new CommandMetaDataAggregate();

        private readonly List<CommandMetaData> metaDataToAdd = new List<CommandMetaData>();

        private readonly ConcurrentPool<CommandMetaData> pool = new ConcurrentPool<CommandMetaData>();

        public CommandMetaDataAggregate GetAllCommandMetaData()
        {
            aggregate.FlushRemovedIds();

            aggregate.ReturnEmptyStorageToPool(pool);

            lock (metaDataToAdd)
            {
                foreach (var data in metaDataToAdd)
                {
                    aggregate.AddMetaData(data);
                }

                metaDataToAdd.Clear();
            }

            return aggregate;
        }

        public void AddMetaData(CommandMetaData commandMetaData)
        {
            if (commandMetaData.IsEmpty())
            {
                pool.Return(commandMetaData);
            }
            else
            {
                lock (metaDataToAdd)
                {
                    metaDataToAdd.Add(commandMetaData);
                }
            }
        }

        public CommandMetaData GetEmptyMetaDataStorage()
        {
            return pool.Rent();
        }
    }
}
