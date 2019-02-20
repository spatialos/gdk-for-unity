using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class ListPool<T>
    {
        private readonly Stack<List<T>> pool = new Stack<List<T>>();

        private int maxBufferLength;
        private int minBufferLength;
        private int maxPoolSize;

        public ListPool(int maxBufferLength, int minBufferLength, int maxPoolSize)
        {
            this.maxBufferLength = maxBufferLength;
            this.minBufferLength = minBufferLength;
            this.maxPoolSize = maxPoolSize;
        }

        public List<T> Rent()
        {
            if (pool.Count == 0)
            {
                return new List<T>(minBufferLength);
            }

            return pool.Pop();
        }

        public void Return(List<T> list)
        {
            if (pool.Count >= maxPoolSize)
            {
                return;
            }

            list.Clear();
            if (list.Capacity > maxBufferLength)
            {
                list.Capacity = minBufferLength;
            }

            pool.Push(list);
        }
    }
}
