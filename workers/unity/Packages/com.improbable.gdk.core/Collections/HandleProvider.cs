using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Collections
{
    internal static class HandleProvider
    {
        private static uint nextHandle = 0;
        private static readonly Dictionary<uint, Action<uint>> FreeActions = new Dictionary<uint, Action<uint>>();

        public static uint AllocateListHandle<TValue>()
        {
            var handle = GetHandle();
            CollectionProvider<List<TValue>, TValue>.AllocateHandle(handle);
            FreeActions.Add(handle, CollectionProvider<List<TValue>, TValue>.FreeHandle);
            return handle;
        }

        public static uint AllocateMapHandle<TValue, TKey>()
        {
            var handle = GetHandle();
            CollectionProvider<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>.AllocateHandle(handle);
            FreeActions.Add(handle,
                CollectionProvider<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>.FreeHandle);
            return handle;
        }

        public static uint AllocateStringHandle()
        {
            var handle = GetHandle();
            StringProvider.AllocateHandle(handle);
            FreeActions.Add(handle, StringProvider.FreeHandle);
            return handle;
        }

        public static void FreeHandle(uint handle)
        {
            FreeActions[handle](handle);
            FreeActions.Remove(handle);
        }

        private static uint GetHandle()
        {
            // Check for duplicates in case of wraparound
            while (FreeActions.ContainsKey(++nextHandle))
            {
            }

            return nextHandle;
        }
    }
}
