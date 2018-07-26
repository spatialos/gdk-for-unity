using System;
using System.Collections.Generic;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Improbable.Gdk.Core.GameObjectRepresentation;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.EditmodeTests.Readers
{
    public abstract class ReaderWriterTestsBase
    {
        public class SpatialOSBlittableComponentReader : BlittableReaderBase<SpatialOSBlittableComponent,
            SpatialOSBlittableComponent.Update>
        {
            internal SpatialOSBlittableComponentReader(Entity entity, EntityManager entityManager) : base(entity,
                entityManager)
            {
            }

            private readonly List<Action<BlittableBool>> boolFieldDelegates = new List<Action<BlittableBool>>();

            public event Action<BlittableBool> BoolFieldUpdated
            {
                add => boolFieldDelegates.Add(value);
                remove => boolFieldDelegates.Remove(value);
            }

            private readonly List<Action<int>> intFieldDelegates = new List<Action<int>>();

            public event Action<int> IntFieldUpdated
            {
                add => intFieldDelegates.Add(value);
                remove => intFieldDelegates.Remove(value);
            }

            private readonly List<Action<long>> longFieldDelegates = new List<Action<long>>();

            public event Action<long> LongFieldUpdated
            {
                add => longFieldDelegates.Add(value);
                remove => longFieldDelegates.Remove(value);
            }

            private readonly List<Action<float>> floatFieldDelegates = new List<Action<float>>();

            public event Action<float> FloatFieldUpdated
            {
                add => floatFieldDelegates.Add(value);
                remove => floatFieldDelegates.Remove(value);
            }

            private readonly List<Action<double>> doubleFieldDelegates = new List<Action<double>>();

            public event Action<double> DoubleFieldUpdated
            {
                add => doubleFieldDelegates.Add(value);
                remove => doubleFieldDelegates.Remove(value);
            }

            protected override void HandleFieldUpdates(SpatialOSBlittableComponent.Update update)
            {
                if (update.BoolField.HasValue)
                {
                    boolFieldDelegates.ForEach(callback =>
                    {
                        try
                        {
                            callback(update.BoolField.Value);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    });
                }

                if (update.IntField.HasValue)
                {
                    intFieldDelegates.ForEach(callback =>
                    {
                        try
                        {
                            callback(update.IntField.Value);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    });
                }

                if (update.LongField.HasValue)
                {
                    longFieldDelegates.ForEach(callback =>
                    {
                        try
                        {
                            callback(update.LongField.Value);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    });
                }

                if (update.FloatField.HasValue)
                {
                    floatFieldDelegates.ForEach(callback =>
                    {
                        try
                        {
                            callback(update.FloatField.Value);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    });
                }

                if (update.DoubleField.HasValue)
                {
                    doubleFieldDelegates.ForEach(callback =>
                    {
                        try
                        {
                            callback(update.DoubleField.Value);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    });
                }
            }
        }

        protected struct SomeOtherComponent : IComponentData
        {
        }

        private World world;
        protected EntityManager EntityManager;
        protected Entity Entity;
        protected SpatialOSBlittableComponentReader Reader;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");

            EntityManager = world.GetOrCreateManager<EntityManager>();

            Entity = EntityManager.CreateEntity(typeof(SpatialOSBlittableComponent));

            Reader = new SpatialOSBlittableComponentReader(Entity, EntityManager);
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }
    }
}
