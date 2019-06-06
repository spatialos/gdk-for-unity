using System;
using Improbable.Gdk.ReactiveComponents;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests.Systems
{
    [TestFixture]
    public class CleanTemporaryComponentsSystemTests
    {
        private World world;

        [SetUp]
        public void Setup()
        {
            // Need to use a different worker instance for every test because the system's world needs to be re-created.
            world = new World("test");
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }

        [Test]
        [TestCase(typeof(NewlyAddedSpatialOSEntity))]
        [TestCase(typeof(OnConnected))]
        [TestCase(typeof(OnDisconnected))]
        public void CleanReactiveComponentsSystem_should_remove_components_when_ticked(Type reactiveComponentType)
        {
            var entityManager = world.EntityManager;

            var entityWithReactiveComponent = entityManager.CreateEntity(reactiveComponentType);

            var cleanReactiveComponentsSystem = world.GetOrCreateSystem<CleanTemporaryComponentsSystem>();

            // Test that the system does not perform removal immediately, but only on Update
            Assert.IsTrue(entityManager.HasComponent(entityWithReactiveComponent, reactiveComponentType));

            cleanReactiveComponentsSystem.Update();

            Assert.IsFalse(entityManager.HasComponent(entityWithReactiveComponent, reactiveComponentType));
        }
    }
}
