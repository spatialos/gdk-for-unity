using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public readonly ref struct ComponentUpdateSlice<T> where T : ISpatialComponentUpdate
    {
        private readonly List<ComponentUpdateReceived<T>> updates;
        private readonly EntityId entityId;

        private readonly int firstIndex;
        private readonly int lastIndex;

        public ComponentUpdateSlice(List<ComponentUpdateReceived<T>> updates, EntityId entityId)
        {
            this.updates = updates;
            this.entityId = entityId;

            firstIndex = -1;
            lastIndex = -1;

            // todo replace with binary search that finds first and last instances of the element
            // or change the data structure this is being based on
            for (int i = 0; i < updates.Count; ++i)
            {
                if (updates[i].EntityId == entityId && firstIndex == -1)
                {
                    firstIndex = i;
                    continue;
                }

                if (!(updates[i].EntityId == entityId || firstIndex == -1))
                {
                    lastIndex = i;
                    break;
                }
            }
        }

        public int Count => lastIndex - firstIndex;

        public ComponentUpdateReceived<T> this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException("Component update slice is empty");
                }

                return updates[firstIndex + index];
            }
        }
    }
}
