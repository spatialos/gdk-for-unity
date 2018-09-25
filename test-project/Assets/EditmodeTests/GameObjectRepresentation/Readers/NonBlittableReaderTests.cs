using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.NonblittableTypes;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.Readers
{
    [TestFixture]
    public class NonBlittableReaderTests
    {
        [Test]
        public void Data_returns_non_blittable_component_instance()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity(typeof(NonBlittableComponent.Component));
                var reader =
                    new NonBlittableComponent.Requirable.ReaderWriterImpl(entity, entityManager,
                        new LoggingDispatcher());

                var schemaComponentData = NonBlittableComponent.Component.CreateSchemaComponentData(
                    boolField: false,
                    intField: 0,
                    longField: 0,
                    floatField: 0,
                    doubleField: 0,
                    stringField: "test string",
                    optionalField: null,
                    listField: new List<int>(),
                    mapField: new Dictionary<int, string>()).SchemaData;

                if (schemaComponentData != null)
                {
                    entityManager.SetComponentData(entity, NonBlittableComponent.Serialization.Deserialize(
                        schemaComponentData.Value.GetFields(), world));
                }

                var data = entityManager.GetComponentData<NonBlittableComponent.Component>(entity);

                Assert.AreEqual(data, reader.Data);
                Assert.AreEqual("test string", reader.Data.StringField);
            }
        }

        [Test]
        public void Field_updates_get_called_for_non_blittable_fields()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity(typeof(NonBlittableComponent.Component));
                var reader =
                    new NonBlittableComponent.Requirable.ReaderWriterImpl(entity, entityManager,
                        new LoggingDispatcher());

                string stringValue = null;
                List<int> listValue = null;

                reader.StringFieldUpdated += newValue => { stringValue = newValue; };
                reader.ListFieldUpdated += newValue => { listValue = newValue; };

                reader.OnComponentUpdate(new NonBlittableComponent.Update
                {
                    StringField = new Option<string>("new string"),
                    ListField = new Option<List<int>>(new List<int>
                    {
                        5,
                        6,
                        7
                    })
                });

                Assert.AreEqual("new string", stringValue);
                Assert.AreEqual(new List<int> { 5, 6, 7 }, listValue);
            }
        }
    }
}
