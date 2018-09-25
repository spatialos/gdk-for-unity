using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.BlittableTypes;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.Readers
{
    [TestFixture]
    internal class WriterSendUpdateTests : ReaderWriterTestsBase
    {
        [Test]
        public void Sending_component_update_changes_field()
        {
            WriterPublic.Send(new BlittableComponent.Update
            {
                IntField = new Option<int>(42)
            });
            int valueRead = EntityManager.GetComponentData<BlittableComponent.Component>(Entity).IntField;
            Assert.AreEqual(42, valueRead);
        }

        [Test]
        public void Updating_field_leaves_other_field_unchanged()
        {
            BlittableComponent.Component newValues = new BlittableComponent.Component();
            newValues.DoubleField = 13.37;
            EntityManager.SetComponentData(Entity, newValues);

            WriterPublic.Send(new BlittableComponent.Update
            {
                IntField = new Option<int>(42)
            });

            double valueRead = EntityManager.GetComponentData<BlittableComponent.Component>(Entity).DoubleField;
            Assert.AreEqual(13.37, valueRead);
        }

        [Test]
        public void Readers_read_updated_value_after_sending_update()
        {
            WriterPublic.Send(new BlittableComponent.Update
            {
                IntField = new Option<int>(42)
            });
            int valueRead = ReaderPublic.Data.IntField;
            Assert.AreEqual(42, valueRead);
        }

        [Test]
        public void Sending_component_update_sets_dirtybit()
        {
            WriterPublic.Send(new BlittableComponent.Update
            {
                IntField = new Option<int>(42)
            });
            Assert.IsTrue(EntityManager.GetComponentData<BlittableComponent.Component>(Entity).DirtyBit);
        }
    }
}
