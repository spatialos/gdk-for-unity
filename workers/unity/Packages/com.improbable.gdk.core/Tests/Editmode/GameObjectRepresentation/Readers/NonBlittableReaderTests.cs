using Generated.Improbable.Gdk.Tests.NonblittableTypes;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.EditmodeTests;
using Improbable.Gdk.Core.GameObjectRepresentation;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.Readers
{
    [TestFixture]
    public class NonBlittableReaderTests
    {
        private class SpatialOSNonBlittableComponentReader :
            NonBlittableReaderBase<SpatialOSNonBlittableComponent, SpatialOSNonBlittableComponent.Update>
        {
            public SpatialOSNonBlittableComponentReader(Entity entity, EntityManager entityManager) : base(entity,
                entityManager)
            {
            }
        }

        [Test]
        public void Data_returns_non_blittable_component_instance()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity(typeof(SpatialOSNonBlittableComponent));
                var reader = new SpatialOSNonBlittableComponentReader(entity, entityManager);

                entityManager.SetComponentObject(entity, new SpatialOSNonBlittableComponent
                {
                    StringField = "test string"
                });

                var data = entityManager.GetComponentObject<SpatialOSNonBlittableComponent>(entity);

                Assert.AreEqual(data, reader.Data);
                Assert.AreEqual("test string", reader.Data.StringField);
            }
        }
    }
}
