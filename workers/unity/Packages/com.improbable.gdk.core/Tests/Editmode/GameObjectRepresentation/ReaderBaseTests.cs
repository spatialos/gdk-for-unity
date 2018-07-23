using System;
using Improbable.Gdk.Core.MonoBehaviours;
using Improbable.Worker;
using NUnit.Framework;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class ReaderBaseTests
    {
        private struct SomeOtherComponent : IComponentData
        {
        }

        private struct TestComponentData : IComponentData, ISpatialComponentData
        {
            public BlittableBool DirtyBit { get; set; }


            public struct Update : ISpatialComponentUpdate<TestComponentData>
            {
                public Option<float> Horizontal;
                public Option<float> Vertical;
                public Option<BlittableBool> Running;
            }

            public class Reader : ReaderBase<TestComponentData, Update>
            {
                public Reader(Entity entity, EntityManager manager) : base(entity, manager)
                {
                }
            }
        }

        private World world;
        private EntityManager entityManager;
        private Entity entity;
        private TestComponentData.Reader reader;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");

            entityManager = world.GetOrCreateManager<EntityManager>();

            entity = entityManager.CreateEntity(typeof(TestComponentData));

            reader = new TestComponentData.Reader(entity, entityManager);
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }

        [Test]
        public void Authority_returns_NotAuthoritative_when_no_authority_components_are_present()
        {
            Assert.AreEqual(Authority.NotAuthoritative, reader.Authority);
        }

        [Test]
        [TestCase(typeof(Authoritative<SomeOtherComponent>), Authority.NotAuthoritative)]
        [TestCase(typeof(Authoritative<TestComponentData>), Authority.Authoritative)]
        [TestCase(typeof(AuthorityLossImminent<TestComponentData>), Authority.AuthorityLossImminent)]
        public void Authority_returns_value_when_components_are_added(Type componentToAdd, Authority authority)
        {
            entityManager.AddComponent(entity, componentToAdd);
            Assert.AreEqual(authority, reader.Authority);
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

            var internalReader = (IReaderInternal<TestComponentData, TestComponentData.Update>) reader;

            internalReader.OnAuthorityChange(Authority.Authoritative);

            Assert.AreEqual(true, authorityChangedCalled);
        }

        [Test]
        public void ComponentUpdated_gets_triggered_when_the_reader_receives_an_update()
        {
            var componentUpdated = false;

            var updateToSend = new TestComponentData.Update
            {
                Horizontal = new Option<float>(5.0f),
                Running = new Option<BlittableBool>(),
                Vertical = new Option<float>()
            };

            reader.ComponentUpdated += update =>
            {
                Assert.AreEqual(updateToSend, update);

                componentUpdated = true;
            };

            Assert.AreEqual(false, componentUpdated, "Adding an event should not fire it immediately");

            var internalReader = (IReaderInternal<TestComponentData, TestComponentData.Update>) reader;

            internalReader.OnComponentUpdate(updateToSend);

            Assert.AreEqual(true, componentUpdated);
        }
    }
}
