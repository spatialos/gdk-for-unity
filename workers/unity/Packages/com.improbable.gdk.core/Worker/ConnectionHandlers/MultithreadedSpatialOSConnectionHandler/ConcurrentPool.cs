using System.Collections.Concurrent;

namespace Improbable.Gdk.Core
{
    internal class ConcurrentPool<T> where T : new()
    {
        private readonly ConcurrentStack<T> pool = new ConcurrentStack<T>();

        public T Rent()
        {
            if (pool.TryPop(out var obj))
            {
                return obj;
            }

            return new T();
        }

        public void Return(T obj)
        {
            pool.Push(obj);
        }
    }
}
