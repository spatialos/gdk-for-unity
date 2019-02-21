using System;
using System.Collections;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Wrapper around an array for use in command and update managers
    ///     Returns values by ref readonly
    /// </summary>
    /// <remarks>
    ///     Should only be used with readonly structs
    ///     Missing most safety checks as it is for internal use only
    ///     T constrained to struct so that null checks aren't needed
    ///     Does not detect the array being edited during iteration
    ///     The internal array is not resized or zeroed on clear
    /// </remarks>
    internal class ReceivedMessageList<T> : IEnumerable<T> where T : struct
    {
        private static readonly T[] EmptyArray = new T[0];

        private T[] items;

        public int Count { get; private set; }

        // Todo should probably have a starting size and a max size
        public ReceivedMessageList()
        {
            Count = 0;
            items = EmptyArray;
        }

        public ref readonly T this[int index] => ref items[index];

        public void Add(T item)
        {
            if (items.Length <= Count)
            {
                int targetLength = items.Length == 0 ? 4 : items.Length * 2;
                var temp = new T[targetLength];
                Array.Copy(items, temp, Count);
                items = temp;
            }

            items[Count] = item;
            ++Count;
        }

        // Similar to the List<> RemoveAll. No return value and the array is not resized down.
        public void RemoveAll(Predicate<T> match)
        {
            int freeIndex = 0;

            while (freeIndex < Count && !match(items[freeIndex]))
            {
                ++freeIndex;
            }

            if (freeIndex >= Count)
            {
                return;
            }

            int currentIndex = freeIndex + 1;
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

        public void Clear()
        {
            Count = 0;
        }

        public void Sort(IComparer<T> comparer)
        {
            Array.Sort(items, 0, Count, comparer);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<T>
        {
            private readonly ReceivedMessageList<T> list;

            private int currentIndex;

            public Enumerator(ReceivedMessageList<T> list)
            {
                this.list = list;
                currentIndex = -1;
            }

            public bool MoveNext()
            {
                ++currentIndex;

                if (currentIndex >= list.Count)
                {
                    currentIndex = -1;
                    return false;
                }

                return true;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public T Current
            {
                get
                {
                    if (currentIndex == -1)
                    {
                        return default(T);
                    }

                    return list[currentIndex];
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}
