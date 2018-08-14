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
    public class ReaderWriterInjectorTests
    {
        private const uint PositionComponentId = 54;
        private const uint BlittableComponentId = 1001;
        private const uint NonBlittableComponentId = 1002;

        private class SingleReaderBehaviour : MonoBehaviour
        {
            [Require] public Position.Reader Reader;
        }

        private class TwoWritersBehaviour : MonoBehaviour
        {
            [Require] public BlittableComponent.Writer Writer1;
            [Require] public NonBlittableComponent.Writer Writer2;
        }

        private class MultipleReadersOfSameType : MonoBehaviour
        {
            [Require] public Position.Reader Reader1;
            [Require] public Position.Reader Reader2;
        }

        private class RequireInvalidMember : MonoBehaviour
        {
            [Require] public int Bad;
        }

        private World world;
        private ReaderWriterInjector injector;
        private Entity testEntity;
        private GameObject testGameObject;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            var entityManager = world.GetOrCreateManager<EntityManager>();
            injector = new ReaderWriterInjector(entityManager, new LoggingDispatcher());
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
        public void SpatialOSBehaviourLibrary_injects_Reader()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            Assert.IsNull(behaviour.Reader);
            injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.NotNull(behaviour.Reader);
            Assert.AreEqual(typeof(Position.ReaderWriterImpl), behaviour.Reader.GetType());
        }

        [Test]
        public void SpatialOSBehaviourLibrary_injects_two_Writers()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.NotNull(behaviour.Writer1);
            Assert.NotNull(behaviour.Writer2);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_deinjects_Reader()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            injector.InjectAllReadersWriters(behaviour, testEntity);
            injector.DeInjectAllReadersWriters(behaviour);
            Assert.IsNull(behaviour.Reader);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_finds_required_Reader_component_id()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            var foundIds = injector.GetRequiredReaderComponentIds(behaviour.GetType());
            var foundId = foundIds.First();
            Assert.AreEqual(PositionComponentId, foundId);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_finds_two_required_Writers()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            var foundIds = injector.GetRequiredWriterComponentIds(behaviour.GetType());
            Assert.AreEqual(2, foundIds.Count);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_ignores_invalid_Require_tags()
        {
            var behaviour = testGameObject.AddComponent<RequireInvalidMember>();
            var foundIds = injector.GetRequiredReaderComponentIds(behaviour.GetType());
            Assert.IsEmpty(foundIds);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_injects_multiple_Readers()
        {
            var behaviour = testGameObject.AddComponent<MultipleReadersOfSameType>();
            Assert.IsNull(behaviour.Reader1);
            Assert.IsNull(behaviour.Reader2);
            injector.InjectAllReadersWriters(behaviour, testEntity);
            Assert.NotNull(behaviour.Reader1);
            Assert.AreEqual(typeof(Position.ReaderWriterImpl), behaviour.Reader1.GetType());
            Assert.NotNull(behaviour.Reader2);
            Assert.AreEqual(typeof(Position.ReaderWriterImpl), behaviour.Reader2.GetType());
        }
    }
}
