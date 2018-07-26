using Improbable.Gdk.Core;
using Improbable.Gdk.Core.EditmodeTests;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.Readers
{
    [TestFixture]
    public class NonBlittableReaderTests
    {
        private class NonBlittableTestComponent : Component, ISpatialComponentData
        {
            public BlittableBool DirtyBit { get; set; }

            public string StringValue;

            public struct Update : ISpatialComponentUpdate<NonBlittableTestComponent>
            {
                public Option<string> StringValue;
            }

            public class Reader : NonBlittableReaderBase<NonBlittableTestComponent, Update>
            {
                public Reader(Entity entity, EntityManager entityManager) : base(entity, entityManager)
                {
                }
            }
        }

        [Test]
        public void Data_returns_non_blittable_component_instance()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();

                var entity = entityManager.CreateEntity(typeof(NonBlittableTestComponent));

                var reader = new NonBlittableTestComponent.Reader(entity, entityManager);

                entityManager.SetComponentObject(entity, new NonBlittableTestComponent
                {
                    StringValue = "test string"
                });

                var data = entityManager.GetComponentObject<NonBlittableTestComponent>(entity);

                Assert.AreEqual(data, reader.Data);
                Assert.AreEqual("test string", reader.Data.StringValue);
            }
        }
    }
}
