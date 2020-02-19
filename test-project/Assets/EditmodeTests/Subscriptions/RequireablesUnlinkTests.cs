using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Test;
using Improbable.Gdk.TestBases;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    public class RequireablesUnlinkTests : SubscriptionsTestBase
    {
        private const long EntityId = 100;
        private GameObject createdGameObject;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            template.AddComponent(new TestCommands.Snapshot(), "worker");
            ConnectionHandler.CreateEntity(EntityId, template);
            Update();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            Object.DestroyImmediate(createdGameObject);
        }

        [Test]
        public void Reader_is_disabled_if_gameobject_unlinked()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<PositionReaderBehaviour>(EntityId);
            var reader = createdGameObject.GetComponent<PositionReaderBehaviour>().Reader;

            Linker.UnlinkGameObjectFromEntity(new EntityId(EntityId), createdGameObject);

            Assert.IsNull(createdGameObject.GetComponent<PositionReaderBehaviour>().Reader);
            Assert.IsFalse(reader.IsValid);
        }

        [Test]
        public void Writer_is_disabled_if_gameobject_unlinked()
        {
            ConnectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            Update();

            createdGameObject = CreateAndLinkGameObjectWithComponent<PositionWriterBehaviour>(EntityId);
            var writer = createdGameObject.GetComponent<PositionWriterBehaviour>().Writer;

            Linker.UnlinkGameObjectFromEntity(new EntityId(EntityId), createdGameObject);

            Assert.IsNull(createdGameObject.GetComponent<PositionWriterBehaviour>().Writer);
            Assert.IsFalse(writer.IsValid);
        }

        [Test]
        public void CommandSender_is_disabled_if_gameobject_unlinked()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<CommandSender>(EntityId);
            var sender = createdGameObject.GetComponent<CommandSender>().Sender;

            Linker.UnlinkGameObjectFromEntity(new EntityId(EntityId), createdGameObject);

            Assert.IsNull(createdGameObject.GetComponent<CommandSender>().Sender);
            Assert.IsFalse(sender.IsValid);
        }

        [Test]
        public void CommandReceiver_is_disabled_if_gameobject_unlinked()
        {
            ConnectionHandler.ChangeAuthority(EntityId, TestCommands.ComponentId, Authority.Authoritative);
            Update();

            createdGameObject = CreateAndLinkGameObjectWithComponent<CommandReceiver>(EntityId);
            var receiver = createdGameObject.GetComponent<CommandReceiver>().Receiver;

            Linker.UnlinkGameObjectFromEntity(new EntityId(EntityId), createdGameObject);

            Assert.IsNull(createdGameObject.GetComponent<CommandReceiver>().Receiver);
            Assert.IsFalse(receiver.IsValid);
        }

        private void Update()
        {
            ReceiveSystem.Update();
            RequireLifecycleSystem.Update();
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
