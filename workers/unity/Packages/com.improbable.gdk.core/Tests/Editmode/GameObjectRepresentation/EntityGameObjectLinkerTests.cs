using Improbable.Gdk.TestUtils;
using Improbable.Worker;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class EntityGameObjectLinkerTests
    {
        private ViewCommandBuffer commandBuffer;
        private World world;
        private EntityManager entityManager;
        private EntityGameObjectLinker entityGameObjectLinker;
        private GameObject testGameObject;
        private Entity testEntity;
        private readonly EntityId testSpatialEntityId = new EntityId(1337);

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            entityManager = world.GetOrCreateManager<EntityManager>();
            entityGameObjectLinker = new EntityGameObjectLinker(world, new LoggingDispatcher());
            testGameObject = new GameObject();
            testEntity = entityManager.CreateEntity();
            commandBuffer = new ViewCommandBuffer(entityManager, new LoggingDispatcher());
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
            entityGameObjectLinker.LinkGameObjectToEntity(testGameObject, testEntity, testSpatialEntityId,
                commandBuffer);
            var spatialOSComponent = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(spatialOSComponent);
            Assert.AreEqual(testEntity, spatialOSComponent.Entity);
            Assert.AreEqual(testSpatialEntityId, spatialOSComponent.SpatialEntityId);
            Assert.AreEqual(world, spatialOSComponent.World);
        }

        [Test]
        public void LinkGameObjectToEntity_adds_GameObject_components_to_entity()
        {
            testGameObject.AddComponent<TestMonoBehaviour>();
            entityGameObjectLinker.LinkGameObjectToEntity(testGameObject, testEntity, testSpatialEntityId,
                commandBuffer);
            Assert.IsFalse(entityManager.HasComponent<TestMonoBehaviour>(testEntity));
            commandBuffer.FlushBuffer();
            Assert.IsTrue(entityManager.HasComponent<TestMonoBehaviour>(testEntity));
        }

        [Test]
        public void LinkGameObjectToEntity_adds_duplicate_GameObject_components_only_once()
        {
            testGameObject.AddComponent<TestMonoBehaviour>();
            testGameObject.AddComponent<TestMonoBehaviour>();
            entityGameObjectLinker.LinkGameObjectToEntity(testGameObject, testEntity, testSpatialEntityId,
                commandBuffer);
            commandBuffer.FlushBuffer();
            Assert.IsTrue(entityManager.HasComponent<TestMonoBehaviour>(testEntity));
        }
    }
}
