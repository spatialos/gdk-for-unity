using System;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    public abstract class SubscriptionManagerBase
    {
        public abstract Type SubscriptionType { get; }

        public abstract ISubscription SubscribeTypeErased(EntityId entityId);
        public abstract void Cancel(ISubscription subscription);
        public abstract void ResetValue(ISubscription subscription);
    }

    public abstract class SubscriptionManager<T> : SubscriptionManagerBase
    {
        public abstract Subscription<T> Subscribe(EntityId entityId);

        public override Type SubscriptionType => typeof(T);

        public override ISubscription SubscribeTypeErased(EntityId entityId)
        {
            return Subscribe(entityId);
        }
    }
}
