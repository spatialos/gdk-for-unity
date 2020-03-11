using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    public class EntityCallbacks<T> where T : struct
    {
        private readonly Dictionary<EntityId, CallbackCollection<T>> indexedCallbacks = new Dictionary<EntityId, CallbackCollection<T>>();
        private readonly Dictionary<ulong, CallbackCollection<T>> callbackKeyToIndex = new Dictionary<ulong, CallbackCollection<T>>();

        private ulong lastCallbackKey;

        public ulong Add(EntityId entityId, Action<T> callback)
        {
            if (!indexedCallbacks.TryGetValue(entityId, out var callbacks))
            {
                callbacks = new CallbackCollection<T>();
                indexedCallbacks.Add(entityId, callbacks);
            }

            lastCallbackKey++;
            callbackKeyToIndex.Add(lastCallbackKey, callbacks);
            callbacks.Add(lastCallbackKey, callback);
            return lastCallbackKey;
        }

        public bool Remove(ulong callbackKey)
        {
            if (!callbackKeyToIndex.TryGetValue(callbackKey, out var callbacks))
            {
                return false;
            }

            callbacks.Remove(callbackKey);
            callbackKeyToIndex.Remove(callbackKey);
            return true;
        }

        public void InvokeAll(EntityId entityId, in T argument)
        {
            if (indexedCallbacks.TryGetValue(entityId, out var callbacks))
            {
                callbacks.InvokeAll(in argument);
            }
        }

        public void InvokeAllReverse(EntityId entityId, in T argument)
        {
            if (indexedCallbacks.TryGetValue(entityId, out var callbacks))
            {
                callbacks.InvokeAllReverse(in argument);
            }
        }
    }
}
