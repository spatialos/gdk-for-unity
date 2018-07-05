using System.Collections.Generic;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class SpatialOSGameObjectCreatorTests
    {
        private const string TestPrefabName = "testPrefab";
        private World world;
        private GameObject testPrefab;
        private SpatialOSGameObjectCreator spatialOSGameObjectCreator;
        private GameObject testGameObject;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            testPrefab = new GameObject();
            var initialCachedPrefabs = new Dictionary<string, GameObject>
            {
                {TestPrefabName, testPrefab}
            };
            spatialOSGameObjectCreator = new SpatialOSGameObjectCreator(world, initialCachedPrefabs);
        }

        [TearDown]
        public void TearDown()
        {
            if (testGameObject != null)
            {
                Object.DestroyImmediate(testGameObject);
            }

            Object.DestroyImmediate(testPrefab);
            world.Dispose();
        }

        [Test]
        public void CreateSpatialOSGameObject_creates_a_gameobject()
        {
            var entity = world.GetOrCreateManager<EntityManager>().CreateEntity();
            var position = Vector3.zero;
            var rotation = Quaternion.identity;
            var viewCommandBuffer = new ViewCommandBuffer();
            long spatialEntityId = 1;
            testGameObject = spatialOSGameObjectCreator.CreateSpatialOSGameObject(entity, TestPrefabName, position,
                rotation, viewCommandBuffer, spatialEntityId);

            Assert.NotNull(testGameObject);
            var spatialOSComponent = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(spatialOSComponent);
            Assert.AreEqual(entity, spatialOSComponent.Entity);
            Assert.AreEqual(spatialEntityId, spatialOSComponent.SpatialEntityId);
            Assert.AreEqual(world, spatialOSComponent.World);
        }
    }
}
