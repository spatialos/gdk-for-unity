using System.Linq;
using Generated.Improbable;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Generated.Improbable.Gdk.Tests.NonblittableTypes;
using Improbable.Gdk.Core.GameObjectRepresentation;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class RequiredFieldInjectorTests
    {
        private const uint BlittableComponentId = 1001;
        private const uint NonBlittableComponentId = 1002;

        private class SingleReaderBehaviour : MonoBehaviour
        {
            [Require] public BlittableComponent.Reader Reader;
        }

        private class TwoWritersBehaviour : MonoBehaviour
        {
            [Require] public BlittableComponent.Writer Writer1;
            [Require] public NonBlittableComponent.Writer Writer2;
        }

        private class MultipleReadersOfSameType : MonoBehaviour
        {
            [Require] public BlittableComponent.Reader Reader1;
            [Require] public BlittableComponent.Reader Reader2;
        }

        private class ReaderAndWriterOfSameType : MonoBehaviour
        {
            [Require] public BlittableComponent.Reader Reader;
            [Require] public BlittableComponent.Writer Writer;
        }

        private class RequireInvalidMember : MonoBehaviour
        {
            [Require] public int Bad;
        }

        private World world;
        private RequiredFieldInjector injector;
        private Entity testEntity;
        private GameObject testGameObject;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            var entityManager = world.GetOrCreateManager<EntityManager>();
            injector = new RequiredFieldInjector(entityManager, new LoggingDispatcher());
            testEntity = entityManager.CreateEntity();
            testGameObject = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            if (testGameObject != null)
            {
                Object.DestroyImmediate(testGameObject);
            }

            world.Dispose();
        }

        [Test]
        public void RequireTagInjector_injects_Reader()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            Assert.IsNull(behaviour.Reader);
            injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.NotNull(behaviour.Reader);
            Assert.AreEqual(typeof(BlittableComponent.ReaderWriterImpl), behaviour.Reader.GetType());
        }

        [Test]
        public void RequireTagInjector_injects_two_Writers()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.NotNull(behaviour.Writer1);
            Assert.NotNull(behaviour.Writer2);
        }

        [Test]
        public void RequireTagInjector_deinjects_Reader()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            injector.InjectAllReadersWriters(behaviour, testEntity);
            injector.DeInjectAllReadersWriters(behaviour);
            Assert.IsNull(behaviour.Reader);
        }

        [Test]
        public void RequireTagInjector_finds_required_Reader_component_id()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            var foundIds = injector.GetRequiredReaderComponentIds(behaviour.GetType());
            var foundId = foundIds.First();
            Assert.AreEqual(BlittableComponentId, foundId);
        }

        [Test]
        public void RequireTagInjector_finds_two_required_Writers()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            var foundIds = injector.GetRequiredWriterComponentIds(behaviour.GetType());
            Assert.AreEqual(2, foundIds.Count);
        }

        [Test]
        public void RequireTagInjector_creates_two_Writers_required_for_different_components()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            var createdReaderWriters = injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.AreEqual(2, createdReaderWriters.Aggregate(0, (cnt, pair) => cnt + pair.Value.Length));
        }

        [Test]
        public void RequireTagInjector_creates_two_Readers_required_for_the_same_component()
        {
            var behaviour = testGameObject.AddComponent<MultipleReadersOfSameType>();
            var createdReaderWriters = injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.AreEqual(2, createdReaderWriters.Aggregate(0, (cnt, pair) => cnt + pair.Value.Length));
        }

        [Test]
        public void RequireTagInjector_creates_one_Reader_and_one_Writer_required_for_same_component()
        {
            var behaviour = testGameObject.AddComponent<ReaderAndWriterOfSameType>();
            var createdReaderWriters = injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.AreEqual(2, createdReaderWriters.Aggregate(0, (cnt, pair) => cnt + pair.Value.Length));
        }

        [Test]
        public void Readers_created_for_same_component_are_different()
        {
            var behaviour = testGameObject.AddComponent<MultipleReadersOfSameType>();
            var createdReaderWriters = injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.AreNotEqual(createdReaderWriters[BlittableComponentId][0], createdReaderWriters[BlittableComponentId][1]);
        }

        [Test]
        public void RequireTagInjector_ignores_invalid_Require_tags()
        {
            var behaviour = testGameObject.AddComponent<RequireInvalidMember>();
            var foundIds = injector.GetRequiredReaderComponentIds(behaviour.GetType());
            Assert.IsEmpty(foundIds);
        }

        [Test]
        public void RequireTagInjector_injects_multiple_Readers()
        {
            var behaviour = testGameObject.AddComponent<MultipleReadersOfSameType>();
            Assert.IsNull(behaviour.Reader1);
            Assert.IsNull(behaviour.Reader2);
            injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.NotNull(behaviour.Reader1);
            Assert.AreEqual(typeof(BlittableComponent.ReaderWriterImpl), behaviour.Reader1.GetType());
            Assert.NotNull(behaviour.Reader2);
            Assert.AreEqual(typeof(BlittableComponent.ReaderWriterImpl), behaviour.Reader2.GetType());
        }
    }
}
