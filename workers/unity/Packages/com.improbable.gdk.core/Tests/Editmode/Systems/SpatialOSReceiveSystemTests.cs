using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Improbable.Worker;
using Improbable.Worker.Core;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.EditmodeTests.Systems
{
    [TestFixture]
    public class SpatialOSReceiveSystemTests
    {
        internal const uint TestComponentId = 1;
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
            foreach (var entity in worker.EntityIdToEntity.Values)
            {
                try
                {
                    WorldCommands.DeallocateWorldCommandRequesters(entityManager, entity);
                }
                catch (Exception e)
                {
                }
            }

            worker.EntityIdToEntity.Clear();

            using (var entities = entityManager.GetAllEntities())
            {
                entityManager.DestroyEntity(entities);
            }

            componentDispatcher.Reset();
        }

        [Test]
        public void OnAddEntity_should_add_entity_and_world_command_components()
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

        [Test]
        public void OnRemoveEntity_should_remove_entity_and_deallocate_world_command_providers()
        {
            var entity = SetupTestEntity();

            var createEntityRequestsHandle =
                entityManager.GetComponentData<WorldCommands.CreateEntity.CommandSender>(entity).Handle;
            var deleteEntityRequestsHandle =
                entityManager.GetComponentData<WorldCommands.DeleteEntity.CommandSender>(entity).Handle;
            var entityQueryRequestsHandle =
                entityManager.GetComponentData<WorldCommands.EntityQuery.CommandSender>(entity).Handle;
            var reserveEntityIdsRequestsHandle =
                entityManager.GetComponentData<WorldCommands.ReserveEntityIds.CommandSender>(entity).Handle;

            var op = WorkerOpFactory.CreateRemoveEntityOp(TestEntityId);
            receiveSystem.OnRemoveEntity(op);

            Assert.IsFalse(entityManager.Exists(entity));
            Assert.IsFalse(worker.TryGetEntity(new EntityId(TestEntityId), out _));

            Assert.Throws<ArgumentException>(() => WorldCommands.CreateEntity.RequestsProvider.Get(createEntityRequestsHandle));
            Assert.Throws<ArgumentException>(() => WorldCommands.DeleteEntity.RequestsProvider.Get(deleteEntityRequestsHandle));
            Assert.Throws<ArgumentException>(() => WorldCommands.EntityQuery.RequestsProvider.Get(entityQueryRequestsHandle));
            Assert.Throws<ArgumentException>(() => WorldCommands.ReserveEntityIds.RequestsProvider.Get(reserveEntityIdsRequestsHandle));
        }

        [Test]
        public void OnAddComponent_should_error_if_unknown_component_id_received()
        {
            var op = WorkerOpFactory.CreateAddComponentOp(TestEntityId, 0);
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            receiveSystem.OnAddComponent(op);
        }

        [Test]
        public void OnAddComponent_should_be_delegated_to_correct_dispatcher()
        {
            var op = WorkerOpFactory.CreateAddComponentOp(TestEntityId, TestComponentId);
            receiveSystem.OnAddComponent(op);

            Assert.IsTrue(componentDispatcher.HasAddComponentReceived);
        }

        [Test]
        public void OnRemoveComponent_should_error_if_unknown_component_id_received()
        {
            var op = WorkerOpFactory.CreateRemoveComponentOp(TestEntityId, 0);
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            receiveSystem.OnRemoveComponent(op);
        }

        [Test]
        public void OnRemoveComponent_should_be_delegated_to_correct_dispatcher()
        {
            var op = WorkerOpFactory.CreateAddComponentOp(TestEntityId, TestComponentId);
            receiveSystem.OnAddComponent(op);

            Assert.IsTrue(componentDispatcher.HasAddComponentReceived);
        }

        private Entity SetupTestEntity()
        {
            var entity = entityManager.CreateEntity();
            var entityId = new EntityId(TestEntityId);
            entityManager.AddComponentData(entity, new SpatialEntityId { EntityId = entityId });
            worker.EntityIdToEntity.Add(entityId, entity);

            WorldCommands.AddWorldCommandRequesters(world, entityManager, entity);

            return entity;
        }
    }

    public class TestComponentDispatcher : ComponentDispatcherHandler, SpatialOSReceiveSystem.ITestComponentDispatcher
    {
        public bool HasAddComponentReceived;
        public bool HasRemoveComponentReceived;
        public bool HasComponentUpdateReceived;
        public bool HasAuthorityChangedReceived;
        public bool HasCommandRequestReceived;
        public bool HasCommandResponseReceived;

        public override uint ComponentId => SpatialOSReceiveSystemTests.TestComponentId;

        public TestComponentDispatcher(WorkerSystem worker, World world) : base(worker, world)
        {
        }

        public void Reset()
        {
            HasAddComponentReceived = false;
            HasRemoveComponentReceived = false;
            HasComponentUpdateReceived = false;
            HasAuthorityChangedReceived = false;
            HasCommandRequestReceived = false;
            HasCommandResponseReceived = false;
        }

        public override void OnAddComponent(AddComponentOp op)
        {
            HasAddComponentReceived = true;
            op.Data.SchemaData.Value.Dispose();
        }

        public override void OnRemoveComponent(RemoveComponentOp op)
        {
            HasRemoveComponentReceived = true;
        }

        public override void OnComponentUpdate(ComponentUpdateOp op)
        {
            HasComponentUpdateReceived = true;
            op.Update.SchemaData.Value.Dispose();
        }

        public override void OnAuthorityChange(AuthorityChangeOp op)
        {
            HasAuthorityChangedReceived = true;
        }

        public override void OnCommandRequest(CommandRequestOp op)
        {
            HasCommandRequestReceived = true;
            op.Request.SchemaData.Value.Dispose();
        }

        public override void OnCommandResponse(CommandResponseOp op)
        {
            HasCommandResponseReceived = true;
            op.Response.SchemaData.Value.Dispose();
        }

        public override void AddCommandComponents(Entity entity)
        {
        }

        public override void Dispose()
        {
        }
    }
}
