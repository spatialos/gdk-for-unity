using System;
using System.Text.RegularExpressions;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Generated.Improbable.Gdk.Tests.ComponentsWithNoFields;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;
using ComponentWithEvents = Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    [TestFixture]
    internal class EventTests
    {
        private ComponentWithEvents.Accessors.Reader readerPublic;
        private ComponentWithEvents.Accessors.ReaderWriterImpl readerWriterInternal;
        private EntityManager entityManager;
        private Entity entity;
        private World world;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");
            entityManager = world.GetOrCreateManager<EntityManager>();
            entity = entityManager.CreateEntity(ComponentType.Create<SpatialOSBlittableComponent>());
            readerWriterInternal = new ComponentWithEvents.Accessors.ReaderWriterImpl(entity, entityManager, new LoggingDispatcher());
            readerPublic = readerWriterInternal;
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }

        [Test]
        public void Event_callback_is_invoked()
        {
            bool callbackInvoked = false;
            readerPublic.OnEvt += (ev => callbackInvoked = true);
            readerWriterInternal.OnEvtEvent(new EvtEvent());
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
            readerWriterInternal.OnEvtEvent(new EvtEvent());
            LogAssert.Expect(LogType.Exception,
                new Regex(".*Exception propagated from user event callback.*", RegexOptions.Singleline));
            Assert.IsTrue(firstCallbackInvoked);
            Assert.IsTrue(secondCallbackInvoked);
            Assert.IsTrue(thirdCallbackInvoked);
        }
    }
}
