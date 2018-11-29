using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [AutoRegisterSubscriptionManager]
    public class WorldSubscriptionManager : SubscriptionManager<World>
    {
        private Subscription<World> worldSubscription;
        private World world;

        public WorldSubscriptionManager(World world)
        {
            this.world = world;
        }

        public override Subscription<World> Subscribe(EntityId entityId)
        {
            if (worldSubscription == null)
            {
                worldSubscription = new Subscription<World>(this, new EntityId(0));
                worldSubscription.SetAvailable(world);
            }

            return worldSubscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            // Could count number of subscriptions and delete after that
        }

        public override void ResetValue(ISubscription subscription)
        {
        }
    }
}
