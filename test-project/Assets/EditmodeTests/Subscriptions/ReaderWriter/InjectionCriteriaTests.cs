using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Tests;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions.ReaderWriter
{
    /// <summary>
    ///     This tests the injection criteria for a reader or writer.
    /// </summary>
    [TestFixture]
    public class InjectionCriteriaTests
    {
        private const long EntityId = 100;

        private World world;
        private SpatialOSReceiveSystem receiveSystem;
        private RequireLifecycleSystem requireLifecycleSystem;

        private MockConnectionHandler connectionHandler;
        private EntityGameObjectLinker linker;

        private GameObject createdGameObject;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            connectionHandler = new MockConnectionHandler();
            world.CreateSystem<WorkerSystem>(connectionHandler, null,
                new LoggingDispatcher(), "TestWorkerType", Vector3.zero);
            receiveSystem = world.CreateSystem<SpatialOSReceiveSystem>();
            world.GetOrCreateSystem<WorkerFlagCallbackSystem>();
            world.GetOrCreateSystem<ComponentUpdateSystem>();
            world.GetOrCreateSystem<ComponentConstraintsCallbackSystem>();
            world.CreateSystem<SubscriptionSystem>();
            world.CreateSystem<CommandCallbackSystem>();
            world.CreateSystem<ComponentCallbackSystem>();
            requireLifecycleSystem = world.CreateSystem<RequireLifecycleSystem>();

            linker = new EntityGameObjectLinker(world);

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            connectionHandler.CreateEntity(EntityId, template);
            receiveSystem.Update();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(createdGameObject);
            world.Dispose();
        }

        [Test]
        public void Reader_is_not_injected_when_entity_component_is_not_checked_out()
        {
            var go = CreateAndLinkGameObjectWithComponent<ExhaustiveSingularReaderBehaviour>(EntityId);
            var readerBehaviour = go.GetComponent<ExhaustiveSingularReaderBehaviour>();

            Assert.IsFalse(readerBehaviour.enabled);
            Assert.IsNull(readerBehaviour.Reader);
        }

        [Test]
        public void Reader_is_injected_when_entity_component_is_checked_out()
        {
            var go = CreateAndLinkGameObjectWithComponent<PositionReaderBehaviour>(EntityId);
            var readerBehaviour = go.GetComponent<PositionReaderBehaviour>();

            Assert.IsTrue(readerBehaviour.enabled);
            Assert.IsNotNull(readerBehaviour.Reader);
        }

        [Test]
        public void Writer_is_not_injected_when_entity_component_is_not_checked_out()
        {
            var go = CreateAndLinkGameObjectWithComponent<ExhaustiveSingularWriterBehaviour>(EntityId);
            var writerBehaviour = go.GetComponent<ExhaustiveSingularWriterBehaviour>();

            Assert.IsFalse(writerBehaviour.enabled);
            Assert.IsNull(writerBehaviour.Writer);
        }

        [Test]
        public void Writer_is_not_injected_when_entity_component_is_checked_out_and_unauthoritative()
        {
            var go = CreateAndLinkGameObjectWithComponent<PositionWriterBehaviour>(EntityId);
            var writerBehaviour = go.GetComponent<PositionWriterBehaviour>();

            Assert.IsFalse(writerBehaviour.enabled);
            Assert.IsNull(writerBehaviour.Writer);
        }

        [Test]
        public void Writer_is_injected_when_entity_component_is_checked_out_and_authoritative()
        {
            connectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            receiveSystem.Update();

            var go = CreateAndLinkGameObjectWithComponent<PositionWriterBehaviour>(EntityId);
            var writerBehaviour = go.GetComponent<PositionWriterBehaviour>();

            Assert.IsTrue(writerBehaviour.enabled);
            Assert.IsNotNull(writerBehaviour.Writer);
        }

        [Test]
        public void Injection_does_not_happen_if_not_all_constraints_are_satisfied()
        {
            var go = CreateAndLinkGameObjectWithComponent<CompositeBehaviour>(EntityId);
            var compositeBehaviour = go.GetComponent<CompositeBehaviour>();

            Assert.IsFalse(compositeBehaviour.enabled);
            Assert.IsNull(compositeBehaviour.Reader);
            Assert.IsNull(compositeBehaviour.Writer);
        }

        [Test]
        public void Injection_happens_if_all_constraints_are_satisfied()
        {
            connectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            receiveSystem.Update();

            var go = CreateAndLinkGameObjectWithComponent<CompositeBehaviour>(EntityId);
            var compositeBehaviour = go.GetComponent<CompositeBehaviour>();

            Assert.IsTrue(compositeBehaviour.enabled);
            Assert.IsNotNull(compositeBehaviour.Reader);
            Assert.IsNotNull(compositeBehaviour.Writer);
        }

        private GameObject CreateAndLinkGameObjectWithComponent<T>(long entityId) where T : MonoBehaviour
        {
            var gameObject = new GameObject("TestGameObject");
            createdGameObject = gameObject;
            gameObject.AddComponent<T>();
            gameObject.GetComponent<T>().enabled = false;

            linker.LinkGameObjectToSpatialOSEntity(new EntityId(entityId), gameObject);
            requireLifecycleSystem.Update();

            return gameObject;
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
