using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Subscriptions
{
    // Efficient ability to remove all callbacks for an index, but slow to remove a single callback
    public class SingleUseIndexCallbacks<T>
    {
        private readonly Dictionary<long, Callbacks<T>> indexedCallbacks = new Dictionary<long, Callbacks<T>>();

        public void Add(long index, ulong callbackKey, Action<T> value)
        {
            if (!indexedCallbacks.TryGetValue(index, out var callbacksForIndex))
            {
                callbacksForIndex = new Callbacks<T>();
                indexedCallbacks.Add(index, callbacksForIndex);
            }

            callbacksForIndex.Add(callbackKey, value);
        }

        public void RemoveAllCallbacksForIndex(long index)
        {
            indexedCallbacks.Remove(index);
        }

        public bool Remove(ulong callbackKey)
        {
            foreach (var pair in indexedCallbacks)
            {
                var callbacks = pair.Value;
                if (callbacks.Remove(callbackKey))
                {
                    return true;
                }
            }

            return false;
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
