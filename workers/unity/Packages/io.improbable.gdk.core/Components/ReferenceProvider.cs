using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.Core
{
    public static class ReferenceProvider<T>
    {
        private static readonly Dictionary<ReferenceHandle, T> Storage = new Dictionary<ReferenceHandle, T>();
        private static readonly Dictionary<ReferenceHandle, global::Unity.Entities.World> WorldMapping = new Dictionary<ReferenceHandle, global::Unity.Entities.World>();

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
        }

        public static ReferenceHandle Create(global::Unity.Entities.World world)
        {
            var handle = GetNextHandle();

            Storage.Add(handle, default);
            WorldMapping.Add(handle, world);

            return handle;
        }

        private static T Get(ReferenceHandle handle)
        {
            if (!Storage.TryGetValue(handle, out var value))
            {
                throw new ArgumentException($"EntityTypeProvider does not contain handle {handle}");
            }

            return value;
        }

        private static void Set(ReferenceHandle handle, T value)
        {
            if (!Storage.ContainsKey(handle))
            {
                throw new ArgumentException($"EntityTypeProvider does not contain handle {handle}");
            }

            Storage[handle] = value;
        }

        private static void Dispose(ReferenceHandle handle)
        {
            Storage.Remove(handle);
            WorldMapping.Remove(handle);
        }

        public static void CleanDataInWorld(global::Unity.Entities.World world)
        {
            var handles = WorldMapping
                .Where(pair => pair.Value == world)
                .Select(pair => pair.Key)
                .ToList();

            foreach (var handle in handles)
            {
                handle.Dispose();
            }
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
    }
}
