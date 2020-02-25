using NUnit.Framework;

namespace Improbable.Gdk.TestUtils
{
    public abstract class MockBase
    {
        protected MockWorld World;

        protected virtual MockWorld.Options GetOptions()
        {
            return new MockWorld.Options();
        }

        [SetUp]
        public void Setup()
        {
            World = MockWorld.Create(GetOptions());
        }

        [TearDown]
        public void TearDown()
        {
            World.Dispose();
        }
    }
}
