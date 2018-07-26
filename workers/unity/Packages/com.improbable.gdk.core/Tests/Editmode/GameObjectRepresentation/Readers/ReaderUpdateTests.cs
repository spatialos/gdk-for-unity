using System;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Improbable.Gdk.Core.GameObjectRepresentation;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;

namespace Improbable.Gdk.Core.EditmodeTests.Readers
{
    [TestFixture]
    public class ReaderUpdateTests : ReaderWriterTestsBase
    {
        private void QueueUpdatesToEntity(params SpatialOSBlittableComponent.Update[] updatesToSend)
        {
            if (!EntityManager.HasComponent<ComponentsUpdated<SpatialOSBlittableComponent.Update>>(Entity))
            {
                EntityManager.AddComponent(Entity, typeof(ComponentsUpdated<SpatialOSBlittableComponent.Update>));

                var componentsUpdated = new ComponentsUpdated<SpatialOSBlittableComponent.Update>();
                EntityManager.SetComponentObject(Entity, componentsUpdated);
            }

            EntityManager.GetComponentObject<ComponentsUpdated<SpatialOSBlittableComponent.Update>>(Entity)
                .Buffer
                .AddRange(updatesToSend);
        }

        private void ClearUpdatesInEntity()
        {
            EntityManager.GetComponentObject<ComponentsUpdated<SpatialOSBlittableComponent.Update>>(Entity).Buffer
                .Clear();
        }

        [Test]
        public void ComponentUpdated_gets_triggered_when_the_reader_receives_an_update()
        {
            var componentUpdated = false;
            var updateToSend = new SpatialOSBlittableComponent.Update();

            Reader.ComponentUpdated += update =>
            {
                Assert.AreEqual(updateToSend, update);

                componentUpdated = true;
            };

            Assert.AreEqual(false, componentUpdated, "Adding an event callback should not fire it immediately");

            QueueUpdatesToEntity(updateToSend);

            var internalReader = (IReaderInternal) Reader;

            internalReader.OnComponentUpdate();

            Assert.AreEqual(true, componentUpdated);
        }

        [Test]
        public void ComponentUpdated_gets_triggered_for_multiple_updates()
        {
            var firstUpdate = new SpatialOSBlittableComponent.Update();
            var secondUpdate = new SpatialOSBlittableComponent.Update();
            var thirdUpdate = new SpatialOSBlittableComponent.Update();

            var updatesToSend = new[]
            {
                firstUpdate,
                secondUpdate,
                thirdUpdate
            };

            var nextUpdateIndex = 0;

            Reader.ComponentUpdated += update =>
            {
                Assert.AreEqual(updatesToSend[nextUpdateIndex], update,
                    $"The update at index {nextUpdateIndex} did not match the received update.");

                nextUpdateIndex++;
            };

            Assert.AreEqual(0, nextUpdateIndex, "Adding an event callback should not fire it immediately");

            var internalReader = (IReaderInternal) Reader;

            QueueUpdatesToEntity(updatesToSend);

            internalReader.OnComponentUpdate();

            Assert.AreEqual(3, nextUpdateIndex);
        }

        [Test]
        public void ComponentUpdated_calls_non_failure_callbacks_even_if_some_callbacks_fail()
        {
            bool secondUpdateCalled = false;

            Reader.ComponentUpdated += update => throw new Exception("divide by zero");
            Reader.ComponentUpdated += update => { secondUpdateCalled = true; };
            Reader.ComponentUpdated += update => throw new Exception("this statement is false");

            QueueUpdatesToEntity(new SpatialOSBlittableComponent.Update());

            var internalReader = (IReaderInternal) Reader;

            LogAssert.Expect(LogType.Exception, "Exception: divide by zero");
            LogAssert.Expect(LogType.Exception, "Exception: this statement is false");

            internalReader.OnComponentUpdate();

            Assert.IsTrue(secondUpdateCalled);
        }

        [Test]
        public void FieldUpdates_get_called_if_property_changes()
        {
            bool floatFieldUpdated = false;
            bool intFieldUpdated = false;

            Reader.FloatFieldUpdated += newValue => { floatFieldUpdated = true; };
            Reader.IntFieldUpdated += newValue => { intFieldUpdated = true; };

            QueueUpdatesToEntity(new SpatialOSBlittableComponent.Update
            {
                FloatField = new Option<float>(10.0f),
                IntField = new Option<int>(),
            });

            var internalReader = (IReaderInternal) Reader;

            internalReader.OnComponentUpdate();

            Assert.IsTrue(floatFieldUpdated);
            Assert.IsFalse(intFieldUpdated);

            floatFieldUpdated = false;

            ClearUpdatesInEntity();

            QueueUpdatesToEntity(new SpatialOSBlittableComponent.Update
            {
                FloatField = new Option<float>(),
                IntField = new Option<int>(20),
            });

            internalReader.OnComponentUpdate();

            Assert.IsFalse(floatFieldUpdated);
            Assert.IsTrue(intFieldUpdated);
        }
    }
}
