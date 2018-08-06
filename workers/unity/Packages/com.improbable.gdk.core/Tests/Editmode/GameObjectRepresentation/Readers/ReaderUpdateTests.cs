using System;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Improbable.Gdk.Core.MonoBehaviours;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    [TestFixture]
    internal class ReaderUpdateTests : ReaderWriterTestsBase
    {
        private void ClearUpdatesInEntity()
        {
            EntityManager.GetComponentObject<ComponentsUpdated<SpatialOSBlittableComponent.Update>>(Entity).Buffer
                .Clear();
        }

        private void QueueUpdatesToEntity<TUpdate>(params TUpdate[] updatesToQueue)
            where TUpdate : ISpatialComponentUpdate
        {
            QueueUpdatesToEntity(EntityManager, Entity, updatesToQueue);
        }

        [Test]
        public void ComponentUpdated_gets_triggered_when_the_reader_receives_an_update()
        {
            var componentUpdated = false;
            var updateToQueue = new SpatialOSBlittableComponent.Update();
            Reader.ComponentUpdated += update =>
            {
                Assert.AreEqual(updateToQueue, update);
                componentUpdated = true;
            };

            Assert.IsFalse(componentUpdated, "Adding an event callback should not fire it immediately");
            QueueUpdatesToEntity(updateToQueue);
            Reader.OnComponentUpdate();
            Assert.IsTrue(componentUpdated);
        }

        [Test]
        public void ComponentUpdated_gets_triggered_for_multiple_updates()
        {
            var updatesToQueue = new[]
            {
                new SpatialOSBlittableComponent.Update(),
                new SpatialOSBlittableComponent.Update(),
                new SpatialOSBlittableComponent.Update()
            };
            var updatesReceived = 0;
            Reader.ComponentUpdated += update =>
            {
                Assert.AreEqual(updatesToQueue[updatesReceived], update,
                    $"The update at index {updatesReceived} did not match the received update.");

                updatesReceived++;
            };

            Assert.AreEqual(0, updatesReceived, "Adding an event callback should not fire it immediately");
            QueueUpdatesToEntity(updatesToQueue);
            Reader.OnComponentUpdate();
            Assert.AreEqual(3, updatesReceived);
        }

        [Test]
        public void ComponentUpdated_calls_all_callbacks_even_if_some_callbacks_fail()
        {
            bool secondUpdateCalled = false;
            Reader.ComponentUpdated += update => throw new Exception("Update Exception: divide by zero");
            Reader.ComponentUpdated += update => { secondUpdateCalled = true; };
            Reader.ComponentUpdated += update => throw new Exception("Update Exception: this statement is false");
            QueueUpdatesToEntity(new SpatialOSBlittableComponent.Update());

            LogAssert.Expect(LogType.Exception, "Exception: Update Exception: divide by zero");
            LogAssert.Expect(LogType.Exception, "Exception: Update Exception: this statement is false");
            Assert.DoesNotThrow(() => { Reader.OnComponentUpdate(); },
                "Exceptions that happen within component update callbacks should not propagate to callers.");
            Assert.IsTrue(secondUpdateCalled);
        }

        [Test]
        public void FieldUpdates_get_called_if_a_field_changes()
        {
            bool floatFieldUpdated = false;
            float receivedFloatValue = 0;
            Reader.FloatFieldUpdated += newValue =>
            {
                floatFieldUpdated = true;
                receivedFloatValue = newValue;
            };
            QueueUpdatesToEntity(new SpatialOSBlittableComponent.Update
            {
                FloatField = new Option<float>(10.0f),
            });
            Reader.OnComponentUpdate();

            Assert.IsTrue(floatFieldUpdated,
                "The update contains a float field but the callback for the float field was not called.");
            Assert.AreEqual(10.0f, receivedFloatValue);
        }

        [Test]
        public void FieldUpdates_do_not_get_called_if_another_field_changes()
        {
            bool floatFieldUpdated = false;
            bool intFieldUpdated = false;
            float receivedFloatValue = 0;
            int receivedIntValue = 0;
            Reader.FloatFieldUpdated += newValue =>
            {
                floatFieldUpdated = true;
                receivedFloatValue = newValue;
            };
            Reader.IntFieldUpdated += newValue =>
            {
                intFieldUpdated = true;
                receivedIntValue = newValue;
            };
            QueueUpdatesToEntity(new SpatialOSBlittableComponent.Update
            {
                FloatField = new Option<float>(10.0f),
            });
            Reader.OnComponentUpdate();

            Assert.IsTrue(floatFieldUpdated,
                "The update contains a float field but the callback for the float field was not called.");
            Assert.AreEqual(10.0f, receivedFloatValue);
            Assert.IsFalse(intFieldUpdated,
                "The update does not contain an int field but the callback for the int field was called. ");

            floatFieldUpdated = false;
            ClearUpdatesInEntity();
            QueueUpdatesToEntity(new SpatialOSBlittableComponent.Update
            {
                IntField = new Option<int>(20),
            });
            Reader.OnComponentUpdate();

            Assert.IsFalse(floatFieldUpdated,
                "The second update does not contain a float field but the callback was called.");
            Assert.IsTrue(intFieldUpdated,
                "The second update contains an int field but the callback for the int field was not called.");
            Assert.AreEqual(20, receivedIntValue);
        }

        [Test]
        public void FieldUpdates_get_called_for_all_changed_fields()
        {
            bool floatFieldUpdated = false;
            bool intFieldUpdated = false;
            float receivedFloatValue = 0;
            int receivedIntValue = 0;
            Reader.FloatFieldUpdated += newValue =>
            {
                floatFieldUpdated = true;
                receivedFloatValue = newValue;
            };
            Reader.IntFieldUpdated += newValue =>
            {
                intFieldUpdated = true;
                receivedIntValue = newValue;
            };
            QueueUpdatesToEntity(new SpatialOSBlittableComponent.Update
            {
                IntField = new Option<int>(30),
                FloatField = new Option<float>(40.0f)
            });
            Reader.OnComponentUpdate();

            Assert.IsTrue(intFieldUpdated,
                "The update contains an int field but the callback for the int field was not called.");
            Assert.AreEqual(30, receivedIntValue);
            Assert.IsTrue(floatFieldUpdated,
                "The update contains an float field but the callback for the float field was not called.");
            Assert.AreEqual(40.0f, receivedFloatValue);
        }

        [Test]
        public void FieldUpdates_call_all_callbacks_even_if_some_callbacks_fail()
        {
            bool intUpdateCalled = false;
            bool floatUpdateCalled = false;
            Reader.IntFieldUpdated += newValue => throw new Exception($"Int Update Exception: {newValue}");
            Reader.IntFieldUpdated += newValue => { intUpdateCalled = true; };
            Reader.IntFieldUpdated += newValue => throw new Exception($"Int Update Exception 2: {newValue}");
            Reader.FloatFieldUpdated += newValue => throw new Exception($"Float Update Exception: {newValue:F2}");
            Reader.FloatFieldUpdated += newValue => { floatUpdateCalled = true; };
            Reader.FloatFieldUpdated += newValue => throw new Exception($"Float Update Exception 2: {newValue:F2}");
            QueueUpdatesToEntity(new SpatialOSBlittableComponent.Update
            {
                IntField = new Option<int>(10),
                FloatField = new Option<float>(20.05f)
            });

            LogAssert.Expect(LogType.Exception, "Exception: Int Update Exception: 10");
            LogAssert.Expect(LogType.Exception, "Exception: Int Update Exception 2: 10");
            LogAssert.Expect(LogType.Exception, "Exception: Float Update Exception: 20.05");
            LogAssert.Expect(LogType.Exception, "Exception: Float Update Exception 2: 20.05");
            Assert.DoesNotThrow(() => { Reader.OnComponentUpdate(); },
                "Exceptions that happen within component update callbacks should not propagate to callers.");
            Assert.IsTrue(intUpdateCalled);
            Assert.IsTrue(floatUpdateCalled);
        }
    }
}
