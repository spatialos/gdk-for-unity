using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.Tests.BlittableTypes;
using Improbable.Gdk.Tests.NonblittableTypes;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation
{
    [TestFixture]
    public class RequiredFieldInjectorTests
    {
        private readonly InjectableId BlittableComponentReaderWriterId
            = new InjectableId(InjectableType.ReaderWriter, 1001);

        private readonly InjectableId NonBlittableComponentReaderWriterId
            = new InjectableId(InjectableType.ReaderWriter, 1002);

        private class SingleReaderBehaviour : MonoBehaviour
        {
            [Require] public BlittableComponent.Requirable.Reader Reader;
        }

        private class TwoWritersBehaviour : MonoBehaviour
        {
            [Require] public BlittableComponent.Requirable.Writer Writer1;
            [Require] public NonBlittableComponent.Requirable.Writer Writer2;
        }

        private class MultipleReadersOfSameType : MonoBehaviour
        {
            [Require] public BlittableComponent.Requirable.Reader Reader1;
            [Require] public BlittableComponent.Requirable.Reader Reader2;
        }

        private class ReaderAndWriterOfSameType : MonoBehaviour
        {
            [Require] public BlittableComponent.Requirable.Reader Reader;
            [Require] public BlittableComponent.Requirable.Writer Writer;
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
                UnityObjectDestroyer.Destroy(testGameObject);
            }

            world.Dispose();
        }

        [Test]
        public void RequireTagInjector_injects_Reader()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            Assert.IsNull(behaviour.Reader);
            injector.InjectAllRequiredFields(behaviour, testEntity);
            Assert.NotNull(behaviour.Reader);
            Assert.AreEqual(typeof(BlittableComponent.Requirable.ReaderWriterImpl), behaviour.Reader.GetType());
        }

        [Test]
        public void RequireTagInjector_injects_two_Writers()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            injector.InjectAllRequiredFields(behaviour, testEntity);
            Assert.NotNull(behaviour.Writer1);
            Assert.NotNull(behaviour.Writer2);
        }

        [Test]
        public void RequireTagInjector_deinjects_Reader()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            injector.InjectAllRequiredFields(behaviour, testEntity);
            injector.DeInjectAllRequiredFields(behaviour);
            Assert.IsNull(behaviour.Reader);
        }

        [Test]
        public void RequireTagInjector_finds_required_Reader_component_id()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            var foundIds = injector.GetComponentPresenceRequirements(behaviour.GetType());
            var foundId = Enumerable.First<uint>(foundIds);
            Assert.AreEqual(BlittableComponentReaderWriterId.componentId, foundId);
        }

        [Test]
        public void RequireTagInjector_finds_two_required_Writers()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            var foundIds = injector.GetComponentAuthorityRequirements(behaviour.GetType());
            Assert.AreEqual(2, foundIds.Count);
        }

        [Test]
        public void RequireTagInjector_creates_two_Writers_required_for_different_components()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            var createdReaderWriters = injector.InjectAllRequiredFields(behaviour, testEntity);
            Assert.AreEqual(2,
                createdReaderWriters.Aggregate<KeyValuePair<InjectableId, IInjectable[]>, int>(0,
                    (cnt, pair) => cnt + pair.Value.Length));
        }

        [Test]
        public void RequireTagInjector_creates_two_Readers_required_for_the_same_component()
        {
            var behaviour = testGameObject.AddComponent<MultipleReadersOfSameType>();
            var createdReaderWriters = injector.InjectAllRequiredFields(behaviour, testEntity);
            Assert.AreEqual(2, createdReaderWriters.Aggregate(0, (cnt, pair) => cnt + pair.Value.Length));
        }

        [Test]
        public void RequireTagInjector_creates_one_Reader_and_one_Writer_required_for_same_component()
        {
            var behaviour = testGameObject.AddComponent<ReaderAndWriterOfSameType>();
            var createdReaderWriters = injector.InjectAllRequiredFields(behaviour, testEntity);
            Assert.AreEqual(2, createdReaderWriters.Aggregate(0, (cnt, pair) => cnt + pair.Value.Length));
        }

        [Test]
        public void Readers_created_for_same_component_are_different()
        {
            var behaviour = testGameObject.AddComponent<MultipleReadersOfSameType>();
            var createdReaderWriters = injector.InjectAllRequiredFields(behaviour, testEntity);
            Assert.AreNotEqual(createdReaderWriters[BlittableComponentReaderWriterId][0],
                createdReaderWriters[BlittableComponentReaderWriterId][1]);
        }

        [Test]
        public void RequireTagInjector_ignores_invalid_Require_tags()
        {
            var behaviour = testGameObject.AddComponent<RequireInvalidMember>();
            var foundIds = injector.GetComponentPresenceRequirements(behaviour.GetType());
            Assert.IsEmpty(foundIds);
        }

        [Test]
        public void RequireTagInjector_injects_multiple_Readers()
        {
            var behaviour = testGameObject.AddComponent<MultipleReadersOfSameType>();
            Assert.IsNull(behaviour.Reader1);
            Assert.IsNull(behaviour.Reader2);
            injector.InjectAllRequiredFields(behaviour, testEntity);
            Assert.NotNull(behaviour.Reader1);
            Assert.AreEqual(typeof(BlittableComponent.Requirable.ReaderWriterImpl), behaviour.Reader1.GetType());
            Assert.NotNull(behaviour.Reader2);
            Assert.AreEqual(typeof(BlittableComponent.Requirable.ReaderWriterImpl), behaviour.Reader2.GetType());
        }
    }
}
