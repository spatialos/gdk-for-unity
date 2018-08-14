using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    internal class ReaderWriterStore
    {
        private readonly Dictionary<MonoBehaviour, Dictionary<uint, IReaderWriterInternal[]>> behaviourToReadersWriters
            = new Dictionary<MonoBehaviour, Dictionary<uint, IReaderWriterInternal[]>>();

        private readonly Dictionary<uint, HashSet<IReaderWriterInternal>> compIdToReadersWriters =
            new Dictionary<uint, HashSet<IReaderWriterInternal>>();

        public void AddReaderWritersForBehaviour(MonoBehaviour behaviour,
            Dictionary<uint, IReaderWriterInternal[]> idsToReaderWriterLists)
        {
            behaviourToReadersWriters[behaviour] = idsToReaderWriterLists;
            foreach (var idToReaderWriterList in idsToReaderWriterLists)
            {
                var id = idToReaderWriterList.Key;
                var readerWriterList = idToReaderWriterList.Value;
                foreach (var readerWriter in readerWriterList)
                {
                    if (!compIdToReadersWriters.TryGetValue(id, out var readersWriters))
                    {
                        readersWriters = new HashSet<IReaderWriterInternal>();
                        compIdToReadersWriters[id] = readersWriters;
                    }

                    readersWriters.Add(readerWriter);
                }
            }
        }

        public void RemoveReaderWritersForBehaviour(MonoBehaviour behaviour)
        {
            foreach (var idToReaderWriterList in behaviourToReadersWriters[behaviour])
            {
                var id = idToReaderWriterList.Key;
                var readerWriterList = idToReaderWriterList.Value;
                foreach (var readerWriter in readerWriterList)
                {
                    compIdToReadersWriters[id].Remove(readerWriter);
                }
            }

            behaviourToReadersWriters.Remove(behaviour);
        }

        public bool TryGetReaderWritersForComponent(uint componentId, out HashSet<IReaderWriterInternal> readers)
        {
            return compIdToReadersWriters.TryGetValue(componentId, out readers);
        }
    }
}
