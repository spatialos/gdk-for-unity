using NUnit.Framework;

namespace Improbable.Gdk.TestUtils
{
    public abstract class HybridGdkSystemTestBase
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        public static void CleanupAllInjectionHooks()
        {
        }
    }
}
