using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [AutoRegisterSubscriptionManager]
    public class LogDispatcherSubscriptionManager : SubscriptionManager<ILogDispatcher>
    {
        private readonly ILogDispatcher logger;
        private Subscription<ILogDispatcher> subscription;

        public LogDispatcherSubscriptionManager(World world)
        {
            logger = world.GetExistingManager<WorkerSystem>().LogDispatcher;
        }

        public override Subscription<ILogDispatcher> Subscribe(EntityId entityId)
        {
            if (subscription == null)
            {
                subscription = new Subscription<ILogDispatcher>(this, new EntityId(0));
                subscription.SetAvailable(logger);
            }

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
