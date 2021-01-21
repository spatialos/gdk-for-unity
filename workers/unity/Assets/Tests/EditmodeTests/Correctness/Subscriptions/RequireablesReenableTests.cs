using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Test;
using Improbable.Gdk.TestUtils;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;


namespace Improbable.Gdk.EditmodeTests
{
    [TestFixture]
    public class RequireablesReenableTests : MockBase
    {
        private const long EntityId = 100;

        [Test]
        public void CommandSenders_and_Receivers_are_valid_after_reinjection()
        {
            World
                .Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world =>
                    world.Connection.ChangeComponentAuthority(EntityId, TestCommands.ComponentId, Authority.Authoritative))
                .Step(world =>
                {
                    var (_, behaviour) = world.CreateGameObject<CommandSenderReceiverTestBehaviour>(EntityId);
                    return behaviour;
                })
                .Step((world, behaviour) =>
                {
                    Assert.IsNotNull(behaviour.CommandReceiver);
                    Assert.IsNotNull(behaviour.CommandSender);
                    Assert.IsTrue(behaviour.CommandReceiver.IsValid);
                    Assert.IsTrue(behaviour.CommandSender.IsValid);
                })
                .Step(world =>
                    world.Connection.ChangeComponentAuthority(EntityId, TestCommands.ComponentId, Authority.NotAuthoritative))
                .Step((world, behaviour) =>
                {
                    Assert.IsNull(behaviour.CommandReceiver);
                    Assert.IsNull(behaviour.CommandSender);
                })
                .Step(world =>
                    world.Connection.ChangeComponentAuthority(EntityId, TestCommands.ComponentId, Authority.Authoritative))
                .Step((world, behaviour) =>
                {
                    Assert.IsNotNull(behaviour.CommandReceiver);
                    Assert.IsNotNull(behaviour.CommandSender);
                    Assert.IsTrue(behaviour.CommandReceiver.IsValid);
                    Assert.IsTrue(behaviour.CommandSender.IsValid);
                });
        }

        [Test]
        public void Readers_and_Writers_are_valid_after_reinjection()
        {
            World
                .Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world =>
                    world.Connection.ChangeComponentAuthority(EntityId, TestCommands.ComponentId, Authority.Authoritative))
                .Step(world =>
                {
                    var (_, behaviour) = world.CreateGameObject<ReaderWriterTestBehaviour>(EntityId);
                    return behaviour;
                })
                .Step((world, behaviour) =>
                {
                    Assert.IsNotNull(behaviour.Writer);
                    Assert.IsNotNull(behaviour.Reader);
                    Assert.IsTrue(behaviour.Writer.IsValid);
                    Assert.IsTrue(behaviour.Reader.IsValid);
                })
                .Step(world =>
                    world.Connection.ChangeComponentAuthority(EntityId, TestCommands.ComponentId, Authority.NotAuthoritative))
                .Step((world, behaviour) =>
                {
                    Assert.IsNull(behaviour.Writer);
                    Assert.IsNull(behaviour.Reader);
                })
                .Step(world =>
                    world.Connection.ChangeComponentAuthority(EntityId, TestCommands.ComponentId, Authority.Authoritative))
                .Step((world, behaviour) =>
                {
                    Assert.IsNotNull(behaviour.Writer);
                    Assert.IsNotNull(behaviour.Reader);
                    Assert.IsTrue(behaviour.Writer.IsValid);
                    Assert.IsTrue(behaviour.Reader.IsValid);
                });
        }

        private static EntityTemplate GetTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new TestCommands.Snapshot());
            return template;
        }

        private class CommandSenderReceiverTestBehaviour : MonoBehaviour
        {
#pragma warning disable 649
            [Require] public TestCommandsCommandReceiver CommandReceiver;
            [Require] public PlayerHeartbeatClientCommandSender CommandSender;
#pragma warning restore 649
        }

        private class ReaderWriterTestBehaviour : MonoBehaviour
        {
#pragma warning disable 649
            [Require] public TestCommandsWriter Writer;
            [Require] public TestCommandsReader Reader;
#pragma warning restore 649
        }
    }
}
