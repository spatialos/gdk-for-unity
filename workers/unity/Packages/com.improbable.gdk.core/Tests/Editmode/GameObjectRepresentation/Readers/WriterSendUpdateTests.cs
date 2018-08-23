using Generated.Improbable.Gdk.Tests.BlittableTypes;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    [TestFixture]
    internal class WriterSendUpdateTests : ReaderWriterTestsBase
    {
        [Test]
        public void Sending_component_update_changes_field()
        {
            WriterPublic.Send(new SpatialOSBlittableComponent.Update
            {
                IntField = new Option<int>(42)
            });
            int valueRead = EntityManager.GetComponentData<SpatialOSBlittableComponent>(Entity).IntField;
            Assert.AreEqual(42, valueRead);
        }

        [Test]
        public void Updating_field_leaves_other_field_unchanged()
        {
            SpatialOSBlittableComponent newValues = new SpatialOSBlittableComponent();
            newValues.DoubleField = 13.37;
            EntityManager.SetComponentData(Entity, newValues);

            WriterPublic.Send(new SpatialOSBlittableComponent.Update
            {
                IntField = new Option<int>(42)
            });

            double valueRead = EntityManager.GetComponentData<SpatialOSBlittableComponent>(Entity).DoubleField;
            Assert.AreEqual(13.37, valueRead);
        }

        [Test]
        public void Readers_read_updated_value_after_sending_update()
        {
            WriterPublic.Send(new SpatialOSBlittableComponent.Update
            {
                IntField = new Option<int>(42)
            });
            int valueRead = ReaderPublic.Data.IntField;
            Assert.AreEqual(42, valueRead);
        }

        [Test]
        public void Sending_component_update_sets_dirtybit()
        {
            WriterPublic.Send(new SpatialOSBlittableComponent.Update
            {
                IntField = new Option<int>(42)
            });
            Assert.IsTrue(EntityManager.GetComponentData<SpatialOSBlittableComponent>(Entity).DirtyBit);
        }
    }
}
