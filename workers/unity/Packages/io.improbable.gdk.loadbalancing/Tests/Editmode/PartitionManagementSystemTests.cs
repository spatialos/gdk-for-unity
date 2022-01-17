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
                        .GetOutboundCommandRequests<Restricted.Worker.AssignPartition.Request>()
                        .ToArray();

                    Assert.AreEqual(1, commands.Length);
                    var (_, command) = commands[0];

                    // We send the command to the target worker.
                    Assert.AreEqual(1, command.TargetEntityId.Id);
                    // And it contains the entity id of the created partition
                    Assert.AreEqual(1, command.Payload.PartitionId);
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
                    Assert.AreEqual(1, registeredWorker.PartitionEntityId.Id);
                });
        }

        private static EntityTemplate Worker(string workerType)
        {
            var et = new EntityTemplate();
            et.AddComponent(new Restricted.Worker.Snapshot(workerType, workerType, new Connection(0, "")));
            return et;
        }
    }
}
