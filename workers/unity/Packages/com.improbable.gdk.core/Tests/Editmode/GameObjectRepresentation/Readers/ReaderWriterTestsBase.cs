using System;
using System.Collections.Generic;
using Generated.Improbable.Gdk.Tests.BlittableTypes;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Gdk.Core.MonoBehaviours;
using NUnit.Framework;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    internal abstract class ReaderWriterTestsBase
    {
        protected BlittableComponent.Reader ReaderPublic;
        protected BlittableComponent.ReaderWriterImpl ReaderInternal;
        protected EntityManager EntityManager;
        protected Entity Entity;
        private World world;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");
            EntityManager = world.GetOrCreateManager<EntityManager>();
            Entity = EntityManager.CreateEntity(typeof(SpatialOSBlittableComponent));
            ReaderPublic = new BlittableComponent.ReaderWriterImpl(Entity, EntityManager, new LoggingDispatcher());
            ReaderInternal = (BlittableComponent.ReaderWriterImpl) ReaderPublic;
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }

        protected struct SomeOtherComponent : IComponentData
        {
        }
    }
}
