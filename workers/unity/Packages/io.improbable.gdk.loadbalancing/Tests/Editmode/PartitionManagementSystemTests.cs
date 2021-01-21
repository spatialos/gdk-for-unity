using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Improbable.Restricted;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using Connection = Improbable.Restricted.Connection;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.LoadBalancing.EditmodeTests
{
    public class PartitionManagementSystemTests : MockBase
    {
        protected override MockWorld.Options GetOptions()
        {
            var opts = base.GetOptions();
            opts.AdditionalSystems = (world) =>
            {
                world.AddSystem(new ClassifyWorkersSystem());
                world.AddSystem(new PartitionManagementSystem { WorkerTypes = new[] { "TestWorker", "OtherWorker" } });
            };

            return opts;
        }

        [TestCase("TestWorker")]
        [TestCase("OtherWorker")]
        public void Partitions_are_created_for_each_new_worker(string workerType)
        {
            World
                .Step(world =>
                {
                    World.Connection.CreateEntity(1, Worker(workerType));
                })
                .Step(world =>
                {
                    // We send a create entity command for the new worker.
                    var commands = world.Connection.GetOutboundCommandRequestIds<WorldCommands.CreateEntity.Request>()
                        .ToArray();
                    Assert.AreEqual(1, commands.Length);
                    return commands[0];
                })
                .Step((world, requestId) =>
                {
                    // We only send the create entity command once!
                    var commands = world.Connection.GetOutboundCommandRequestIds<WorldCommands.CreateEntity.Request>()
                        .ToArray();
                    Assert.AreEqual(1, commands.Length);

                    Assert.AreEqual(requestId, commands[0]);
                });
        }

        [Test]
        public void Partition_creation_retries_if_failed()
        {
            World
                .Step(world =>
                {
                    World.Connection.CreateEntity(1, Worker("TestWorker"));
                })
                .Step(world =>
                {
                    var commands = world.Connection.GetOutboundCommandRequestIds<WorldCommands.CreateEntity.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);
                    var requestId = commands[0];

                    world.Connection.GenerateWorldCommandResponse(requestId,
                        (reqId, request) => new WorldCommands.CreateEntity.ReceivedResponse(
                            new CreateEntityResponseOp
                            {
                                RequestId = reqId.Raw,
                                StatusCode = StatusCode.Timeout,
                                Message = "No worky :(",
                                EntityId = null
                            }, Entity.Null, request, reqId));

                    return requestId;
                })
                .Step(world => { })
                .Step((world, previousRequestId) =>
                {
                    // We send a new command since the last one failed!
                    var commands = world.Connection.GetOutboundCommandRequestIds<WorldCommands.CreateEntity.Request>()
                        .ToArray();
                    Assert.AreEqual(1, commands.Length);

                    Assert.AreNotEqual(previousRequestId, commands[0]);
                });
        }

        [Test]
        public void Partitions_are_assigned_once_created()
        {
            World
                .Step(world =>
                {
                    World.Connection.CreateEntity(1, Worker("TestWorker"));
                })
                .Step(world =>
                {
                    var commands = world
                        .Connection
                        .GetOutboundCommandRequestIds<WorldCommands.CreateEntity.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);
                    var requestId = commands[0];

                    world.Connection.GenerateWorldCommandResponse(requestId,
                        (reqId, request) => new WorldCommands.CreateEntity.ReceivedResponse(
                            new CreateEntityResponseOp
                            {
                                RequestId = reqId.Raw,
                                StatusCode = StatusCode.Success,
                                Message = null,
                                EntityId = 2,
                            }, Entity.Null, request, reqId));
                })
                .Step(world =>
                {
                    var commands = world
                        .Connection
                        .GetOutboundCommandRequests<Restricted.Worker.AssignPartition.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);
                    var (_, command) = commands[0];

                    // We send the command to the target worker.
                    Assert.AreEqual(1, command.TargetEntityId.Id);
                    // And it contains the entity id of the created partition
                    Assert.AreEqual(2, command.Payload.PartitionId);
                });
        }

        [TestCase(StatusCode.Timeout)]
        [TestCase(StatusCode.AuthorityLost)]
        [TestCase(StatusCode.ApplicationError)]
        [TestCase(StatusCode.InternalError)]
        public void Partition_assignment_is_retried_with_certain_failure_modes(StatusCode commandStatusCode)
        {
            World
                .Step(world =>
                {
                    World.Connection.CreateEntity(1, Worker("TestWorker"));
                })
                .Step(world =>
                {
                    var commands = world
                        .Connection
                        .GetOutboundCommandRequestIds<WorldCommands.CreateEntity.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);
                    var requestId = commands[0];

                    world.Connection.GenerateWorldCommandResponse(requestId,
                        (reqId, request) => new WorldCommands.CreateEntity.ReceivedResponse(
                            new CreateEntityResponseOp
                            {
                                RequestId = reqId.Raw,
                                StatusCode = StatusCode.Success,
                                Message = null,
                                EntityId = 2,
                            }, Entity.Null, request, reqId));
                })
                .Step(world =>
                {
                    var commands = world
                        .Connection
                        .GetOutboundCommandRequestIds<Restricted.Worker.AssignPartition.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);

                    world.Connection.GenerateResponse(commands[0],
                        (CommandRequestId reqId, Restricted.Worker.AssignPartition.Request request) =>
                            new Restricted.Worker.AssignPartition.ReceivedResponse(Entity.Null, new EntityId(),
                                "No worky :(", commandStatusCode, null, request.Payload, null, reqId));

                    return commands[0];
                })
                .Step((world, previousRequestId) =>
                {
                    // We send a new command since the last one failed!
                    var commands = world.Connection
                        .GetOutboundCommandRequestIds<Restricted.Worker.AssignPartition.Request>()
                        .ToArray();
                    Assert.AreEqual(1, commands.Length);

                    Assert.AreNotEqual(previousRequestId, commands[0]);
                });
        }

        [Test]
        public void Partition_entity_is_removed_if_NotFound_status_code()
        {
            World
                .Step(world =>
                {
                    World.Connection.CreateEntity(1, Worker("TestWorker"));
                })
                .Step(world =>
                {
                    var commands = world
                        .Connection
                        .GetOutboundCommandRequestIds<WorldCommands.CreateEntity.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);
                    var requestId = commands[0];

                    world.Connection.GenerateWorldCommandResponse(requestId,
                        (reqId, request) => new WorldCommands.CreateEntity.ReceivedResponse(
                            new CreateEntityResponseOp
                            {
                                RequestId = reqId.Raw,
                                StatusCode = StatusCode.Success,
                                Message = null,
                                EntityId = 2,
                            }, Entity.Null, request, reqId));
                })
                .Step(world =>
                {
                    var commands = world
                        .Connection
                        .GetOutboundCommandRequestIds<Restricted.Worker.AssignPartition.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);

                    world.Connection.GenerateResponse(commands[0],
                        (CommandRequestId reqId, Restricted.Worker.AssignPartition.Request request) =>
                            new Restricted.Worker.AssignPartition.ReceivedResponse(Entity.Null, new EntityId(),
                                "No worky :(", StatusCode.NotFound, null, request.Payload, null, reqId));

                    return commands[0];
                })
                .Step((world, previousRequestId) =>
                {
                    // We try and delete the partition we created.
                    var commands = world.Connection.GetOutboundCommandRequestIds<WorldCommands.DeleteEntity.Request>()
                        .ToArray();
                    Assert.AreEqual(1, commands.Length);
                });
        }

        [Test]
        public void Worker_marked_with_RegisteredWorker_component_if_assigned_successfully()
        {
            World
                .Step(world =>
                {
                    World.Connection.CreateEntity(1, Worker("TestWorker"));
                })
                .Step(world =>
                {
                    var commands = world
                        .Connection
                        .GetOutboundCommandRequestIds<WorldCommands.CreateEntity.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);
                    var requestId = commands[0];

                    world.Connection.GenerateWorldCommandResponse(requestId,
                        (reqId, request) => new WorldCommands.CreateEntity.ReceivedResponse(
                            new CreateEntityResponseOp
                            {
                                RequestId = reqId.Raw,
                                StatusCode = StatusCode.Success,
                                Message = null,
                                EntityId = 2,
                            }, Entity.Null, request, reqId));
                })
                .Step(world =>
                {
                    var commands = world
                        .Connection
                        .GetOutboundCommandRequestIds<Restricted.Worker.AssignPartition.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);

                    world.Connection.GenerateResponse(commands[0],
                        (CommandRequestId reqId, Restricted.Worker.AssignPartition.Request request) =>
                            new Restricted.Worker.AssignPartition.ReceivedResponse(Entity.Null, new EntityId(),
                                null, StatusCode.Success, new AssignPartitionResponse(), request.Payload, null, reqId));
                })
                .Step(world =>
                {
                    var workerEntity = world.GetSystem<WorkerSystem>().GetEntity(new EntityId(1));
                    Assert.IsTrue(world.EntityManager.HasComponent<RegisteredWorker>(workerEntity));

                    var registeredWorker = world.EntityManager.GetComponentData<RegisteredWorker>(workerEntity);
                    Assert.AreEqual(2, registeredWorker.PartitionEntityId.Id);
                });
        }

        [Test]
        public void Disconnected_workers_have_their_partitions_deleted()
        {
            World
                .Step((world) =>
                {
                    World.Connection.CreateEntity(1, Worker("TestWorker"));
                    World.Connection.CreateEntity(2,
                        PartitionManagementSystem.GetPartitionEntity("TestWorker", new EntityId(1)));
                })
                .Step(world =>
                {
                    var workerEntity = world.GetSystem<WorkerSystem>().GetEntity(new EntityId(1));
                    world.EntityManager.AddComponentData(workerEntity,
                        new RegisteredWorker { PartitionEntityId = new EntityId(2) });
                })
                .Step(world =>
                {
                    World.Connection.RemoveEntity(1);
                }).Step(world =>
                {
                    var deleteEntityRequests = world
                        .Connection
                        .GetOutboundCommandRequests<WorldCommands.DeleteEntity.Request>()
                        .ToArray();

                    Assert.AreEqual(1, deleteEntityRequests.Length);
                    var (_, request) = deleteEntityRequests[0];
                    Assert.AreEqual(2, request.EntityId.Id);
                });
        }

        [Test]
        public void Partition_checked_out_without_worker_is_removed()
        {
            World
                .Step(world =>
                {
                    World.Connection.CreateEntity(2,
                        PartitionManagementSystem.GetPartitionEntity("TestWorker", new EntityId(1)));
                })
                .Step(world =>
                {
                    var deleteEntityRequests = world
                        .Connection
                        .GetOutboundCommandRequests<WorldCommands.DeleteEntity.Request>()
                        .ToArray();

                    Assert.AreEqual(1, deleteEntityRequests.Length);
                    var (_, request) = deleteEntityRequests[0];
                    Assert.AreEqual(2, request.EntityId.Id);
                });
        }

        private static EntityTemplate Worker(string workerType)
        {
            var et = new EntityTemplate();
            et.AddComponent(new Restricted.Worker.Snapshot(workerType, workerType, new Connection(0)));
            return et;
        }
    }
}
