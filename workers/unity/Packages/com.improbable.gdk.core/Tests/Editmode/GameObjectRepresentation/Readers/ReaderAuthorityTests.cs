using System;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Improbable.Gdk.Core.EditmodeTests.Readers
{
    [TestFixture]
    public class ReaderAuthorityTests : ReaderWriterTestsBase
    {
        [Test]
        public void Authority_throws_if_the_entity_has_no_authority_components()
        {
            // entity is fresh so no components on it
            Assert.Throws<Exception>(() => { Debug.Log(Reader.Authority); });
        }

        [Test]
        public void Authority_returns_NotAuthoritative_when_no_authority_components_are_present()
        {
            // TODO, do we need to clean up the components here and in other functions?
            EntityManager.AddComponent(Entity, typeof(NotAuthoritative<SpatialOSBlittableComponent>));
            Assert.AreEqual(Authority.NotAuthoritative, Reader.Authority);
        }

        [Test]
        public void Authority_returns_Authoritative_when_Authuritative_component_is_present()
        {
            EntityManager.AddComponent(Entity, typeof(Authoritative<SpatialOSBlittableComponent>));
            Assert.AreEqual(Authority.Authoritative, Reader.Authority);
        }

        [Test]
        public void Authority_returns_AuthorityLossImminent_when_Authuritative_component_is_present()
        {
            EntityManager.AddComponent(Entity, typeof(AuthorityLossImminent<SpatialOSBlittableComponent>));
            Assert.AreEqual(Authority.AuthorityLossImminent, Reader.Authority);
        }

        [Test]
        public void Authority_can_be_NotAuthoritative_if_another_component_is_authoritative()
        {
            EntityManager.AddComponent(Entity, typeof(NotAuthoritative<SpatialOSBlittableComponent>));
            EntityManager.AddComponent(Entity, typeof(Authoritative<SomeOtherComponent>));
            Assert.AreEqual(Authority.NotAuthoritative, Reader.Authority);
        }

        [Test]
        public void AuthorityChanged_gets_triggered_when_the_reader_receives_an_authority_change()
        {
            var authorityChangedCalled = false;

            Reader.AuthorityChanged += authority =>
            {
                Assert.AreEqual(Authority.Authoritative, authority);

                authorityChangedCalled = true;
            };

            Assert.AreEqual(false, authorityChangedCalled, "Adding an event should not fire it immediately");

            var internalReader = (IReaderInternal) Reader;

            internalReader.OnAuthorityChange(Authority.Authoritative);

            Assert.AreEqual(true, authorityChangedCalled);
        }

        [Test]
        public void AuthorityChanged_calls_non_failure_callbacks_even_if_some_callbacks_fail()
        {
            var secondAuthorityChangeCalled = false;

            Reader.AuthorityChanged += authority => throw new Exception("backwards time travel");
            Reader.AuthorityChanged += authority => { secondAuthorityChangeCalled = true; };
            Reader.AuthorityChanged += authority => throw new Exception("help I'm stuck in an exception factory");

            var internalReader = (IReaderInternal) Reader;

            LogAssert.Expect(LogType.Exception, "Exception: backwards time travel");
            LogAssert.Expect(LogType.Exception, "Exception: help I'm stuck in an exception factory");

            internalReader.OnAuthorityChange(Authority.NotAuthoritative);

            Assert.IsTrue(secondAuthorityChangeCalled);
        }
    }
}
