using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    [AutoRegisterSubscriptionManager]
    internal class LinkedGameObjectMapSubscriptionManager : SubscriptionManager<LinkedGameObjectMap>
    {
        public LinkedGameObjectMapSubscriptionManager(World world) : base(world)
        {
        }

        public override Subscription<LinkedGameObjectMap> Subscribe(EntityId entityId)
        {
            var linkedGameObjectMapSubscription = new Subscription<LinkedGameObjectMap>(this, new EntityId(0));

            var goSystem = world.GetExistingSystem<GameObjectInitializationSystem>();
            if (goSystem != null)
            {
                linkedGameObjectMapSubscription.SetAvailable(new LinkedGameObjectMap(goSystem.Linker));
            }

            return linkedGameObjectMapSubscription;
        }

        public override void Cancel(ISubscription subscription)
        {
        }

        public override void ResetValue(ISubscription subscription)
        {
        }
    }
}
