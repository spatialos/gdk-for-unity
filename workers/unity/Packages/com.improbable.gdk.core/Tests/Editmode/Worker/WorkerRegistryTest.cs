using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class WorkerRegistryTest
    {
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            World.DisposeAllWorlds();
        }

        [Test]
        public void SetWorkerForWorld_sets_worker_for_world_twice()
        {
            using (var testWorker = new UnityTestWorker("someId", Vector3.zero))
            {
                testWorker.RegisterSystems();
                var exception =
                    Assert.Throws<System.ArgumentException>(() => WorkerRegistry.SetWorkerForWorld(testWorker));
                Assert.IsTrue(exception.Message.Contains("worker") && exception.Message.Contains("world"));
            }
        }
    }
}
