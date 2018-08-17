using Generated.Improbable.Gdk.Tests.BlittableTypes;
using NUnit.Framework;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.Readers
{
    internal abstract class ReaderWriterTestsBase
    {
        protected BlittableComponent.Accessors.Reader ReaderPublic;
        protected BlittableComponent.Accessors.Writer WriterPublic;
        protected BlittableComponent.Accessors.ReaderWriterImpl ReaderWriterInternal;
        protected EntityManager EntityManager;
        protected Entity Entity;
        private World world;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");
            EntityManager = world.GetOrCreateManager<EntityManager>();
            Entity = EntityManager.CreateEntity(typeof(SpatialOSBlittableComponent));
            ReaderWriterInternal = new BlittableComponent.Accessors.ReaderWriterImpl(Entity, EntityManager, new LoggingDispatcher());
            ReaderPublic = ReaderWriterInternal;
            WriterPublic = ReaderWriterInternal;
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
