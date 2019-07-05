using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [AutoRegisterSubscriptionManager]
    public class WorkerIdSubscriptionManager : SubscriptionManager<WorkerId>
    {
        private readonly World world;

        public WorkerIdSubscriptionManager(World world)
        {
            this.world = world;
        }

        public override Subscription<WorkerId> Subscribe(EntityId entityId)
        {
            var subscription = new Subscription<WorkerId>(this, new EntityId(0));
            subscription.SetAvailable(new WorkerId(world.GetExistingSystem<WorkerSystem>().WorkerId));
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
        }

        public override void ResetValue(ISubscription subscription)
        {
        }
    }

    public struct WorkerId
    {
        public WorkerId(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
