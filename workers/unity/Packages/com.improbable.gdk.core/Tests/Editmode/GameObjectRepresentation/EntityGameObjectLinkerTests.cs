using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class EntityGameObjectLinkerTests
    {
        private World world;
        private EntityManager entityManager;
        private EntityGameObjectLinker entityGameObjectLinker;
        private GameObject testGameObject;
        private Entity testEntity;
        private const long TestSpatialEntityId = 1337;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            entityManager = world.GetOrCreateManager<EntityManager>();
            entityGameObjectLinker = new EntityGameObjectLinker(world, new MutableView(world, new LoggingDispatcher()));
            testGameObject = new GameObject();
            testEntity = entityManager.CreateEntity();
        }

        [TearDown]
        public void TearDown()
        {
            if (testGameObject != null)
            {
                Object.DestroyImmediate(testGameObject);
            }

            world.Dispose();
        }

        [Test]
        public void LinkGameObjectToEntity_adds_SpatialOSComponent_component()
        {
            entityGameObjectLinker.LinkGameObjectToEntity(testGameObject, testEntity, TestSpatialEntityId,
                new ViewCommandBuffer());
            var spatialOSComponent = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(spatialOSComponent);
            Assert.AreEqual(testEntity, spatialOSComponent.Entity);
            Assert.AreEqual(TestSpatialEntityId, spatialOSComponent.SpatialEntityId);
            Assert.AreEqual(world, spatialOSComponent.World);
        }

        [Test]
        public void LinkGameObjectToEntity_adds_GameObject_components_to_entity()
        {
            testGameObject.AddComponent<TestMonoBehaviour>();
            var viewCommandBuffer = new ViewCommandBuffer();
            entityGameObjectLinker.LinkGameObjectToEntity(testGameObject, testEntity, TestSpatialEntityId,
                viewCommandBuffer);
            Assert.IsFalse(entityManager.HasComponent<TestMonoBehaviour>(testEntity));
            viewCommandBuffer.FlushBuffer(new MutableView(world, new LoggingDispatcher()));
            Assert.IsTrue(entityManager.HasComponent<TestMonoBehaviour>(testEntity));
        }

        [Test]
        public void LinkGameObjectToEntity_adds_duplicate_GameObject_components_only_once()
        {
            testGameObject.AddComponent<TestMonoBehaviour>();
            testGameObject.AddComponent<TestMonoBehaviour>();
            var viewCommandBuffer = new ViewCommandBuffer();
            entityGameObjectLinker.LinkGameObjectToEntity(testGameObject, testEntity, TestSpatialEntityId,
                viewCommandBuffer);
            viewCommandBuffer.FlushBuffer(new MutableView(world, new LoggingDispatcher()));
            Assert.IsTrue(entityManager.HasComponent<TestMonoBehaviour>(testEntity));
        }
    }
}
