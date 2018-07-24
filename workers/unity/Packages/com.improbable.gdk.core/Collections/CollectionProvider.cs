using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Collections
{
    internal static class CollectionProvider<TCollection, TValue> where TCollection : class, ICollection<TValue>, new()
    {
        private static readonly Dictionary<uint, TCollection> Storage = new Dictionary<uint, TCollection>();

        internal static void AllocateHandle(uint handle, TCollection initialValue = default(TCollection))
        {
            if (Storage.ContainsKey(handle))
            {
                throw new InvalidOperationException(
                    $"CollectionProvider<{typeof(TCollection)}> already contains handle `{handle}`");
            }

            Storage.Add(handle, initialValue ?? new TCollection());
        }

        internal static TCollection Get(uint handle)
        {
            if (!Storage.TryGetValue(handle, out var item))
            {
                throw new ArgumentException(
                    $"CollectionProvider<{typeof(TCollection)}> does not contain handle `{handle}`");
            }

            return item;
        }

        internal static void FreeHandle(uint handle)
        {
            if (!Storage.TryGetValue(handle, out var item))
            {
                throw new ArgumentException(
                    $"CollectionProvider<{typeof(TCollection)}> does not contain handle `{handle}`");
            }

            item.Clear();
            Storage.Remove(handle);
        }
    }
}
