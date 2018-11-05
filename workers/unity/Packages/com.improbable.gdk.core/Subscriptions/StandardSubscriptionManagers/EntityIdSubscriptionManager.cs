using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [AutoRegisterSubscriptionManager]
    public class EntityIdSubscriptionManager : SubscriptionManager<EntityId>
    {
        public EntityIdSubscriptionManager(World world)
        {
        }

        public override Subscription<EntityId> Subscribe(EntityId entityId)
        {
            var subscription = new Subscription<EntityId>(this, entityId);
            subscription.SetAvailable(entityId);

            return subscription;
        }

        public override void Cancel(EntityId entityId, ITypeErasedSubscription subscription)
        {
        }

        public override void Invalidate(EntityId entityId, ITypeErasedSubscription subscription)
        {
        }

        public override void Restore(EntityId entityId, ITypeErasedSubscription subscription)
        {
        }
    }
}
