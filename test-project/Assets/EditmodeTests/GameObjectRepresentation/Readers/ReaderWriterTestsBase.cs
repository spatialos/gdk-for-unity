using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.BlittableTypes;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.Readers
{
    internal abstract class ReaderWriterTestsBase
    {
        protected BlittableComponent.Requirable.Reader ReaderPublic;
        protected BlittableComponent.Requirable.Writer WriterPublic;
        protected BlittableComponent.Requirable.ReaderWriterImpl ReaderWriterInternal;
        protected EntityManager EntityManager;
        protected Entity Entity;
        private World world;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");
            EntityManager = world.GetOrCreateManager<EntityManager>();
            Entity = EntityManager.CreateEntity(typeof(BlittableComponent.Component));
            ReaderWriterInternal =
                new BlittableComponent.Requirable.ReaderWriterImpl(Entity, EntityManager, new LoggingDispatcher());
            ReaderPublic = ReaderWriterInternal;
            WriterPublic = ReaderWriterInternal;
        }

        [TearDown]
        public void TearDown()
        {
            world.Dispose();
        }

        protected struct SomeOtherComponent : ISpatialComponentData, IComponentData
        {
            public BlittableBool DirtyBit { get; set; }
            public uint ComponentId { get; }
        }
    }
}
