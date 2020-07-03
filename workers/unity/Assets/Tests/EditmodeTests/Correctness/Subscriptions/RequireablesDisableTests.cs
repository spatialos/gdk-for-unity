using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Test;
using Improbable.Gdk.TestUtils;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    public class RequireablesDisableTests : MockBase
    {
        private const long EntityId = 100;

        [Test]
        public void Reader_is_disabled_if_component_removed()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    var (_, readerBehaviour) = world.CreateGameObject<PositionReaderBehaviour>(EntityId);
                    return (readerBehaviour: readerBehaviour, reader: readerBehaviour.Reader);
                })
                .Step(world =>
                {
                    world.Connection.RemoveComponent(EntityId, Position.ComponentId);
                })
                .Step((world, context) =>
                {
                    var (readerBehaviour, reader) = context;

                    Assert.IsNull(readerBehaviour.Reader);
                    Assert.IsFalse(reader.IsValid);
                });
        }

        [Test]
        public void Reader_is_reenabled_if_component_regained()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    var (_, readerBehaviour) = world.CreateGameObject<PositionReaderBehaviour>(EntityId);
                    return readerBehaviour;
                })
                .Step(world =>
                {
                    world.Connection.RemoveComponent(EntityId, Position.ComponentId);
                })
                .Step(world =>
                {
                    world.Connection.AddComponent(EntityId, Position.ComponentId, new Position.Update());
                })
                .Step((world, context) =>
                {
                    var readerBehaviour = context;

                    Assert.IsNotNull(readerBehaviour.Reader);
                    Assert.IsTrue(readerBehaviour.Reader.IsValid);
                });
        }

        [Test]
        public void Reader_is_reenabled_if_component_regained_with_aggregate()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    var (_, readerBehaviour) = world.CreateGameObject<MultipleReaderBehaviour>(EntityId);
                    return readerBehaviour;
                })
                .Step(world =>
                {
                    world.Connection.RemoveComponent(EntityId, Position.ComponentId);
                })
                .Step(world =>
                {
                    world.Connection.AddComponent(EntityId, Position.ComponentId, new Position.Update());
                })
                .Step((world, context) =>
                {
                    var readerBehaviour = context;

                    Assert.IsNotNull(readerBehaviour.TestCommandsReader);
                    Assert.IsTrue(readerBehaviour.TestCommandsReader.IsValid);
                });
        }

        [Test]
        public void Writer_is_disabled_if_loses_auth()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, GetTemplate()); })
                .Step(world =>
                {
                    world.Connection.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
                })
                .Step(world =>
                {
                    var (_, writerBehaviour) = world.CreateGameObject<PositionWriterBehaviour>(EntityId);
                    return (writerBehaviour: writerBehaviour, writer: writerBehaviour.Writer);
                })
                .Step(world =>
                {
                    world.Connection.ChangeAuthority(EntityId, Position.ComponentId, Authority.NotAuthoritative);
                })
                .Step((world, context) =>
                {
                    var (writerBehaviour, positionWriter) = context;
                    Assert.IsNull(writerBehaviour.Writer);
                    Assert.IsFalse(positionWriter.IsValid);
                });
        }

        [Test]
        public void CommandSender_is_disabled_if_entity_removed()
        {
            World
                .Step(world => { world.Connection.CreateEntity(EntityId, GetTemplate()); })
                .Step(world =>
                {
                    var (_, senderBehaviour) = world.CreateGameObject<CommandSender>(EntityId);
                    return (senderBehaviour: senderBehaviour, sender: senderBehaviour.Sender);
                })
                .Step(world =>
                {
                    world.Connection.RemoveComponent(EntityId, Position.ComponentId);
                    world.Connection.RemoveComponent(EntityId, TestCommands.ComponentId);
                    world.Connection.RemoveEntity(EntityId);
                })
                .Step((world, context) =>
                {
                    var (senderBehaviour, testCommandsCommandSender) = context;
                    Assert.IsNull(senderBehaviour.Sender);
                    Assert.IsFalse(testCommandsCommandSender.IsValid);
                });
        }

        [Test]
        public void CommandReceiver_is_disabled_if_loses_auth()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                    world.Connection.ChangeAuthority(EntityId, TestCommands.ComponentId, Authority.Authoritative);
                })
                .Step(world =>
                {
                    var (_, receiverBehaviour) = world.CreateGameObject<CommandReceiver>(EntityId);
                    return (receiverBehaviour: receiverBehaviour, receiver: receiverBehaviour.Receiver);
                })
                .Step(world =>
                {
                    world.Connection.ChangeAuthority(EntityId, TestCommands.ComponentId,
                        Authority.NotAuthoritative);
                })
                .Step((world, context) =>
                {
                    var (receiverBehaviour, testCommandsCommandReceiver) = context;
                    Assert.IsNull(receiverBehaviour.Receiver);
                    Assert.IsFalse(testCommandsCommandReceiver.IsValid);
                });
        }

        [Test]
        public void Only_field_which_lost_constraints_is_invalid()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(EntityId, GetTemplate());
                })
                .Step(world =>
                {
                    var (_, component) = world.CreateGameObject<MultipleReaderBehaviour>(EntityId);
                    return (component: component, positionReader: component.PositionReader,
                        testCommandsReader: component.TestCommandsReader);
                })
                .Step(world =>
                {
                    world.Connection.RemoveComponent(EntityId, TestCommands.ComponentId);
                })
                .Step((world, context) =>
                {
                    var (component, positionReader, testCommandsReader) = context;

                    Assert.IsNull(component.PositionReader);
                    Assert.IsNull(component.TestCommandsReader);
                    Assert.IsFalse(positionReader.IsValid);
                    Assert.IsFalse(testCommandsReader.IsValid);
                });
        }

        private static EntityTemplate GetTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            template.AddComponent(new TestCommands.Snapshot(), "worker");
            return template;
        }

#pragma warning disable 649

        private class PositionReaderBehaviour : MonoBehaviour
        {
            [Require] public PositionReader Reader;
        }

        private class PositionWriterBehaviour : MonoBehaviour
        {
            [Require] public PositionWriter Writer;
        }

        private class CommandSender : MonoBehaviour
        {
            [Require] public TestCommandsCommandSender Sender;
        }

        private class CommandReceiver : MonoBehaviour
        {
            [Require] public TestCommandsCommandReceiver Receiver;
        }

        private class MultipleReaderBehaviour : MonoBehaviour
        {
            [Require] public PositionReader PositionReader;
            [Require] public TestCommandsReader TestCommandsReader;
        }

#pragma warning restore 649
    }
}
