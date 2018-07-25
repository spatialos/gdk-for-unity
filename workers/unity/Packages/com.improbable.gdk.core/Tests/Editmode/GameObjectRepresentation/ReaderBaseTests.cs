using System;
using Improbable.Worker;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.EditmodeTests
{
    public class ReaderBaseTests
    {
        [UsedImplicitly]
        public class TestComponent : Component, ISpatialComponentData
        {
            public BlittableBool DirtyBit { get; set; }

            public struct Update : ISpatialComponentUpdate<TestComponent>
            {
                public Option<float> Horizontal;
                public Option<float> Vertical;
                public Option<BlittableBool> Running;
            }

            public class Reader : NonBlittableReaderBase<TestComponent, Update>
            {
                public Reader(Entity entity, EntityManager entityManager) : base(entity, entityManager)
                {
                }
            }
        }

        public struct TestComponentData : IComponentData, ISpatialComponentData
        {
            public BlittableBool DirtyBit { get; set; }

            public struct Update : ISpatialComponentUpdate<TestComponentData>
            {
                public Option<float> Horizontal;
                public Option<float> Vertical;
                public Option<BlittableBool> Running;
            }

            public class Reader : BlittableReaderBase<TestComponentData, Update>
            {
                public Reader(Entity entity, EntityManager entityManager) : base(entity, entityManager)
                {
                }
            }
        }

        [TestFixture]
        public class Blittable : ReaderBaseTestsBase<
            TestComponentData,
            TestComponentData.Update,
            TestComponentData.Reader>
        {
            protected override TestComponentData.Reader CreateReader(Entity entity, EntityManager entityManager)
            {
                return new TestComponentData.Reader(entity, entityManager);
            }

            protected override TestComponentData CreateDataForEntity(Entity entity, EntityManager entityManager)
            {
                entityManager.SetComponentData(entity, new TestComponentData());

                return entityManager.GetComponentData<TestComponentData>(entity);
            }

            protected override TestComponentData.Update CreateUpdate()
            {
                return new TestComponentData.Update
                {
                    Horizontal = new Option<float>(4)
                };
            }
        }

        [TestFixture]
        public class NonBlittable : ReaderBaseTestsBase<
            TestComponent,
            TestComponent.Update,
            TestComponent.Reader>
        {
            protected override TestComponent.Reader CreateReader(Entity entity, EntityManager entityManager)
            {
                return new TestComponent.Reader(entity, entityManager);
            }

            protected override TestComponent CreateDataForEntity(Entity entity, EntityManager entityManager)
            {
                entityManager.SetComponentObject(entity, new TestComponent());

                return entityManager.GetComponentObject<TestComponent>(entity);
            }

            protected override TestComponent.Update CreateUpdate()
            {
                return new TestComponent.Update()
                {
                    Vertical = new Option<float>(20)
                };
            }
        }
    }

    public abstract class ReaderBaseTestsBase<TComponent, TUpdate, TReader>
        where TReader : IReader<TComponent>
        where TComponent : ISpatialComponentData
        where TUpdate : ISpatialComponentUpdate
    {
        private World world;
        private EntityManager entityManager;
        private Entity entity;
        private TReader reader;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");

            entityManager = world.GetOrCreateManager<EntityManager>();

            entity = entityManager.CreateEntity(typeof(TComponent));

            reader = CreateReader(entity, entityManager);
        }

        protected abstract TComponent CreateDataForEntity(Entity entity, EntityManager entityManager);
        protected abstract TUpdate CreateUpdate();
        protected abstract TReader CreateReader(Entity entity, EntityManager entityManager);

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
            entityManager.AddComponent(entity, typeof(Authoritative<TComponent>));
            Assert.AreEqual(Authority.Authoritative, reader.Authority);
        }

        [Test]
        public void Authority_returns_AuthorityLossImminent_when_Authuritative_component_is_present()
        {
            entityManager.AddComponent(entity, typeof(AuthorityLossImminent<TComponent>));
            Assert.AreEqual(Authority.AuthorityLossImminent, reader.Authority);
        }

        private struct SomeOtherComponent : IComponentData
        {
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

            var updateToSend = CreateUpdate();

            reader.ComponentUpdated += update =>
            {
                Assert.AreEqual(updateToSend, update);

                componentUpdated = true;
            };

            Assert.AreEqual(false, componentUpdated, "Adding an event should not fire it immediately");

            var internalReader = (IReaderInternal) reader;

            entityManager.AddComponent(entity, typeof(ComponentsUpdated<TUpdate>));

            var componentsUpdated = new ComponentsUpdated<TUpdate>();

            componentsUpdated.Buffer.Add(updateToSend);

            entityManager.SetComponentObject(entity, componentsUpdated);

            internalReader.OnComponentUpdate();

            Assert.AreEqual(true, componentUpdated);
        }

        [Test]
        public void Data_returns_data_component()
        {
            var data = CreateDataForEntity(entity, entityManager);

            Assert.AreEqual(data, reader.Data);
        }
    }
}
