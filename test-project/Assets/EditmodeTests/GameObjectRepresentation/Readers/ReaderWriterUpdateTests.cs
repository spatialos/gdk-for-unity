using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.BlittableTypes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.Readers
{
    [TestFixture]
    internal class ReaderWriterUpdateTests : ReaderWriterTestsBase
    {
        [Test]
        public void ComponentUpdated_gets_triggered_when_the_reader_receives_an_update()
        {
            var componentUpdated = false;
            var updateToQueue = new BlittableComponent.Update();
            ReaderPublic.ComponentUpdated += update =>
            {
                Assert.AreEqual(updateToQueue, update);
                componentUpdated = true;
            };

            Assert.IsFalse(componentUpdated, "Adding an event callback should not fire it immediately");
            ReaderWriterInternal.OnComponentUpdate(new BlittableComponent.Update());
            Assert.IsTrue(componentUpdated);
        }

        [Test]
        public void ComponentUpdated_gets_triggered_for_multiple_updates()
        {
            var updatesToQueue = new[]
            {
                new BlittableComponent.Update(),
                new BlittableComponent.Update(),
                new BlittableComponent.Update()
            };
            var updatesReceived = 0;
            ReaderPublic.ComponentUpdated += update =>
            {
                Assert.AreEqual(updatesToQueue[updatesReceived], update,
                    $"The update at index {updatesReceived} did not match the received update.");

                updatesReceived++;
            };

            Assert.AreEqual(0, updatesReceived, "Adding an event callback should not fire it immediately");
            foreach (var update in updatesToQueue)
            {
                ReaderWriterInternal.OnComponentUpdate(update);
            }

            Assert.AreEqual(3, updatesReceived);
        }

        [Test]
        public void ComponentUpdated_calls_all_callbacks_even_if_some_callbacks_fail()
        {
            bool secondUpdateCalled = false;
            ReaderPublic.ComponentUpdated += update => throw new Exception("Update Exception: divide by zero");
            ReaderPublic.ComponentUpdated += update => { secondUpdateCalled = true; };
            ReaderPublic.ComponentUpdated += update => throw new Exception("Update Exception: this statement is false");

            LogAssert.Expect(LogType.Exception, "Exception: Update Exception: divide by zero");
            LogAssert.Expect(LogType.Exception, "Exception: Update Exception: this statement is false");
            Assert.DoesNotThrow(() => { ReaderWriterInternal.OnComponentUpdate(new BlittableComponent.Update()); },
                "Exceptions that happen within component update callbacks should not propagate to callers.");
            Assert.IsTrue(secondUpdateCalled);
        }

        [Test]
        public void FieldUpdates_get_called_if_a_field_changes()
        {
            bool floatFieldUpdated = false;
            float receivedFloatValue = 0;
            ReaderPublic.FloatFieldUpdated += newValue =>
            {
                floatFieldUpdated = true;
                receivedFloatValue = newValue;
            };
            ReaderWriterInternal.OnComponentUpdate(new BlittableComponent.Update
            {
                FloatField = new Option<float>(10.0f),
            });

            Assert.IsTrue(floatFieldUpdated,
                "The update contains a float field but the callback for the float field was not called.");
            Assert.AreEqual(10.0f, receivedFloatValue);
        }

        [Test]
        public void FieldUpdates_get_called_through_writer()
        {
            bool floatFieldUpdated = false;
            float receivedFloatValue = 0;
            WriterPublic.FloatFieldUpdated += newValue =>
            {
                floatFieldUpdated = true;
                receivedFloatValue = newValue;
            };
            ReaderWriterInternal.OnComponentUpdate(new BlittableComponent.Update
            {
                FloatField = new Option<float>(10.0f),
            });

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
            ReaderPublic.FloatFieldUpdated += newValue =>
            {
                floatFieldUpdated = true;
                receivedFloatValue = newValue;
            };
            ReaderPublic.IntFieldUpdated += newValue =>
            {
                intFieldUpdated = true;
                receivedIntValue = newValue;
            };
            ReaderWriterInternal.OnComponentUpdate(new BlittableComponent.Update
            {
                FloatField = new Option<float>(10.0f),
            });

            Assert.IsTrue(floatFieldUpdated,
                "The update contains a float field but the callback for the float field was not called.");
            Assert.AreEqual(10.0f, receivedFloatValue);
            Assert.IsFalse(intFieldUpdated,
                "The update does not contain an int field but the callback for the int field was called. ");

            floatFieldUpdated = false;
            ReaderWriterInternal.OnComponentUpdate(new BlittableComponent.Update
            {
                IntField = new Option<int>(20),
            });

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
            ReaderPublic.FloatFieldUpdated += newValue =>
            {
                floatFieldUpdated = true;
                receivedFloatValue = newValue;
            };
            ReaderPublic.IntFieldUpdated += newValue =>
            {
                intFieldUpdated = true;
                receivedIntValue = newValue;
            };
            ReaderWriterInternal.OnComponentUpdate(new BlittableComponent.Update
            {
                IntField = new Option<int>(30),
                FloatField = new Option<float>(40.0f)
            });

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
            ReaderPublic.IntFieldUpdated += newValue => throw new Exception($"Int Update Exception: {newValue}");
            ReaderPublic.IntFieldUpdated += newValue => { intUpdateCalled = true; };
            ReaderPublic.IntFieldUpdated += newValue => throw new Exception($"Int Update Exception 2: {newValue}");
            ReaderPublic.FloatFieldUpdated += newValue => throw new Exception($"Float Update Exception: {newValue:F2}");
            ReaderPublic.FloatFieldUpdated += newValue => { floatUpdateCalled = true; };
            ReaderPublic.FloatFieldUpdated +=
                newValue => throw new Exception($"Float Update Exception 2: {newValue:F2}");

            LogAssert.Expect(LogType.Exception, "Exception: Int Update Exception: 10");
            LogAssert.Expect(LogType.Exception, "Exception: Int Update Exception 2: 10");
            LogAssert.Expect(LogType.Exception, "Exception: Float Update Exception: 20.05");
            LogAssert.Expect(LogType.Exception, "Exception: Float Update Exception 2: 20.05");
            Assert.DoesNotThrow(() =>
                {
                    ReaderWriterInternal.OnComponentUpdate(new BlittableComponent.Update
                    {
                        IntField = new Option<int>(10),
                        FloatField = new Option<float>(20.05f)
                    });
                },
                "Exceptions that happen within component update callbacks should not propagate to callers.");
            Assert.IsTrue(intUpdateCalled);
            Assert.IsTrue(floatUpdateCalled);
        }
    }
}
