using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    // Slight adjustment to the component callbacks class
    // todo check if this can have in params for ops (i.e. can it be used with a non readonly struct)
    public class IndexedCallbacks<T>
    {
        private readonly Dictionary<long, Callbacks<T>> callbacks = new Dictionary<long, Callbacks<T>>();
        private readonly Dictionary<ulong, long> callbackKeyToIndex = new Dictionary<ulong, long>();

        public void Add(long index, ulong callbackKey, Action<T> value)
        {
            if (!callbacks.TryGetValue(index, out var callbacksForIndex))
            {
                callbacksForIndex = new Callbacks<T>();
                callbacks.Add(index, callbacksForIndex);
            }

            callbackKeyToIndex.Add(callbackKey, index);
            callbacks[index].Add(callbackKey, value);
        }

        public bool Remove(ulong callbackKey)
        {
            if (!callbackKeyToIndex.TryGetValue(callbackKey, out var index))
            {
                return false;
            }

            callbacks[index].Remove(callbackKey);
            callbackKeyToIndex.Remove(callbackKey);
            return true;
        }

        public void InvokeAll(long index, T op)
        {
            if (callbacks.ContainsKey(index))
            {
                callbacks[index].InvokeAll(op);
            }
        }

        public void InvokeAllReverse(long index, T op)
        {
            if (callbacks.ContainsKey(index))
            {
                callbacks[index].InvokeAllReverse(op);
            }
        }
    }

    // Efficient ability to remove all callbacks for an index, but slow to remove a single callback
    public class SingleUseIndexCallbacks<T>
    {
        private readonly Dictionary<long, Callbacks<T>> callbacks = new Dictionary<long, Callbacks<T>>();

        public void Add(long index, ulong callbackKey, Action<T> value)
        {
            if (!callbacks.TryGetValue(index, out var callbacksForIndex))
            {
                callbacksForIndex = new Callbacks<T>();
                callbacks.Add(index, callbacksForIndex);
            }

            callbacksForIndex.Add(callbackKey, value);
        }

        public void RemoveAllCallbacksForIndex(long index)
        {
            if (callbacks.ContainsKey(index))
            {
                callbacks.Remove(index);
            }
        }

        public bool Remove(ulong callbackKey)
        {
            foreach (var callback in callbacks)
            {
                if (callback.Value.Remove(callbackKey))
                {
                    return true;
                }
            }

            return false;
        }

        public void InvokeAll(long index, T op)
        {
            if (callbacks.ContainsKey(index))
            {
                callbacks[index].InvokeAll(op);
            }
        }

        public void InvokeAllReverse(long index, T op)
        {
            if (callbacks.ContainsKey(index))
            {
                callbacks[index].InvokeAllReverse(op);
            }
        }
    }
}
