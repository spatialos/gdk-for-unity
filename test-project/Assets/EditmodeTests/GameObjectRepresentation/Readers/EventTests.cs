using System;
using System.Text.RegularExpressions;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.BlittableTypes;
using Improbable.Gdk.Tests.ComponentsWithNoFields;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;
using ComponentWithEvents = Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.Readers
{
    [TestFixture]
    internal class EventTests
    {
        private ComponentWithEvents.Requirable.Reader readerPublic;
        private ComponentWithEvents.Requirable.Writer writerPublic;
        private ComponentWithEvents.Requirable.ReaderWriterImpl readerWriterInternal;
        private EntityManager entityManager;
        private Entity entity;
        private World world;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");
            entityManager = world.GetOrCreateManager<EntityManager>();
            entity = entityManager.CreateEntity(ComponentType.Create<BlittableComponent.Component>());
            readerWriterInternal =
                new ComponentWithEvents.Requirable.ReaderWriterImpl(entity, entityManager, new LoggingDispatcher());
            readerPublic = readerWriterInternal;
            writerPublic = readerWriterInternal;
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }

        [Test]
        public void Event_callback_is_invoked_through_reader()
        {
            bool callbackInvoked = false;
            readerPublic.OnEvt += (ev => callbackInvoked = true);
            readerWriterInternal.OnEvtEvent(new Empty());
            Assert.IsTrue(callbackInvoked);
        }

        [Test]
        public void Event_callback_is_invoked_through_writer()
        {
            bool callbackInvoked = false;
            writerPublic.OnEvt += (ev => callbackInvoked = true);
            readerWriterInternal.OnEvtEvent(new Empty());
            Assert.IsTrue(callbackInvoked);
        }

        [Test]
        public void All_event_callbacks_invoked_if_one_throws_exceptiom()
        {
            bool firstCallbackInvoked = false;
            readerPublic.OnEvt += (ev => firstCallbackInvoked = true);
            bool secondCallbackInvoked = false;
            readerPublic.OnEvt += (ev =>
            {
                secondCallbackInvoked = true;
                throw new Exception("Exception propagated from user event callback");
            });
            bool thirdCallbackInvoked = false;
            readerPublic.OnEvt += (ev => thirdCallbackInvoked = true);
            readerWriterInternal.OnEvtEvent(new Empty());
            LogAssert.Expect(LogType.Exception,
                new Regex(".*Exception propagated from user event callback.*", RegexOptions.Singleline));
            Assert.IsTrue(firstCallbackInvoked);
            Assert.IsTrue(secondCallbackInvoked);
            Assert.IsTrue(thirdCallbackInvoked);
        }
    }
}
