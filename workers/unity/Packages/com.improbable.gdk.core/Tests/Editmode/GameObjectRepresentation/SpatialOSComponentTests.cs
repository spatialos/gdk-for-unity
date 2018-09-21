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
        private World world;
        private WorkerSystem worker;
        private EntityManager entityManager;
        private GameObject testGameObject;
        private Entity testEntity;
        private readonly EntityId testSpatialEntityId = new EntityId(1337);

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            entityManager = world.GetOrCreateManager<EntityManager>();
            worker = world.CreateManager<WorkerSystem>(null, new TestLogDispatcher(), "TestWorker", Vector3.zero);
            testGameObject = new GameObject();
            testEntity = entityManager.CreateEntity();

            var spatialOSComponent = testGameObject.AddComponent<SpatialOSComponent>();
            spatialOSComponent.Worker = worker;
            spatialOSComponent.World = world;
            spatialOSComponent.Entity = testEntity;
            spatialOSComponent.SpatialEntityId = testSpatialEntityId;
        }

        [TearDown]
        public void TearDown()
        {
            if (testGameObject != null)
            {
                UnityObjectDestroyer.Destroy(testGameObject);
            }

            world.Dispose();
        }

        [Test]
        public void TryGetGameObjectForEntity_returns_true_when_gameobject_is_linked()
        {
            worker.EntityIdToEntity.Add(testSpatialEntityId, testEntity);
            var viewCommandBuffer = new ViewCommandBuffer(entityManager, worker.LogDispatcher);
            viewCommandBuffer.AddComponent(testEntity, new GameObjectReference { GameObject = testGameObject });
            viewCommandBuffer.FlushBuffer();

            var component = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(component);

            var succeeded = component.TryGetGameObjectForSpatialOSEntityId(testSpatialEntityId, out var linkedGameObject);
            Assert.IsTrue(succeeded);
            Assert.AreEqual(testGameObject, linkedGameObject);
        }

        [Test]
        public void TryGetGameObjectForEntity_returns_false_when_gameobject_is_not_linked()
        {
            worker.EntityIdToEntity.Add(testSpatialEntityId, testEntity);
            var component = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(component);

            var succeeded = component.TryGetGameObjectForSpatialOSEntityId(testSpatialEntityId, out var linkedGameObject);
            Assert.IsFalse(succeeded);
            Assert.IsNull(linkedGameObject);
        }

        [Test]
        public void TryGetGameObjectForEntity_returns_false_when_entity_does_not_exist()
        {
            var component = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(component);

            var succeeded = component.TryGetGameObjectForSpatialOSEntityId(testSpatialEntityId, out var linkedGameObject);
            Assert.IsFalse(succeeded);
            Assert.IsNull(linkedGameObject);
        }

        [Test]
        public void TryGetSpatialOSEntityIdForGameObject_returns_true_when_gameobject_is_linked()
        {
            var component = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(component);

            var succeeded = component.TryGetSpatialOSEntityIdForGameObject(testGameObject, out var linkedSpatialEntityId);
            Assert.IsTrue(succeeded);
            Assert.AreEqual(testSpatialEntityId, linkedSpatialEntityId);
        }

        [Test]
        public void TryGetSpatialOSEntityIdForGameObject_returns_false_when_gameobject_is_not_linked()
        {
            worker.EntityIdToEntity.Add(testSpatialEntityId, testEntity);
            var component = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(component);

            var succeeded = component.TryGetSpatialOSEntityIdForGameObject(new GameObject(), out var linkedSpatialEntityId);
            Assert.IsFalse(succeeded);
            Assert.AreEqual(default(EntityId), linkedSpatialEntityId);
        }

        [Test]
        public void TryGetSpatialOSEntityIdForGameObject_returns_false_when_entity_on_different_worker()
        {
            worker.EntityIdToEntity.Add(testSpatialEntityId, testEntity);
            var component = testGameObject.GetComponent<SpatialOSComponent>();
            Assert.NotNull(component);

            var testGameObject2 = new GameObject();
            var spatialOSComponent = testGameObject.AddComponent<SpatialOSComponent>();
            spatialOSComponent.Worker = world.CreateManager<WorkerSystem>(null, new TestLogDispatcher(), "TestWorker", Vector3.zero);;
            spatialOSComponent.World = world;
            spatialOSComponent.Entity = testEntity;
            spatialOSComponent.SpatialEntityId = testSpatialEntityId;

            var succeeded = component.TryGetSpatialOSEntityIdForGameObject(testGameObject2, out var linkedSpatialEntityId);
            Assert.IsFalse(succeeded);
            Assert.AreEqual(default(EntityId), linkedSpatialEntityId);
        }
    }
}
