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
            // entity is fresh so no components on it
            Assert.AreEqual(Authority.NotAuthoritative, reader.Authority);
        }

        [Test]
        public void Authority_returns_Authoritative_when_Authuritative_component_is_present()
        {
            entityManager.AddComponent(entity, typeof(Authoritative<TestComponentData>));
            Assert.AreEqual(Authority.Authoritative, reader.Authority);
        }

        [Test]
        public void Authority_returns_AuthorityLossImminent_when_Authuritative_component_is_present()
        {
            entityManager.AddComponent(entity, typeof(AuthorityLossImminent<TestComponentData>));
            Assert.AreEqual(Authority.AuthorityLossImminent, reader.Authority);
        }

        [Test]
        public void Authority_can_be_NotAuthoritative_if_another_component_is_authoritative()
        {
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

            var internalReader = (IReaderInternal) reader;

            entityManager.AddComponent(entity, typeof(ComponentsUpdated<TestComponentData.Update>));

            var componentsUpdated = new ComponentsUpdated<TestComponentData.Update>();

            componentsUpdated.Buffer.Add(updateToSend);

            entityManager.SetComponentObject(entity, componentsUpdated);

            internalReader.OnComponentUpdate();

            Assert.AreEqual(true, componentUpdated);
        }
    }
}
