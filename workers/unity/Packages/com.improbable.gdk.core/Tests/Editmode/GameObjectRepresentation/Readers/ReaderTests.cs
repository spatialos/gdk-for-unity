using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.EditmodeTests.Readers
{
    public abstract class ReaderTestsBase
    {
        protected struct ReaderTestComponent : ISpatialComponentData, IComponentData
        {
            public BlittableBool DirtyBit { get; set; }

            public float FloatValue;
            public float IntValue;

            public class Reader : BlittableReaderBase<ReaderTestComponent, ReaderTestComponent.Update>
            {
                public Reader(Entity entity, EntityManager entityManager) : base(entity, entityManager)
                {
                }

                private readonly List<Action<float>> floatValueDelegates = new List<Action<float>>();

                public event Action<float> FloatValueUpdated
                {
                    add => floatValueDelegates.Add(value);

                    remove => floatValueDelegates.Remove(value);
                }

                private readonly List<Action<int>> intValueDelegates = new List<Action<int>>();

                public event Action<int> IntValueUpdated
                {
                    add => intValueDelegates.Add(value);

                    remove => intValueDelegates.Remove(value);
                }

                protected override void HandleFieldUpdates(Update update)
                {
                    if (update.FloatValue.HasValue)
                    {
                        floatValueDelegates.ForEach(callback =>
                        {
                            try
                            {
                                callback(update.FloatValue.Value);
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        });
                    }

                    if (update.IntValue.HasValue)
                    {
                        intValueDelegates.ForEach(callback =>
                        {
                            try
                            {
                                callback(update.IntValue.Value);
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        });
                    }
                }
            }

            public class Update : ISpatialComponentUpdate<ReaderTestComponent>
            {
                public Option<float> FloatValue;
                public Option<int> IntValue;
            }
        }

        protected struct SomeOtherComponent : IComponentData
        {
        }

        private World world;
        protected EntityManager EntityManager;
        protected Entity Entity;
        protected ReaderTestComponent.Reader Reader;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");

            EntityManager = world.GetOrCreateManager<EntityManager>();

            Entity = EntityManager.CreateEntity(typeof(ReaderTestComponent));

            Reader = new ReaderTestComponent.Reader(Entity, EntityManager);
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }
    }
}
