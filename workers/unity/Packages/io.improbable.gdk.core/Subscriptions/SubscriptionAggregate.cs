using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Improbable.Gdk.Subscriptions
{
    public class SubscriptionAggregate
    {
        private readonly ISubscription[] subscriptions;
        private readonly Dictionary<Type, int> typesToSubscriptionIndexes = new Dictionary<Type, int>();

        private ISubscriptionAvailabilityHandler availabilityHandler;
        private int subscriptionsSatisfied;

        private bool IsSatisfied => subscriptionsSatisfied == subscriptions.Length;

        public SubscriptionAggregate(IReadOnlyList<Type> types, ISubscription[] subscriptions)
        {
            Assert.AreEqual(types.Count, subscriptions.Length);

            this.subscriptions = subscriptions;
            for (var i = 0; i < types.Count; i++)
            {
                var type = types[i];
                typesToSubscriptionIndexes.Add(type, i);
                subscriptions[i].SetAvailabilityHandler(Handler.Pool.Rent(this));
            }
        }

        public void SetAvailabilityHandler(ISubscriptionAvailabilityHandler handler)
        {
            availabilityHandler = handler;
            if (IsSatisfied)
            {
                availabilityHandler?.OnAvailable();
            }
        }

        public ISubscriptionAvailabilityHandler GetAvailabilityHandler()
        {
            return availabilityHandler;
        }

        public T GetValue<T>()
        {
            var subscriptionIndex = GetTypeIndex(typeof(T));
            var subscription = (Subscription<T>) subscriptions[subscriptionIndex];

            return subscription.Value;
        }

        public object GetErasedValue(Type type)
        {
            var subscriptionIndex = GetTypeIndex(type);
            return subscriptions[subscriptionIndex].GetErasedValue();
        }

        private int GetTypeIndex(Type type)
        {
            if (!IsSatisfied)
            {
                throw new InvalidOperationException("Subscriptions not all satisfied");
            }

            if (!typesToSubscriptionIndexes.TryGetValue(type, out var subscriptionIndex))
            {
                throw new InvalidOperationException("No subscription with that type");
            }

            return subscriptionIndex;
        }

        public void Cancel()
        {
            foreach (var sub in subscriptions)
            {
                Handler.Pool.Return((Handler) sub.GetAvailabilityHandler());
                sub.Cancel();
            }
        }

        private void HandleSubscriptionUnavailable()
        {
            if (IsSatisfied)
            {
                foreach (var sub in subscriptions)
                {
                    sub.ResetValue();
                }

                availabilityHandler?.OnUnavailable();
            }

            --subscriptionsSatisfied;
        }

        private void HandleSubscriptionAvailable()
        {
            ++subscriptionsSatisfied;
            if (IsSatisfied)
            {
                availabilityHandler?.OnAvailable();
            }
        }

        private class Handler : ISubscriptionAvailabilityHandler
        {
            private SubscriptionAggregate aggregate;

            public void OnAvailable()
            {
                aggregate?.HandleSubscriptionAvailable();
            }

            public void OnUnavailable()
            {
                aggregate?.HandleSubscriptionUnavailable();
            }

            public static class Pool
            {
                private static readonly Stack<Handler> HandlerPool = new Stack<Handler>();

                public static Handler Rent(SubscriptionAggregate aggregate)
                {
                    var handler = HandlerPool.Count == 0
                        ? new Handler()
                        : HandlerPool.Pop();

                    handler.aggregate = aggregate;
                    return handler;
                }

                // todo the disposal pattern for this is currently awful and needs to be improved
                public static void Return(Handler handler)
                {
                    // todo this should be bounded
                    handler.aggregate = null;
                    HandlerPool.Push(handler);
                }
            }
        }
    }
}
