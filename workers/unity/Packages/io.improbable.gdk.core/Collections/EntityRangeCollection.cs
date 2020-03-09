using System;
using System.Collections;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public class EntityRangeCollection : IEnumerable<EntityId>
    {
        private EntityIdRange firstElement;
        private readonly Queue<EntityIdRange> queue = new Queue<EntityIdRange>();
        private int version = 0;

        public uint Count { get; private set; } = 0;

        public EntityRangeCollection Take(uint count)
        {
            if (count > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Collection does not have enough ids stored");
            }

            var collection = new EntityRangeCollection();
            while (count > 0)
            {
                if (firstElement.Count <= count)
                {
                    count -= firstElement.Count;
                    collection.Add(firstElement);
                    firstElement = queue.Count > 0
                        ? queue.Dequeue()
                        : new EntityIdRange();
                }
                else
                {
                    var splitRange = new EntityIdRange(firstElement.FirstEntityId, count);
                    collection.Add(splitRange);
                    firstElement.FirstEntityId = new EntityId(firstElement.FirstEntityId.Id + count);
                    firstElement.Count -= count;
                    break;
                }
            }

            Count -= count;
            version++;

            return collection;
        }

        internal void Add(EntityIdRange range)
        {
            if (!range.FirstEntityId.IsValid())
            {
                throw new ArgumentException("Invalid EntityId in range", nameof(range));
            }

            if (range.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(range), "EntityId range cannot be 0 in length.");
            }

            if (firstElement.Count == 0)
            {
                firstElement = range;
            }
            else
            {
                queue.Enqueue(range);
            }

            Count += range.Count;
            version++;
        }

        public void Clear()
        {
            firstElement = new EntityIdRange();
            queue.Clear();
            Count = 0;
            version++;
        }

        internal struct EntityIdRange
        {
            public EntityId FirstEntityId;
            public uint Count;

            public EntityIdRange(EntityId firstEntityId, uint count)
            {
                Count = count;
                FirstEntityId = firstEntityId;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<EntityId> IEnumerable<EntityId>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<EntityId>
        {
            private EntityRangeCollection collection;

            private EntityIdRange activeRange;
            private Queue<EntityIdRange>.Enumerator enumerator;

            private int index;
            private int version;
            private EntityId current;

            public Enumerator(EntityRangeCollection collection)
            {
                this.collection = collection;
                activeRange = collection.firstElement;
                enumerator = collection.queue.GetEnumerator();

                index = 0;
                version = collection.version;
                current = default;
            }

            public bool MoveNext()
            {
                if (version != collection.version)
                {
                    throw new InvalidOperationException("Cannot iterate over collection which has been altered.");
                }

                if (index >= collection.Count)
                {
                    return false;
                }

                current = activeRange.FirstEntityId;
                activeRange.FirstEntityId = new EntityId(activeRange.FirstEntityId.Id + 1);
                --activeRange.Count;

                // Grab next range in the queue
                if (activeRange.Count == 0)
                {
                    activeRange = enumerator.MoveNext()
                        ? enumerator.Current
                        : default;
                }

                index++;

                return true;
            }

            public void Reset()
            {
                index = 0;
                activeRange = collection.firstElement;
                enumerator = collection.queue.GetEnumerator();
                current = default;
            }

            public EntityId Current
            {
                get
                {
                    return current;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}
