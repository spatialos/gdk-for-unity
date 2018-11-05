using System;
using System.Collections.Generic;
using Improbable.Collections;

namespace Improbable.Gdk.Subscriptions
{
    // Stole this from worker - slightly faster than writing our own
    public class Callbacks<T>
    {
        private Map<ulong, Action<T>> map = new Map<ulong, Action<T>>();
        private Map<ulong, Action<T>> toAdd = new Map<ulong, Action<T>>();

        private Map<ulong, uint> toRemove = new Map<ulong, uint>();

        // Current call level with 0 for the top-level call.
        private int callDepth = 0;

        public void Add(ulong key, Action<T> callback)
        {
            try
            {
                EnterUpdateGuard();

                //Contract.Requires<ArgumentNullException>(callback != null, "Cannot register a null callback.");
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

                bool isInMap = map.ContainsKey(key);
                bool hadPendingAdd = toAdd.Remove(key);

                if (isInMap)
                {
                    bool createdPendingRemove = !toRemove.ContainsKey(key);
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

                for (var node = map.First; node != null;)
                {
                    var next = node.Next;
                    try
                    {
                        node.Value.Value(op);
                    }
                    catch (Exception ex)
                    {
                    }

                    node = next;
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

                for (var node = map.Last; node != null;)
                {
                    var previous = node.Previous;
                    try
                    {
                        node.Value.Value(op);
                    }
                    catch (Exception ex)
                    {
                    }

                    node = previous;
                }
            }
            finally
            {
                ExitUpdateGuard();
            }
        }

        /// <summary>
        /// Merge toAdd and toRemove with map.
        /// </summary>
        private void UpdateCallbacks()
        {
            for (var node = toAdd.First; node != null; node = node.Next)
            {
                map.Add(node.Value.Key, node.Value.Value);
            }

            toAdd.Clear();
            for (var node = toRemove.First; node != null; node = node.Next)
            {
                map.Remove(node.Value.Key);
            }

            toRemove.Clear();
        }

        /// <summary>
        /// Registers that a call is entered to update the state of the map accordingly.
        /// </summary>
        private void EnterUpdateGuard()
        {
            ++callDepth;
        }

        /// <summary>
        /// Registers that a call is exited to update the state of the map accordingly.
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
    public class IndexedCallbacks<T>
    {
        private readonly Dictionary<long, Callbacks<T>> callbacks = new Dictionary<long, Callbacks<T>>();
        private readonly Dictionary<ulong, long> callbackKeyToIndex = new Dictionary<ulong, long>();

        public void Add(long index, ulong callbackKey, Action<T> value)
        {
            if (!callbacks.ContainsKey(index))
            {
                callbacks.Add(index, new Callbacks<T>());
                callbackKeyToIndex.Add(callbackKey, index);
            }

            callbacks[index].Add(callbackKey, value);
        }

        public bool Remove(ulong callbackKey)
        {
            if (!callbackKeyToIndex.TryGetValue(callbackKey, out var index))
            {
                return false;
            }

            callbacks.Remove(index);
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
}
