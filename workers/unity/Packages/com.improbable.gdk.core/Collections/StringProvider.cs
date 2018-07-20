using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Collections
{
    internal static class StringProvider
    {
        private static readonly Dictionary<uint, string> Storage = new Dictionary<uint, string>();

        internal static void AllocateHandle(uint handle)
        {
            if (Storage.ContainsKey(handle))
            {
                throw new InvalidOperationException($"StringProvider already contains handle `{handle}`");
            }

            var item = string.Empty;
            Storage.Add(handle, item);
        }

        internal static string Get(uint handle)
        {
            if (!Storage.TryGetValue(handle, out var item))
            {
                throw new ArgumentException($"StringProvider does not contain handle `{handle}`");
            }

            return item;
        }

        internal static void Set(uint handle, string value)
        {
            if (!Storage.ContainsKey(handle))
            {
                throw new ArgumentException($"StringProvider does not contain handle `{handle}`");
            }

            Storage[handle] = value;
        }

        internal static void FreeHandle(uint handle)
        {
            if (!Storage.TryGetValue(handle, out var item))
            {
                throw new ArgumentException($"StringProvider does not contain handle `{handle}`");
            }

            Storage.Remove(handle);
        }
    }
}
