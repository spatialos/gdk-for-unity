using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Test;
using Improbable.Gdk.TestUtils;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    public class RequireablesUnlinkTests : MockBase
    {
        private const long EntityId = 100;

        private static EntityTemplate GetTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            template.AddComponent(new TestCommands.Snapshot(), "worker");
            return template;
        }

        [Test]
        public void Reader_is_disabled_if_gameobject_unlinked()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    var (go, readerBehaviour) = world.CreateGameObject<PositionReaderBehaviour>(EntityId);
                    return (gameObject: go, reader: readerBehaviour.Reader);
                })
                .Step((world, context) =>
                {
                    world.Linker.UnlinkGameObjectFromEntity(new EntityId(EntityId), context.gameObject);
                })
                .Step((world, context) =>
                {
                    var (gameObject, positionReader) = context;
                    Assert.IsNull(gameObject.GetComponent<PositionReaderBehaviour>().Reader);
                    Assert.IsFalse(positionReader.IsValid);
                });
        }

        [Test]
        public void Writer_is_disabled_if_gameobject_unlinked()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                    World.Connection.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
                })
                .Step(world =>
                {
                    var (go, writerBehaviour) = world.CreateGameObject<PositionWriterBehaviour>(EntityId);
                    return (gameObject: go, writer: writerBehaviour.Writer);
                })
                .Step((world, context) =>
                {
                    world.Linker.UnlinkGameObjectFromEntity(new EntityId(EntityId), context.gameObject);
                })
                .Step((world, context) =>
                {
                    var (gameObject, writer) = context;
                    Assert.IsNull(gameObject.GetComponent<PositionWriterBehaviour>().Writer);
                    Assert.IsFalse(writer.IsValid);
                });
        }

        [Test]
        public void CommandSender_is_disabled_if_gameobject_unlinked()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    var (go, commandSender) = world.CreateGameObject<CommandSender>(EntityId);
                    return (gameObject: go, sender: commandSender.Sender);
                })
                .Step((world, context) =>
                {
                    world.Linker.UnlinkGameObjectFromEntity(new EntityId(EntityId), context.gameObject);
                })
                .Step((world, context) =>
                {
                    var (gameObject, commandSender) = context;
                    Assert.IsNull(gameObject.GetComponent<CommandSender>().Sender);
                    Assert.IsFalse(commandSender.IsValid);
                });
        }

        [Test]
        public void CommandReceiver_is_disabled_if_gameobject_unlinked()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                    World.Connection.ChangeAuthority(EntityId, TestCommands.ComponentId, Authority.Authoritative);
                })
                .Step(world =>
                {
                    var (go, commandReceiver) = world.CreateGameObject<CommandReceiver>(EntityId);
                    return (gameObject: go, receiver: commandReceiver.Receiver);
                })
                .Step((world, context) =>
                {
                    world.Linker.UnlinkGameObjectFromEntity(new EntityId(EntityId), context.gameObject);
                })
                .Step((world, context) =>
                {
                    var (gameObject, receiver) = context;
                    Assert.IsNull(gameObject.GetComponent<CommandReceiver>().Receiver);
                    Assert.IsFalse(receiver.IsValid);
                });
        }

        private class PositionReaderBehaviour : MonoBehaviour
        {
#pragma warning disable 649
            [Require] public PositionReader Reader;
#pragma warning restore 649
        }

        private class PositionWriterBehaviour : MonoBehaviour
        {
#pragma warning disable 649
            [Require] public PositionWriter Writer;
#pragma warning restore 649
        }

        private class CommandSender : MonoBehaviour
        {
#pragma warning disable 649

            [Require] public TestCommandsCommandSender Sender;
#pragma warning restore 649
        }

        private class CommandReceiver : MonoBehaviour
        {
#pragma warning disable 649
            [Require] public TestCommandsCommandReceiver Receiver;
#pragma warning restore 649
        }
    }
}
