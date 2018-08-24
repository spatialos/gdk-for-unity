using System.Collections.Generic;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Generated.Improbable.Gdk.Tests.NonblittableTypes;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    [TestFixture]
    public class NonBlittableWriterTests
    {
        [Test]
        public void Sending_component_update_changes_field()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity(typeof(SpatialOSNonBlittableComponent));
                entityManager.SetComponentData(entity, new SpatialOSNonBlittableComponent());
                var writer = new NonBlittableComponent.Requirables.ReaderWriterImpl(entity, entityManager, new LoggingDispatcher());

                writer.Send(new SpatialOSNonBlittableComponent.Update
                {
                    IntField = new Option<int>(42)
                });

                int valueRead = entityManager.GetComponentData<SpatialOSNonBlittableComponent>(entity).IntField;
                Assert.AreEqual(42, valueRead);
            }
        }

        [Test]
        public void Updating_field_leaves_other_field_unchanged()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity(typeof(SpatialOSNonBlittableComponent));

                var schemaComponentData = SpatialOSNonBlittableComponent.CreateSchemaComponentData(
                    boolField: false,
                    intField: 0,
                    longField: 0,
                    floatField: 0,
                    doubleField: 13.37,
                    stringField: "stringy",
                    optionalField: null,
                    listField: new List<int>(),
                    mapField: new Dictionary<int, string>()).SchemaData;

                if (schemaComponentData != null)
                {
                    entityManager.SetComponentData(entity, SpatialOSNonBlittableComponent.Serialization.Deserialize(
                        schemaComponentData.Value.GetFields(), world));
                }

                var writer = new NonBlittableComponent.Requirables.ReaderWriterImpl(entity, entityManager, new LoggingDispatcher());
                writer.Send(new SpatialOSNonBlittableComponent.Update
                {
                    IntField = new Option<int>(42),
                    ListField = new Option<List<int>>(new List<int>
                    {
                        5,
                        6,
                        7
                    })
                });

                double doubleRead = entityManager.GetComponentData<SpatialOSNonBlittableComponent>(entity).DoubleField;
                Assert.AreEqual(13.37, doubleRead);
                string stringRead = entityManager.GetComponentData<SpatialOSNonBlittableComponent>(entity).StringField;
                Assert.AreEqual("stringy", stringRead);
            }
        }
    }
}
