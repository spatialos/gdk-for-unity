using System;
using System.Collections.Generic;

namespace Assets.Gdk.Core.Collections
{
    internal static class HandleProvider
    {
        private static uint nextHandle = 0;
        private static Dictionary<uint, Action<uint>> freeActions;

        public static uint AllocateListHandle<TValue>()
        {
            var handle = nextHandle++;
            ListProvider<TValue>.AllocateHandle(handle);
            freeActions.Add(handle, ListProvider<TValue>.FreeHandle);
            return handle;
        }

        public static uint AllocateMapHandle<TValue, TKey>()
        {
            var handle = nextHandle++;
            MapProvider<TValue, TKey>.AllocateHandle(handle);
            freeActions.Add(handle, MapProvider<TValue, TKey>.FreeHandle);
            return handle;
        }

        public static void FreeHandle(uint handle)
        {
            freeActions[handle](handle);
        }
    }
}
