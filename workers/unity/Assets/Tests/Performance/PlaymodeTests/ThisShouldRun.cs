using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Improbable.Gdk.PlaymodePerformanceTests
{
    public class ThisShouldRun
    {
        [Performance, Test]
        public void SamplePlaymodePerformanceTest()
        {
            Assert.IsTrue(true);
        }
    }
}
