using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Collections
{
    internal static class CollectionProvider<TProvider, TValue> where TProvider : ICollection<TValue>, new()
    {
        private static readonly Dictionary<uint, TProvider> Storage = new Dictionary<uint, TProvider>();

        internal static void AllocateHandle(uint handle)
        {
            if (Storage.ContainsKey(handle))
            {
                throw new InvalidOperationException(
                    $"CollectionProvider<{typeof(TProvider)}> already contains handle `{handle}`");
            }

            var item = new TProvider();
            Storage.Add(handle, item);
        }

        internal static TProvider Get(uint handle)
        {
            if (!Storage.TryGetValue(handle, out var item))
            {
                throw new ArgumentException(
                    $"CollectionProvider<{typeof(TProvider)}> does not contain handle `{handle}`");
            }

            return item;
        }

        internal static void FreeHandle(uint handle)
        {
            if (!Storage.TryGetValue(handle, out var item))
            {
                throw new ArgumentException(
                    $"CollectionProvider<{typeof(TProvider)}> does not contain handle `{handle}`");
            }

            item.Clear();
            Storage.Remove(handle);
        }
    }
}
