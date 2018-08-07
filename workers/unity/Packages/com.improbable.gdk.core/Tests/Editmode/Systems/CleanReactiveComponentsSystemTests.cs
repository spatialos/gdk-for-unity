using System;
using System.Linq;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.Systems
{
    [TestFixture]
    public class CleanReactiveComponentsSystemTests
    {
        private SpatialOSWorld world;

        [SetUp]
        public void Setup()
        {
            world = new SpatialOSWorld("test");
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
            var entityManager = world.GetOrCreateManager<EntityManager>();

            var entityWithReactiveComponent = entityManager.CreateEntity(reactiveComponentType);

            var cleanReactiveComponentsSystem = world.GetOrCreateManager<CleanReactiveComponentsSystem>();

            // Test that the system does not perform removal immediately, but only on Update
            Assert.IsTrue(entityManager.HasComponent(entityWithReactiveComponent, reactiveComponentType));

            cleanReactiveComponentsSystem.Update();

            Assert.IsFalse(entityManager.HasComponent(entityWithReactiveComponent, reactiveComponentType));
        }
    }
}
