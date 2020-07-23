using System;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Improbable.Gdk.Subscriptions;
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

        private MockWorld SetupWorld()
        {
            return World.Step(world =>
            {
                world.Connection.CreateEntity(EntityId, GetTemplate());
            });
        }

        [Test]
        public void SubscriptionSystem_invokes_callback_on_receiving_response()
        {
            var pass = false;
            SetupWorld().Step(world =>
            {
                var (_, commander) = world.CreateGameObject<LaunchCommander>(EntityId);
                return commander.sender;
            }).Step((world, sender) =>
            {
                sender.SendLaunchEntityCommand(GetRequest(), response => pass = true);
                world.CommandSender.GenerateResponses<Launcher.LaunchEntity.Request, Launcher.LaunchEntity.ReceivedResponse>(ResponseGenerator);
            }).Step(world =>
            {
                Assert.IsTrue(pass);
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
#pragma warning disable 649
        private class LaunchCommander : MonoBehaviour
        {
            [Require] public LauncherCommandSender sender;
        }
#pragma warning restore 649
    }
}
