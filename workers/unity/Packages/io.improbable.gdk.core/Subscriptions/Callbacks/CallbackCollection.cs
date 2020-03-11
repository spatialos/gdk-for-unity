using System;
using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    internal class CallbackCollection<T> where T : struct
    {
        private readonly List<WrappedCallback> callbacks = new List<WrappedCallback>();
        private readonly HashSet<ulong> currentKeys = new HashSet<ulong>();

        // These can be lists as we expect the number of callbacks added/removed during invocation of other callbacks
        // to be low.
        private readonly List<ulong> toRemove = new List<ulong>();
        private readonly List<WrappedCallback> toAdd = new List<WrappedCallback>();

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
            return callbacks.Remove(new WrappedCallback(key, default));
        }

        public void InvokeAll(in T op)
        {
            isInInvoke = true;

            for (var i = 0; i < callbacks.Count; i++)
            {
                try
                {
                    callbacks[i].Invoke(in op);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            isInInvoke = false;

            FlushDeferredOperations();
        }

        public void InvokeAllReverse(in T op)
        {
            isInInvoke = true;

            for (var i = callbacks.Count - 1; i >= 0; i--)
            {
                try
                {
                    callbacks[i].Invoke(op);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            isInInvoke = false;

            FlushDeferredOperations();
        }

        private void FlushDeferredOperations()
        {
            if (toRemove.Count > 0)
            {
                for (var i = callbacks.Count - 1; i >= 0; i--)
                {
                    if (toRemove.Contains(callbacks[i].Key))
                    {
                        callbacks.RemoveAt(i);
                    }
                }

                foreach (var key in toRemove)
                {
                    currentKeys.Remove(key);
                }

                toRemove.Clear();
            }

            if (toAdd.Count > 0)
            {
                foreach (var callback in toAdd)
                {
                    callbacks.Add(callback);
                    currentKeys.Add(callback.Key);
                }

                toAdd.Clear();
            }
        }

        private readonly struct WrappedCallback : IEquatable<WrappedCallback>
        {
            public readonly ulong Key;
            private readonly Action<T> action;

            public WrappedCallback(ulong key, Action<T> callback)
            {
                Key = key;
                action = callback;
            }

            public void Invoke(in T arg)
            {
                action(arg);
            }

            public bool Equals(WrappedCallback other)
            {
                return Key == other.Key;
            }

            public override bool Equals(object obj)
            {
                return obj is WrappedCallback other && Equals(other);
            }

            public override int GetHashCode()
            {
                return Key.GetHashCode();
            }
        }
    }
}
