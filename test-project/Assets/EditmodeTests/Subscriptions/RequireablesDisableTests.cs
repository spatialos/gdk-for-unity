using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Test;
using Improbable.Gdk.TestBases;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    public class RequireablesDisableTests : SubscriptionsTestBase
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

        [Test]
        public void Reader_is_disabled_if_component_removed()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<PositionReaderBehaviour>(EntityId);
            var reader = createdGameObject.GetComponent<PositionReaderBehaviour>().Reader;

            ConnectionHandler.RemoveComponent(EntityId, Position.ComponentId);
            Update();

            Assert.IsNull(createdGameObject.GetComponent<PositionReaderBehaviour>().Reader);
            Assert.IsFalse(reader.IsValid);
        }

        [Test]
        public void Writer_is_disabled_if_loses_auth()
        {
            ConnectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            Update();

            createdGameObject = CreateAndLinkGameObjectWithComponent<PositionWriterBehaviour>(EntityId);
            var writer = createdGameObject.GetComponent<PositionWriterBehaviour>().Writer;

            ConnectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.NotAuthoritative);
            Update();

            Assert.IsNull(createdGameObject.GetComponent<PositionWriterBehaviour>().Writer);
            Assert.IsFalse(writer.IsValid);
        }

        [Test]
        public void CommandSender_is_disabled_if_entity_removed()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<CommandSender>(EntityId);
            var sender = createdGameObject.GetComponent<CommandSender>().Sender;

            ConnectionHandler.RemoveComponent(EntityId, Position.ComponentId);
            ConnectionHandler.RemoveComponent(EntityId, TestCommands.ComponentId);
            ConnectionHandler.RemoveEntity(EntityId);
            Update();

            Assert.IsNull(createdGameObject.GetComponent<CommandSender>().Sender);
            Assert.IsFalse(sender.IsValid);
        }

        [Test]
        public void CommandReceiver_is_disabled_if_loses_auth()
        {
            ConnectionHandler.ChangeAuthority(EntityId, TestCommands.ComponentId, Authority.Authoritative);
            Update();

            createdGameObject = CreateAndLinkGameObjectWithComponent<CommandReceiver>(EntityId);
            var receiver = createdGameObject.GetComponent<CommandReceiver>().Receiver;

            ConnectionHandler.ChangeAuthority(EntityId, TestCommands.ComponentId, Authority.NotAuthoritative);
            Update();

            Assert.IsNull(createdGameObject.GetComponent<CommandReceiver>().Receiver);
            Assert.IsFalse(receiver.IsValid);
        }

        [Test]
        public void Only_field_which_lost_constraints_is_invalid()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<MultipleReaderBehaviour>(EntityId);
            var component = createdGameObject.GetComponent<MultipleReaderBehaviour>();
            var position = component.PositionReader;
            var testCommands = component.TestCommandsReader;

            ConnectionHandler.RemoveComponent(EntityId, TestCommands.ComponentId);
            Update();

            Assert.IsNull(component.PositionReader);
            Assert.IsNull(component.TestCommandsReader);
            Assert.IsFalse(position.IsValid);
            Assert.IsFalse(testCommands.IsValid);
        }

        public override void TearDown()
        {
            base.TearDown();
            Object.DestroyImmediate(createdGameObject);
        }

        private void Update()
        {
            ReceiveSystem.Update();
            RequireLifecycleSystem.Update();
        }

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
    }
}
