using System;
using System.Collections.Generic;
using Improbable.Worker;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.EditmodeTests.Readers
{
    [TestFixture]
    public class ReaderTests
    {
        private struct ReaderTestComponent : ISpatialComponentData, IComponentData
        {
            public BlittableBool DirtyBit { get; set; }

            internal class Reader : BlittableReaderBase<ReaderTestComponent, ReaderTestComponent.Update>
            {
                public Reader(Entity entity, EntityManager entityManager) : base(entity, entityManager)
                {
                }
            }

            internal class Update : ISpatialComponentUpdate<ReaderTestComponent>
            {
            }
        }

        private struct SomeOtherComponent : IComponentData
        {
        }

        private World world;
        private EntityManager entityManager;
        private Entity entity;
        private ReaderTestComponent.Reader reader;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");

            entityManager = world.GetOrCreateManager<EntityManager>();

            entity = entityManager.CreateEntity(typeof(ReaderTestComponent));

            reader = new ReaderTestComponent.Reader(entity, entityManager);
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }

        [Test]
        public void Data_returns_component_instance()
        {
            entityManager.SetComponentData(entity, new ReaderTestComponent());

            var data = entityManager.GetComponentData<ReaderTestComponent>(entity);

            Assert.AreEqual(data, reader.Data);
        }

        [Test]
        public void Authority_throws_if_the_entity_has_no_authority_components()
        {
            // entity is fresh so no components on it
            Assert.Throws<Exception>(() => { Debug.Log(reader.Authority); });
        }

        [Test]
        public void Authority_returns_NotAuthoritative_when_no_authority_components_are_present()
        {
            entityManager.AddComponent(entity, typeof(NotAuthoritative<ReaderTestComponent>));
            Assert.AreEqual(Authority.NotAuthoritative, reader.Authority);
        }

        [Test]
        public void Authority_returns_Authoritative_when_Authuritative_component_is_present()
        {
            entityManager.AddComponent(entity, typeof(Authoritative<ReaderTestComponent>));
            Assert.AreEqual(Authority.Authoritative, reader.Authority);
        }

        [Test]
        public void Authority_returns_AuthorityLossImminent_when_Authuritative_component_is_present()
        {
            entityManager.AddComponent(entity, typeof(AuthorityLossImminent<ReaderTestComponent>));
            Assert.AreEqual(Authority.AuthorityLossImminent, reader.Authority);
        }

        [Test]
        public void Authority_can_be_NotAuthoritative_if_another_component_is_authoritative()
        {
            entityManager.AddComponent(entity, typeof(NotAuthoritative<ReaderTestComponent>));
            entityManager.AddComponent(entity, typeof(Authoritative<SomeOtherComponent>));
            Assert.AreEqual(Authority.NotAuthoritative, reader.Authority);
        }

        [Test]
        public void AuthorityChanged_gets_triggered_when_the_reader_receives_an_authority_change()
        {
            var authorityChangedCalled = false;

            reader.AuthorityChanged += authority =>
            {
                Assert.AreEqual(Authority.Authoritative, authority);

                authorityChangedCalled = true;
            };

            Assert.AreEqual(false, authorityChangedCalled, "Adding an event should not fire it immediately");

            var internalReader = (IReaderInternal) reader;

            internalReader.OnAuthorityChange(Authority.Authoritative);

            Assert.AreEqual(true, authorityChangedCalled);
        }

        [Test]
        public void AuthorityChanged_calls_non_failure_callbacks_even_if_some_callbacks_fail()
        {
            var secondAuthorityChangeCalled = false;

            reader.AuthorityChanged += authority => throw new Exception("backwards time travel");
            reader.AuthorityChanged += authority => { secondAuthorityChangeCalled = true; };
            reader.AuthorityChanged += authority => throw new Exception("help I'm stuck in an exception factory");

            QueueUpdatesToEntity(new ReaderTestComponent.Update());

            var internalReader = (IReaderInternal) reader;

            LogAssert.Expect(LogType.Exception, "Exception: backwards time travel");
            LogAssert.Expect(LogType.Exception, "Exception: help I'm stuck in an exception factory");

            internalReader.OnAuthorityChange(Authority.NotAuthoritative);

            Assert.IsTrue(secondAuthorityChangeCalled);
        }

        private void QueueUpdatesToEntity(params ReaderTestComponent.Update[] updatesToSend)
        {
            entityManager.AddComponent(entity, typeof(ComponentsUpdated<ReaderTestComponent.Update>));

            var componentsUpdated = new ComponentsUpdated<ReaderTestComponent.Update>();

            componentsUpdated.Buffer.AddRange(updatesToSend);

            entityManager.SetComponentObject(entity, componentsUpdated);
        }

        [Test]
        public void ComponentUpdated_gets_triggered_when_the_reader_receives_an_update()
        {
            var componentUpdated = false;

            var updateToSend = new ReaderTestComponent.Update();

            reader.ComponentUpdated += update =>
            {
                Assert.AreEqual(updateToSend, update);

                componentUpdated = true;
            };

            Assert.AreEqual(false, componentUpdated, "Adding an event callback should not fire it immediately");

            var internalReader = (IReaderInternal) reader;

            QueueUpdatesToEntity(updateToSend);

            internalReader.OnComponentUpdate();

            Assert.AreEqual(true, componentUpdated);
        }

        [Test]
        public void ComponentUpdated_gets_triggered_for_multiple_updates()
        {
            var firstUpdate = new ReaderTestComponent.Update();
            var secondUpdate = new ReaderTestComponent.Update();
            var thirdUpdate = new ReaderTestComponent.Update();

            var updatesToSend = new[]
            {
                firstUpdate,
                secondUpdate,
                thirdUpdate
            };

            var nextUpdateIndex = 0;

            reader.ComponentUpdated += update =>
            {
                Assert.AreEqual(updatesToSend[nextUpdateIndex], update,
                    $"The update at index {nextUpdateIndex} did not match the received update.");

                nextUpdateIndex++;
            };

            Assert.AreEqual(0, nextUpdateIndex, "Adding an event callback should not fire it immediately");

            var internalReader = (IReaderInternal) reader;

            QueueUpdatesToEntity(updatesToSend);

            internalReader.OnComponentUpdate();

            Assert.AreEqual(3, nextUpdateIndex);
        }

        [Test]
        public void ComponentUpdated_calls_non_failure_callbacks_even_if_some_callbacks_fail()
        {
            bool secondUpdateCalled = false;

            reader.ComponentUpdated += update => throw new Exception("divide by zero");
            reader.ComponentUpdated += update => { secondUpdateCalled = true; };
            reader.ComponentUpdated += update => throw new Exception("this statement is false");

            QueueUpdatesToEntity(new ReaderTestComponent.Update());

            var internalReader = (IReaderInternal) reader;

            LogAssert.Expect(LogType.Exception, "Exception: divide by zero");
            LogAssert.Expect(LogType.Exception, "Exception: this statement is false");

            internalReader.OnComponentUpdate();

            Assert.IsTrue(secondUpdateCalled);
        }
    }
}
