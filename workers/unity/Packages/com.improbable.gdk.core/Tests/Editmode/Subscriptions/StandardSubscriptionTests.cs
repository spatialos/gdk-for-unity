using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.Subscriptions
{
    /// <summary>
    ///     This class tests the following subscriptions:
    ///         - EntityId
    ///         - ECS Entity
    ///         - Log Dispatcher
    ///         - World Commands
    ///         - ECS World
    /// </summary>
    [TestFixture]
    public class StandardSubscriptionTests
    {
        private World world;
        private WorkerSystem worker;
        private SubscriptionSystem subscriptionSystem;
        private ILogDispatcher logDispatcher;

        private EntityId entityId = new EntityId(100);

        [SetUp]
        public void Setup()
        {
            // This is the minimal set required for subscriptions to work.
            // TODO: Look into untangling these!
            world = new World("test-world");
            worker = world.CreateManager<WorkerSystem>(null, new TestLogDispatcher(), "TestWorkerType", Vector3.zero);
            world.CreateManager<SpatialOSReceiveSystem>();
            world.GetOrCreateManager<ComponentConstraintsCallbackSystem>();
            subscriptionSystem = world.CreateManager<SubscriptionSystem>();

            logDispatcher = worker.LogDispatcher;
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }

        [Test]
        public void Subscribe_to_entity_id_should_always_be_available()
        {
            var entitySubscription = subscriptionSystem.Subscribe<EntityId>(entityId);
            Assert.True(entitySubscription.HasValue);
            Assert.AreEqual(entityId, entitySubscription.Value);
        }

        [Test]
        public void Subscribe_to_ecs_entity_should_not_be_available_if_the_entity_does_not_exist()
        {
            var ecsEntitySubscription = subscriptionSystem.Subscribe<Entity>(entityId);
            Assert.IsFalse(ecsEntitySubscription.HasValue);
        }

        [Test]
        public void Subscribe_to_ecs_entity_should_be_available_if_the_entity_exists()
        {
            var entity = world.GetExistingManager<EntityManager>().CreateEntity();
            worker.EntityIdToEntity.Add(entityId, entity);

            var ecsEntitySubscription = subscriptionSystem.Subscribe<Entity>(entityId);
            Assert.IsTrue(ecsEntitySubscription.HasValue);
            Assert.AreEqual(entity, ecsEntitySubscription.Value);
        }

        [Test]
        public void Subscribe_to_log_dispatcher_should_always_be_available()
        {
            var logSubscription = subscriptionSystem.Subscribe<ILogDispatcher>(entityId);
            Assert.IsTrue(logSubscription.HasValue);
            Assert.AreEqual(logDispatcher, logSubscription.Value);
        }

        [Test]
        public void Subscribe_to_world_commands_should_not_be_available_if_the_entity_does_not_exist()
        {
            var worldCommandSub = subscriptionSystem.Subscribe<WorldCommandSender>(entityId);
            Assert.IsFalse(worldCommandSub.HasValue);
        }

        [Test]
        public void Subscribe_to_world_commands_should_be_available_if_the_entity_exists()
        {
            var entity = world.GetExistingManager<EntityManager>().CreateEntity();
            worker.EntityIdToEntity.Add(entityId, entity);

            var worldCommandSub = subscriptionSystem.Subscribe<WorldCommandSender>(entityId);
            Assert.IsTrue(worldCommandSub.HasValue);
            Assert.IsNotNull(worldCommandSub.Value);
            Assert.IsTrue(worldCommandSub.Value.IsValid);
        }

        [Test]
        public void Subscribe_to_world_should_always_be_available()
        {
            var worldSubscription = subscriptionSystem.Subscribe<World>(entityId);
            Assert.IsTrue(worldSubscription.HasValue);
            Assert.AreEqual(world, worldSubscription.Value);
        }
    }
}
