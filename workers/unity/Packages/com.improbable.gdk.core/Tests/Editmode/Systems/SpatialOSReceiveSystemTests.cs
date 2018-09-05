using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Improbable.Worker;
using Improbable.Worker.Core;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.EditmodeTests.Systems
{
    [TestFixture]
    public class SpatialOSReceiveSystemTests
    {
        internal const uint TestComponentId = 0;
        private const string TestWorkerType = "TestWorker";
        private const long TestEntityId = 1;

        private World world;
        private EntityManager entityManager;
        private WorkerSystem worker;
        private SpatialOSReceiveSystem receiveSystem;
        private TestComponentDispatcher componentDispatcher;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            world = new World("test-world");
            entityManager = world.GetOrCreateManager<EntityManager>();
            worker = world.CreateManager<WorkerSystem>(null, new LoggingDispatcher(), TestWorkerType, Vector3.zero);
            componentDispatcher = new TestComponentDispatcher(worker, world);

            receiveSystem = world.GetOrCreateManager<SpatialOSReceiveSystem>();
            receiveSystem.AddDispatcherHandler(componentDispatcher);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            world.Dispose();
        }

        [TearDown]
        public void TearDown()
        {
            using (var entities = entityManager.GetAllEntities())
            {
                entityManager.DestroyEntity(entities);
            }

            worker.EntityIdToEntity.Clear();
        }

        [Test]
        public void SpatialOSReceiveSystem_should_add_entity()
        {
            var op = WorkerOpFactory.CreateAddEntityOp(TestEntityId);
            receiveSystem.OnAddEntity(op);

            Assert.IsTrue(worker.TryGetEntity(new EntityId(TestEntityId), out var entity));

            var id = new SpatialEntityId(); // Default value
            Assert.DoesNotThrow(() => { id = entityManager.GetComponentData<SpatialEntityId>(entity); });
            Assert.AreEqual(TestEntityId, id.EntityId.Id);

            ComponentType[] worldCommandComponentTypes =
            {
                ComponentType.Create<WorldCommands.CreateEntity.CommandSender>(),
                ComponentType.Create<WorldCommands.DeleteEntity.CommandSender>(),
                ComponentType.Create<WorldCommands.EntityQuery.CommandSender>(),
                ComponentType.Create<WorldCommands.ReserveEntityIds.CommandSender>()
            };

            foreach (var type in worldCommandComponentTypes)
            {
                Assert.IsTrue(entityManager.HasComponent(entity, type));
            }
        }
    }

    public class TestComponentDispatcher : ComponentDispatcherHandler, SpatialOSReceiveSystem.ITestComponentDispatcher
    {
        public TestComponentDispatcher(WorkerSystem worker, World world) : base(worker, world)
        {
        }

        public override uint ComponentId => SpatialOSReceiveSystemTests.TestComponentId;

        public override void OnAddComponent(AddComponentOp op)
        {
            throw new System.NotImplementedException();
        }

        public override void OnRemoveComponent(RemoveComponentOp op)
        {
            throw new System.NotImplementedException();
        }

        public override void OnComponentUpdate(ComponentUpdateOp op)
        {
            throw new System.NotImplementedException();
        }

        public override void OnAuthorityChange(AuthorityChangeOp op)
        {
            throw new System.NotImplementedException();
        }

        public override void OnCommandRequest(CommandRequestOp op)
        {
            throw new System.NotImplementedException();
        }

        public override void OnCommandResponse(CommandResponseOp op)
        {
            throw new System.NotImplementedException();
        }

        public override void AddCommandComponents(Entity entity)
        {
        }

        public override void Dispose()
        {
        }
    }
}
