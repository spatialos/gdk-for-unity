using System;
using Improbable.Gdk.Subscriptions;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests.Subscriptions
{
    [TestFixture]
    public class AggregateSubscriptionTests : SubscriptionTestBase
    {
        private EntityId entityId = new EntityId(100);

        [Test]
        public void AggregateSubscription_should_be_available_if_all_constraints_satisfied()
        {
            var handler = new AvailabilityHandler();
            var aggSub = new SubscriptionAggregate(SubscriptionSystem, entityId, typeof(EntityId), typeof(World));
            aggSub.SetAvailabilityHandler(handler);

            Assert.IsTrue(handler.IsAvailable);
            Assert.AreEqual(entityId, aggSub.GetValue<EntityId>());
            Assert.AreEqual(World, aggSub.GetValue<World>());
        }

        [Test]
        public void AggregateSubscription_should_not_be_available_if_not_all_constraints_satisfied()
        {
            var handler = new AvailabilityHandler();
            var aggSub = new SubscriptionAggregate(SubscriptionSystem, entityId, typeof(Entity), typeof(World));
            aggSub.SetAvailabilityHandler(handler);

            Assert.IsFalse(handler.IsAvailable);
            Assert.Throws<InvalidOperationException>(() => aggSub.GetValue<Entity>());
        }

        private class AvailabilityHandler : ISubscriptionAvailabilityHandler
        {
            public bool IsAvailable { get; private set; }

            public void OnAvailable()
            {
                IsAvailable = true;
            }

            public void OnUnavailable()
            {
                IsAvailable = false;
            }
        }
    }
}
