using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    public class EntityCallbacks<T> where T : struct
    {
        private readonly Dictionary<EntityId, CallbackCollection<T>> indexedCallbacks = new Dictionary<EntityId, CallbackCollection<T>>();
        private readonly Dictionary<ulong, EntityId> callbackKeyToEntityId = new Dictionary<ulong, EntityId>();

        private ulong lastCallbackKey;

        public ulong Add(EntityId entityId, Action<T> callback)
        {
            if (!indexedCallbacks.TryGetValue(entityId, out var callbacks))
            {
                callbacks = new CallbackCollection<T>();
                indexedCallbacks.Add(entityId, callbacks);
            }

            lastCallbackKey++;
            callbackKeyToEntityId.Add(lastCallbackKey, entityId);
            callbacks.Add(lastCallbackKey, callback);
            return lastCallbackKey;
        }

        public bool Remove(ulong callbackKey)
        {
            if (!callbackKeyToEntityId.TryGetValue(callbackKey, out var entityId))
            {
                return false;
            }

            var callbackCollection = indexedCallbacks[entityId];

            callbackCollection.Remove(callbackKey);
            callbackKeyToEntityId.Remove(callbackKey);

            // Remove CallbackCollection if there are no callbacks left
            if (callbackCollection.Count == 0)
            {
                indexedCallbacks.Remove(entityId);
            }

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
