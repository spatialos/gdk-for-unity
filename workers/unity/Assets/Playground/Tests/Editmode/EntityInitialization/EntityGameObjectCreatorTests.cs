using System.Collections.Generic;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Playground.EditmodeTests
{
    [TestFixture]
    public class EntityGameObjectCreatorTests
    {
        private const string TestPrefabName = "testPrefab";
        private World world;
        private GameObject testPrefab;
        private EntityGameObjectCreator entityGameObjectCreator;
        private GameObject testGameObject;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            testPrefab = new GameObject();
            var initialCachedPrefabs = new Dictionary<string, GameObject>
            {
                { TestPrefabName, testPrefab }
            };
            entityGameObjectCreator = new EntityGameObjectCreator(world, initialCachedPrefabs);
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
            var position = Vector3.one * 1337;
            var rotation = new Quaternion(0, 0, 1, 0);
            const long spatialEntityId = 1;
            testGameObject = entityGameObjectCreator.CreateEntityGameObject(entity, TestPrefabName, position,
                rotation, spatialEntityId);

            Assert.NotNull(testGameObject);
            Assert.AreEqual(position, testGameObject.transform.position);
            Assert.AreEqual(rotation, testGameObject.transform.rotation);
        }

        [Test]
        public void CreateSpatialOSGameObject_throws_if_prefab_not_found()
        {
            var entity = world.GetOrCreateManager<EntityManager>().CreateEntity();
            var position = Vector3.zero;
            var rotation = Quaternion.identity;
            const long spatialEntityId = 1;
            Assert.Throws<PrefabNotFoundException>(() =>
            {
                testGameObject =
                    entityGameObjectCreator.CreateEntityGameObject(entity, "foobar", position, rotation,
                        spatialEntityId);
            });
        }
    }
}
