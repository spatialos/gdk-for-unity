using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.PlayerLifecycle
{
    [TestFixture]
    public class PlayerLifecycleHelperTests
    {
        private SpatialOSReceiveSystem receiveSystem;
        private MockConnectionHandler connectionHandler;
        private WorkerInWorld worker;

        [SetUp]
        public void Setup()
        {
            // TODO: The WorkerEntityId for this worker is '0'. Probably should refactor this in the future.
            var builder = new MockConnectionHandlerBuilder();
            connectionHandler = builder.ConnectionHandler;

            worker = WorkerInWorld.CreateWorkerInWorldAsync(builder, "TestWorkerType", new LoggingDispatcher(),
                Vector3.zero).Result;

            receiveSystem = worker.World.GetExistingSystem<SpatialOSReceiveSystem>();
        }

        [Test]
        public void IsOwningWorker_should_return_false_if_entity_does_not_exist()
        {
            Assert.IsFalse(PlayerLifecycleHelper.IsOwningWorker(new EntityId(10), worker.World));
        }

        [Test]
        public void IsOwningWorker_should_return_false_if_entity_doesnt_have_OwningWorker_component()
        {
            connectionHandler.CreateEntity(1, new EntityTemplate());
            receiveSystem.Update();

            Assert.IsFalse(PlayerLifecycleHelper.IsOwningWorker(new EntityId(1), worker.World));
        }

        [Test]
        public void IsOwningWorker_should_return_false_if_entity_isnt_owned_by_this_worker()
        {
            connectionHandler.CreateEntity(1, GetOwnedEntity(new EntityId(1)));
            receiveSystem.Update();

            Assert.IsFalse(PlayerLifecycleHelper.IsOwningWorker(new EntityId(1), worker.World));
        }

        [Test]
        public void IsOwningWorker_should_return_true_if_OwningWorker_component_has_my_worker_id()
        {
            connectionHandler.CreateEntity(1, GetOwnedEntity(worker.WorkerEntityId));
            receiveSystem.Update();

            Assert.IsTrue(PlayerLifecycleHelper.IsOwningWorker(new EntityId(1), worker.World));
        }

        [TearDown]
        public void TearDown()
        {
            worker.Dispose();
        }

        private static EntityTemplate GetOwnedEntity(EntityId workerEntityId)
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new OwningWorker.Snapshot(workerEntityId));
            return template;
        }
    }
}
