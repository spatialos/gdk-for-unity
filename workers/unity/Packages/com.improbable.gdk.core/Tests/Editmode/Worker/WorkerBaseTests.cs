using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class WorkerBaseTests
    {
        [Test]
        public void WorkerId_should_be_dynamically_generated_when_parameter_is_null_or_empty()
        {
            using (var workerWithEmptyWorkerIdParameter = new UnityTestWorker("", Vector3.zero))
            {
                Assert.IsNotNull(workerWithEmptyWorkerIdParameter.WorkerId,
                    "The worker should have an ID generated for it if an empty string is passed into the constructor parameter, but it did not.");
                Assert.IsTrue(workerWithEmptyWorkerIdParameter.WorkerId.StartsWith("UnityTestWorker-"),
                    "The generated worker ID should start with the worker type, but it did not.");
            }

            using (var workerWithNullWorkerIdParameter = new UnityTestWorker(null, Vector3.zero))
            {
                Assert.IsNotNull(workerWithNullWorkerIdParameter.WorkerId,
                    "The worker should have an ID generated for it if null is passed into the constructor parameter, but it did not.");
                Assert.IsTrue(workerWithNullWorkerIdParameter.WorkerId.StartsWith("UnityTestWorker-"),
                    "The generated worker ID should start with the worker type, but it did not.");
            }
        }

        [Test]
        public void WorkerId_should_be_unique_when_dynamically_generated()
        {
            using (var workerWithEmptyWorkerIdParameter = new UnityTestWorker("", Vector3.zero))
            {
                using (var workerWithNullWorkerIdParameter = new UnityTestWorker(null, Vector3.zero))
                {
                    Assert.AreNotEqual(
                        workerWithEmptyWorkerIdParameter.WorkerId,
                        workerWithNullWorkerIdParameter.WorkerId,
                        "Auto-generated worker IDs should be unique.");
                }
            }
        }

        [Test]
        public void WorkerBase_should_accept_an_arbitrary_WorkerId()
        {
            using (WorkerBase worker = new UnityTestWorker("worker-id", Vector3.zero))
            {
                Assert.AreEqual("worker-id", worker.WorkerId);
            }

            using (WorkerBase worker = new UnityTestWorker("another-worker-id", Vector3.zero))
            {
                Assert.AreEqual("another-worker-id", worker.WorkerId);
            }
        }
    }
}
