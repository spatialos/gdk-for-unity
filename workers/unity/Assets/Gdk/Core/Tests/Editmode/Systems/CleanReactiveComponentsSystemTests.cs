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
        private UnityTestWorker worker;

        [SetUp]
        public void Setup()
        {
            // Need to use a different worker instance for every test because the system's world needs to be re-created.
            worker = new UnityTestWorker("worker-id", new Vector3());
        }

        [TearDown]
        public void TearDown()
        {
            worker.Dispose();
        }

        [Test]
        [TestCase(typeof(NewlyAddedSpatialOSEntity))]
        [TestCase(typeof(OnConnected))]
        [TestCase(typeof(OnDisconnected))]
        public void CleanReactiveComponentsSystem_should_remove_components_when_ticked(Type reactiveComponentType)
        {
            var world = worker.World;

            var entityManager = world.GetOrCreateManager<EntityManager>();

            var entityWithReactiveComponent = entityManager.CreateEntity(reactiveComponentType);

            var cleanReactiveComponentsSystem = world.GetOrCreateManager<CleanReactiveComponentsSystem>();

            // Test that the system does not perform removal immediately, but only on Update
            Assert.IsTrue(entityManager.HasComponent(entityWithReactiveComponent, reactiveComponentType));

            cleanReactiveComponentsSystem.Update();

            Assert.IsFalse(entityManager.HasComponent(entityWithReactiveComponent, reactiveComponentType));
        }

        [Test]
        public void CleanReactiveComponentsSystem_should_prepare_translation_groups_when_initialised()
        {
            var world = worker.World;

            foreach (var translationUnit in worker.View.TranslationUnits.Values)
            {
                Assert.IsNull(translationUnit.CleanUpComponentGroups);
            }

            world.GetOrCreateManager<CleanReactiveComponentsSystem>();

            foreach (var translationUnit in worker.View.TranslationUnits.Values)
            {
                Assert.IsNotNull(translationUnit.CleanUpComponentGroups);
                Assert.IsTrue(translationUnit.CleanUpComponentGroups.Any());
            }
        }
    }
}
