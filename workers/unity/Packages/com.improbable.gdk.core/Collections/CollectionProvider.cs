using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Collections
{
    internal static class CollectionProvider<TProvider, TValue> where TProvider : class, ICollection<TValue>, new()
    {
        private static readonly Dictionary<uint, TProvider> Storage = new Dictionary<uint, TProvider>();

        internal static void AllocateHandle(uint handle, TProvider initialValue = default(TProvider))
        {
            if (Storage.ContainsKey(handle))
            {
                throw new InvalidOperationException(
                    $"CollectionProvider<{typeof(TProvider)}> already contains handle `{handle}`");
            }

            Storage.Add(handle, initialValue ?? new TProvider());
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
