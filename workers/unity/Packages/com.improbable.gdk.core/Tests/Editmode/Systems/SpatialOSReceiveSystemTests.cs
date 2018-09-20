using System;
using System.Linq;
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
        internal const uint FirstTestComponentId = 1;
        internal const uint SecondTestComponentId = 2;

        private const uint InvalidComponentId = 0;
        private const string TestWorkerType = "TestWorker";
        private const long TestEntityId = 1;

        private const uint TestCommandIndex = 1;
        private const long TestCommandRequestId = 1;

        private World world;
        private EntityManager entityManager;

        private WorkerSystem worker;
        private SpatialOSReceiveSystem receiveSystem;

        private FirstComponentDispatcher firstComponentDispatcher;
        private SecondComponentDispatcher secondComponentDispatcher;
        private TestLogDispatcher logDispatcher;

        private WorldCommands.CreateEntity.Storage createEntityStorage;
        private WorldCommands.DeleteEntity.Storage deleteEntityStorage;
        private WorldCommands.ReserveEntityIds.Storage reserveEntityIdsStorage;
        private WorldCommands.EntityQuery.Storage entityQueryStorage;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            world = new World("test-world");
            entityManager = world.GetOrCreateManager<EntityManager>();
            logDispatcher = new TestLogDispatcher();

            worker = world.CreateManager<WorkerSystem>(null, logDispatcher, TestWorkerType, Vector3.zero);

            firstComponentDispatcher = new FirstComponentDispatcher(worker, world);
            secondComponentDispatcher = new SecondComponentDispatcher(worker, world);

            receiveSystem = world.GetOrCreateManager<SpatialOSReceiveSystem>();

            // Do not add command components from any generated code.
            receiveSystem.AddAllCommandComponents.Clear();
            receiveSystem.AddDispatcherHandler(firstComponentDispatcher);
            receiveSystem.AddDispatcherHandler(secondComponentDispatcher);

            var requestTracker = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            createEntityStorage = requestTracker.GetCommandStorageForType<WorldCommands.CreateEntity.Storage>();
            deleteEntityStorage = requestTracker.GetCommandStorageForType<WorldCommands.DeleteEntity.Storage>();
            reserveEntityIdsStorage = requestTracker.GetCommandStorageForType<WorldCommands.ReserveEntityIds.Storage>();
            entityQueryStorage = requestTracker.GetCommandStorageForType<WorldCommands.EntityQuery.Storage>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            logDispatcher.Dispose();
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
                catch (Exception)
                {
                }
            }

            worker.EntityIdToEntity.Clear();

            using (var allEntities = entityManager.GetAllEntities())
            {
                foreach (var entity in allEntities.Where(x => x != worker.WorkerEntity))
                {
                    entityManager.DestroyEntity(entity);
                }
            }

            firstComponentDispatcher.Reset();
            secondComponentDispatcher.Reset();

            createEntityStorage.CommandRequestsInFlight.Clear();
            deleteEntityStorage.CommandRequestsInFlight.Clear();
            reserveEntityIdsStorage.CommandRequestsInFlight.Clear();
            entityQueryStorage.CommandRequestsInFlight.Clear();

            WorldCommands.CreateEntity.ResponsesProvider.CleanDataInWorld(world);
            WorldCommands.EntityQuery.ResponsesProvider.CleanDataInWorld(world);
            WorldCommands.DeleteEntity.ResponsesProvider.CleanDataInWorld(world);
            WorldCommands.ReserveEntityIds.ResponsesProvider.CleanDataInWorld(world);
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

        [Test]
        public void OnAddEntity_should_add_entity_and_command_components()
        {
            using (var wrappedOp = WorkerOpFactory.CreateAddEntityOp(TestEntityId))
            {
                receiveSystem.OnAddEntity(wrappedOp.Op);
            }

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

            Assert.IsTrue(entityManager.HasComponent(entity,
                ComponentType.Create<FirstComponentDispatcher.CommandComponent>()));
            Assert.IsTrue(entityManager.HasComponent(entity,
                ComponentType.Create<SecondComponentDispatcher.CommandComponent>()));
        }

        [Test]
        public void OnAddEntity_should_throw_if_entity_already_exists()
        {
            SetupTestEntity();

            using (var wrappedOp = WorkerOpFactory.CreateAddEntityOp(TestEntityId))
            {
                Assert.Throws<InvalidSpatialEntityStateException>(() => { receiveSystem.OnAddEntity(wrappedOp.Op); });
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

            using (var wrappedOp = WorkerOpFactory.CreateRemoveEntityOp(TestEntityId))
            {
                receiveSystem.OnRemoveEntity(wrappedOp.Op);
            }

            Assert.IsFalse(entityManager.Exists(entity));
            Assert.IsFalse(worker.TryGetEntity(new EntityId(TestEntityId), out _));

            Assert.Throws<ArgumentException>(() =>
                WorldCommands.CreateEntity.RequestsProvider.Get(createEntityRequestsHandle));
            Assert.Throws<ArgumentException>(() =>
                WorldCommands.DeleteEntity.RequestsProvider.Get(deleteEntityRequestsHandle));
            Assert.Throws<ArgumentException>(() =>
                WorldCommands.EntityQuery.RequestsProvider.Get(entityQueryRequestsHandle));
            Assert.Throws<ArgumentException>(() =>
                WorldCommands.ReserveEntityIds.RequestsProvider.Get(reserveEntityIdsRequestsHandle));
        }

        [Test]
        public void OnRemoveEntity_should_error_if_entity_does_not_exist()
        {
            using (var wrappedOp = WorkerOpFactory.CreateRemoveEntityOp(TestEntityId))
            {
                Assert.Throws<InvalidSpatialEntityStateException>(() =>
                {
                    receiveSystem.OnRemoveEntity(wrappedOp.Op);
                });
            }
        }

        [Test]
        public void OnAddComponent_should_be_delegated_to_correct_dispatcher()
        {
            using (var wrappedOp = WorkerOpFactory.CreateAddComponentOp(TestEntityId, FirstTestComponentId))
            {
                receiveSystem.OnAddComponent(wrappedOp.Op);
            }

            Assert.IsTrue(firstComponentDispatcher.HasAddComponentReceived);
            Assert.IsFalse(secondComponentDispatcher.HasAddComponentReceived);
        }

        [Test]
        public void OnAddComponent_should_error_if_unknown_component_id_received()
        {
            using (var wrappedOp = WorkerOpFactory.CreateAddComponentOp(TestEntityId, InvalidComponentId))
            {
                Assert.Throws<UnknownComponentIdException>(() => { receiveSystem.OnAddComponent(wrappedOp.Op); });
            }
        }

        [Test]
        public void OnRemoveComponent_should_be_delegated_to_correct_dispatcher()
        {
            using (var wrappedOp = WorkerOpFactory.CreateRemoveComponentOp(TestEntityId, FirstTestComponentId))
            {
                receiveSystem.OnRemoveComponent(wrappedOp.Op);
            }

            Assert.IsTrue(firstComponentDispatcher.HasRemoveComponentReceived);
            Assert.IsFalse(secondComponentDispatcher.HasRemoveComponentReceived);
        }

        [Test]
        public void OnRemoveComponent_should_error_if_unknown_component_id_received()
        {
            using (var wrappedOp = WorkerOpFactory.CreateRemoveComponentOp(TestEntityId, InvalidComponentId))
            {
                Assert.Throws<UnknownComponentIdException>(() => { receiveSystem.OnRemoveComponent(wrappedOp.Op); });
            }
        }

        [Test]
        public void OnComponentUpdate_should_be_delegated_to_correct_dispatcher()
        {
            using (var wrappedOp = WorkerOpFactory.CreateComponentUpdateOp(TestEntityId, FirstTestComponentId))
            {
                receiveSystem.OnComponentUpdate(wrappedOp.Op);
            }

            Assert.IsTrue(firstComponentDispatcher.HasComponentUpdateReceived);
            Assert.IsFalse(secondComponentDispatcher.HasComponentUpdateReceived);
        }

        [Test]
        public void OnComponentUpdate_should_error_if_unknown_component_id_received()
        {
            using (var wrappedOp = WorkerOpFactory.CreateComponentUpdateOp(TestEntityId, InvalidComponentId))
            {
                Assert.Throws<UnknownComponentIdException>(() => { receiveSystem.OnComponentUpdate(wrappedOp.Op); });
            }
        }

        [Test]
        public void OnAuthorityChange_should_be_delegated_to_correct_dispatcher()
        {
            using (var wrappedOp = WorkerOpFactory.CreateAuthorityChangeOp(TestEntityId, FirstTestComponentId))
            {
                receiveSystem.OnAuthorityChange(wrappedOp.Op);
            }

            Assert.IsTrue(firstComponentDispatcher.HasAuthorityChangedReceived);
            Assert.IsFalse(secondComponentDispatcher.HasAuthorityChangedReceived);
        }

        [Test]
        public void OnAuthorityChange_should_error_if_unknown_component_id_received()
        {
            using (var wrappedOp = WorkerOpFactory.CreateAuthorityChangeOp(TestEntityId, InvalidComponentId))
            {
                Assert.Throws<UnknownComponentIdException>(() => { receiveSystem.OnAuthorityChange(wrappedOp.Op); });
            }
        }

        [Test]
        public void OnCommandRequest_should_be_delegated_to_correct_dispatcher()
        {
            using (var wrappedOp =
                WorkerOpFactory.CreateCommandRequestOp(FirstTestComponentId, TestCommandIndex, TestCommandRequestId))
            {
                receiveSystem.OnCommandRequest(wrappedOp.Op);
            }

            Assert.IsTrue(firstComponentDispatcher.HasCommandRequestReceived);
            Assert.IsFalse(secondComponentDispatcher.HasCommandRequestReceived);
        }

        [Test]
        public void OnCommandRequest_should_error_if_unknown_component_id_received()
        {
            using (var wrappedOp =
                WorkerOpFactory.CreateCommandRequestOp(InvalidComponentId, TestCommandIndex, TestCommandRequestId))
            {
                Assert.Throws<UnknownComponentIdException>(() => { receiveSystem.OnCommandRequest(wrappedOp.Op); });
            }
        }

        [Test]
        public void OnCommandResponse_should_be_delegated_to_correct_dispatcher()
        {
            using (var wrappedOp =
                WorkerOpFactory.CreateCommandResponseOp(FirstTestComponentId, TestCommandIndex, TestCommandRequestId))
            {
                receiveSystem.OnCommandResponse(wrappedOp.Op);
            }

            Assert.IsTrue(firstComponentDispatcher.HasCommandResponseReceived);
            Assert.IsFalse(secondComponentDispatcher.HasCommandResponseReceived);
        }

        [Test]
        public void OnCommandResponse_should_error_if_unknown_component_id_received()
        {
            using (var wrappedOp =
                WorkerOpFactory.CreateCommandResponseOp(InvalidComponentId, TestCommandIndex, TestCommandRequestId))
            {
                Assert.Throws<UnknownComponentIdException>(() => { receiveSystem.OnCommandResponse(wrappedOp.Op); });
            }
        }

        [Test]
        public void OnDisconnect_should_add_OnDisconnected_to_WorkerEntity_and_deallocate_world_command_senders()
        {
            const string DisconnectReason = "Testing disconnect.";

            var createEntityRequestsHandle =
                entityManager.GetComponentData<WorldCommands.CreateEntity.CommandSender>(worker.WorkerEntity).Handle;
            var deleteEntityRequestsHandle =
                entityManager.GetComponentData<WorldCommands.DeleteEntity.CommandSender>(worker.WorkerEntity).Handle;
            var entityQueryRequestsHandle =
                entityManager.GetComponentData<WorldCommands.EntityQuery.CommandSender>(worker.WorkerEntity).Handle;
            var reserveEntityIdsRequestsHandle =
                entityManager.GetComponentData<WorldCommands.ReserveEntityIds.CommandSender>(worker.WorkerEntity)
                    .Handle;

            using (var wrappedOp = WorkerOpFactory.CreateDisconnectOp(DisconnectReason))
            {
                receiveSystem.OnDisconnect(wrappedOp.Op);
            }

            Assert.IsTrue(entityManager.HasComponent<OnDisconnected>(worker.WorkerEntity));
            Assert.AreEqual(DisconnectReason,
                entityManager.GetSharedComponentData<OnDisconnected>(worker.WorkerEntity).ReasonForDisconnect);

            Assert.Throws<ArgumentException>(() =>
                WorldCommands.CreateEntity.RequestsProvider.Get(createEntityRequestsHandle));
            Assert.Throws<ArgumentException>(() =>
                WorldCommands.DeleteEntity.RequestsProvider.Get(deleteEntityRequestsHandle));
            Assert.Throws<ArgumentException>(() =>
                WorldCommands.EntityQuery.RequestsProvider.Get(entityQueryRequestsHandle));
            Assert.Throws<ArgumentException>(() =>
                WorldCommands.ReserveEntityIds.RequestsProvider.Get(reserveEntityIdsRequestsHandle));
        }

        [Test]
        public void OnCreateEntityResponse_should_add_received_responses_to_entity()
        {
            var entity = SetupTestEntity();

            var emptyRequest = new WorldCommands.CreateEntity.Request();
            var context = "Some context";

            createEntityStorage.CommandRequestsInFlight.Add(TestCommandRequestId,
                new CommandRequestStore<WorldCommands.CreateEntity.Request>(entity,
                    emptyRequest, context, TestCommandRequestId));

            using (var wrappedOp = WorkerOpFactory.CreateCreateEntityResponseOp(TestCommandRequestId))
            {
                receiveSystem.OnCreateEntityResponse(wrappedOp.Op);

                Assert.IsTrue(entityManager.HasComponent<WorldCommands.CreateEntity.CommandResponses>(entity));

                var responses = entityManager.GetComponentData<WorldCommands.CreateEntity.CommandResponses>(entity);

                var count = 0;
                Assert.DoesNotThrow(() => { count = responses.Responses.Count; });
                Assert.AreEqual(1, count);

                var response = responses.Responses[0];

                Assert.AreEqual(emptyRequest, response.RequestPayload);
                Assert.AreEqual(context, response.Context);
                Assert.AreEqual(TestCommandRequestId, response.RequestId);
                Assert.AreEqual(wrappedOp.Op.StatusCode, response.StatusCode);
                Assert.AreEqual(wrappedOp.Op.Message, response.Message);
                Assert.AreEqual(wrappedOp.Op.EntityId, response.EntityId);
            }
        }

        [Test]
        public void OnCreateEntityResponse_should_error_if_request_id_not_found()
        {
            using (var wrappedOp = WorkerOpFactory.CreateCreateEntityResponseOp(TestCommandRequestId))
            {
                Assert.Throws<UnknownRequestIdException>(() => { receiveSystem.OnCreateEntityResponse(wrappedOp.Op); });
            }
        }

        [Test]
        public void OnCreateEntityResponse_should_log_if_corresponding_entity_not_found()
        {
            var emptyRequest = new WorldCommands.CreateEntity.Request();

            createEntityStorage.CommandRequestsInFlight.Add(TestCommandRequestId,
                new CommandRequestStore<WorldCommands.CreateEntity.Request>(Entity.Null,
                    emptyRequest, null, TestCommandRequestId));

            using (var wrappedOp = WorkerOpFactory.CreateCreateEntityResponseOp(TestCommandRequestId))
            {
                using (var expectingScope = logDispatcher.EnterExpectingScope())
                {
                    expectingScope.Expect(LogType.Log, LoggingUtils.LoggerName, "Op");
                    receiveSystem.OnCreateEntityResponse(wrappedOp.Op);
                }
            }
        }

        [Test]
        public void OnDeleteEntityResponse_should_add_received_responses_to_entity()
        {
            var entity = SetupTestEntity();

            var emptyRequest = new WorldCommands.DeleteEntity.Request();
            var context = "Some context";

            deleteEntityStorage.CommandRequestsInFlight.Add(TestCommandRequestId,
                new CommandRequestStore<WorldCommands.DeleteEntity.Request>(entity,
                    emptyRequest, context, TestCommandRequestId));

            using (var wrappedOp = WorkerOpFactory.CreateDeleteEntityResponseOp(TestCommandRequestId))
            {
                receiveSystem.OnDeleteEntityResponse(wrappedOp.Op);

                Assert.IsTrue(entityManager.HasComponent<WorldCommands.DeleteEntity.CommandResponses>(entity));

                var responses = entityManager.GetComponentData<WorldCommands.DeleteEntity.CommandResponses>(entity);

                var count = 0;
                Assert.DoesNotThrow(() => { count = responses.Responses.Count; });
                Assert.AreEqual(1, count);

                var response = responses.Responses[0];

                Assert.AreEqual(emptyRequest, response.RequestPayload);
                Assert.AreEqual(context, response.Context);
                Assert.AreEqual(TestCommandRequestId, response.RequestId);
                Assert.AreEqual(wrappedOp.Op.StatusCode, response.StatusCode);
                Assert.AreEqual(wrappedOp.Op.Message, response.Message);
                Assert.AreEqual(wrappedOp.Op.EntityId, response.EntityId);
            }
        }

        [Test]
        public void OnDeleteEntityResponse_should_error_if_request_id_not_found()
        {
            using (var wrappedOp = WorkerOpFactory.CreateDeleteEntityResponseOp(TestCommandRequestId))
            {
                Assert.Throws<UnknownRequestIdException>(() => { receiveSystem.OnDeleteEntityResponse(wrappedOp.Op); });
            }
        }

        [Test]
        public void OnDeleteEntityResponse_should_log_if_corresponding_entity_not_found()
        {
            var emptyRequest = new WorldCommands.DeleteEntity.Request();

            deleteEntityStorage.CommandRequestsInFlight.Add(TestCommandRequestId,
                new CommandRequestStore<WorldCommands.DeleteEntity.Request>(Entity.Null,
                    emptyRequest, null, TestCommandRequestId));

            using (var wrappedOp = WorkerOpFactory.CreateDeleteEntityResponseOp(TestCommandRequestId))
            {
                using (var expectingScope = logDispatcher.EnterExpectingScope())
                {
                    expectingScope.Expect(LogType.Log, LoggingUtils.LoggerName, "Op");
                    receiveSystem.OnDeleteEntityResponse(wrappedOp.Op);
                }
            }
        }

        [Test]
        public void OnEntityQueryResponse_should_add_received_responses_to_entity()
        {
            var entity = SetupTestEntity();

            var emptyRequest = new WorldCommands.EntityQuery.Request();
            var context = "Some context";

            entityQueryStorage.CommandRequestsInFlight.Add(TestCommandRequestId,
                new CommandRequestStore<WorldCommands.EntityQuery.Request>(entity,
                    emptyRequest, context, TestCommandRequestId));

            using (var wrappedOp = WorkerOpFactory.CreateEntityQueryResponseOp(TestCommandRequestId))
            {
                receiveSystem.OnEntityQueryResponse(wrappedOp.Op);

                Assert.IsTrue(entityManager.HasComponent<WorldCommands.EntityQuery.CommandResponses>(entity));

                var responses = entityManager.GetComponentData<WorldCommands.EntityQuery.CommandResponses>(entity);

                var count = 0;
                Assert.DoesNotThrow(() => { count = responses.Responses.Count; });
                Assert.AreEqual(1, count);

                var response = responses.Responses[0];

                Assert.AreEqual(emptyRequest, response.RequestPayload);
                Assert.AreEqual(context, response.Context);
                Assert.AreEqual(TestCommandRequestId, response.RequestId);
                Assert.AreEqual(wrappedOp.Op.StatusCode, response.StatusCode);
                Assert.AreEqual(wrappedOp.Op.Message, response.Message);
                Assert.AreEqual(wrappedOp.Op.Result, response.Result);
                Assert.AreEqual(wrappedOp.Op.ResultCount, response.ResultCount);
            }
        }

        [Test]
        public void OnEntityQueryResponse_should_error_if_request_id_not_found()
        {
            using (var wrappedOp = WorkerOpFactory.CreateEntityQueryResponseOp(TestCommandRequestId))
            {
                Assert.Throws<UnknownRequestIdException>(() => { receiveSystem.OnEntityQueryResponse(wrappedOp.Op); });
            }
        }

        [Test]
        public void OnEntityQueryResponse_should_log_if_corresponding_entity_not_found()
        {
            var emptyRequest = new WorldCommands.EntityQuery.Request();

            entityQueryStorage.CommandRequestsInFlight.Add(TestCommandRequestId,
                new CommandRequestStore<WorldCommands.EntityQuery.Request>(Entity.Null,
                    emptyRequest, null, TestCommandRequestId));

            using (var wrappedOp = WorkerOpFactory.CreateEntityQueryResponseOp(TestCommandRequestId))
            {
                using (var expectingScope = logDispatcher.EnterExpectingScope())
                {
                    expectingScope.Expect(LogType.Log, LoggingUtils.LoggerName, "Op");
                    receiveSystem.OnEntityQueryResponse(wrappedOp.Op);
                }
            }
        }

        [Test]
        public void OnReserveEntityIdsResponse_should_add_received_responses_to_entity()
        {
            var entity = SetupTestEntity();

            var emptyRequest = new WorldCommands.ReserveEntityIds.Request();
            var context = "Some context";

            reserveEntityIdsStorage.CommandRequestsInFlight.Add(TestCommandRequestId,
                new CommandRequestStore<WorldCommands.ReserveEntityIds.Request>(entity,
                    emptyRequest, context, TestCommandRequestId));

            using (var wrappedOp = WorkerOpFactory.CreateReserveEntityIdsResponseOp(TestCommandRequestId))
            {
                receiveSystem.OnReserveEntityIdsResponse(wrappedOp.Op);

                Assert.IsTrue(entityManager.HasComponent<WorldCommands.ReserveEntityIds.CommandResponses>(entity));

                var responses = entityManager.GetComponentData<WorldCommands.ReserveEntityIds.CommandResponses>(entity);

                var count = 0;
                Assert.DoesNotThrow(() => { count = responses.Responses.Count; });
                Assert.AreEqual(1, count);

                var response = responses.Responses[0];

                Assert.AreEqual(emptyRequest, response.RequestPayload);
                Assert.AreEqual(context, response.Context);
                Assert.AreEqual(TestCommandRequestId, response.RequestId);
                Assert.AreEqual(wrappedOp.Op.StatusCode, response.StatusCode);
                Assert.AreEqual(wrappedOp.Op.Message, response.Message);
                Assert.AreEqual(wrappedOp.Op.FirstEntityId, response.FirstEntityId);
                Assert.AreEqual(wrappedOp.Op.NumberOfEntityIds, response.NumberOfEntityIds);
            }
        }

        [Test]
        public void OnReserveEntityIdsResponse_should_error_if_request_id_not_found()
        {
            using (var wrappedOp = WorkerOpFactory.CreateReserveEntityIdsResponseOp(TestCommandRequestId))
            {
                Assert.Throws<UnknownRequestIdException>(() =>
                {
                    receiveSystem.OnReserveEntityIdsResponse(wrappedOp.Op);
                });
            }
        }

        [Test]
        public void OnReserveEntityIdsResponse_should_log_if_corresponding_entity_not_found()
        {
            var emptyRequest = new WorldCommands.ReserveEntityIds.Request();

            reserveEntityIdsStorage.CommandRequestsInFlight.Add(TestCommandRequestId,
                new CommandRequestStore<WorldCommands.ReserveEntityIds.Request>(Entity.Null,
                    emptyRequest, null, TestCommandRequestId));

            using (var wrappedOp = WorkerOpFactory.CreateReserveEntityIdsResponseOp(TestCommandRequestId))
            {
                using (var expectingScope = logDispatcher.EnterExpectingScope())
                {
                    expectingScope.Expect(LogType.Log, LoggingUtils.LoggerName, "Op");
                    receiveSystem.OnReserveEntityIdsResponse(wrappedOp.Op);
                }
            }
        }
    }

    public class FirstComponentDispatcher : TestComponentDispatcherBase
    {
        public override uint ComponentId => SpatialOSReceiveSystemTests.FirstTestComponentId;

        public FirstComponentDispatcher(WorkerSystem worker, World world) : base(worker, world)
        {
        }

        public override void AddCommandComponents(Entity entity)
        {
            EntityManager.AddComponentData(entity, new CommandComponent());
        }

        internal struct CommandComponent : IComponentData
        {
            public uint Test;
        }
    }

    public class SecondComponentDispatcher : TestComponentDispatcherBase
    {
        public override uint ComponentId => SpatialOSReceiveSystemTests.SecondTestComponentId;

        public SecondComponentDispatcher(WorkerSystem worker, World world) : base(worker, world)
        {
        }

        public override void AddCommandComponents(Entity entity)
        {
            EntityManager.AddComponentData(entity, new CommandComponent());
        }

        internal struct CommandComponent : IComponentData
        {
            public uint Test;
        }
    }

    [DisableAutoRegister]
    public abstract class TestComponentDispatcherBase : ComponentDispatcherHandler
    {
        public bool HasAddComponentReceived;
        public bool HasRemoveComponentReceived;
        public bool HasComponentUpdateReceived;
        public bool HasAuthorityChangedReceived;
        public bool HasCommandRequestReceived;
        public bool HasCommandResponseReceived;

        protected EntityManager EntityManager;

        public TestComponentDispatcherBase(WorkerSystem worker, World world) : base(worker, world)
        {
            EntityManager = world.GetOrCreateManager<EntityManager>();
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
        }

        public override void OnRemoveComponent(RemoveComponentOp op)
        {
            HasRemoveComponentReceived = true;
        }

        public override void OnComponentUpdate(ComponentUpdateOp op)
        {
            HasComponentUpdateReceived = true;
        }

        public override void OnAuthorityChange(AuthorityChangeOp op)
        {
            HasAuthorityChangedReceived = true;
        }

        public override void OnCommandRequest(CommandRequestOp op)
        {
            HasCommandRequestReceived = true;
        }

        public override void OnCommandResponse(CommandResponseOp op)
        {
            HasCommandResponseReceived = true;
        }

        public override void Dispose()
        {
        }
    }
}
