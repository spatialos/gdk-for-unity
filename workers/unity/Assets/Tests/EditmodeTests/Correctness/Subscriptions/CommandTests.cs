using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Test;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;
using Playground;

namespace Improbable.Gdk.Core.EditmodeTests.Subscriptions
{
    [TestFixture]
    public class CommandTests : MockBase
    {
        private const long EntityId = 101;
        private const long LaunchedId = 102;

        [Test]
        public void SubscriptionSystem_invokes_callback_on_receiving_response()
        {
            var pass = false;
            World.Step(world =>
            {
                world.Connection.CreateEntity(EntityId, GetTemplate());
            }).Step(world =>
            {
                var (_, commander) = world.CreateGameObject<CommandStub>(EntityId);
                return commander.Sender;
            }).Step((world, sender) =>
            {
                sender.SendTestCommand(GetRequest(), response => pass = true);
                world.Connection.GenerateResponses<Launcher.LaunchEntity.Request, Launcher.LaunchEntity.ReceivedResponse>(ResponseGenerator);
            }).Step(world =>
            {
                Assert.IsTrue(pass);
            });
        }

        private static TestCommands.Test.ReceivedResponse ResponseGenerator(CommandRequestId id, TestCommands.Test.Request request)
        {
            return new TestCommands.Test.ReceivedResponse(
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

        private static TestCommands.Test.Request GetRequest()
        {
            return new TestCommands.Test.Request(new EntityId(EntityId), new Empty());
        }
#pragma warning disable 649
        private class CommandStub : MonoBehaviour
        {
            [Require] public TestCommandsCommandSender Sender;
        }
#pragma warning restore 649
    }
}
