using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
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

        public override void Cancel(EntityId entityId, ITypeErasedSubscription subscription)
        {
            // Could count number of subscriptions and delete after that
        }

        public override void Invalidate(EntityId entityId, ITypeErasedSubscription subscription)
        {
            // Could set to 0 or something invalid
        }

        public override void Restore(EntityId entityId, ITypeErasedSubscription subscription)
        {
            // Only needs to do something if invalidate does
        }
    }
}
