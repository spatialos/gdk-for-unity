using System;
using System.Collections.Generic;

namespace Assets.Gdk.Core.Collections
{
    internal static class MapProvider<TKey, TValue>
    {
        private static readonly Dictionary<uint, Dictionary<TKey, TValue>> Storage = new Dictionary<uint, Dictionary<TKey, TValue>>();

        internal static void AllocateHandle(uint handle)
        {
            var item = new Dictionary<TKey, TValue>();
            Storage.Add(handle, item);
        }

        internal static Dictionary<TKey, TValue> Get(uint handle)
        {
            return Storage[handle];
        }

        internal static void FreeHandle(uint handle)
        {
            Dictionary<TKey, TValue> item;
            if (!Storage.TryGetValue(handle, out item))
            {
                throw new ArgumentException($"MapProvider<{typeof(TKey)}, {typeof(TValue)}> does not contain handle `{handle}`");
            }

            item.Clear();
            Storage.Remove(handle);
        }
    }
}
