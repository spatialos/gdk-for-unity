using System;
using System.CodeDom;
using System.Linq;
using System.Text.RegularExpressions;
using Improbable.Gdk.Core.MonoBehaviours;
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
        private const uint ComponentId1 = 1337;
        private const uint ComponentId2 = 1338;
        private const uint ComponentId3 = 1339;

        [ReaderInterface]
        [ComponentId(ComponentId1)]
        private interface DummyReader
        {
        }

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
            [Require] public DummyReader Reader;
        }

        private class TwoWritersBehaviour : MonoBehaviour
        {
            [Require] public DummyWriter1 Writer1;
            [Require] public DummyWriter2 Writer2;
        }

        private class MultipleReadersOfSameType : MonoBehaviour
        {
            [Require] public DummyReader Reader1;
            [Require] public DummyReader Reader2;
        }

        private class RequireInvalidMember : MonoBehaviour
        {
            [Require] public int Bad;
        }

        private SpatialOSBehaviourLibrary library;
        private GameObject testGameObject;
        private Entity testEntity;
        private World world;

        [SetUp]
        public void Setup()
        {
            library = new SpatialOSBehaviourLibrary(new LoggingDispatcher());
            testGameObject = new GameObject();
            world = new World("Test World");
            var entityManager = world.GetOrCreateManager<EntityManager>();
            testEntity = entityManager.CreateEntity();
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
            library.InjectAllReadersWriters(behaviour, testEntity);
            Assert.NotNull(behaviour.Reader);
            Assert.IsTrue(true);
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
            var id = foundIds.First();
            Assert.AreEqual(ComponentId1, id);
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
