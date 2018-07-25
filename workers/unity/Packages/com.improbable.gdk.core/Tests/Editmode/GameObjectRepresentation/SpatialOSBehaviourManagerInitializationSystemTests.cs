using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class SpatialOSBehaviourManagerInitializationSystemTests : HybridGdkSystemTestBase
    {
        private UnityTestWorker worker;

        [SetUp]
        public void Setup()
        {
            worker = new UnityTestWorker("worker-id", new Vector3());
        }

        [TearDown]
        public void TearDown()
        {
            worker.Dispose();
        }

        [Test]
        public void OnCreateManager_throws_if_GameObjectDispatcherSystem_not_present()
        {
            Assert.Throws<SpatialOSBehaviourManagerInitializationSystem.GameObjectDispatcherSystemNotFoundException>(() =>
            {
                worker.World.GetOrCreateManager<SpatialOSBehaviourManagerInitializationSystem>();
            });
        }

        [Test]
        public void OnCreateManager_does_not_throw_if_GameObjectDispatcherSystem_present()
        {
            worker.World.GetOrCreateManager<GameObjectDispatcherSystem>();
            Assert.DoesNotThrow(() =>
            {
                worker.World.GetOrCreateManager<SpatialOSBehaviourManagerInitializationSystem>();
            });
        }
    }
}
