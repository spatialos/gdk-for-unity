using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Subscriptions
{
    internal class Callbacks<T>
    {
        private List<WrappedCallback> callbacks = new List<WrappedCallback>();
        private HashSet<ulong> currentKeys = new HashSet<ulong>();

        // These can be lists as we expect the number of callbacks added/removed during invocation of other callbacks
        // to be low.
        private List<ulong> toRemove = new List<ulong>();
        private List<WrappedCallback> toAdd = new List<WrappedCallback>();

        private bool isInInvoke = false;

        public void Add(ulong key, Action<T> callback)
        {
            if (isInInvoke)
            {
                toAdd.Add(new WrappedCallback(key, callback));
                return;
            }

            callbacks.Add(new WrappedCallback(key, callback));
            currentKeys.Add(key);
        }

        public bool Remove(ulong key)
        {
            if (!currentKeys.Contains(key))
            {
                return false;
            }

            if (isInInvoke)
            {
                if (toRemove.Contains(key))
                {
                    return false;
                }

                toRemove.Add(key);
                return true;
            }

            currentKeys.Remove(key);
            return callbacks.RemoveAll(callback => callback.Key == key) == 1;
        }

        public void InvokeAll(T op)
        {
            isInInvoke = true;

            try
            {
                for (int i = 0; i < callbacks.Count; i++)
                {
                    callbacks[i].Action(op);
                }
            }
            finally
            {
                isInInvoke = false;
            }

            FlushDeferredOperations();
        }

        public void InvokeAllReverse(T op)
        {
            isInInvoke = true;

            try
            {
                for (int i = callbacks.Count - 1; i >= 0; i--)
                {
                    callbacks[i].Action(op);
                }
            }
            finally
            {
                isInInvoke = false;
            }

            FlushDeferredOperations();
        }

        private void FlushDeferredOperations()
        {
            callbacks.RemoveAll(callback => toRemove.Contains(callback.Key));

            foreach (var key in toRemove)
            {
                currentKeys.Remove(key);
            }

            foreach (var callback in toAdd)
            {
                callbacks.Add(callback);
            }

            toRemove.Clear();
            toAdd.Clear();
        }

        private struct WrappedCallback
        {
            public ulong Key;
            public Action<T> Action;

            public WrappedCallback(ulong key, Action<T> action)
            {
                Key = key;
                Action = action;
            }
        }
    }

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
