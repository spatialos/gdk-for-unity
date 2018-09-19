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
    }
}
