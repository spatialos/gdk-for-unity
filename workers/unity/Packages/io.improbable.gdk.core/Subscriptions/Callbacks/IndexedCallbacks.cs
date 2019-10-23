using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Subscriptions
{
    public class IndexedCallbacks<T>
    {
        private readonly Dictionary<long, Callbacks<T>> indexedCallbacks = new Dictionary<long, Callbacks<T>>();
        private readonly Dictionary<ulong, long> callbackKeyToIndex = new Dictionary<ulong, long>();

        public void Add(long index, ulong callbackKey, Action<T> value)
        {
            if (!indexedCallbacks.TryGetValue(index, out var callbacks))
            {
                callbacks = new Callbacks<T>();
                indexedCallbacks.Add(index, callbacks);
            }

            callbackKeyToIndex.Add(callbackKey, index);
            callbacks.Add(callbackKey, value);
        }

        public bool Remove(ulong callbackKey)
        {
            if (!callbackKeyToIndex.TryGetValue(callbackKey, out var index))
            {
                return false;
            }

            indexedCallbacks[index].Remove(callbackKey);
            callbackKeyToIndex.Remove(callbackKey);
            return true;
        }

        public void InvokeAll(long index, T argument)
        {
            if (indexedCallbacks.TryGetValue(index, out var callbacks))
            {
                callbacks.InvokeAll(argument);
            }
        }

        public void InvokeAllReverse(long index, T argument)
        {
            if (indexedCallbacks.TryGetValue(index, out var callbacks))
            {
                callbacks.InvokeAllReverse(argument);
            }
        }
    }
}
