using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class WorkerRegistryTest
    {
        internal class UnityTestWorker : WorkerBase
        {
            public const string WorkerType = "UnityTestWorker";

            public override string GetWorkerType => WorkerType;

            public UnityTestWorker(string workerId, Vector3 origin) : base(workerId, origin)
            {
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            World.DisposeAllWorlds();
        }

        [Test]
        public void SetWorkerForWorld_sets_worker_for_world_twice()
        {
            var testWorker = new UnityTestWorker("someId", Vector3.zero);
            testWorker.RegisterSystems();
            var exception = Assert.Throws<System.Exception>(() => WorkerRegistry.SetWorkerForWorld(testWorker));
            Assert.IsTrue(exception.Message.Contains("worker") && exception.Message.Contains("world"));
        }
    }
}
