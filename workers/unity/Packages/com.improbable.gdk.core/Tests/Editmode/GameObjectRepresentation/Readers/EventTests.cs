using System;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Generated.Improbable.Gdk.Tests.ComponentsWithNoFields;
using NUnit.Framework;
using Unity.Entities;
using ComponentWithEvents = Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    [TestFixture]
    internal class EventTests
    {
        protected ComponentWithEvents.Reader ReaderPublic;
        protected ComponentWithEvents.ReaderWriterImpl ReaderWriterInternal;
        protected EntityManager EntityManager;
        protected Entity Entity;
        private World world;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");
            EntityManager = world.GetOrCreateManager<EntityManager>();
            Entity = EntityManager.CreateEntity(typeof(SpatialOSBlittableComponent));
            ReaderWriterInternal = new ComponentWithEvents.ReaderWriterImpl(Entity, EntityManager, new LoggingDispatcher());
            ReaderPublic = ReaderWriterInternal;
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
            ReaderPublic.OnEvt += (ev => callbackInvoked = true);
            ReaderWriterInternal.OnEvtEvent(new EvtEvent());
            Assert.IsTrue(callbackInvoked);
        }

        [Test]
        public void All_event_callbacks_invoked_if_one_throws_exceptiom()
        {
            bool firstCallbackInvoked = false;
            ReaderPublic.OnEvt += (ev => firstCallbackInvoked = true);
            ReaderWriterInternal.OnEvtEvent(new EvtEvent());
            bool secondCallbackInvoked = false;
            ReaderPublic.OnEvt += (ev =>
            {
                secondCallbackInvoked = true;
                throw new Exception("no u");
            });
            ReaderWriterInternal.OnEvtEvent(new EvtEvent());
            bool thirdCallbackInvoked = false;
            ReaderPublic.OnEvt += (ev => thirdCallbackInvoked = true);
            ReaderWriterInternal.OnEvtEvent(new EvtEvent());
            Assert.IsTrue(firstCallbackInvoked);
            Assert.IsTrue(secondCallbackInvoked);
            Assert.IsTrue(thirdCallbackInvoked);
        }
    }
}
