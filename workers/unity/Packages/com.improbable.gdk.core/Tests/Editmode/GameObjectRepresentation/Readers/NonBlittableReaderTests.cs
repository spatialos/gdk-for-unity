using System;
using System.Collections.Generic;
using Generated.Improbable.Gdk.Tests.NonblittableTypes;
using Improbable.Gdk.Core.GameObjectRepresentation;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
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
                var entity = entityManager.CreateEntity(typeof(SpatialOSNonBlittableComponent));
                var reader = new NonBlittableComponent.ReaderWriterImpl(entity, entityManager, new LoggingDispatcher());

                entityManager.SetComponentObject(entity, new SpatialOSNonBlittableComponent
                {
                    StringField = "test string"
                });

                var data = entityManager.GetComponentObject<SpatialOSNonBlittableComponent>(entity);

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
                var entity = entityManager.CreateEntity(typeof(SpatialOSNonBlittableComponent));
                var reader = new NonBlittableComponent.ReaderWriterImpl(entity, entityManager, new LoggingDispatcher());

                string stringValue = null;
                List<int> listValue = null;

                reader.StringFieldUpdated += newValue => { stringValue = newValue; };
                reader.ListFieldUpdated += newValue => { listValue = newValue; };

                reader.OnComponentUpdate(new SpatialOSNonBlittableComponent.Update
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
