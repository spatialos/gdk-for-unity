using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [AutoRegisterSubscriptionManager]
    public class LogDispatcherSubscriptionManager : SubscriptionManager<ILogDispatcher>
    {
        private readonly ILogDispatcher logger;

        public LogDispatcherSubscriptionManager(World world) : base(world)
        {
            logger = world.GetExistingSystem<WorkerSystem>().LogDispatcher;
        }

        public override Subscription<ILogDispatcher> Subscribe(EntityId entityId)
        {
            var subscription = new Subscription<ILogDispatcher>(this, new EntityId(0));
            subscription.SetAvailable(logger);

            return subscription;
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
