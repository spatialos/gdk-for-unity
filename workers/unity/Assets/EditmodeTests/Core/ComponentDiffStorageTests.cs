using Improbable.DependentSchema;
using Improbable.Gdk.Core;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Core
{
    [TestFixture]
    public class ComponentDiffStorageTests
    {
        private const long EntityId = 10;

        private WorkerInWorld workerInWorld;
        private SpatialOSReceiveSystem receiveSystem;
        private ComponentUpdateSystem componentUpdateSystem;
        private MockConnectionHandler connectionHandler;

        [SetUp]
        public void Setup()
        {
            var connectionBuilder = new MockConnectionHandlerBuilder();
            connectionHandler = connectionBuilder.ConnectionHandler;

            workerInWorld = WorkerInWorld
                .CreateWorkerInWorldAsync(connectionBuilder, "TestWorkerType", new LoggingDispatcher(), Vector3.zero)
                .Result;

            var world = workerInWorld.World;
            receiveSystem = world.GetExistingSystem<SpatialOSReceiveSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
        }

        [TearDown]
        public void TearDown()
        {
            workerInWorld.Dispose();
        }

        [Test]
        public void Component_updates_are_available_immediately()
        {
            CreateEntity();
            Update();

            UpdatePosition();
            Update();

            var updates = componentUpdateSystem.GetComponentUpdatesReceived<Position.Update>();
            Assert.AreEqual(expected: 1, updates.Count);

            var entitySpecificUpdates =
                componentUpdateSystem.GetEntityComponentUpdatesReceived<Position.Update>(new EntityId(10));
            Assert.AreEqual(expected: 1, entitySpecificUpdates.Count);
        }

        [Test]
        public void Component_updates_are_dropped_if_component_removed()
        {
            CreateEntity();
            Update();

            UpdatePosition();
            connectionHandler.RemoveComponent(entityId: 10, Position.ComponentId);
            Update();

            var updates = componentUpdateSystem.GetComponentUpdatesReceived<Position.Update>();
            Assert.AreEqual(expected: 0, updates.Count);

            var entitySpecificUpdates =
                componentUpdateSystem.GetEntityComponentUpdatesReceived<Position.Update>(new EntityId(10));
            Assert.AreEqual(expected: 0, entitySpecificUpdates.Count);
        }

        [Test]
        public void Events_are_available_immediately()
        {
            CreateEntity();
            Update();

            SendDependentDataEvent();
            Update();

            var events = componentUpdateSystem.GetEventsReceived<DependentDataComponent.FooEvent.Event>();
            Assert.AreEqual(1, events.Count);

            var entitySpecificEvents =
                componentUpdateSystem.GetEventsReceived<DependentDataComponent.FooEvent.Event>(new EntityId(EntityId));
            Assert.AreEqual(1, entitySpecificEvents.Count);
        }

        [Test]
        public void Events_are_dropped_if_component_removed()
        {
            CreateEntity();
            Update();

            SendDependentDataEvent();
            connectionHandler.RemoveComponent(EntityId, DependentDataComponent.ComponentId);
            Update();

            var events = componentUpdateSystem.GetEventsReceived<DependentDataComponent.FooEvent.Event>();
            Assert.AreEqual(0, events.Count);

            var entitySpecificEvents =
                componentUpdateSystem.GetEventsReceived<DependentDataComponent.FooEvent.Event>(new EntityId(EntityId));
            Assert.AreEqual(0, entitySpecificEvents.Count);
        }

        [Test]
        public void Messages_on_different_components_are_not_coupled()
        {
            CreateEntity();
            Update();

            SendDependentDataEvent();
            UpdatePosition();
            connectionHandler.RemoveComponent(EntityId, DependentDataComponent.ComponentId);
            Update();

            var updates = componentUpdateSystem.GetComponentUpdatesReceived<Position.Update>();
            Assert.AreEqual(expected: 1, updates.Count);

            var entitySpecificUpdates =
                componentUpdateSystem.GetEntityComponentUpdatesReceived<Position.Update>(new EntityId(10));
            Assert.AreEqual(expected: 1, entitySpecificUpdates.Count);
        }

        private void CreateEntity()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            template.AddComponent(new DependentDataComponent.Snapshot(), "worker");
            connectionHandler.CreateEntity(EntityId, template);
        }

        private void UpdatePosition()
        {
            const float coordsValue = 5.0f;

            connectionHandler.UpdateComponent(EntityId, Position.ComponentId, new Position.Update
            {
                Coords = new Coordinates(coordsValue, coordsValue, coordsValue)
            });
        }

        private void SendDependentDataEvent()
        {
            connectionHandler.AddEvent(EntityId, DependentDataComponent.ComponentId,
                new DependentDataComponent.FooEvent.Event());
        }

        private void Update()
        {
            receiveSystem.Update();
        }
    }
}
