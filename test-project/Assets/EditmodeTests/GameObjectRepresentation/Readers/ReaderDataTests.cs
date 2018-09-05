using Generated.Improbable.Gdk.Tests.BlittableTypes;
using NUnit.Framework;

namespace Improbable.Gdk.Generated.EditmodeTests.MonoBehaviours.Readers
{
    [TestFixture]
    internal class ReaderDataTests : ReaderWriterTestsBase
    {
        [Test]
        public void Data_returns_component_instance()
        {
            EntityManager.SetComponentData(Entity, new BlittableComponent.Component { FloatField = 5.0f });

            var data = EntityManager.GetComponentData<BlittableComponent.Component>(Entity);

            Assert.AreEqual(data, ReaderPublic.Data);
            Assert.AreEqual(data.FloatField, ReaderPublic.Data.FloatField);
        }
    }
}
