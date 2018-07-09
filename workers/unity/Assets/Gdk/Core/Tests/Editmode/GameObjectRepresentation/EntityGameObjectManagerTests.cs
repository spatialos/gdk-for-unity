using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class EntityGameObjectManagerTests
    {
        private World world;
        private EntityGameObjectManager entityGameObjectManager;
        private GameObject testGameObject;
        private Entity testEntity;
        private const long testSpatialEntityId = 1337;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            entityGameObjectManager = new EntityGameObjectManager(world);
            testGameObject = new GameObject();
            testEntity = world.GetOrCreateManager<EntityManager>().CreateEntity();
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
            entityGameObjectManager.LinkGameObjectToEntity(testGameObject, testEntity, testSpatialEntityId,
                new ViewCommandBuffer());
            var spatialOSComponent = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(spatialOSComponent);
            Assert.AreEqual(testEntity, spatialOSComponent.Entity);
            Assert.AreEqual(testSpatialEntityId, spatialOSComponent.SpatialEntityId);
            Assert.AreEqual(world, spatialOSComponent.World);
        }
    }
}
