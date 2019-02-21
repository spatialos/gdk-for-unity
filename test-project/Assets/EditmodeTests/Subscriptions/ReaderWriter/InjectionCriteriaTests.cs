using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Tests;
using Improbable.Gdk.TestUtils;
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
        private World world;
        private SpatialOSReceiveSystem receiveSystem;
        private EntityGameObjectLinker linker;

        private GameObject createdGameObject;
        private EntityId entityId = new EntityId(100);

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            world.CreateManager<WorkerSystem>(null, new LoggingDispatcher(), "TestWorkerType", Vector3.zero);
            receiveSystem = world.CreateManager<SpatialOSReceiveSystem>();
            world.GetOrCreateManager<ComponentConstraintsCallbackSystem>();
            world.CreateManager<SubscriptionSystem>();
            world.CreateManager<CommandCallbackSystem>();
            world.CreateManager<ComponentCallbackSystem>();
            world.CreateManager<RequireLifecycleSystem>();

            linker = new EntityGameObjectLinker(world);

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            var diff = new DiffBuilder().CreateEntity(100, template);
            receiveSystem.ApplyDiff(diff.Diff);
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
            var go = CreateAndLinkGameObjectWithComponent<ExhaustiveSingularReaderBehaviour>(entityId);
            var readerBehaviour = go.GetComponent<ExhaustiveSingularReaderBehaviour>();

            Assert.IsNull(readerBehaviour.Reader);
        }

        [Test]
        public void Reader_is_injected_when_entity_component_is_checked_out()
        {
            var go = CreateAndLinkGameObjectWithComponent<PositionReaderBehaviour>(entityId);
            var readerBehaviour = go.GetComponent<PositionReaderBehaviour>();

            Assert.IsNotNull(readerBehaviour.Reader);
        }

        [Test]
        public void Writer_is_not_injected_when_entity_component_is_not_checked_out()
        {
            var go = CreateAndLinkGameObjectWithComponent<ExhaustiveSingularWriterBehaviour>(entityId);
            var readerBehaviour = go.GetComponent<ExhaustiveSingularWriterBehaviour>();

            Assert.IsNull(readerBehaviour.Writer);
        }

        [Test]
        public void Writer_is_not_injected_when_entity_component_is_checked_out_and_unauthoritative()
        {
            var go = CreateAndLinkGameObjectWithComponent<PositionWriterBehaviour>(entityId);
            var readerBehaviour = go.GetComponent<PositionWriterBehaviour>();

            Assert.IsNull(readerBehaviour.Writer);
        }

        [Test]
        public void Writer_is_injected_when_entity_component_is_checked_out_and_authoritative()
        {
            var diff = new DiffBuilder().ChangeAuthority(100, Position.ComponentId, Authority.Authoritative);
            receiveSystem.ApplyDiff(diff.Diff);

            var go = CreateAndLinkGameObjectWithComponent<PositionWriterBehaviour>(entityId);
            var writerBehaviour = go.GetComponent<PositionWriterBehaviour>();

            Assert.IsNotNull(writerBehaviour.Writer);
        }

        [Test]
        public void Injection_does_not_happen_if_not_all_constraints_are_satisfied()
        {
            var go = CreateAndLinkGameObjectWithComponent<CompositeBehaviour>(entityId);
            var compositeBehaviour = go.GetComponent<CompositeBehaviour>();

            Assert.IsNull(compositeBehaviour.Reader);
            Assert.IsNull(compositeBehaviour.Writer);
        }

        [Test]
        public void Injection_happens_if_all_constraints_are_satisfied()
        {
            var diff = new DiffBuilder().ChangeAuthority(100, Position.ComponentId, Authority.Authoritative);
            receiveSystem.ApplyDiff(diff.Diff);

            var go = CreateAndLinkGameObjectWithComponent<CompositeBehaviour>(entityId);
            var compositeBehaviour = go.GetComponent<CompositeBehaviour>();

            Assert.IsNotNull(compositeBehaviour.Reader);
            Assert.IsNotNull(compositeBehaviour.Writer);
        }

        private GameObject CreateAndLinkGameObjectWithComponent<T>(EntityId entityId) where T : MonoBehaviour
        {
            var gameObject = new GameObject("TestGameObject");
            createdGameObject = gameObject;
            gameObject.AddComponent<T>();

            linker.LinkGameObjectToSpatialOSEntity(entityId, gameObject);

            return gameObject;
        }

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
    }
}
