using Improbable.Gdk.Subscriptions;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Subscriptions
{
    /// <summary>
    ///     This tests the interoperability of the WorkerId struct with strings.
    /// </summary>
    [TestFixture]
    public class WorkerIdEquatableTests
    {
        private readonly string workerIdString = "WorkerName-someR4nd0mch4r4ct3r5";

        [Test]
        public void WorkerId_can_be_implicitly_cast_to_string()
        {
            Assert.DoesNotThrow(() =>
            {
                string id = new WorkerId(workerIdString);
            });
        }

        [Test]
        public void WorkerId_implicitly_cast_to_string_has_the_same_string_contents()
        {
            string id = new WorkerId(workerIdString);

            Assert.IsTrue(id.Equals(workerIdString));
        }

        [Test]
        public void Identical_WorkerIds_are_equal()
        {
            WorkerId firstWorkerId = new WorkerId(workerIdString);
            WorkerId secondWorkerId = new WorkerId(workerIdString);

            Assert.IsTrue(firstWorkerId == secondWorkerId);
        }

        [Test]
        public void Different_WorkerIds_are_not_equal()
        {
            WorkerId firstWorkerId = new WorkerId("some-workerId");
            WorkerId secondWorkerId = new WorkerId("some-different-workerId");

            Assert.IsFalse(firstWorkerId == secondWorkerId);
        }

        [Test]
        public void WorkerId_struct_compared_to_equivalent_string()
        {
            string stringWorkerId = workerIdString;
            WorkerId structWorkerId = new WorkerId(stringWorkerId);

            Assert.IsTrue(stringWorkerId == structWorkerId);
        }

        [Test]
        public void WorkerId_struct_compared_to_unequivalent_string()
        {
            string stringWorkerId = "some-workerId";
            WorkerId structWorkerId = new WorkerId("some-different-workerId");

            Assert.IsFalse(stringWorkerId == structWorkerId);
        }
    }
}
