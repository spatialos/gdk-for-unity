using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public readonly ref struct ComponentUpdateSlice<T> where T : ISpatialComponentUpdate
    {
        private readonly List<ComponentUpdateReceived<T>> updates;

        private readonly int firstIndex;

        public ComponentUpdateSlice(List<ComponentUpdateReceived<T>> updates, EntityId entityId)
        {
            this.updates = updates;

            firstIndex = -1;
            var lastIndex = -1;

            firstIndex = GetFirstIndex(entityId.Id, updates, out var lastIndexUpperBound);

            if (firstIndex != -1)
            {
                lastIndex = GetLastIndex(firstIndex, lastIndexUpperBound, entityId.Id, updates);
                Count = lastIndex - firstIndex + 1;
            }
            else
            {
                Count = 0;
            }
        }

        public int Count { get; }

        // todo consider storing updates in an array rather than a list to allow this to be a ref return
        public ComponentUpdateReceived<T> this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }

                return updates[firstIndex + index];
            }
        }

        public ComponentUpdateReceived<T>[] ToArray()
        {
            var array = new ComponentUpdateReceived<T>[Count];
            for (int i = 0; i < Count; ++i)
            {
                array[i] = this[i];
            }

            return array;
        }

        // binary search for the first update for given entity ID
        // invariant: lower < target <= upper
        private static int GetFirstIndex(long targetId, List<ComponentUpdateReceived<T>> updates,
            out int lastIndexUpperBound)
        {
            int lower = -1;
            int upper = updates.Count - 1;
            lastIndexUpperBound = updates.Count;

            while (upper > lower)
            {
                var index = (lower + upper + 1) / 2;

                long id = updates[index].EntityId.Id;

                if (id == targetId)
                {
                    if (index == 0 || updates[index - 1].EntityId.Id != targetId)
                    {
                        return index;
                    }

                    upper = index;
                }
                else if (id > targetId)
                {
                    lastIndexUpperBound = index;
                    upper = index - 1;
                }
                else if (id < targetId)
                {
                    lower = index;
                }
            }

            return -1;
        }

        // binary search for the last update for given entity ID with bounds
        // invariant: lower <= target < upper
        // starting bounds must obey the invariant
        private static int GetLastIndex(int lowerBound, int upperBound, long targetId,
            List<ComponentUpdateReceived<T>> updates)
        {
            int lower = lowerBound;
            int upper = upperBound;

            while (upper > lower)
            {
                var index = (lower + upper) / 2;

                long id = updates[index].EntityId.Id;

                if (id == targetId)
                {
                    if (index == updates.Count - 1 || updates[index + 1].EntityId.Id != targetId)
                    {
                        return index;
                    }

                    lower = index + 1;
                }
                else if (id > targetId)
                {
                    upper = index;
                }
                else if (id < targetId)
                {
                    lower = index + 1;
                }
            }

            return -1;
        }
    }
}
