using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Improbable.Gdk.EditmodePerformanceTests
{
    public class ThisShouldRun
    {
        [Performance, Test]
        public void SampleEditmodePerformanceTest()
        {
            Assert.IsTrue(true);
        }
    }
}
