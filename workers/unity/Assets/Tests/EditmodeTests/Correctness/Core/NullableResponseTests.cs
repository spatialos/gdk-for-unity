using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using Playground;

namespace Improbable.Gdk.EditmodeTests.Core
{
    public class NullableResponseTests : MockBase
    {
        private const long EntityId = 101;
        private const long LaunchedId = 102;

        [Test]
        public void ReceivedResponse_isNull_when_Response_is_not_received()
        {
            World.Step(world =>
            {
                world.Connection.CreateEntity(EntityId, GetTemplate());
            }).Step(world => world.GetSystem<CommandSystem>().SendCommand(GetRequest())).Step((world, id) =>
            {
                Assert.IsFalse(world.GetSystem<CommandSystem>().GetResponse<Launcher.LaunchEntity.ReceivedResponse>(id).HasValue);
            });
        }

        [Test]
        public void ReceivedResponse_isNotNull_when_Response_is_received()
        {
            World.Step(world =>
            {
                world.Connection.CreateEntity(EntityId, GetTemplate());
            }).Step(world =>
            {
                var id = world.GetSystem<CommandSystem>().SendCommand(GetRequest());
                world.CommandSender.GenerateResponse<Launcher.LaunchEntity.Request, Launcher.LaunchEntity.ReceivedResponse>(id, ResponseGenerator);
                return id;
            }).Step((world, id) =>
            {
                Assert.IsTrue(world.GetSystem<CommandSystem>().GetResponse<Launcher.LaunchEntity.ReceivedResponse>(id).HasValue);
            });
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
