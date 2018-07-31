using System.Linq;
using System.Text.RegularExpressions;
using Generated.Improbable;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class SpatialOSBehaviourLibraryTests
    {
        // The dummy code needs to be removed once we have codegenerated writers.
        private const uint PositionComponentId = 54;
        private const uint ComponentId2 = 1338;
        private const uint ComponentId3 = 1339;

        [WriterInterface]
        [ComponentId(ComponentId2)]
        private interface DummyWriter1
        {
        }

        [WriterInterface]
        [ComponentId(ComponentId3)]
        private interface DummyWriter2
        {
        }

        private class SingleReaderBehaviour : MonoBehaviour
        {
            [Require] public Position.Reader Reader;
        }

        private class TwoWritersBehaviour : MonoBehaviour
        {
            [Require] public DummyWriter1 Writer1;
            [Require] public DummyWriter2 Writer2;
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
        private SpatialOSBehaviourLibrary library;
        private Entity testEntity;
        private GameObject testGameObject;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            var entityManager = world.GetOrCreateManager<EntityManager>();
            library = new SpatialOSBehaviourLibrary(entityManager, new LoggingDispatcher());
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
            library.InjectAllReadersWriters(behaviour, testEntity);
            Assert.NotNull(behaviour.Reader);
            Assert.AreEqual(typeof(Position.ReaderWriterImpl), behaviour.Reader.GetType());
        }

        [Test]
        public void SpatialOSBehaviourLibrary_injects_two_Writers()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            library.InjectAllReadersWriters(behaviour, testEntity);
            Assert.NotNull(behaviour.Writer1);
            Assert.NotNull(behaviour.Writer2);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_deinjects_Reader()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            library.InjectAllReadersWriters(behaviour, testEntity);
            library.DeInjectAllReadersWriters(behaviour);
            Assert.IsNull(behaviour.Reader);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_finds_required_Reader_component_id()
        {
            var behaviour = testGameObject.AddComponent<SingleReaderBehaviour>();
            var foundIds = library.GetRequiredReaderComponentIds(behaviour.GetType());
            var foundId = foundIds.First();
            Assert.AreEqual(PositionComponentId, foundId);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_finds_two_required_Writers()
        {
            var behaviour = testGameObject.AddComponent<TwoWritersBehaviour>();
            var foundIds = library.GetRequiredWriterComponentIds(behaviour.GetType());
            Assert.AreEqual(2, foundIds.Count);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_ignores_invalid_Require_tags()
        {
            var behaviour = testGameObject.AddComponent<RequireInvalidMember>();
            var foundIds = library.GetRequiredReaderComponentIds(behaviour.GetType());
            Assert.IsEmpty(foundIds);
        }

        [Test]
        public void SpatialOSBehaviourLibrary_refuses_to_inject_multiple_Readers()
        {
            var behaviour = testGameObject.AddComponent<MultipleReadersOfSameType>();
            library.InjectAllReadersWriters(behaviour, testEntity);
            LogAssert.Expect(LogType.Error, new Regex(".*", RegexOptions.Singleline));
            Assert.IsNull(behaviour.Reader1);
            Assert.IsNull(behaviour.Reader2);
        }
    }
}
