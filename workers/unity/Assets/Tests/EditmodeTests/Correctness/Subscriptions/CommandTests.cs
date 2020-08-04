using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Test;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.Subscriptions
{
    [TestFixture]
    public class CommandTests : MockBase
    {
        private const long EntityId = 101;

        private const string WorkerID = "worker";

        [Test]
        public void SubscriptionSystem_invokes_callback_on_receiving_response()
        {
            var pass = false;
            World.Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    return world.CreateGameObject<CommandStub>(EntityId).Item2.Sender;
                })
                .Step((world, sender) =>
                {
                    sender.SendTestCommand(GetRequest(), response => pass = true);
                })
                .Step(world =>
                {
                    world.Connection.GenerateResponses<TestCommands.Test.Request, TestCommands.Test.ReceivedResponse>(
                        ResponseGenerator);
                })
                .Step(world =>
                {
                    Assert.IsTrue(pass);
                });
        }

        [Test]
        public void SubscriptionSystem_does_not_invoke_callback_when_sender_is_invalid()
        {
            World.Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    return world.CreateGameObject<CommandStub>(EntityId).Item2.Sender;
                })
                .Step((world, sender) =>
                {
                    sender.SendTestCommand(GetRequest(), response => throw new AssertionException("Don't call"));
                })
                .Step((world, sender) =>
                {
                    world.Connection.GenerateResponses<TestCommands.Test.Request, TestCommands.Test.ReceivedResponse>(
                        ResponseGenerator);
                    sender.IsValid = false;
                })
                .Step(world => { });
        }

        [Test]
        public void SubscriptionSystem_does_not_invoke_callback_when_gameobject_is_unlinked()
        {
            World.Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    return world.CreateGameObject<CommandStub>(EntityId).Item2;
                })
                .Step((world, mono) =>
                {
                    mono.Sender.SendTestCommand(GetRequest(), response => throw new AssertionException("Don't call"));
                })
                .Step((world, mono) =>
                {
                    world.Connection.GenerateResponses<TestCommands.Test.Request, TestCommands.Test.ReceivedResponse>(ResponseGenerator);
                    world.Linker.UnlinkGameObjectFromEntity(new EntityId(EntityId), mono.gameObject);
                })
                .Step((world, mono) =>
                {
                    Assert.IsFalse(mono.enabled);
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
            template.AddComponent(new Position.Snapshot(), WorkerID);
            template.AddComponent(new TestCommands.Snapshot(), WorkerID);
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
