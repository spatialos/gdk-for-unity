using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.Subscriptions
{
    /// <summary>
    ///     This tests the availability constraints for the following subscriptions :
    ///         - EntityId
    ///         - ECS Entity
    ///         - Log Dispatcher
    ///         - World Commands
    ///         - ECS World
    /// </summary>
    [TestFixture]
    public class StandardSubscriptionTests : SubscriptionTestBase
    {
        private EntityId entityId = new EntityId(100);

        [Test]
        public void Subscribe_to_entity_id_should_always_be_available()
        {
            var entitySubscription = SubscriptionSystem.Subscribe<EntityId>(entityId);
            Assert.True(entitySubscription.HasValue);
            Assert.AreEqual(entityId, entitySubscription.Value);
        }

        [Test]
        public void Subscribe_to_ecs_entity_should_not_be_available_if_the_entity_does_not_exist()
        {
            var ecsEntitySubscription = SubscriptionSystem.Subscribe<Entity>(entityId);
            Assert.IsFalse(ecsEntitySubscription.HasValue);
        }

        [Test]
        public void Subscribe_to_ecs_entity_should_be_available_if_the_entity_exists()
        {
            var entity = World.EntityManager.CreateEntity();
            Worker.EntityIdToEntity.Add(entityId, entity);

            var ecsEntitySubscription = SubscriptionSystem.Subscribe<Entity>(entityId);
            Assert.IsTrue(ecsEntitySubscription.HasValue);
            Assert.AreEqual(entity, ecsEntitySubscription.Value);
        }

        [Test]
        public void Subscribe_to_log_dispatcher_should_always_be_available()
        {
            var logSubscription = SubscriptionSystem.Subscribe<ILogDispatcher>(entityId);
            Assert.IsTrue(logSubscription.HasValue);
            Assert.AreEqual(LogDispatcher, logSubscription.Value);
        }

        [Test]
        public void Subscribe_to_world_commands_should_not_be_available_if_the_entity_does_not_exist()
        {
            var worldCommandSub = SubscriptionSystem.Subscribe<WorldCommandSender>(entityId);
            Assert.IsFalse(worldCommandSub.HasValue);
        }

        [Test]
        public void Subscribe_to_world_commands_should_be_available_if_the_entity_exists()
        {
            var entity = World.EntityManager.CreateEntity();
            Worker.EntityIdToEntity.Add(entityId, entity);

            var worldCommandSub = SubscriptionSystem.Subscribe<WorldCommandSender>(entityId);
            Assert.IsTrue(worldCommandSub.HasValue);
            Assert.IsNotNull(worldCommandSub.Value);
            Assert.IsTrue(worldCommandSub.Value.IsValid);
        }

        [Test]
        public void Subscribe_to_world_should_always_be_available()
        {
            var worldSubscription = SubscriptionSystem.Subscribe<World>(entityId);
            Assert.IsTrue(worldSubscription.HasValue);
            Assert.AreEqual(World, worldSubscription.Value);
        }
    }
}
