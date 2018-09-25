using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.Tests.BlittableTypes;
using Improbable.Worker.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.Readers
{
    [TestFixture]
    internal class ReaderAuthorityTests : ReaderWriterTestsBase
    {
        [Test]
        public void Authority_throws_if_the_entity_has_no_authority_components()
        {
            // entity is fresh so no components on it
            Assert.Throws<AuthorityComponentNotFoundException>(() => { Debug.Log(ReaderPublic.Authority); });
        }

        [Test]
        public void Authority_returns_NotAuthoritative_when_NotAuthoritative_component_is_present()
        {
            EntityManager.AddComponent(Entity, typeof(NotAuthoritative<BlittableComponent.Component>));
            Assert.AreEqual(Authority.NotAuthoritative, ReaderPublic.Authority);
        }

        [Test]
        public void Authority_returns_Authoritative_when_Authoritative_component_is_present()
        {
            EntityManager.AddComponent(Entity, typeof(Authoritative<BlittableComponent.Component>));
            Assert.AreEqual(Authority.Authoritative, ReaderPublic.Authority);
        }

        [Test]
        public void Authority_returns_AuthorityLossImminent_when_AuthorityLossImminent_component_is_present()
        {
            EntityManager.AddComponent(Entity, typeof(AuthorityLossImminent<BlittableComponent.Component>));
            Assert.AreEqual(Authority.AuthorityLossImminent, ReaderPublic.Authority);
        }

        [Test]
        public void
            Authority_returns_AuthorityLossImminent_when_Authoritative_And_AuthorityLossImminent_components_are_present()
        {
            EntityManager.AddComponent(Entity, typeof(AuthorityLossImminent<BlittableComponent.Component>));
            EntityManager.AddComponent(Entity, typeof(Authoritative<BlittableComponent.Component>));
            Assert.AreEqual(Authority.AuthorityLossImminent, ReaderPublic.Authority);
        }

        [Test]
        public void Authority_can_be_NotAuthoritative_if_another_component_is_authoritative()
        {
            EntityManager.AddComponent(Entity, typeof(NotAuthoritative<BlittableComponent.Component>));
            EntityManager.AddComponent(Entity, typeof(Authoritative<SomeOtherComponent>));
            Assert.AreEqual(Authority.NotAuthoritative, ReaderPublic.Authority);
        }

        [Test]
        public void AuthorityChanged_gets_triggered_when_the_reader_receives_an_authority_change()
        {
            var authorityChangedCalled = false;

            ReaderPublic.AuthorityChanged += authority =>
            {
                Assert.AreEqual(Authority.Authoritative, authority);

                authorityChangedCalled = true;
            };

            Assert.AreEqual(false, authorityChangedCalled, "Adding an event should not fire it immediately");

            ReaderWriterInternal.OnAuthorityChange(Authority.Authoritative);

            Assert.AreEqual(true, authorityChangedCalled);
        }

        [Test]
        public void AuthorityChanged_calls_non_failure_callbacks_even_if_some_callbacks_fail()
        {
            var secondAuthorityChangeCalled = false;

            ReaderPublic.AuthorityChanged += authority =>
                throw new Exception("Authority failure: backwards time travel");
            ReaderPublic.AuthorityChanged += authority => { secondAuthorityChangeCalled = true; };
            ReaderPublic.AuthorityChanged += authority =>
                throw new Exception("Authority failure: help I'm stuck in an exception factory");

            LogAssert.Expect(LogType.Exception, "Exception: Authority failure: backwards time travel");
            LogAssert.Expect(LogType.Exception, "Exception: Authority failure: help I'm stuck in an exception factory");

            Assert.DoesNotThrow(() => { ReaderWriterInternal.OnAuthorityChange(Authority.NotAuthoritative); },
                "Exceptions that happen within authority change callbacks should not propagate to callers.");

            Assert.IsTrue(secondAuthorityChangeCalled);
        }
    }
}
