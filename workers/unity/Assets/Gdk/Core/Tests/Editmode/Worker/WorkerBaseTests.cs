using Improbable.Gdk.Core.EditmodeTests.Utils;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class WorkerBaseTests
    {
        [Test]
        public void WorkerBase_should_throw_exception_when_WorkerId_is_null_or_empty()
        {
            // Empty check
            var empty_exception = Assert.Throws<System.ArgumentException>(() =>
            {
                var worker = new UnityTestWorker("", new Vector3());
            });
            Assert.IsTrue(empty_exception.Message.Contains("WorkerId"));

            // Null check
            var null_exception = Assert.Throws<System.ArgumentException>(() =>
            {
                var worker = new UnityTestWorker(null, new Vector3());
            });
            Assert.IsTrue(null_exception.Message.Contains("WorkerId"));
        }

        [Test]
        public void WorkerBase_should_accept_an_arbitrary_WorkerId()
        {
            Assert.DoesNotThrow(() =>
            {
                var worker = new UnityTestWorker("worker-id", new Vector3());
                worker.Dispose();
            });
        }
    }
}
