using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Subscriptions
{
    internal class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }

    // We need to avoid changing the underlying map while iterating over it. For example, if a
    // callback removes itself, a call to InvokeAll iterates over the map to invoke each callback.
    // This then indirectly invokes Remove, which makes the iteration crash.
    //
    // A simple solution to this problem would be to copy the map for each iteration, but this
    // allocates a significant amount of memory each time. Instead, we fix this issue by postponing
    // changes to the map until the end of the top-level call to this object. In the mentioned
    // example, when InvokeAll indirectly calls Remove, the removal is not done immediately, but
    // postponed to the end of the (top-level) InvokeAll call.
    //
    // The current callbacks are those in the map, plus toAdd, minus toRemove. The changes are
    // merged via the UpdateGuard.
    internal class Callbacks<T>
    {
        private SortedDictionary<ulong, Action<T>> map = new SortedDictionary<ulong, Action<T>>();

        private SortedDictionary<ulong, Action<T>> mapReversed =
            new SortedDictionary<ulong, Action<T>>(new DescendingComparer<ulong>());

        private SortedDictionary<ulong, Action<T>> toAdd = new SortedDictionary<ulong, Action<T>>();
        private SortedDictionary<ulong, uint> toRemove = new SortedDictionary<ulong, uint>();

        // Current call level with 0 for the top-level call.
        private int callDepth = 0;

        public void Add(ulong key, Action<T> callback)
        {
            try
            {
                EnterUpdateGuard();
                toAdd.Add(key, callback);
                toRemove.Remove(key);
            }
            finally
            {
                ExitUpdateGuard();
            }
        }

        public bool Remove(ulong key)
        {
            try
            {
                EnterUpdateGuard();

                var isInMap = map.ContainsKey(key);
                var hadPendingAdd = toAdd.Remove(key);

                if (isInMap)
                {
                    var createdPendingRemove = !toRemove.ContainsKey(key);
                    toRemove.Add(key, /* ignored */ 0);
                    return createdPendingRemove;
                }
                else
                {
                    return hadPendingAdd;
                }
            }
            finally
            {
                ExitUpdateGuard();
            }
        }

        public void InvokeAll(T op)
        {
            try
            {
                EnterUpdateGuard();
                foreach (var pair in map)
                {
                    pair.Value(op);
                }
            }
            finally
            {
                ExitUpdateGuard();
            }
        }

        public void InvokeAllReverse(T op)
        {
            try
            {
                EnterUpdateGuard();
                foreach (var pair in mapReversed)
                {
                    pair.Value(op);
                }
            }
            finally
            {
                ExitUpdateGuard();
            }
        }

        /// <summary>
        ///     Merge toAdd and toRemove with map.
        /// </summary>
        private void UpdateCallbacks()
        {
            foreach (var node in toAdd)
            {
                map.Add(node.Key, node.Value);
                mapReversed.Add(node.Key, node.Value);
            }

            toAdd.Clear();
            foreach (var node in toRemove)
            {
                map.Remove(node.Key);
                mapReversed.Remove(node.Key);
            }

            toRemove.Clear();
        }

        /// <summary>
        ///     Registers that a call is entered to update the state of the map accordingly.
        /// </summary>
        private void EnterUpdateGuard()
        {
            ++callDepth;
        }

        /// <summary>
        ///     Registers that a call is exited to update the state of the map accordingly.
        /// </summary>
        private void ExitUpdateGuard()
        {
            if (--callDepth == 0)
            {
                UpdateCallbacks();
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
