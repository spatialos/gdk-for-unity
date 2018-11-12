using System;
using Improbable.Worker;

namespace Improbable.Gdk.Subscriptions
{
    public abstract class SubscriptionManagerBase
    {
        public abstract Type SubscriptionType { get; }

        public abstract ITypeErasedSubscription SubscribeTypeErased(EntityId entityId);
        public abstract void Cancel(ITypeErasedSubscription subscription);
        public abstract void ResetValue(ITypeErasedSubscription subscription);
    }

    public abstract class SubscriptionManager<T> : SubscriptionManagerBase
    {
        public abstract Subscription<T> Subscribe(EntityId entityId);

        public override Type SubscriptionType => typeof(T);

        public override ITypeErasedSubscription SubscribeTypeErased(EntityId entityId)
        {
            return Subscribe(entityId);
        }
    }
}
