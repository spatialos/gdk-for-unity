using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    internal class ReaderWriterStore
    {
        private readonly Dictionary<MonoBehaviour, Dictionary<uint, IReaderWriterInternal[]>> behaviourToReadersWriters
            = new Dictionary<MonoBehaviour, Dictionary<uint, IReaderWriterInternal[]>>();

        private readonly Dictionary<uint, HashSet<IReaderWriterInternal>> componentIdToReadersWriters =
            new Dictionary<uint, HashSet<IReaderWriterInternal>>();

        public void AddReaderWritersForBehaviour(MonoBehaviour behaviour,
            Dictionary<uint, IReaderWriterInternal[]> componentIdsToReaderWriterLists)
        {
            behaviourToReadersWriters.Add(behaviour, componentIdsToReaderWriterLists);
            foreach (var idToReaderWriterList in componentIdsToReaderWriterLists)
            {
                var id = idToReaderWriterList.Key;
                var readerWriterList = idToReaderWriterList.Value;
                foreach (var readerWriter in readerWriterList)
                {
                    if (!componentIdToReadersWriters.TryGetValue(id, out var readersWriters))
                    {
                        readersWriters = new HashSet<IReaderWriterInternal>();
                        componentIdToReadersWriters.Add(id, readersWriters);
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
                var readerWritersToComponent = componentIdToReadersWriters[id];
                foreach (var readerWriter in readerWriterList)
                {
                    readerWritersToComponent.Remove(readerWriter);
                }
            }

            behaviourToReadersWriters.Remove(behaviour);
        }

        public bool TryGetReaderWritersForComponent(uint componentId, out HashSet<IReaderWriterInternal> readers)
        {
            return componentIdToReadersWriters.TryGetValue(componentId, out readers);
        }
    }
}
