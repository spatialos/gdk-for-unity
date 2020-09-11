using System;
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
        [TestCase(typeof(OnConnected))]
        [TestCase(typeof(OnDisconnected))]
        public void CleanTemporaryComponentsSystem_should_remove_components_when_ticked(Type temporaryComponentType)
        {
            var entityManager = world.EntityManager;

            var entityWithTemporaryComponent = entityManager.CreateEntity(temporaryComponentType);

            var cleanTemporaryComponentsSystem = world.GetOrCreateSystem<CleanTemporaryComponentsSystem>();

            // Test that the system does not perform removal immediately, but only on Update
            Assert.IsTrue(entityManager.HasComponent(entityWithTemporaryComponent, temporaryComponentType));

            cleanTemporaryComponentsSystem.Update();

            Assert.IsFalse(entityManager.HasComponent(entityWithTemporaryComponent, temporaryComponentType));
        }
    }
}
