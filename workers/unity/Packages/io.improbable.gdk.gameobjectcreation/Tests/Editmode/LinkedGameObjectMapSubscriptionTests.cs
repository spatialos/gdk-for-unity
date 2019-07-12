using Improbable.Gdk.Core;
using Improbable.Gdk.Core.EditmodeTests.Subscriptions;
using Improbable.Gdk.Subscriptions;
using NUnit.Framework;

namespace Improbable.Gdk.GameObjectCreation.EditmodeTests
{
    public class LinkedGameObjectMapSubscriptionTests : SubscriptionTestBase
    {
        private EntityId entityId = new EntityId(100);

        [Test]
        public void Subscribe_to_LinkedGameObjectMap_should_not_be_available_if_GameObjectCreation_systems_are_not_present()
        {
            var goMapSubscription = SubscriptionSystem.Subscribe<LinkedGameObjectMap>(entityId);
            Assert.IsFalse(goMapSubscription.HasValue);
        }

        [Test]
        public void Subscribe_to_LinkedGameObjectMap_should_be_available_if_GameObjectCreation_systems_are_added()
        {
            GameObjectCreationHelper.EnableStandardGameObjectCreation(World, new MockGameObjectCreator());

            var goMapSubscription = SubscriptionSystem.Subscribe<LinkedGameObjectMap>(entityId);
            Assert.IsTrue(goMapSubscription.HasValue);
            Assert.IsNotNull(goMapSubscription.Value);
        }
    }
}
