using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation.EditmodeTests
{
    [TestFixture]
    public class WorkerEntityLinkingTests : HybridGdkSystemTestBase
    {
        private class MonoBehaviourOnWorker : MonoBehaviour
        {
            [Require] public WorldCommands.Requirable.WorldCommandRequestSender WorldCommandSender;
        }

        private World world;
        private GameObject workerGameObject;
        private WorkerSystem worker;
        private EntityManager entityManager;
        private WorkerEntityGameObjectLinkerSystem workerLinkerSystem;
        private GameObjectDispatcherSystem gameObjectDispatcherSystem;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            entityManager = world.GetOrCreateManager<EntityManager>();
            worker = world.CreateManager<WorkerSystem>(null, new LoggingDispatcher(), "TestWorker", Vector3.zero);
            world.CreateManager<EntityGameObjectLinkerSystem>();

            gameObjectDispatcherSystem = world.GetOrCreateManager<GameObjectDispatcherSystem>();

            workerGameObject = new GameObject();
            workerGameObject.AddComponent<MonoBehaviourOnWorker>();
            GameObjectCreationHelper.EnableStandardGameObjectCreation(world, workerGameObject: workerGameObject);
            workerLinkerSystem = world.GetExistingManager<WorkerEntityGameObjectLinkerSystem>();
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
            if (workerGameObject != null)
            {
                Object.DestroyImmediate(workerGameObject);
            }
        }

        [Test]
        public void GameObject_Is_Linked_To_Worker_Entity_On_Startup()
        {
            workerLinkerSystem.Update();
            Assert.NotNull(workerGameObject.GetComponent<SpatialOSComponent>());
            Assert.NotNull(entityManager.HasComponent<GameObjectReference>(worker.WorkerEntity));
        }

        [Test]
        public void Requirable_Injected_Into_Worker_GameObject()
        {
            workerLinkerSystem.Update();
            gameObjectDispatcherSystem.Update();
            Assert.NotNull(workerGameObject.GetComponent<MonoBehaviourOnWorker>().WorldCommandSender);
        }

        [Test]
        public void GameObject_Is_Unlinked_From_Worker_Entity_On_Disconnect()
        {
            workerLinkerSystem.Update();
            entityManager.RemoveComponent<OnConnected>(worker.WorkerEntity);
            entityManager.AddSharedComponentData(worker.WorkerEntity, new OnDisconnected());
            workerLinkerSystem.Update();
            Assert.IsNull(workerGameObject.GetComponent<SpatialOSComponent>());
            Assert.IsFalse(entityManager.HasComponent<GameObjectReference>(worker.WorkerEntity));
        }
    }
}
