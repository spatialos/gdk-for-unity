using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public static class ReferenceProvider<T>
    {
        private static readonly Dictionary<ReferenceHandle, T> Storage = new Dictionary<ReferenceHandle, T>();

        private static uint nextHandleId = 0;

        public readonly struct ReferenceHandle : IDisposable, IEquatable<ReferenceHandle>
        {
            private readonly uint handleId;

            internal ReferenceHandle(uint handleId)
            {
                this.handleId = handleId;
            }

            public T Get()
            {
                return ReferenceProvider<T>.Get(this);
            }

            public void Set(T value)
            {
                ReferenceProvider<T>.Set(this, value);
            }

            public void Dispose()
            {
                ReferenceProvider<T>.Dispose(this);
            }

            public bool Equals(ReferenceHandle other)
            {
                return handleId == other.handleId;
            }

            public override bool Equals(object obj)
            {
                return obj is ReferenceHandle other && Equals(other);
            }

            public override int GetHashCode()
            {
                return (int) handleId;
            }

            public override string ToString()
            {
                return $"({typeof(T)}:{handleId})";
            }
        }

        public static ReferenceHandle Create()
        {
            var handle = GetNextHandle();

            Storage.Add(handle, default);

            return handle;
        }

        private static T Get(ReferenceHandle handle)
        {
            if (!Storage.TryGetValue(handle, out var value))
            {
                throw new ArgumentException($"{typeof(ReferenceProvider<T>)} does not contain handle {handle}");
            }

            return value;
        }

        private static void Set(ReferenceHandle handle, T value)
        {
            if (!Storage.ContainsKey(handle))
            {
                throw new ArgumentException($"{typeof(ReferenceProvider<T>)} does not contain handle {handle}");
            }

            Storage[handle] = value;
        }

        private static void Dispose(ReferenceHandle handle)
        {
            Storage.Remove(handle);
        }

        private static ReferenceHandle GetNextHandle()
        {
            nextHandleId++;

            while (Storage.ContainsKey(new ReferenceHandle(nextHandleId)))
            {
                nextHandleId++;
            }

            return new ReferenceHandle(nextHandleId);
        }

        internal static int Count => Storage.Count;
    }
}
