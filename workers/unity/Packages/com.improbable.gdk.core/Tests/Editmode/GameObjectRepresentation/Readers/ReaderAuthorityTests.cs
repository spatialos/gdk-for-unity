using System;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Gdk.Core.MonoBehaviours;
using Improbable.Worker;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    [TestFixture]
    internal class ReaderAuthorityTests : ReaderWriterTestsBase
    {
        [Test]
        public void Authority_throws_if_the_entity_has_no_authority_components()
        {
            // entity is fresh so no components on it
            Assert.Throws<AuthorityComponentNotFoundException>(() => { Debug.Log(Reader.Authority); });
        }

        [Test]
        public void Authority_returns_NotAuthoritative_when_NotAuthoritative_component_is_present()
        {
            EntityManager.AddComponent(Entity, typeof(NotAuthoritative<SpatialOSBlittableComponent>));
            Assert.AreEqual(Authority.NotAuthoritative, Reader.Authority);
        }

        [Test]
        public void Authority_returns_Authoritative_when_Authoritative_component_is_present()
        {
            EntityManager.AddComponent(Entity, typeof(Authoritative<SpatialOSBlittableComponent>));
            Assert.AreEqual(Authority.Authoritative, Reader.Authority);
        }

        [Test]
        public void Authority_returns_AuthorityLossImminent_when_AuthorityLossImminent_component_is_present()
        {
            EntityManager.AddComponent(Entity, typeof(AuthorityLossImminent<SpatialOSBlittableComponent>));
            Assert.AreEqual(Authority.AuthorityLossImminent, Reader.Authority);
        }

        [Test]
        public void Authority_returns_AuthorityLossImminent_when_Authoritative_And_AuthorityLossImminent_components_are_present()
        {
            EntityManager.AddComponent(Entity, typeof(AuthorityLossImminent<SpatialOSBlittableComponent>));
            EntityManager.AddComponent(Entity, typeof(Authoritative<SpatialOSBlittableComponent>));
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

            Reader.OnAuthorityChange(Authority.Authoritative);

            Assert.AreEqual(true, authorityChangedCalled);
        }

        [Test]
        public void AuthorityChanged_calls_non_failure_callbacks_even_if_some_callbacks_fail()
        {
            var secondAuthorityChangeCalled = false;

            Reader.AuthorityChanged += authority =>
                throw new Exception("Authority failure: backwards time travel");
            Reader.AuthorityChanged += authority => { secondAuthorityChangeCalled = true; };
            Reader.AuthorityChanged += authority =>
                throw new Exception("Authority failure: help I'm stuck in an exception factory");

            LogAssert.Expect(LogType.Exception, "Exception: Authority failure: backwards time travel");
            LogAssert.Expect(LogType.Exception, "Exception: Authority failure: help I'm stuck in an exception factory");

            Assert.DoesNotThrow(() => { Reader.OnAuthorityChange(Authority.NotAuthoritative); },
                "Exceptions that happen within authority change callbacks should not propagate to callers.");

            Assert.IsTrue(secondAuthorityChangeCalled);
        }
    }
}
