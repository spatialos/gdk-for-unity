using Generated.Improbable.Gdk.Tests.BlittableTypes;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    [TestFixture]
    public class ReaderDataTests : ReaderWriterTestsBase
    {
        [Test]
        public void Data_returns_component_instance()
        {
            EntityManager.SetComponentData(Entity, new SpatialOSBlittableComponent { FloatField = 5.0f });

            var data = EntityManager.GetComponentData<SpatialOSBlittableComponent>(Entity);

            Assert.AreEqual(data, Reader.Data);
            Assert.AreEqual(5, Reader.Data.FloatField);
        }
    }
}
