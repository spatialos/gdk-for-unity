#if !DISABLE_REACTIVE_COMPONENTS
using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    public static class AuthorityChangesProvider
    {
        private static readonly Dictionary<uint, List<Authority>> Storage = new Dictionary<uint, List<Authority>>();
        private static readonly Dictionary<uint, World> WorldMapping = new Dictionary<uint, World>();

        private static uint nextHandle;

        public static uint Allocate(World world)
        {
            var handle = GetNextHandle();
            Storage.Add(handle, default(List<Authority>));
            WorldMapping.Add(handle, world);

            return handle;
        }

        public static List<Authority> Get(uint handle)
        {
            if (!Storage.TryGetValue(handle, out var value))
            {
                throw new ArgumentException($"AuthorityChangesProvider does not contain handle {handle}");
            }

            return value;
        }

        public static void Set(uint handle, List<Authority> value)
        {
            if (!Storage.ContainsKey(handle))
            {
                throw new ArgumentException($"AuthorityChangesProvider does not contain handle {handle}");
            }

            Storage[handle] = value;
        }

        public static void Free(uint handle)
        {
            Storage.Remove(handle);
            WorldMapping.Remove(handle);
        }

        public static void CleanDataInWorld(World world)
        {
            var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();

            foreach (var handle in handles)
            {
                Free(handle);
            }
        }

        private static uint GetNextHandle()
        {
            nextHandle++;

            while (Storage.ContainsKey(nextHandle))
            {
                nextHandle++;
            }

            return nextHandle;
        }
    }
}
#endif
