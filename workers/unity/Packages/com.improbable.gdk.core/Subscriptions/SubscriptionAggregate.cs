using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    public class SubscriptionAggregate
    {
        private readonly ISubscription[] subscriptions;
        private readonly Dictionary<Type, int> typesToSubscriptionIndexes = new Dictionary<Type, int>();

        private ISubscriptionAvailabilityHandler availabilityHandler;

        private int subscriptionsSatisfied;

        public SubscriptionAggregate(SubscriptionSystem subscriptionSystem, EntityId entityId,
            params Type[] typesToSubscribeTo)
        {
            subscriptions = new ISubscription[typesToSubscribeTo.Length];

            for (int i = 0; i < typesToSubscribeTo.Length; ++i)
            {
                var subscription = subscriptionSystem.Subscribe(entityId, typesToSubscribeTo[i]);
                typesToSubscriptionIndexes.Add(typesToSubscribeTo[i], i);
                subscriptions[i] = subscription;

                subscription.SetAvailabilityHandler(Handler.Pool.Rent(this));
            }
        }

        public void SetAvailabilityHandler(ISubscriptionAvailabilityHandler handler)
        {
            availabilityHandler = handler;
            if (subscriptionsSatisfied == subscriptions.Length)
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
            if (subscriptionsSatisfied != subscriptions.Length)
            {
                throw new InvalidOperationException("Subscriptions not all satisfied");
            }

            if (!typesToSubscriptionIndexes.TryGetValue(typeof(T), out var subscriptionIndex))
            {
                throw new InvalidOperationException("No subscription with that type");
            }

            var sub = (Subscription<T>) subscriptions[subscriptionIndex];

            return sub.Value;
        }

        public object GetErasedValue(Type type)
        {
            if (subscriptionsSatisfied != subscriptions.Length)
            {
                throw new InvalidOperationException("Subscriptions not all satisfied");
            }

            if (!typesToSubscriptionIndexes.TryGetValue(type, out var subscriptionIndex))
            {
                throw new InvalidOperationException("No subscription with that type");
            }

            return subscriptions[subscriptionIndex].GetErasedValue();
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
            if (subscriptionsSatisfied == subscriptions.Length)
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
            if (subscriptionsSatisfied == subscriptions.Length)
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

            public class Pool
            {
                private static readonly Stack<Handler> HandlerPool = new Stack<Handler>();

                public static Handler Rent(SubscriptionAggregate aggregate)
                {
                    Handler handler = HandlerPool.Count == 0
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
