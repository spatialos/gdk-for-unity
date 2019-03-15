using System;
using System.Collections.Concurrent;
using System.Threading;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    internal class ThreadedDeserializationHandler : IDisposable
    {
        private static readonly ViewDiff EmptyDiff = new ViewDiff();

        private readonly CommandMetaDataManager commandMetaDataManager;
        private readonly BlockingCollection<OpList> opsToDeserialize;
        private readonly ConcurrentOpListConverter converter;

        private readonly Thread deserializationThread;

        public ThreadedDeserializationHandler(CommandMetaDataManager commandMetaDataManager)
        {
            this.commandMetaDataManager = commandMetaDataManager;

            opsToDeserialize = new BlockingCollection<OpList>(new ConcurrentQueue<OpList>());
            converter = new ConcurrentOpListConverter();

            deserializationThread = new Thread(DeserializeOps);
            deserializationThread.Start();
        }

        public void GetDiff(ref ViewDiff viewDiff)
        {
            if (converter.TryGetViewDiff(out var diff))
            {
                viewDiff = diff;
            }
            else
            {
                viewDiff = EmptyDiff;
            }
        }

        public void AddOpListToDeserialize(OpList opList)
        {
            opsToDeserialize.Add(opList);
        }

        private void DeserializeOps()
        {
            while (opsToDeserialize.TryTake(out var opList, -1))
            {
                var metaData = commandMetaDataManager.GetAllCommandMetaData();
                converter.ParseOpListIntoDiff(opList, metaData);
                opList.Dispose();
            }
        }

        public void Dispose()
        {
            // Signal that no more ops will be added and wait for the deserialization thread to finish.
            opsToDeserialize.CompleteAdding();
            deserializationThread.Join();

            // Dispose of un-deserialized op lists.
            while (!opsToDeserialize.IsCompleted)
            {
                opsToDeserialize.Take().Dispose();
            }

            opsToDeserialize.Dispose();
        }
    }
}
