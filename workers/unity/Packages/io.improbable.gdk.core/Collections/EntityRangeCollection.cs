using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class EntityRangeCollection
    {
        private EntityIdRange firstElement;
        private readonly Queue<EntityIdRange> queue = new Queue<EntityIdRange>();

        public uint Count { get; private set; } = 0;

        public EntityId Dequeue()
        {
            if (firstElement.Count <= 0)
            {
                throw new InvalidOperationException($"{nameof(EntityRangeCollection)} is empty");
            }

            var (splitRange, remainder) = firstElement.Split(1);
            firstElement = remainder;
            Count -= 1;
            return splitRange.FirstEntityId;
        }

        public EntityId[] Take(uint count)
        {
            if (count > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Collection does not have enough stored IDs.");
            }

            var collection = new EntityId[count];
            uint index = 0;
            while (count > 0)
            {
                if (firstElement.Count <= count)
                {
                    count -= firstElement.Count;
                    firstElement.CopyTo(collection, index);
                    index += firstElement.Count;
                    firstElement = queue.Count > 0
                        ? queue.Dequeue()
                        : new EntityIdRange();
                }
                else
                {
                    var (splitRange, remainder) = firstElement.Split(count);
                    firstElement = remainder;
                    splitRange.CopyTo(collection, index);
                    break;
                }
            }

            Count -= count;

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
        }

        public void Clear()
        {
            firstElement = new EntityIdRange();
            queue.Clear();
            Count = 0;
        }

        internal readonly struct EntityIdRange
        {
            public readonly EntityId FirstEntityId;
            public readonly uint Count;

            public EntityIdRange(EntityId firstEntityId, uint count)
            {
                Count = count;
                FirstEntityId = firstEntityId;
            }

            public (EntityIdRange part, EntityIdRange remainder) Split(uint count)
            {
                if (count > Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count), count, "Split count out of range");
                }

                var part = new EntityIdRange(FirstEntityId, count);
                var remainder = new EntityIdRange(new EntityId(FirstEntityId.Id + count), Count - count);

                return (part, remainder);
            }

            public void CopyTo(EntityId[] collection, uint index)
            {
                if (collection.Length < index + Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, "Target array too small for given index");
                }

                for (var i = 0; i < Count; i++)
                {
                    collection[index + i] = new EntityId(FirstEntityId.Id + i);
                }
            }
        }
    }
}
