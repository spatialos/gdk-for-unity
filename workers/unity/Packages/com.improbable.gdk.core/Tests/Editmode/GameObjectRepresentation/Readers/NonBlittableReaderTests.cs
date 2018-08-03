using System;
using System.Collections.Generic;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Generated.Improbable.Gdk.Tests.NonblittableTypes;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.EditmodeTests;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Gdk.Core.MonoBehaviours;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    [TestFixture]
    public class NonBlittableReaderTests
    {
        private class SpatialOSNonBlittableComponentReader :
            NonBlittableReaderBase<SpatialOSNonBlittableComponent, SpatialOSNonBlittableComponent.Update>
        {
            public SpatialOSNonBlittableComponentReader(Entity entity,
                EntityManager entityManager,
                ILogDispatcher logDispatcher)
                : base(entity, entityManager, logDispatcher)
            {
            }

            private readonly List<Action<string>> stringFieldDelegates = new List<Action<string>>();

            public event Action<string> StringFieldUpdated
            {
                add => stringFieldDelegates.Add(value);
                remove => stringFieldDelegates.Remove(value);
            }

            private readonly List<Action<List<int>>> listFieldDelegates = new List<Action<List<int>>>();

            public event Action<List<int>> ListFieldUpdates
            {
                add => listFieldDelegates.Add(value);
                remove => listFieldDelegates.Remove(value);
            }

            protected override void TriggerFieldCallbacks(SpatialOSNonBlittableComponent.Update update)
            {
                DispatchWithErrorHandling(update.StringField, stringFieldDelegates);
                DispatchWithErrorHandling(update.ListField, listFieldDelegates);
            }
        }

        [Test]
        public void Data_returns_non_blittable_component_instance()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity(typeof(SpatialOSNonBlittableComponent));
                var reader = new SpatialOSNonBlittableComponentReader(entity, entityManager, new LoggingDispatcher());

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
                var reader = new SpatialOSNonBlittableComponentReader(entity, entityManager, new LoggingDispatcher());

                string stringValue = null;
                List<int> listValue = null;

                reader.StringFieldUpdated += newValue => { stringValue = newValue; };
                reader.ListFieldUpdates += newValue => { listValue = newValue; };

                ReaderWriterTestsBase.QueueUpdatesToEntity(entityManager, entity, new SpatialOSNonBlittableComponent.Update
                {
                    StringField = new Option<string>("new string"),
                    ListField = new Option<List<int>>(new List<int>
                    {
                        5,
                        6,
                        7
                    })
                });

                reader.OnComponentUpdate();

                Assert.AreEqual("new string", stringValue);
                Assert.AreEqual(new List<int> { 5, 6, 7 }, listValue);
            }
        }
    }
}
