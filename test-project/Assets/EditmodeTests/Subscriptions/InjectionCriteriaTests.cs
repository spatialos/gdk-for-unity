using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.TestSchema;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    /// <summary>
    ///     This tests the injection criteria for a reader or writer.
    /// </summary>
    [TestFixture]
    public class ReaderWriterInjectionCriteriaTests : SubscriptionsTestBase
    {
        private const long EntityId = 100;

        private GameObject createdGameObject;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            ConnectionHandler.CreateEntity(EntityId, template);
            ReceiveSystem.Update();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            Object.DestroyImmediate(createdGameObject);
        }

        [Test]
        public void Reader_is_not_injected_when_entity_component_is_not_checked_out()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<ExhaustiveSingularReaderBehaviour>(EntityId);
            var readerBehaviour = createdGameObject.GetComponent<ExhaustiveSingularReaderBehaviour>();

            Assert.IsFalse(readerBehaviour.enabled);
            Assert.IsNull(readerBehaviour.Reader);
        }

        [Test]
        public void Reader_is_injected_when_entity_component_is_checked_out()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<PositionReaderBehaviour>(EntityId);
            var readerBehaviour = createdGameObject.GetComponent<PositionReaderBehaviour>();

            Assert.IsTrue(readerBehaviour.enabled);
            Assert.IsNotNull(readerBehaviour.Reader);
        }

        [Test]
        public void Writer_is_not_injected_when_entity_component_is_not_checked_out()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<ExhaustiveSingularWriterBehaviour>(EntityId);
            var writerBehaviour = createdGameObject.GetComponent<ExhaustiveSingularWriterBehaviour>();

            Assert.IsFalse(writerBehaviour.enabled);
            Assert.IsNull(writerBehaviour.Writer);
        }

        [Test]
        public void Writer_is_not_injected_when_entity_component_is_checked_out_and_unauthoritative()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<PositionWriterBehaviour>(EntityId);
            var writerBehaviour = createdGameObject.GetComponent<PositionWriterBehaviour>();

            Assert.IsFalse(writerBehaviour.enabled);
            Assert.IsNull(writerBehaviour.Writer);
        }

        [Test]
        public void Writer_is_injected_when_entity_component_is_checked_out_and_authoritative()
        {
            ConnectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            ReceiveSystem.Update();

            createdGameObject = CreateAndLinkGameObjectWithComponent<PositionWriterBehaviour>(EntityId);
            var writerBehaviour = createdGameObject.GetComponent<PositionWriterBehaviour>();

            Assert.IsTrue(writerBehaviour.enabled);
            Assert.IsNotNull(writerBehaviour.Writer);
        }

        [Test]
        public void Injection_does_not_happen_if_not_all_constraints_are_satisfied()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<CompositeBehaviour>(EntityId);
            var compositeBehaviour = createdGameObject.GetComponent<CompositeBehaviour>();

            Assert.IsFalse(compositeBehaviour.enabled);
            Assert.IsNull(compositeBehaviour.Reader);
            Assert.IsNull(compositeBehaviour.Writer);
        }

        [Test]
        public void Injection_happens_if_all_constraints_are_satisfied()
        {
            ConnectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            ReceiveSystem.Update();

            createdGameObject = CreateAndLinkGameObjectWithComponent<CompositeBehaviour>(EntityId);
            var compositeBehaviour = createdGameObject.GetComponent<CompositeBehaviour>();

            Assert.IsTrue(compositeBehaviour.enabled);
            Assert.IsNotNull(compositeBehaviour.Reader);
            Assert.IsNotNull(compositeBehaviour.Writer);
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

        private class ExhaustiveSingularReaderBehaviour : MonoBehaviour
        {
            [Require] public ExhaustiveSingularReader Reader;
        }

        private class ExhaustiveSingularWriterBehaviour : MonoBehaviour
        {
            [Require] public ExhaustiveSingularWriter Writer;
        }

        private class CompositeBehaviour : MonoBehaviour
        {
            [Require] public PositionReader Reader;
            [Require] public PositionWriter Writer;
        }
#pragma warning restore 649
    }
}
