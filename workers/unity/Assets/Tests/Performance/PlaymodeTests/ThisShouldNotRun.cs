using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Improbable.Gdk.PerformanceTests
{
    public class ThisShouldNotRun
    {
        [Performance, Test]
        public void This_should_not_run()
        {
            Assert.IsTrue(false);
        }
    }
}
