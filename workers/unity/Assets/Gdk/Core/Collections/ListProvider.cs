using System;
using System.Collections.Generic;

namespace Assets.Gdk.Core.Collections
{
    internal static class ListProvider<T>
    {
        private static readonly Dictionary<uint, List<T>> Storage = new Dictionary<uint, List<T>>();

        internal static void AllocateHandle(uint handle)
        {
            var item = new List<T>();
            Storage.Add(handle, item);
        }

        internal static List<T> Get(uint handle)
        {
            return Storage[handle];
        }

        internal static void FreeHandle(uint handle)
        {
            List<T> list;
            if (!Storage.TryGetValue(handle, out list))
            {
                throw new ArgumentException($"ListProvider<{typeof(T).Name}> does not contain handle `{handle}`");
            }

            list.Clear();
            Storage.Remove(handle);
        }
    }
}
