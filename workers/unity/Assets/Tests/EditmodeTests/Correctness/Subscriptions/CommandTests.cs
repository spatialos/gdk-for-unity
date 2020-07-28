using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Test;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests.Subscriptions
{
    [TestFixture]
    public class CommandTests : MockBase
    {
        private const long EntityId = 101;

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
        public void SubscriptionSystem_does_not_invoke_callback_when_receiver_becomes_invalid()
        {
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
            template.AddComponent(new TestCommands.Snapshot(), "worker");
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

        private class StubWithLog : MonoBehaviour
        {
            [Require] public TestCommandsCommandSender Sender;
            [Require] public WorldCommandSender CommandSender;
            [Require] public ILogDispatcher Logger;

            public static LogEvent MakeEvent(EntityId? id, string message)
            {
                return new LogEvent("CreateEntity failed.")
                    .WithField(LoggingUtils.EntityId, id)
                    .WithField("Reason", message);
            }

            public void OnEntityCreated(WorldCommands.CreateEntity.ReceivedResponse response)
            {
                if (response.StatusCode != StatusCode.Success)
                {
                    Logger.HandleLog(LogType.Error, MakeEvent(response.RequestPayload.EntityId, response.Message));
                }
            }
        }
#pragma warning restore 649
    }
}
