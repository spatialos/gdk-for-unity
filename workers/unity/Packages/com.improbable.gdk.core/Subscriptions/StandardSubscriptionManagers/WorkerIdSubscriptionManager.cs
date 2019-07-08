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
            var workerId = world.GetExistingSystem<WorkerSystem>().WorkerId;
            subscription.SetAvailable(new WorkerId(workerId));
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
        }

        public override void ResetValue(ISubscription subscription)
        {
        }
    }

    public readonly struct WorkerId
    {
        public readonly string Id;

        public WorkerId(string id)
        {
            Id = id;
        }
    }
}
