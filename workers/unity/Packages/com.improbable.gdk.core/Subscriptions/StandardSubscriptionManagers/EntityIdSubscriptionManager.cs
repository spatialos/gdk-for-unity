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

        public override void Cancel(ITypeErasedSubscription subscription)
        {
        }

        public override void ResetValue(ITypeErasedSubscription subscription)
        {
        }
    }
}
