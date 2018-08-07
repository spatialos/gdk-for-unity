using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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
            World world = new World("test-world");
            var entityManager = world.GetOrCreateManager<EntityManager>();
            using (var worker = new UnityTestWorker(entityManager))
            {
                WorkerRegistry.SetWorkerForWorld(worker, world);
                var exception =
                    Assert.Throws<System.ArgumentException>(() => WorkerRegistry.SetWorkerForWorld(worker, world));
                Assert.IsTrue(exception.Message.Contains("Worker") && exception.Message.Contains("world"));
            }
        }
    }
}
