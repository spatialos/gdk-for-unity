using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Wrapper around an array for use in command and update managers
    ///     Returns values by ref readonly
    /// </summary>
    /// <remarks>
    ///     Should only be used with readonly structs
    ///     Missing most safety checks as it is for internal use only
    ///     Does not detect the array being edited during iteration
    ///     The internal array is not resized or zeroed on clear
    /// </remarks>
    internal class MessageList<T> where T : struct
    {
        private static readonly T[] EmptyArray = new T[0];

        private T[] items;

        public int Count { get; private set; }

        public MessageList()
        {
            Count = 0;
            items = EmptyArray;
        }

        public ref readonly T this[int index] => ref items[index];

        public MessagesSpan<T> Slice()
        {
            return new MessagesSpan<T>(this, 0, Count);
        }

        public MessagesSpan<T> Slice(int index, int count)
        {
            Assert.IsTrue(index >= 0);
            Assert.IsTrue(index + count <= Count);
            return count == 0
                ? MessagesSpan<T>.Empty()
                : new MessagesSpan<T>(this, index, count);
        }

        public void Add(in T item)
        {
            if (items.Length <= Count)
            {
                var targetLength = items.Length == 0 ? 4 : items.Length * 2;
                var temp = new T[targetLength];
                Array.Copy(items, temp, Count);
                items = temp;
            }

            items[Count] = item;
            ++Count;
        }

        public void Insert(int index, in T item)
        {
            if (items.Length <= Count)
            {
                var targetLength = items.Length == 0 ? 4 : items.Length * 2;
                var temp = new T[targetLength];
                Array.Copy(items, 0, temp, 0, index);
                temp[index] = item;
                Array.Copy(items, index, temp, index + 1, Count - index);
                items = temp;
            }
            else
            {
                Array.Copy(items, index, items, index + 1, Count - index);
                items[index] = item;
            }

            ++Count;
        }

        public void InsertSorted(in T item, IComparer<T> comparer)
        {
            var index = Array.BinarySearch(items, 0, Count, item, comparer);
            if (index < 0)
            {
                Insert(~index, in item);
            }
            else
            {
                Insert(index, in item);
            }
        }

        // Similar to the List<> RemoveAll. No return value and the array is not resized down.
        public void RemoveAll(Predicate<T> match)
        {
            var freeIndex = 0;
            while (freeIndex < Count && !match(items[freeIndex]))
            {
                ++freeIndex;
            }

            if (freeIndex >= Count)
            {
                return;
            }

            var currentIndex = freeIndex + 1;
            while (currentIndex < Count)
            {
                while (currentIndex < Count && match(items[currentIndex]))
                {
                    ++currentIndex;
                }

                if (currentIndex < Count)
                {
                    items[freeIndex++] = items[currentIndex++];
                }
            }

            Count = freeIndex;
        }

        public void CopyTo(MessageList<T> other)
        {
            if (other.items.Length < Count)
            {
                other.items = new T[Count];
            }

            Array.Copy(items, other.items, Count);
            other.Count = Count;
        }

        public void Clear()
        {
            Count = 0;
        }

        public void Sort(IComparer<T> comparer)
        {
            Array.Sort(items, 0, Count, comparer);
        }
    }
}
