using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Readers
{
    [TestFixture]
    public class ReaderDataTests : ReaderTestsBase
    {
        [Test]
        public void Data_returns_component_instance()
        {
            EntityManager.SetComponentData(Entity, new ReaderTestComponent { FloatValue = 5 });

            var data = EntityManager.GetComponentData<ReaderTestComponent>(Entity);

            Assert.AreEqual(data, Reader.Data);
            Assert.AreEqual(5, Reader.Data.FloatValue);
        }
    }
}
