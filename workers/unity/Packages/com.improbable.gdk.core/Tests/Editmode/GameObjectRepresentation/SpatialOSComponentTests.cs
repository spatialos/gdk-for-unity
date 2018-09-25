using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.TestUtils;
using Improbable.Worker;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class SpatialOSComponentTests
    {
        private SpatialOSComponent testComponent;
        private World testWorld;
        private WorkerSystem testWorker;
        private EntityManager entityManager;
        private GameObject testGameObject;
        private Entity testEntity;
        private readonly EntityId testSpatialEntityId = new EntityId(1337);

        [SetUp]
        public void Setup()
        {
            testWorld = new World("TestWorld");
            entityManager = testWorld.GetOrCreateManager<EntityManager>();
            testWorker = testWorld.CreateManager<WorkerSystem>(null, new TestLogDispatcher(), "TestWorker", Vector3.zero);
            testGameObject = new GameObject();
            testEntity = entityManager.CreateEntity();
            testComponent = LinkGameObjectToEntity(testSpatialEntityId, testEntity, testGameObject, testWorld);
        }

        [TearDown]
        public void TearDown()
        {
            if (testGameObject != null)
            {
                UnityObjectDestroyer.Destroy(testGameObject);
            }

            testWorld.Dispose();
        }

        [Test]
        public void TryGetGameObjectForEntity_returns_true_when_gameobject_is_linked()
        {
            LinkEntityToGameObject(testSpatialEntityId, testEntity, testGameObject, testWorld);
            var foundGameObject = testComponent.TryGetGameObjectForSpatialOSEntityId(testSpatialEntityId, out var linkedGameObject);
            Assert.IsTrue(foundGameObject);
            Assert.AreEqual(testGameObject, linkedGameObject);
        }

        [Test]
        public void TryGetGameObjectForEntity_returns_false_when_entity_is_not_linked()
        {
            testWorker.EntityIdToEntity.Add(testSpatialEntityId, testEntity);
            var foundGameObject = testComponent.TryGetGameObjectForSpatialOSEntityId(testSpatialEntityId, out var linkedGameObject);
            Assert.IsFalse(foundGameObject);
            Assert.IsNull(linkedGameObject);
        }

        [Test]
        public void TryGetGameObjectForEntity_returns_false_when_gameobject_is_not_linked()
        {
            testWorker.EntityIdToEntity.Add(testSpatialEntityId, testEntity);
            var emptyGameObject = new GameObject();
            LinkGameObjectToEntity(testSpatialEntityId, testEntity, emptyGameObject, testWorld);
            var foundGameObject = testComponent.TryGetGameObjectForSpatialOSEntityId(testSpatialEntityId, out var linkedGameObject);
            Assert.IsFalse(foundGameObject);
            Assert.IsNull(linkedGameObject);
        }

        [Test]
        public void TryGetGameObjectForEntity_returns_false_when_gameobject_has_wrong_entity_id()
        {
            var gameObjectWithDifferentEntity = new GameObject();
            LinkEntityToGameObject(testSpatialEntityId, testEntity, gameObjectWithDifferentEntity, testWorld);
            LinkGameObjectToEntity(new EntityId(100), new Entity(), gameObjectWithDifferentEntity, testWorld);
            var foundGameObject = testComponent.TryGetGameObjectForSpatialOSEntityId(testSpatialEntityId, out var linkedGameObject);
            Assert.IsFalse(foundGameObject);
            Assert.IsNull(linkedGameObject);
        }

        [Test]
        public void TryGetGameObjectForEntity_returns_false_when_entity_does_not_exist()
        {
            LinkGameObjectToEntity(testSpatialEntityId, testEntity, testGameObject, testWorld);
            var foundGameObject = testComponent.TryGetGameObjectForSpatialOSEntityId(testSpatialEntityId, out var linkedGameObject);
            Assert.IsFalse(foundGameObject);
            Assert.IsNull(linkedGameObject);
        }

        [Test]
        public void TryGetSpatialOSEntityIdForGameObject_returns_true_when_gameobject_is_linked()
        {
            LinkEntityToGameObject(testSpatialEntityId, testEntity, testGameObject, testWorld);
            var foundEntityId = testComponent.TryGetSpatialOSEntityIdForGameObject(testGameObject, out var linkedSpatialEntityId);
            Assert.IsTrue(foundEntityId);
            Assert.AreEqual(testSpatialEntityId, linkedSpatialEntityId);
        }

        [Test]
        public void TryGetSpatialOSEntityIdForGameObject_returns_false_when_gameobject_is_not_linked()
        {
            var foundEntityId = testComponent.TryGetSpatialOSEntityIdForGameObject(testGameObject, out var linkedSpatialEntityId);
            Assert.IsFalse(foundEntityId);
            Assert.AreEqual(default(EntityId), linkedSpatialEntityId);
        }

        [Test]
        public void TryGetSpatialOSEntityIdForGameObject_returns_false_when_entity_is_not_linked()
        {
            var foundEntityId = testComponent.TryGetSpatialOSEntityIdForGameObject(new GameObject(), out var linkedSpatialEntityId);
            Assert.IsFalse(foundEntityId);
            Assert.AreEqual(default(EntityId), linkedSpatialEntityId);
        }

        [Test]
        public void TryGetSpatialOSEntityIdForGameObject_returns_false_when_entity_on_different_worker()
        {
            var worldOnDifferentWorker = new World("different-world");
            worldOnDifferentWorker.CreateManager<WorkerSystem>(null, new TestLogDispatcher(), "TestWorker", Vector3.zero);
            var gameObjectOnDifferentWorker = new GameObject();
            LinkGameObjectToEntity(testSpatialEntityId, testEntity, gameObjectOnDifferentWorker, worldOnDifferentWorker);

            var foundEntityId = testComponent.TryGetSpatialOSEntityIdForGameObject(gameObjectOnDifferentWorker, out var linkedSpatialEntityId);
            Assert.IsFalse(foundEntityId);
            Assert.AreEqual(default(EntityId), linkedSpatialEntityId);
        }

        private static void LinkEntityToGameObject(EntityId entityId, Entity entity, GameObject gameObject, World world)
        {
            var manager = world.GetOrCreateManager<EntityManager>();
            var worker = world.GetExistingManager<WorkerSystem>();
            worker.EntityIdToEntity.Add(entityId, entity);
            var viewCommandBuffer = new ViewCommandBuffer(manager, worker.LogDispatcher);
            viewCommandBuffer.AddComponent(entity, new GameObjectReference { GameObject = gameObject });
            viewCommandBuffer.FlushBuffer();
        }

        private static SpatialOSComponent LinkGameObjectToEntity(EntityId entityId, Entity entity, GameObject gameObject, World world)
        {
            var spatialOSComponent = gameObject.AddComponent<SpatialOSComponent>();
            spatialOSComponent.Worker = world.GetExistingManager<WorkerSystem>();
            spatialOSComponent.World = world;
            spatialOSComponent.Entity = entity;
            spatialOSComponent.SpatialEntityId = entityId;
            return spatialOSComponent;
        }
    }
}
