using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.NonblittableTypes;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.Readers
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
                var entity = entityManager.CreateEntity(typeof(NonBlittableComponent.Component));
                entityManager.SetComponentData(entity, new NonBlittableComponent.Component());
                var writer =
                    new NonBlittableComponent.Requirable.ReaderWriterImpl(entity, entityManager,
                        new LoggingDispatcher());

                writer.Send(new NonBlittableComponent.Update
                {
                    IntField = new Option<int>(42)
                });

                int valueRead = entityManager.GetComponentData<NonBlittableComponent.Component>(entity).IntField;
                Assert.AreEqual(42, valueRead);
            }
        }

        [Test]
        public void Updating_field_leaves_other_field_unchanged()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity(typeof(NonBlittableComponent.Component));

                var schemaComponentData = NonBlittableComponent.Component.CreateSchemaComponentData(
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
                    entityManager.SetComponentData(entity, NonBlittableComponent.Serialization.Deserialize(
                        schemaComponentData.Value.GetFields(), world));
                }

                var writer =
                    new NonBlittableComponent.Requirable.ReaderWriterImpl(entity, entityManager,
                        new LoggingDispatcher());
                writer.Send(new NonBlittableComponent.Update
                {
                    IntField = new Option<int>(42),
                    ListField = new Option<List<int>>(new List<int>
                    {
                        5,
                        6,
                        7
                    })
                });

                double doubleRead = entityManager.GetComponentData<NonBlittableComponent.Component>(entity).DoubleField;
                Assert.AreEqual(13.37, doubleRead);
                string stringRead = entityManager.GetComponentData<NonBlittableComponent.Component>(entity).StringField;
                Assert.AreEqual("stringy", stringRead);
            }
        }
    }
}
