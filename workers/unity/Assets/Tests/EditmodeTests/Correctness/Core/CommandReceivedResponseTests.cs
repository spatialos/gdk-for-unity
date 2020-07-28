using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using Playground;

namespace Improbable.Gdk.EditmodeTests.Core
{
    public class CommandReceivedResponseTests : MockBase
    {
        private const long EntityId = 101;
        private const long LaunchedId = 102;

        [Test]
        public void ReceivedResponse_isNull_when_Response_is_not_received()
        {
<<<<<<< HEAD
            World.Step(world =>
<<<<<<< HEAD
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    return world.GetSystem<CommandSystem>().SendCommand(GetRequest());
                })
                .Step((world, id) =>
                {
                    Assert.IsFalse(world.GetSystem<CommandSystem>()
                        .GetResponse<Launcher.LaunchEntity.ReceivedResponse>(id).HasValue);
                });
=======
            {
                world.Connection.CreateEntity(EntityId, GetTemplate());
            }).Step(world => world.GetSystem<CommandSystem>().SendCommand(GetRequest())).Step((world, id) =>
            {
                Assert.IsFalse(world.GetSystem<CommandSystem>().GetResponse<Launcher.LaunchEntity.ReceivedResponse>(id).HasValue);
            });
>>>>>>> Adjust access modifiers
=======
            World.Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world => world.GetSystem<CommandSystem>().SendCommand(GetRequest()))
                .Step((world, id) => Assert.IsFalse(world.GetSystem<CommandSystem>().GetResponse<Launcher.LaunchEntity.ReceivedResponse>(id).HasValue));
>>>>>>> Next frame response
        }

        [Test]
        public void ReceivedResponse_isNotNull_when_Response_is_received()
        {
<<<<<<< HEAD
            World.Step(world =>
<<<<<<< HEAD
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    return world.GetSystem<CommandSystem>().SendCommand(GetRequest());
                })
=======
            World.Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world => world.GetSystem<CommandSystem>().SendCommand(GetRequest()))
>>>>>>> Next frame response
                .Step((world, id) =>
                {
                    world.Connection
                        .GenerateResponse<Launcher.LaunchEntity.Request, Launcher.LaunchEntity.ReceivedResponse>(id,
                            ResponseGenerator);
                    return id;
                })
                .Step((world, id) =>
                {
                    var response = world.GetSystem<CommandSystem>()
                        .GetResponse<Launcher.LaunchEntity.ReceivedResponse>(id);
                    Assert.IsTrue(response.HasValue);
                    Assert.AreEqual(response.Value.EntityId, GetRequest().TargetEntityId);
                    Assert.AreEqual(response.Value.RequestPayload, GetRequest().Payload);
                });
<<<<<<< HEAD
=======
            {
                world.Connection.CreateEntity(EntityId, GetTemplate());
            }).Step(world =>
            {
                var id = world.GetSystem<CommandSystem>().SendCommand(GetRequest());
                world.Connection.GenerateResponse<Launcher.LaunchEntity.Request, Launcher.LaunchEntity.ReceivedResponse>(id, ResponseGenerator);
                return id;
            }).Step((world, id) =>
            {
                Assert.IsTrue(world.GetSystem<CommandSystem>().GetResponse<Launcher.LaunchEntity.ReceivedResponse>(id).HasValue);
            });
>>>>>>> Adjust access modifiers
=======
>>>>>>> Next frame response
        }

        private static Launcher.LaunchEntity.ReceivedResponse ResponseGenerator(CommandRequestId id, Launcher.LaunchEntity.Request request)
        {
            return new Launcher.LaunchEntity.ReceivedResponse(
                default,
                request.TargetEntityId,
                null,
                StatusCode.Success,
                default,
                request.Payload,
                null,
                id
            );
        }

        private static EntityTemplate GetTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            template.AddComponent(new Launcher.Snapshot(), "worker");
            return template;
        }

        private static Launcher.LaunchEntity.Request GetRequest()
        {
            return new Launcher.LaunchEntity.Request(new EntityId(EntityId), new LaunchCommandRequest(new EntityId(LaunchedId),
                new Vector3f(1, 0, 1),
                new Vector3f(0, 1, 0), 5, new EntityId(EntityId)));
        }
    }
}
